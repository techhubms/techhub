---
primary_section: ai
date: 2026-03-23 07:00:00 +00:00
title: Building real-world AI automation with Foundry Local and the Microsoft Agent Framework
feed_name: Microsoft Tech Community
author: Lee_Stott
section_names:
- ai
tags:
- Agent Orchestration
- AI
- Community
- CUDA
- Foundry Local
- Inverse Kinematics
- JSON Tool Schema
- Local LLM
- MediaRecorder
- Microsoft Agent Framework
- Model Benchmarking
- Multi Agent Pipeline
- Offline AI
- On Device Inference
- ONNX
- OpenAI Compatible API
- Phi 4 Mini
- PyBullet
- Python
- QNN NPU
- Qwen2.5 Coder 0.5b
- Robotics Simulation
- Safety Validation
- Speech To Text
- Structured Outputs
- Voice Commands
- Whisper
- Winget
external_url: https://techcommunity.microsoft.com/t5/microsoft-developer-community/building-real-world-ai-automation-with-foundry-local-and-the/ba-p/4501898
---

Lee_Stott walks through building an offline, on-device AI robotics simulator using Foundry Local and the Microsoft Agent Framework, where an LLM produces structured JSON plans that are validated for safety and executed in PyBullet—no cloud subscription, API keys, or internet required.<!--excerpt_end-->

## Overview

This guide shows how to build an offline AI automation pipeline that converts natural-language commands (text or voice) into safe, validated robot actions in a physics simulator.

Core idea:

- Run an LLM locally via **Foundry Local** (OpenAI-compatible API)
- Orchestrate multiple specialized agents with the **Microsoft Agent Framework**
- Execute validated actions in **PyBullet** (robotics physics simulation)
- Optionally accept **voice input** via a local Whisper model

The project is open source:

- Repository: https://github.com/leestott/robot-simulator-foundrylocal

## Why offline / on-device AI

Many AI demos depend on cloud endpoints. That’s fine for prototypes, but it introduces:

- Latency
- Ongoing token costs
- Data privacy and data residency concerns

For robotics and industrial automation, local inference can be a better fit because you can run where the hardware is (factory floor, lab, dev machine) without network dependency.

Foundry Local provides an OpenAI-compatible endpoint that runs entirely on-device:

- https://foundrylocal.ai

## Architecture

The system uses four specialised agents orchestrated by the Microsoft Agent Framework:

| Agent | What it does | Speed |
| --- | --- | --- |
| **PlannerAgent** | Sends user command to Foundry Local LLM → JSON action plan | 4–45 s |
| **SafetyAgent** | Validates against workspace bounds + schema | < 1 ms |
| **ExecutorAgent** | Dispatches actions to PyBullet (IK, gripper) | < 2 s |
| **NarratorAgent** | Template summary (LLM opt-in via env var) | < 1 ms |

High-level flow:

```text
User (text / voice)
  |
  v
Orchestrator
  |
  +--> Planner
  |
  +--> Narrator
  |
  v
Safety
  |
  v
Executor
  |
  v
PyBullet
```

Key design choice: the LLM never directly touches simulator APIs. It outputs structured JSON tool calls that are validated before execution.

## Setting up Foundry Local

Example Python usage with an OpenAI-compatible client:

```python
from foundry_local import FoundryLocalManager
import openai

manager = FoundryLocalManager("qwen2.5-coder-0.5b")

client = openai.OpenAI(
    base_url=manager.endpoint,
    api_key=manager.api_key,
)

resp = client.chat.completions.create(
    model=manager.get_model_info("qwen2.5-coder-0.5b").id,
    messages=[{"role": "user", "content": "pick up the cube"}],
    max_tokens=128,
    stream=True,
)
```

Hardware backend selection is automatic:

- CUDA GPU → QNN NPU → CPU

## How the LLM drives the simulator

### From words to JSON

When a user says “pick up the cube”, the PlannerAgent sends the command to the Foundry Local LLM with a system prompt that:

- Lists every permitted tool
- Specifies the required JSON format

Example structured plan:

