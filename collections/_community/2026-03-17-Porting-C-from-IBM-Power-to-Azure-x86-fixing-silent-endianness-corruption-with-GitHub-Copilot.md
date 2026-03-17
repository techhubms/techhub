---
date: 2026-03-17 07:36:07 +00:00
section_names:
- ai
- azure
- devops
- github-copilot
feed_name: Microsoft Tech Community
primary_section: github-copilot
title: 'Porting C++ from IBM Power to Azure x86: fixing silent endianness corruption with GitHub Copilot'
external_url: https://techcommunity.microsoft.com/t5/azure-migration-and/porting-c-from-ibm-power-to-x86-solving-silent-endianness/ba-p/4501666
author: OsvaldoDaibert
tags:
- AI
- AKS
- Application Insights
- Azure
- Azure App Service
- Azure Container Apps
- Azure Container Registry
- Azure Monitor
- Azure Virtual Machines
- Big Endian
- Binary Data Corruption
- Byte Swapping
- C++
- C++20
- Cloud Adoption Framework
- Community
- DevOps
- EBCDIC
- Endianness
- GitHub Copilot
- GitHub Copilot Chat
- IBM I
- IBM Power Systems
- Iconv
- Little Endian
- Memcpy
- Migration Refactoring
- OS/400
- Static Assert
- Std::endian
- Struct Padding
- UTF 8
- VS Code
---

OsvaldoDaibert explains a common IBM Power → Azure x86 migration failure mode in C++: silent integer corruption caused by Big-Endian vs Little-Endian byte order, and shows a practical refactoring workflow—accelerated with GitHub Copilot—to add portable byte-swapping, guard against struct padding, and deploy to Azure.<!--excerpt_end-->

## Overview

Migrating C++ workloads from **IBM Power / IBM i (OS/400)** to **Azure x86/x64** often fails in a non-obvious way: the application compiles cleanly and runs without exceptions, but produces **wrong values in production**.

The core issue is **byte order (endianness)**. Code that implicitly assumed Big-Endian binary layouts will silently corrupt **multi-byte integers** when recompiled on Little-Endian x86.

This guide explains the problem, shows a concrete example, and outlines a practical modernization pipeline, including how **GitHub Copilot** can speed up refactoring.

## Why this migration is different

The C++ language itself is portable, but decades of development on IBM Power commonly results in code that assumes:

- **Big-Endian** byte ordering
- particular **memory layouts**
- IBM i / OS-specific APIs and services

When recompiled for x86 Linux/Windows, the most dangerous failure mode is that binary data corruption is **silent**:

- Transaction ID `1` becomes `16,777,216`
- `$50.00` becomes `$22M`
- no compile errors
- no runtime exceptions

Microsoft’s Cloud Adoption Framework describes migration strategies such as rehosting/refactoring/replatforming; this scenario is squarely **refactoring**, because the code must change to behave correctly on the new architecture.

- Cloud Adoption Framework overview: https://learn.microsoft.com/en-us/azure/cloud-adoption-framework/overview

## Understanding the core problem: endianness

**Endianness** is how a CPU stores multi-byte values in memory.

For a 4-byte integer `0x00000001` (decimal `1`):

- **Big-Endian** stores the most significant byte first: `00 00 00 01`
- **Little-Endian** stores the least significant byte first: `01 00 00 00`

IBM Power systems are typically **Big-Endian**; Intel/AMD x86/x64 (including Azure VMs) are **Little-Endian**.

- Azure Virtual Machines: https://learn.microsoft.com/azure/virtual-machines/

### Architecture differences (summary)

| Property | IBM Power (Source) | Azure x86 (Target) |
| --- | --- | --- |
| Byte order | Big-Endian | Little-Endian |
| Text encoding | EBCDIC | ASCII / UTF-8 |
| Architecture | RISC | CISC |
| Scaling model | Vertical | Horizontal |
| Compiler | IBM XL C/C++ | GCC / Clang / MSVC |
| Primary OS | IBM i (OS/400) | Linux / Windows |

## What breaks: reading Big-Endian binary records on x86

Legacy code often reads binary records by copying raw bytes into a struct (e.g., `memcpy`/`memmove`). That “works” on Big-Endian hosts, but produces wrong integer values on Little-Endian hosts.

