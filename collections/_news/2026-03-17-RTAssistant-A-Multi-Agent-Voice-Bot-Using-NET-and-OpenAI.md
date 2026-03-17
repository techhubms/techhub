---
title: 'RT.Assistant: A Multi-Agent Voice Bot Using .NET and OpenAI'
section_names:
- ai
- dotnet
author: Faisal Waris
feed_name: Microsoft .NET Blog
primary_section: ai
tags:
- .NET
- Agent Orchestration
- AI
- Async Workflows
- Deterministic State Machine
- Developer Stories
- Discriminated Unions
- F#
- Fabulous
- HybridWebView
- MAUI
- Microsoft.Extensions.AI
- Multi Agent
- Multi Agent Systems
- News
- OpenAI
- OpenAI Realtime API
- OPUS Codec
- Prolog
- RAG
- RT.Assistant
- RTFlow
- Tau Prolog
- Tool Calling
- Voice
- Voice Assistant
- WebRTC
date: 2026-03-17 17:05:00 +00:00
external_url: https://devblogs.microsoft.com/dotnet/rt-assistant-a-realtime-multiagent-voice-bot-using-dotnet-and-open-ai-api/
---

Faisal Waris walks through RT.Assistant, a production-style, voice-enabled multi-agent app built in .NET that uses the OpenAI Realtime API over WebRTC, F# for orchestration, .NET MAUI for cross-platform UI, and a Prolog-based approach to retrieval for more precise answers.<!--excerpt_end-->

# RT.Assistant: A Multi-Agent Voice Bot Using .NET and OpenAI

This is a guest post by **Faisal Waris**, an AI strategist in the telecom industry. RT.Assistant is a voice-enabled, multi-agent assistant built entirely in .NET, combining OpenAI’s Realtime API over WebRTC, F# for agent orchestration, .NET MAUI for a cross-platform UI, and Microsoft.Extensions.AI for portable LLM integration.

## What RT.Assistant is

RT.Assistant is designed as a low-latency, bidirectional voice assistant:

- **Voice I/O**: OpenAI Realtime API over **WebRTC** for realtime audio + data.
- **Agent orchestration**: **F#** discriminated unions and async state machines.
- **UI**: **.NET MAUI** (via **Fabulous**) for iOS, Android, macOS, and Windows.
- **LLM portability**: **Microsoft.Extensions.AI** to target both OpenAI and Anthropic models.

Under the hood, a custom framework called **RTFlow** hosts multiple agents that communicate over a strongly typed async bus. A deterministic “Flow” state machine is used to keep non-deterministic LLM behavior controlled.

## Why telecom plan selection?

The sample uses telecom plan selection because plans are hard to compare due to many interacting variables:

- Base plan voice/text/data rates and limits
- Mobile hotspot limits (e.g., 20GB/50GB/100GB)
- Premium data prioritization
- Bundled streaming services (Netflix, Hulu, Apple TV, etc.)
- Taxes and fees inclusion/exclusion
- Discounts (military veterans, first responders, seniors, etc.)
- In-flight data limits
- Seasonal promotions
- Rules that vary by number of lines (e.g., streaming included only for 2+ lines)

## Technologies showcased

The sample integrates these components:

- **RTFlow**: multi-agent framework for realtime GenAI apps (F#)
- **RTOpenAI**: F# library for the OpenAI Realtime API via **WebRTC**
- **Fabulous for .NET MAUI**: build native apps in F#
- **Tau**: JavaScript Prolog engine used via **.NET MAUI HybridWebView**
- **Microsoft.Extensions.AI**: for LLM calls (OpenAI and Anthropic) to generate Prolog queries

## System overview

Key system behaviors:

- **Voice-enabled interaction**: users ask plan questions via speech.
- **Structured knowledge base**: plan details exist as logically consistent **Prolog facts**.
- **Specialized agents**:
  - **Voice Agent**: maintains the realtime voice connection to OpenAI; routes tool calls containing natural language queries; returns answers to the voice model for audio output.
  - **CodeGen Agent**: converts natural language to Prolog (via an LLM) and executes it against Tau.
  - **Query Agent**: runs predefined (“canned”) Prolog queries.
  - **App Agent**: coordinates communication and reports activity to the UI.

Diagram from the post:

![RTFlow multi-agent arrangement diagram](https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2026/03/RTflow.webp)

## 1) RTFlow

RTFlow is a framework for real-time agentic apps with three core elements:

- **Flow**: an async, deterministic state machine
- **Bus**: the message substrate
- **Agents**: specialized components that do work

### Bus

The Bus exposes two logical channels:

- **Agent broadcast channel**: agent-intent messages broadcast to all agents.
- **Flow input channel**: messages delivered only to the Flow (agents don’t receive these).

This separates agent collaboration from system-level orchestration.

### Messages and state

Flow and Agents keep private state and communicate via **strongly-typed async messages**. Message “schemas” are defined as F# discriminated unions (DUs), enabling:

- Compile-time exhaustiveness checking
- Explicit modeling of intent and system events
- Separation between agent-level and flow-level concerns

### Flow

The Flow is deterministic: state transitions happen only due to messages arriving on the Flow input channel. Two styles are described:

- **Minimal control**: simple lifecycle (`Start → Run → Terminate`)
- **Orchestrated control**: granular state machine where Flow coordinates agent behavior

The post positions this as a way to introduce determinism into LLM-driven systems.

### F# state machine snippet

The post illustrates Mealy-machine-like state functions with pattern matching over DU messages:

```fsharp
let rec s_start msg = async {
    match msg with
    | M_Start ->
        return F(s_run,[M_Started]) //transition to run
    | _ ->
        return F(s_start,[]) //stay in start state
}

and s_run msg = async {
    match msg with
    | M_DoSomething ->
        do! doSomething()
        return F(s_run,[M_DidSomething])
    | M_Terminate ->
        return F(s_terminate,[])
    | _ ->
        return F(s_run,[])
}

and s_terminate msg = async {
    ...
}
```

### Topology

RTFlow is described as a hybrid **bus–star** topology:

- Bus enables broadcast peer-style communication
- Flow acts as a central coordinator when needed

## 2) RTOpenAI

RTOpenAI wraps the OpenAI realtime voice API for native mobile (+) apps. Two highlighted features:

- **WebRTC support**
- **Strongly-typed realtime protocol messages**

### Why WebRTC (vs WebSockets)

The post argues WebRTC has advantages for realtime voice:

- Designed for bidirectional realtime communication; more resilient to minor disconnects.
- Separate channels for voice and data (and video, unused here), reducing the need for the app to manually handle audio streams.
- Uses **OPUS** codec with good compression and retained quality.
  - Over WebSockets, “high quality audio” may be sent as uncompressed 24KHz PCM base64, requiring ~10x the bandwidth compared to OPUS.

### Strongly-typed event handling

RTOpenAI defines F# types for OpenAI realtime protocol messages. Server/client messages are wrapped in DUs, enabling pattern matching.

Impressionistic handler snippet from the post:

```fsharp
let handleEvent (ev:ServerEvent) = async {
    match ev with
    | SessionCreated -> ...
    | ResponseOutputItemDone ev when isFunctionCall ev -> ...
    | _ -> ... //choose to ignore
}
```

## 3) Fabulous + .NET MAUI controls

**.NET MAUI** is used for cross-platform native apps. The F# library **Fabulous.MauiControls** enables building MAUI apps in F# using a functional-reactive model.

Key ideas:

- Declarative UI defined in F#
- UI events as DU messages handled via pattern matching
- State updates drive rendering

### Counter app snippet

```fsharp
/// A simple Counter app

type Model = //application state
    { Count: int }

type Msg = //DU message types
    | Increment
    | Decrement

let init () = { Count = 0 }

let update msg model = //function to handle UI events/messages
    match msg with
    | Increment -> { model with Count = model.Count + 1 }
    | Decrement -> { model with Count = model.Count - 1 }

let view model =
    Application(
        ContentPage(
            VStack(spacing = 16.) {
                //view
                Image("fabulous.png")

                Label($"Count is {model.Count}")

                Button("Increment", Increment)
                Button("Decrement", Decrement)
            }
        )
    )
```

### MAUI project structure and platform specifics

MAUI is a single project targeting multiple platforms, with platform specifics under `Platforms`:

```text
/RT.Assistant
  /Platforms
    /Android
    /IOS
    /MacCatalyst
    /Windows
```

The post notes ~90% shared code, with native platform interop for hardware integration (notably WebRTC):

- iOS binding wraps `WebRTC.xcframework` (C++)
- Android binding wraps `libwebrtc.aar`

## 4) Prolog for RAG (an alternative to vector search)

Instead of vector search, the sample uses an “unconventional RAG approach”:

- User query is translated into **Prolog**
- Query executes against a Prolog knowledge base embedded via **HybridWebView** (Tau Prolog)
- Output is used to generate answers with fewer hallucinations

The post’s reasoning:

- Generating a Prolog query from the schema is a smaller, more constrained task than generating the final answer from a large text context.
- If the Prolog query transformation fails, it will likely fail obviously (no results/errors), which is safer than subtle hallucinations in direct answer generation.

### Prolog representation

Skeleton schema:

```plaintext
plan(title,category,prices,features) % where each feature may have a different attribute set
```

Example partial fact (as shown in the post):

```plaintext
plan( "Connect", category("all"),
  prices([
    line(1, monthly_price(20), original_price(25)),
    line(2, monthly_price(40), original_price(26)),
    ...
  ]),
  features([

    feature(
      netflix(
        desc("Netflix Standard with Ads On Us"),
        included(yes)
      ),
      applies_to_lines(lines(2, 2))
    ),

    feature(
      autopay_monthly_discount(
        desc("$5 disc. per line up to 8 lines w/AutoPay & eligible payment method."),
        discount_per_line(5),
        lines_up_to(8),
        included_in_monthly_price(yes)
      ),
      applies_to_lines(all)
    ),
    ...

  ])
).
```

### Query processing and example

Natural language query:

```text
Find the plans in the category 'military_veteran' for 2 lines and list their costs.
```

Generated Prolog query:

```plaintext
plan(Title, category(military_veteran), prices(Lines), _),
member(price(2, monthly_price(Price), _), Lines).
```

Results example (as shown):

```text
Title = Connect Next Military, Lines = [line(1,monthly_price(85),original_price(90)), line(2,monthly_price(130),original_price(140)), line(3,monthly_price(165),original_price(180)), line(4,monthly_price(200),original_price(220)), line(5,monthly_price(235),original_price(260))], Price = 130

Title = Core, Lines = ...
```

If Prolog errors occur, the system retries query generation, including the error message in the prompt, up to a limit.

### Model choices for Prolog generation

For Prolog code generation, the app can use Claude Sonnet 4.5 or GPT 5.1 (via settings). The post notes GPT Codex had too much latency for realtime needs, and claims GPT-5.1 produced more concise/relevant output.

## Links from the post

- Original post: https://devblogs.microsoft.com/dotnet/rt-assistant-a-realtime-multiagent-voice-bot-using-dotnet-and-open-ai-api/
- YouTube (overview): https://youtu.be/bSMByJvYLoY
- YouTube (code walkthrough): https://youtu.be/0ghPhQyzyaI
- GitHub repo: https://github.com/fwaris/RTOpenAI/blob/master/readme.md
- LinkedIn post: https://www.linkedin.com/posts/activity-7410082754836725761-lquf?utm_source=share&utm_medium=member_desktop&rcm=ACoAAAAbaagBCG-0LlGBjghxmo7KKzbEXRHmiZ0


[Read the entire article](https://devblogs.microsoft.com/dotnet/rt-assistant-a-realtime-multiagent-voice-bot-using-dotnet-and-open-ai-api/)

