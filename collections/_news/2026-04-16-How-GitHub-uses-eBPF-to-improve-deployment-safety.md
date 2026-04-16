---
primary_section: devops
author: Lawrence Gripper
external_url: https://github.blog/engineering/infrastructure/how-github-uses-ebpf-to-improve-deployment-safety/
feed_name: GitHub Engineering Blog
title: How GitHub uses eBPF to improve deployment safety
tags:
- BPF PROG TYPE CGROUP SKB
- BPF PROG TYPE CGROUP SOCK ADDR
- Bpf2go
- Cgroups
- Cilium/ebpf
- Circular Dependencies
- Deployment
- Deployment Safety
- Deployments
- DevOps
- DNS Proxy
- Domain Allowlist
- Domain Blocklist
- Ebpf
- Ebpf Maps
- Engineering
- Fix Forward
- GitHub
- Go
- Incident Response
- Infrastructure
- Linux Kernel
- Network Egress Filtering
- News
- Observability
- Platform Engineering
- Rollback
date: 2026-04-16 16:00:00 +00:00
section_names:
- devops
---

Lawrence Gripper explains how GitHub uses eBPF and cgroups to prevent deployment scripts from introducing circular dependencies (like needing github.com during an outage), including a Go-based proof of concept that monitors and conditionally blocks network/DNS access for deploy processes.<!--excerpt_end-->

# How GitHub uses eBPF to improve deployment safety

GitHub hosts and deploys github.com using its own infrastructure and code hosted on github.com. That creates a classic risk: if github.com is down, you may be unable to access the code or artifacts needed to deploy a fix.

GitHub mitigates the obvious form of this by keeping a mirror of their code for fixing forward and built assets for rolling back. But there are more subtle ways circular dependencies can creep in—especially through deployment scripts and the tools they run.

## The problem: circular dependencies during incidents

A hypothetical scenario: a MySQL outage prevents GitHub from serving `release` data. To recover, a configuration change must be rolled out to stateful MySQL nodes via a deploy script.

Circular dependencies that can break recovery:

1. **Direct dependency**
   - The deploy script pulls the latest release of an open source tool from GitHub.
   - GitHub can’t serve release data during the outage, so the deploy fails.

   ![Diagram showing a MySQL deploy script fails after attempting to pull a release from GitHub](https://github.blog/wp-content/uploads/2026/04/Screenshot-2026-04-06-at-6.36.24-PM.png?resize=1433%2C194)

2. **Hidden dependencies**
   - The deploy script runs a tool already on disk.
   - That tool silently checks GitHub for updates.
   - If it can’t reach GitHub, it may fail or hang depending on error handling.

   ![Diagram showing a script failing after being unable to contact GitHub](https://github.blog/wp-content/uploads/2026/04/Screenshot-2026-04-06-at-6.36.34-PM.png?resize=1439%2C363)

3. **Transient dependencies**
   - The deploy script calls another internal service (for example, a migrations service).
   - That service fetches a new tool binary from GitHub releases.
   - The failure propagates back to the deploy script.

   ![Diagram showing a deploy script calling an internal service that tries to fetch a release from GitHub](https://github.blog/wp-content/uploads/2026/04/Screenshot-2026-04-06-at-6.36.41-PM.png?resize=1450%2C202)

## Why "just block github.com" doesn’t work

A straightforward test would be blocking access to github.com on the hosts to ensure deploys don’t depend on it.

But GitHub’s hosts are stateful and still serve production traffic during rolling deploys, drains, and restarts. Fully blocking github.com would also break legitimate production behaviors.

So the goal becomes: **block or monitor network access only for the deployment process**, not for the whole machine.

## Using eBPF + cgroups for per-process network filtering

GitHub evaluated eBPF as a way to selectively monitor and block network calls.

Key primitives:

- **eBPF**: load custom programs into the Linux kernel and hook into core primitives like networking.
- **cgroups**: isolate and apply limits to sets of processes (commonly used by Docker, but usable directly without Docker).

The idea:

- Create a cgroup
- Put the deployment script (and only it) into that cgroup
- Apply outbound network filtering at the cgroup boundary

### Hooking cgroup network egress (CGROUP_SKB)

GitHub focused on the eBPF program type:

- `BPF_PROG_TYPE_CGROUP_SKB` (network egress from a particular cgroup)
  - Docs: https://docs.ebpf.io/linux/program-type/BPF_PROG_TYPE_CGROUP_SKB/

They built a proof of concept in Go using the `cilium/ebpf` library.

- `cilium/ebpf` examples: https://github.com/cilium/ebpf/tree/main/examples/cgroup_skb

Example of attaching an eBPF program to count egress packets:

```go
//go:generate go tool bpf2go -tags linux bpf cgroup_skb.c -- -I../headers

func main() {
    // Load pre-compiled programs and maps into the kernel.
    objs := bpfObjects{}
    if err := loadBpfObjects(&objs, nil); err != nil {
        log.Fatalf("loading objects: %v", err)
    }
    defer objs.Close()

    // Link the count_egress_packets program to the cgroup.
    l, err := link.AttachCgroup(link.CgroupOptions{
        Path:    "/sys/fs/cgroup/system.slice",
        Attach:  ebpf.AttachCGroupInetEgress,
        Program: objs.CountEgressPackets,
    })
    if err != nil {
        log.Fatal(err)
    }
    defer l.Close()

    log.Println("Counting packets...")

    // Read loop reporting the total amount of times the kernel function was entered.
    ticker := time.NewTicker(1 * time.Second)
    defer ticker.Stop()

    for range ticker.C {
        var value uint64
        if err := objs.PktCount.Lookup(uint32(0), &value); err != nil {
            log.Fatalf("reading map: %v", err)
        }
        log.Printf("number of packets: %d\n", value)
    }
}
```

And the corresponding eBPF C code:

```c
//go:build ignore

#include "common.h"

char __license[] SEC("license") = "Dual MIT/GPL";

struct {
    __uint(type, BPF_MAP_TYPE_ARRAY);
    __type(key, u32);
    __type(value, u64);
    __uint(max_entries, 1);
} pkt_count SEC(".maps");

SEC("cgroup_skb/egress")
int count_egress_packets(struct __sk_buff *skb) {
    u32 key = 0;
    u64 init_val = 1;

    u64 *count = bpf_map_lookup_elem(&pkt_count, &key);
    if (!count) {
        bpf_map_update_elem(&pkt_count, &key, &init_val, BPF_ANY);
        return 1;
    }

    __sync_fetch_and_add(count, 1);
    return 1;
}
```

The `//go:generate` line uses `bpf2go` to compile the C eBPF program and generate a Go struct (`bpfObjects`) to attach and interact with the program.

### Problem: IP-based filtering isn’t stable enough

`CGROUP_SKB` works at the IP address level. For a large, changing system, maintaining an up-to-date IP block list is difficult.

GitHub explored a DNS-based approach instead.

### Intercepting DNS using CGROUP_SOCK_ADDR

They used:

- `BPF_PROG_TYPE_CGROUP_SOCK_ADDR` (hook socket syscalls and rewrite destination)
  - Docs: https://docs.ebpf.io/linux/program-type/BPF_PROG_TYPE_CGROUP_SOCK_ADDR/

A simplified approach:

- Hook `connect4`
- If destination port is DNS (`53`), rewrite it to `localhost:53` (or similar)
- Forward DNS queries to a local userspace DNS proxy

Example of attaching:

```go
cgroupLink, err := link.AttachCgroup(link.CgroupOptions{
    Path:   cgroup.Name(),
    Attach: ebpf.AttachCGroupInet4Connect,
    Program: obj.Connect4,
})
if err != nil {
    return nil, fmt.Errorf("attaching eBPF program Connect4 to cgroup: %w", err)
}
```

Simplified eBPF snippet for rewriting DNS connects:

```c
/* This is the hexadecimal representation of 127.0.0.1 address */
const __u32 ADDRESS_LOCALHOST_NETBYTEORDER = bpf_htonl(0x7f000001);

SEC("cgroup/connect4")
int connect4(struct bpf_sock_addr *ctx) {
    __be32 original_ip = ctx->user_ip4;
    __u16 original_port = bpf_ntohs(ctx->user_port);

    if (ctx->user_port == bpf_htons(53)) {
        /* For DNS Query (*:53) rewire service to backend
         * 127.0.0.1:const_dns_proxy_port */
        ctx->user_ip4 = const_mitm_proxy_address;
        ctx->user_port = bpf_htons(const_dns_proxy_port);
    }

    return 1;
}
```

Now:

- The deploy script’s DNS traffic is forced through the proxy
- The proxy checks domains against a block list
- The proxy uses **eBPF Maps** to communicate decisions to the `CGROUP_SKB` program (allow/deny)

Related concept docs:

- eBPF Maps: https://docs.ebpf.io/linux/concepts/maps/

Proof of concept repo:

- https://github.com/lawrencegripper/ebpf-cgroup-firewall/

## Correlating blocked DNS requests to the responsible process

GitHub also wanted to tell teams *which command* triggered the blocked request.

In `BPF_PROG_TYPE_CGROUP_SKB`, they can:

- Read the `skb_buff` context: https://docs.ebpf.io/linux/program-context/__sk_buff/
- Extract DNS transaction IDs from DNS packets
- Capture the initiating process ID (PID) via helper: https://docs.ebpf.io/linux/helper-function/bpf_get_current_pid_tgid/

They store `DNS transaction ID -> PID` in an eBPF map, then in userspace:

- Match the DNS transaction ID to the resolved domain name
- Use `/proc/{PID}/cmdline` to identify the command line

Simplified code:

```c
__u32 pid = bpf_get_current_pid_tgid() >> 32;

__u16 skb_read_offset = sizeof(struct iphdr) + sizeof(struct udphdr);
__u16 dns_transaction_id = get_transaction_id_from_dns_header(skb, skb_read_offset);

if (pid && dns_transaction_id != 0) {
    bpf_map_update_elem(&dns_transaction_id_to_pid, &dns_transaction_id, pid, BPF_ANY);
}
```

Example log output:

```text
> WARN DNS BLOCKED reason=FromDNSRequest blocked=true blockedAt=dns domain=github.com. pid=266767 cmd="curl github.com " firewallMethod=blocklist
```

## What this enables

With this approach, GitHub can:

- Conditionally block domains that would introduce circular dependencies from deployment scripts.
- Report which command triggered a blocked request.
- Provide an audit list of all domains contacted during a deployment.
- Use cgroups to enforce CPU/memory limits on deploy scripts to avoid runaway resource usage impacting workloads.

## Current status and next steps

GitHub reports the circular dependency detection process is live after a six-month rollout. It now flags newly introduced problematic dependencies (including new dependencies added indirectly by existing tools/binaries).

They expect there will still be edge cases and plan to iterate as they discover new failure modes.

## Want to dive in?

- `cilium/ebpf` examples: https://github.com/cilium/ebpf/tree/main/examples
- eBPF documentation: http://docs.ebpf.io
- Tools powered by eBPF:
  - bpftrace tutorial one-liners: https://bpftrace.org/tutorial-one-liners#lesson-3-file-opens
  - ptcpdump: https://github.com/mozillazg/ptcpdump


[Read the entire article](https://github.blog/engineering/infrastructure/how-github-uses-ebpf-to-improve-deployment-safety/)