### Example: simulated Big-Endian buffer

```cpp
// Simulated Big-Endian binary buffer from an OS/400 system:
const char buffer[] = {
  0x00, 0x00, 0x00, 0x01,       // txnId = 1
  0x00, 0x00, 0x13, (char)0x88, // amountCents = 5000 ($50.00)
  0x00, 0x64,                   // storeNumber = 100
  0x00, 0x07,                   // pumpNumber = 7
  'V', 'I', 'S', 'A'            // cardType = "VISA"
};
```

On x86, copying this into a struct and interpreting integers directly yields:

| Field | Expected value | x86 result | What happened |
| --- | --- | --- | --- |
| txnId | 1 | 16,777,216 | `00 00 00 01` read as `0x01000000` |
| amountCents | 5000 ($50.00) | 2,282,946,560 ($22.8M) | bytes reversed |
| storeNumber | 100 | 25,600 | `00 64` read as `0x6400` |
| pumpNumber | 7 | 1,792 | `00 07` read as `0x0700` |
| cardType | VISA | VISA | char arrays are unaffected |

**Key detail:** single-byte data (chars/strings) is fine; **multi-byte integers** (`uint32_t`, `uint16_t`, etc.) are corrupted.

## Three “silent killers” in cross-architecture migration

1. **Binary data corruption**
   - Any `memcpy`/`memmove` into structs with integer fields can break.

2. **EBCDIC vs ASCII/UTF-8**
   - IBM i commonly uses EBCDIC; x86 platforms commonly use UTF-8.
   - `iconv` can be used to convert.

3. **Struct padding differences**
   - Different compilers (IBM XL C/C++ vs GCC/Clang) may align/pad structs differently.
   - Use `static_assert(sizeof(YourStruct) == expected_size)` to detect mismatches early.

## A practical modernization workflow (7 steps)

1. **Inventory**
   - Find binary I/O, pointer casts, raw buffer copies, IBM XL C++ extensions.

2. **Refactor endianness**
   - Add portable byte-swap utilities at every binary boundary.
   - Use `if constexpr (std::endian::native == std::endian::little)` so it compiles down without runtime branching.

3. **Replace OS/400 APIs**
   - Replace Data Queues, User Spaces, Message Queues, record-level file access with POSIX or standard C++.

4. **Convert text encoding**
   - Convert EBCDIC to UTF-8 using `iconv` (or equivalent).

5. **Recompile**
   - Build with GCC/Clang using `-std=c++20` (or later) and run your test suite on x86 Linux.

6. **Containerize**
   - Build a Linux container image and push to Azure Container Registry.
   - ACR: https://learn.microsoft.com/azure/container-registry/

7. **Deploy**
   - Deploy to AKS or VMs; consider Container Apps or App Service depending on the workload.
   - AKS: https://learn.microsoft.com/azure/aks/
   - Azure Container Apps: https://learn.microsoft.com/azure/container-apps/
   - Azure App Service (Linux): https://learn.microsoft.com/azure/app-service/

### Deployment options

| Deployment Model | Azure Service | Best For |
| --- | --- | --- |
| Single VM | Azure Virtual Machines (Dv5 / Fsv2) | Direct replacement of the Power partition |
| Containers | Azure Kubernetes Service (AKS) | Horizontal scaling, rolling updates, microservices |
| Serverless Containers | Azure Container Apps | Event-driven workloads, no Kubernetes management |
| PaaS | Azure App Service (Linux) | Web APIs with built-in TLS, autoscaling, deployment slots |

For observability post-migration:

- Azure Monitor: https://learn.microsoft.com/azure/azure-monitor/
- Application Insights: https://learn.microsoft.com/azure/azure-monitor/app/app-insights-overview

## Accelerating the refactoring with GitHub Copilot

The content proposes using **GitHub Copilot** in VS Code to speed up identification and refactoring of endianness-sensitive patterns (e.g., `memcpy` into structs, pointer casts, binary file reads).

- Copilot docs: https://docs.github.com/copilot

### Before: legacy OS/400-style buffer copy