```json
{
  "type": "plan",
  "actions": [
    {"tool": "describe_scene", "args": {}},
    {"tool": "pick", "args": {"object": "cube_1"}}
  ]
}
```

Planner behavior:

- Parses and validates JSON against the action schema
- Retries once if JSON is malformed

Why this helps small models:

- Constrained schemas reduce output entropy
- Even smaller models (0.5B params) can reliably produce correct tool JSON when the response space is narrow

### From JSON to motion

After SafetyAgent approves the plan, ExecutorAgent maps tool calls to PyBullet operations.

Actions described:

1. **`move_ee(target_xyz)`**
   - Sends a Cartesian target to PyBullet’s inverse kinematics solver
   - Solver returns joint angles (seven joints)
   - Robot interpolates smoothly while stepping physics simulation
2. **`pick(object)`**
   - Looks up object position
   - Moves above object, descends, closes gripper with configurable force, lifts
   - PyBullet resolves contacts and friction for realistic behavior
3. **`place(target_xyz)`**
   - Carries object to target and opens gripper
   - Physics engine drops/settles the object naturally
4. **`describe_scene()`**
   - Queries simulation state
   - Returns object names + poses and end-effector pose

### The abstraction boundary

The LLM operates only at a high level (tool calls like `pick`, `move_ee`). It does not know about:

- Joint angles
- Inverse kinematics details
- Physics simulation specifics

Benefits:

- Prompts remain simpler
- Safety layer can validate without understanding kinematics
- Executor can be swapped without retraining or re-prompting

## Voice input pipeline

Voice commands follow three stages:

1. **Browser capture**: `MediaRecorder` captures audio and client-side resamples to 16 kHz mono WAV
2. **Server transcription**: Foundry Local Whisper (ONNX, cached after first load) with automatic 30 s chunking
3. **Command execution**: transcribed text goes through the same Planner → Safety → Executor pipeline

UI detail:

- The mic button only appears when a Whisper model is cached or loaded
- Whisper models are filtered out of the LLM dropdown

## Performance: model choice matters

Reported timings:

| Model | Params | Inference | Pipeline total |
| --- | --- | --- | --- |
| `qwen2.5-coder-0.5b` | 0.5 B | ~4 s | ~5 s |
| `phi-4-mini` | 3.6 B | ~35 s | ~36 s |
| `qwen2.5-coder-7b` | 7 B | ~45 s | ~46 s |

Observation:

- For interactive robot control, `qwen2.5-coder-0.5b` is fastest while still producing valid JSON for a 7-tool schema.

## Get running in five minutes

### 1) Install Foundry Local

```bash
winget install Microsoft.FoundryLocal  # Windows
brew install foundrylocal              # macOS
```

### 2) Download models (cached locally)

```bash
foundry model run qwen2.5-coder-0.5b   # Chat brain (~4 s inference)
foundry model run whisper-base         # Voice input (194 MB)
```

### 3) Clone and set up

```bash
git clone https://github.com/leestott/robot-simulator-foundrylocal
cd robot-simulator-foundrylocal
.\setup.ps1   # or ./setup.sh on macOS/Linux
```

### 4) Launch the web UI

```bash
python -m src.app --web --no-gui  # → http://localhost:8080
```

Try commands:

- "pick up the cube"
- "describe the scene"
- "move to 0.3 0.2 0.5"
- "reset"

Voice mode uses a local Whisper model so audio stays on-device.

## Experiment ideas

### Add a new robot action

The robot understands seven tools. To add an eighth:

1. Define schema in `TOOL_SCHEMAS` (`src/brain/action_schema.py`)
2. Add a `_do_<tool>` handler in `src/executor/action_executor.py`
3. Register it in `ActionExecutor._dispatch`
4. Add a test in `tests/test_executor.py`

Example idea: `rotate_ee` for roll/pitch/yaw rotation without changing position.

### Add a new agent

Pattern: each agent has `async run(context)` with a shared dictionary.

Steps:

- Create a file in `src/agents/`
- Register it in `orchestrator.py`

Suggested agents:

- VisionAgent
- CostEstimatorAgent
- ExplanationAgent

### Swap the LLM

