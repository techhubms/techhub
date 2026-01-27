---
layout: "post"
title: "Troubleshooting Azure Redis: Key TTL and Size Analysis with Bash and Lua"
description: "This post, by LuisFilipe, presents practical Bash and Lua scripting techniques for collecting and analyzing key time-to-live (TTL) and value sizes in Azure Cache for Redis. Readers will learn how key expiration, TTL strategies, and key sizes impact performance and memory use, as well as how to safely run scripts for cache diagnostics."
author: "LuisFilipe"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-paas-blog/redis-keys-statistics/ba-p/4486079"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2026-01-21 15:50:34 +00:00
permalink: "/2026-01-21-Troubleshooting-Azure-Redis-Key-TTL-and-Size-Analysis-with-Bash-and-Lua.html"
categories: ["Azure", "Coding", "DevOps"]
tags: ["Azure", "Azure Cache For Redis", "Azure Managed Redis", "Bash Scripting", "Best Practices", "Cache Management", "Coding", "Community", "DevOps", "Key Size", "Linux", "Lua Scripting", "Memory Optimization", "Monitoring", "Performance Tuning", "Redis", "Redis CLI", "Troubleshooting", "TTL"]
tags_normalized: ["azure", "azure cache for redis", "azure managed redis", "bash scripting", "best practices", "cache management", "coding", "community", "devops", "key size", "linux", "lua scripting", "memory optimization", "monitoring", "performance tuning", "redis", "redis cli", "troubleshooting", "ttl"]
---

LuisFilipe shares practical Bash and Lua scripts for collecting TTL and key size statistics in Azure Cache for Redis, detailing how to optimize memory, performance, and cache longevity.<!--excerpt_end-->

# Troubleshooting Azure Redis: Key TTL and Size Analysis with Bash and Lua

Understanding the time-to-live (TTL) for Redis keys and their sizes is foundational for maintaining optimal performance and reliability in Azure Cache for Redis. In this guide, LuisFilipe explains how these factors affect cache behavior, memory usage, and system throughput, and offers ready-made Bash and Lua scripts to automate diagnostics and statistics gathering.

## Key Concepts: TTL and Key Sizes

### Key Time-to-Live (TTL)

- **TTL** determines how long a key remains in the cache before expiration.
- If TTL is not set (`-1`), the key is persistent and won’t expire unless removed manually.
- Keys may also expire due to memory eviction if Redis memory is saturated, or if their TTL has elapsed.
- TTL can be set at creation or updated later using commands like `SET ... EX`, `SET ... PX`, `EXPIRE`, `TTL`, and `PERSIST`.
- Redis uses a combination of *lazy* (on key access) and *active* (background job) expiration strategies, so key deletion may not be immediate upon TTL expiry.

#### Example TTL Management Commands

- `SET key1 value1 EX 60` — sets TTL as 60 seconds.
- `EXPIRE key1 60` — sets a timeout of 60 seconds.
- `TTL key1` — gets the remaining TTL in seconds.
- `PERSIST key1` — removes TTL, making the key persistent.

### Key Sizes

- Large key values can degrade performance due to the single-threaded nature of Redis.
- Recommended: keep keys small (ideally <1KB, max 100KB in Azure Redis).
- Response size is determined by operation and may aggregate across keys in multi-key requests.

## Analyzing TTL and Size in Azure Cache for Redis

### Solution 1: Get Key Statistics

- Bash and Lua scripts work with `redis-cli` to scan all keys and summarize:
  - Number of keys with no TTL set
  - Number of keys above or below a specified TTL threshold
  - Number of keys above or below a specified size threshold
  - Total number of keys, start and end timestamps, and scan duration
- **Warning:** Scanning all keys is resource-intensive and may block Redis operations, especially for large datasets. Test in dev/staging first.
- **Example script output:**
  
  ```
  Scanning number of keys with TTL threshold 100 Seconds, and Key size threshold 500 Bytes
  Start time: 18:12:15
  -----------------------
  Total keys scanned: 1227
  ------------
  TTL not set : 2
  TTL >= 100 seconds: 1225
  TTL < 100 seconds: 0
  ...
  Keys with Size >= 500 Bytes: 1225
  Keys with Size < 500 Bytes: 2
  ...
  End time: 19:12:16
  Duration : 0 days 00:00:00.630
  ```

#### How to Use

1. Save the provided `getKeyStats.sh` and `getKeyStats.lua` in the same folder.
2. Run with: `./getKeyStats.sh <host> <password> [port] [ttl_threshold] [size_threshold]`
3. Example:

   ```
   ./getKeyStats.sh mycache.redis.cache.windows.net mypassword 10000 600 102400
   ```

### Solution 2: List Key Names

- Bash and Lua scripts list all keys meeting TTL and size criteria for further inspection.
- Can select for keys with or without TTL, above or below certain TTLs or sizes.
- Supports combinations (e.g., keys with no TTL and size > 100KB).
- **Warning:** Also potentially blocks Redis during execution; use with care.

#### Example Command and Output

  ```
  ./listKeys.sh mycache.redis.cache.windows.net mypassword 10000 +600 +102400
  ```

  Output includes each matching key name, their TTL, and size.

## Best Practices and References

- Prefer short-lived, small-sized keys for optimal performance.
- Avoid frequent full key scans on production systems.
- Microsoft Azure recommends not exceeding 100KB per key.
- Refer to the official documentation for scripting, Redis commands, and Azure cache best practices:
  - [Azure Managed Redis](https://learn.microsoft.com/en-us/azure/redis/overview)
  - [Azure Redis Best Practices](https://learn.microsoft.com/en-us/azure/azure-cache-for-redis/cache-best-practices-development)
  - [Redis Command Reference](https://redis.io/docs/latest/commands/)

## Useful Scripts

### getKeyStats.sh (Bash)

```bash
# !/usr/bin/env bash

# Linux Bash Script to get statistics from Redis Keys TTL values and Key value sizes

# ...

# Full script in article
```

### getKeyStats.lua (Lua)

```lua
local ttl_threshold = tonumber(ARGV[1])
local size_threshold = tonumber(ARGV[2])
local cursor = "0"

-- Counters
local no_ttl = 0
local nonexist = 0
local ttl_high = 0
local ttl_low = 0
local ttl_invalid = 0
local size_high = 0
local size_low = 0
local size_nil = 0
local total = 0

repeat
  local scan = redis.call("SCAN", cursor, "COUNT", 1000)
  cursor = scan[1]
  local keys = scan[2]
  for _, key in ipairs(keys) do
    local ttl = redis.call("TTL", key)
    local size = redis.call("MEMORY","USAGE", key)
    total = total + 1
    if ttl == -1 then
      no_ttl = no_ttl + 1
    elseif ttl == -2 then
      nonexist = nonexist + 1
    elseif type(ttl) ~= "number" then
      ttl_invalid = ttl_invalid + 1
    elseif ttl >= ttl_threshold then
      ttl_high = ttl_high + 1
    else
      ttl_low = ttl_low + 1
    end
    if size == nil then
      size_nil = size_nil + 1
    elseif size >= size_threshold then
      size_high = size_high + 1
    else
      size_low = size_low + 1
    end
  end
until cursor == "0"

return {no_ttl, nonexist, ttl_high, ttl_low, ttl_invalid, size_high, size_low, size_nil, total}
```

## Final Notes

- Always test these scripts on non-production data to prevent disruption.
- Adjust thresholds to your workload for actionable diagnostics.
- Refer to linked docs for ongoing updates and Azure Redis platform changes.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-paas-blog/redis-keys-statistics/ba-p/4486079)