```cpp
void processTxn(const char* rawBuffer) {
  TxnRecord txn;

  // Direct memory copy — NO byte-order conversion.
  std::memcpy(&txn, rawBuffer, sizeof(TxnRecord));

  std::cout << "Txn ID : " << txn.txnId << "\n";
  std::cout << "Amount ($) : " << txn.amountCents / 100.0 << "\n";
  std::cout << "Store : " << txn.storeNumber << "\n";
  std::cout << "Pump : " << txn.pumpNumber << "\n";
}
```

**Example wrong output on x86:**

```text
Txn ID : 16777216  ← should be 1
Amount ($) : 2.28295e+07 ← should be 50
Store : 25600      ← should be 100
Pump : 1792        ← should be 7
```

### Copilot-generated portable byte-swap utility (C++20)

```cpp
#include <bit> // C++20: std::endian

inline uint32_t fromBigEndian32(uint32_t v) {
  if constexpr (std::endian::native == std::endian::big)
    return v; // No-op on Big-Endian hosts (zero overhead)

#if defined(__GNUC__) || defined(__clang__)
  return __builtin_bswap32(v);
#elif defined(_MSC_VER)
  return _byteswap_ulong(v);
#else
  return ((v >> 24) & 0x000000FF) |
         ((v >> 8) & 0x0000FF00) |
         ((v << 8) & 0x00FF0000) |
         ((v << 24) & 0xFF000000);
#endif
}

inline uint16_t fromBigEndian16(uint16_t v) {
  if constexpr (std::endian::native == std::endian::big)
    return v;

#if defined(__GNUC__) || defined(__clang__)
  return __builtin_bswap16(v);
#elif defined(_MSC_VER)
  return _byteswap_ushort(v);
#else
  return static_cast<uint16_t>((v >> 8) | (v << 8));
#endif
}
```

### After: modernized code (correct on all platforms)

```cpp
// Compile-time guard: catch unexpected padding
static_assert(sizeof(TxnRecord) == 16, "TxnRecord size mismatch — check struct alignment/padding");

void processTxn(const char* rawBuffer) {
  TxnRecord txn;

  // Step 1: Raw copy (identical to OS/400 code)
  std::memcpy(&txn, rawBuffer, sizeof(TxnRecord));

  // Step 2: Byte-order conversion — Big-Endian source → host order
  txn.txnId = fromBigEndian32(txn.txnId);
  txn.amountCents = fromBigEndian32(txn.amountCents);
  txn.storeNumber = fromBigEndian16(txn.storeNumber);
  txn.pumpNumber = fromBigEndian16(txn.pumpNumber);
  // cardType: char array — no conversion needed

  // Step 3: Process as normal
  std::cout << "Txn ID : " << txn.txnId << "\n";
  std::cout << "Amount ($) : " << txn.amountCents / 100.0 << "\n";
  std::cout << "Store : " << txn.storeNumber << "\n";
  std::cout << "Pump : " << txn.pumpNumber << "\n";
}
```

**Example correct output on x86:**

```text
Txn ID : 1
Amount ($) : 50
Store : 100
Pump : 7
```

### What to watch for (Copilot won’t catch everything)

- **Packed decimals (COMP-3)**: needs a dedicated parser.
- **Mixed EBCDIC text and binary integers**: you may need byte swaps and charset conversion in the same pipeline.
- **Struct padding**: keep `static_assert` checks.

## Companion repository

Working example repository (legacy + modernized code):

- https://github.com/odaibert/os400-cpp-modernization

Included (as described):

- before/after C++ source
- solution guide (endianness theory, architecture mapping, Azure deployment options, VM sizing tables, Well-Architected considerations)
- Copilot modernization guide with prompts
- replication task plan

### Build and run (as provided)

```bash
g++ -std=c++17 -o pos_legacy src/pos_transaction.cpp
g++ -std=c++20 -o pos_modern src/pos_transaction_x86.cpp

./pos_legacy # wrong output on x86
./pos_modern # correct output
```

## Additional references mentioned

- Azure Accelerate: https://azure.microsoft.com/en-us/solutions/azure-accelerate/
- Cloud Adoption Framework: https://learn.microsoft.com/en-us/azure/cloud-adoption-framework/overview


[Read the entire article](https://techcommunity.microsoft.com/t5/azure-migration-and/porting-c-from-ibm-power-to-x86-solving-silent-endianness/ba-p/4501666)