```bash
python -m src.app --web --model phi-4-mini
```

Notes:

- Smaller models are faster but may output malformed JSON more often
- Larger models are slower but more accurate
- Retry logic in the planner helps smooth over occasional formatting errors

### Swap the simulator

Alternatives mentioned:

- MuJoCo
- Isaac Sim
- Gazebo (ROS)

Requirement: implement the same interface as `PandaRobot` and `GraspController`.

### Apply the pattern elsewhere

The “LLM → structured JSON → safety validation → executor” pattern can be adapted to:

- Home automation (MQTT/Zigbee)
- Game AI
- CAD automation (OpenSCAD/FreeCAD)
- Lab instrumentation

## Moving from simulator to a real robot

### What stays the same

- Planner produces JSON plans without hardware knowledge
- Safety agent validates schema + workspace bounds (can be tightened)
- Orchestrator sequencing stays the same
- Narrator can summarize results regardless of executor

### What changes

Replace the executor layer:

- `PandaRobot` and `GraspController`
- Swap `_do_move_ee`, `_do_pick`, `_do_place` to call real drivers instead of PyBullet

Real Panda (Franka Emika) options mentioned:

- libfranka
- ROS 2 with MoveIt
- Franka ROS 2 driver

Key considerations:

- Safety: add real-time collision checking with sensors
- Perception: object detection / fiducials to locate targets
- Calibration: hand-eye calibration for coordinate frames
- Latency: wait for hardware motion completion vs simulation stepping
- Gripper feedback: use real force/torque signals

## Key takeaways

1. Foundry Local provides an OpenAI-compatible API for local inference; switching from cloud can be a one-line `base_url` change.
2. Small models can work well when constrained to structured JSON schemas.
3. Multi-agent design (plan/validate/execute/narrate) improves reliability and testability.
4. Simulation-first iteration reduces risk before moving to physical hardware.
5. The architecture generalizes beyond robotics.

## Links

- Foundry Local: https://foundrylocal.ai
- Repository: https://github.com/leestott/robot-simulator-foundrylocal

## Images (from the post)

- ![Robot Arm Simulator Architecture](https://raw.githubusercontent.com/leestott/robot-simulator-foundrylocal/main/docs/screenshots/architecture.png)
- ![Voice Pipeline: Speech to Robot Action](https://raw.githubusercontent.com/leestott/robot-simulator-foundrylocal/main/docs/screenshots/voice_pipeline.png)
- ![Pick Cube](https://raw.githubusercontent.com/leestott/robot-simulator-foundrylocal/main/docs/screenshots/app_pick.png)
- ![Describe Scene](https://raw.githubusercontent.com/leestott/robot-simulator-foundrylocal/main/docs/screenshots/app_describe.png)
- ![Move End-Effector](https://raw.githubusercontent.com/leestott/robot-simulator-foundrylocal/main/docs/screenshots/app_move.png)
- ![Reset Robot](https://raw.githubusercontent.com/leestott/robot-simulator-foundrylocal/main/docs/screenshots/app_reset.png)
- ![Simulator overview](https://raw.githubusercontent.com/leestott/robot-simulator-foundrylocal/main/docs/screenshots/01_overview.png)
- ![Reaching](https://raw.githubusercontent.com/leestott/robot-simulator-foundrylocal/main/docs/screenshots/02_reaching.png)
- ![Above the cube](https://raw.githubusercontent.com/leestott/robot-simulator-foundrylocal/main/docs/screenshots/03_above_cube.png)
- ![Gripper detail](https://raw.githubusercontent.com/leestott/robot-simulator-foundrylocal/main/docs/screenshots/04_gripper_detail.png)
- ![Front interaction](https://raw.githubusercontent.com/leestott/robot-simulator-foundrylocal/main/docs/screenshots/05_front_interaction.png)
- ![Side layout](https://raw.githubusercontent.com/leestott/robot-simulator-foundrylocal/main/docs/screenshots/06_side_layout.png)


[Read the entire article](https://techcommunity.microsoft.com/t5/microsoft-developer-community/building-real-world-ai-automation-with-foundry-local-and-the/ba-p/4501898)

