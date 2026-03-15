---
external_url: https://techcommunity.microsoft.com/t5/microsoft-developer-community/phi-4-reasoning-vision-15b-use-cases-in-depth/ba-p/4499210
title: 'Phi-4-Reasoning-Vision-15B: In-Depth Overview and Use Cases'
author: kinfey
primary_section: ai
feed_name: Microsoft Tech Community
date: 2026-03-04 18:00:00 +00:00
tags:
- AI
- Azure
- Azure AI Foundry
- Bounding Box Extraction
- Chart Analysis
- Code Sample
- Community
- Deep Learning
- Dynamic Inference
- GUI Automation
- Hybrid Reasoning
- Mathematical Reasoning
- Microsoft Foundry
- Multimodal AI
- Phi 4 Reasoning Vision 15B
- Python
- Scientific Visual Reasoning
- SLM
- Vision Models
- .NET
section_names:
- ai
- azure
- dotnet
---
kinfey explores Phi-4-Reasoning-Vision-15B, Microsoft's new vision reasoning SLM. The article provides developers with detailed design analysis, example code, and real-world applications for visual understanding and actionable decision-making.<!--excerpt_end-->

# Phi-4-Reasoning-Vision-15B: In-Depth Overview and Use Cases

**Author:** kinfey

## Introduction

Phi-4-Reasoning-Vision-15B is Microsoft's newest small language model (SLM) focused on vision reasoning, available via Microsoft Foundry. Unlike traditional visual perception models that only recognize objects, this model enables both high-resolution visual perception and deep, structured reasoning, letting it "see clearly" and "think deeply," even with a relatively compact 15B parameter size.

## Core Design Features

### Selective Reasoning

- **Hybrid reasoning behavior:** Model switches between deep reasoning and shallow perception based on user prompts, optimizing either for accuracy or fast inference.
- Developers control the level of reasoning by setting the `thinking_mode` parameter:
  - **hybrid:** The model autonomously selects the appropriate reasoning mode.
  - **think:** Forces a full reasoning chain for complex problems.
  - **nothink:** Minimizes reasoning for low-latency perception tasks.

#### Sample Code Implementation

```python
def run_inference(processor, model, prompt, image, thinking_mode="hybrid"):
    messages = [{"role": "user", "content": prompt}]
    prompt = processor.tokenizer.apply_chat_template(messages, tokenize=False, add_generation_prompt=True, return_dict=False)
    if thinking_mode == "think":
        prompt = str(prompt) + "<think>"
    elif thinking_mode == "nothink":
        prompt = str(prompt) + "<|dummy_84|>"
    print(f"Prompt: {prompt}")
    inputs = processor(text=prompt, images=[image], return_tensors="pt").to(model.device)
    output_ids = model.generate(**inputs, max_new_tokens=1024, temperature=None, top_p=None, do_sample=False, use_cache=False)
    sequence_length = inputs["input_ids"].shape[1]
    sequence_length -= 1 if thinking_mode == "think" else 0
    new_output_ids = output_ids[:, sequence_length:]
    model_output = processor.batch_decode(new_output_ids, skip_special_tokens=True, clean_up_tokenization_spaces=False)[0]
    return model_output
```

This design allows runtime adjustment between latency and accuracy, making it a practical fit for a broad range of interactive applications.

## Key Use Cases

### 1. GUI Agents (Computer Use Agents)

- **Input:** UI screenshot and a natural-language instruction.
- **Output:** Normalized bounding box coordinates for UI elements the user wants to interact with.
- Example: E-commerce bots that locate products, prices, and controls for automation tasks.

### 2. Mathematical and Scientific Visual Reasoning

- Interprets geometric figures, charts, and function graphs to help solve math and science problems.
- Use in educational applications: Students upload problem images, the model provides step-by-step reasoning.

### 3. Document, Chart, and Table Understanding

- **IT Operations:** Dashboard and report interpretation for troubleshooting.
- **Financial Analysis:** Extracts key data from screenshots, enabling number/crunching and trend spotting.
- **Enterprise Automation:** Processes scanned documents or tables to produce structured outputs for downstream automation.

## Model Performance

Phi-4-Reasoning-Vision-15B is benchmarked against other vision models, showing significant advantages especially in math reasoning and actionable GUI grounding, while still providing competitive multimodal understanding in general tasks.

## Example Resources

- [Jaywalking detection sample](https://github.com/microsoft/PhiCookBook/blob/main/md/02.Application/10.ReasoningVision/Phi_4_reasoning_vision_15b_Jaywalking.ipynb)
- [Math problem solving sample](https://github.com/microsoft/PhiCookBook/blob/main/md/02.Application/10.ReasoningVision/Phi_4_reasoning_vision_15b_Math.ipynb)
- [GUI agent bounding box sample](https://github.com/microsoft/PhiCookBook/blob/main/md/02.Application/10.ReasoningVision/Phi_4_reasoning_vision_15b_ui.ipynb)

## Summary

Phi-4-Reasoning-Vision-15B makes top-tier vision reasoning available in a deployable SLM size, letting developers build:

- E-commerce shopping agents
- IT operations automation tools
- Educational step-by-step tutors
- Chart/figure interpreters
- Interactive UI automation bots

**Key takeaways:**

- Flexible reasoning control via modes (`hybrid`, `think`, `nothink`)
- Strong real-time performance with trade-off options for accuracy vs. speed
- Code samples available for immediate developer experimentation

## References

- [Phi-4-Reasoning-Vision official Blog](https://www.microsoft.com/en-us/research/?p=1163159&preview=1&_ppp=5ad3787055)
- [Phi-4-Reasoning-Vision-15B at Huggingface](https://huggingface.co/microsoft/Phi-4-reasoning-vision-15B)
- [Microsoft Phi CookBook](https://aka.ms/PhiCookbook)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/microsoft-developer-community/phi-4-reasoning-vision-15b-use-cases-in-depth/ba-p/4499210)
