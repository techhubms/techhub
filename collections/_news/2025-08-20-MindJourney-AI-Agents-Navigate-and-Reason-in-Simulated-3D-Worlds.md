---
external_url: https://www.microsoft.com/en-us/research/blog/mindjourney-enables-ai-to-explore-simulated-3d-worlds-to-improve-spatial-interpretation/
title: 'MindJourney: AI Agents Navigate and Reason in Simulated 3D Worlds'
author: sbaynes
feed_name: Microsoft News
date: 2025-08-20 21:00:30 +00:00
tags:
- 3D Simulation
- AI Agents
- AI Evaluation
- Autonomous Systems
- Company News
- Computer Vision
- Microsoft Research
- MindJourney
- Simulation
- Spatial Beam Search
- Spatial Reasoning
- Vision Language Models
- VLM
- World Models
section_names:
- ai
---
Authored by sbaynes, this news feature explains how MindJourney—a research framework from Microsoft—enables AI agents to navigate simulated 3D worlds and enhances their ability to answer spatial questions.<!--excerpt_end-->

# MindJourney: AI Agents Navigate and Reason in Simulated 3D Worlds

*by sbaynes*

![MindJourney visual summary](https://www.microsoft.com/en-us/research/wp-content/uploads/2025/08/ImprovingImagination-BlogHeroFeature-1400x788-1.jpg)

## Overview

MindJourney is a research framework developed at Microsoft to empower AI agents with the ability to explore and understand three-dimensional environments that they cannot directly sense. The main objective is to improve how vision-language models (VLMs) respond to spatial reasoning tasks by simulating movement and perspective shifts—much like human imagination.

## The Challenge with Vision-Language Models

Traditional VLMs excel at identifying objects in static 2D images. However, they struggle with spatial reasoning—questions that require understanding an environment from multiple views, such as “If I face the chairs while sitting on the couch, where’s the kitchen?” Without the capacity for mental simulation, VLMs find it difficult to deduce relationships that extend beyond the visible frame.

## How MindJourney Works

### World Model-Based Exploration

MindJourney uses a pre-trained world model (e.g., a video-generation system trained on first-person video data) to simulate possible movements—such as turning or moving forward—from a starting point. For each candidate movement, the model generates photo-realistic images that depict what an agent would observe from these new perspectives.

### Spatial Beam Search

The system uses a ‘spatial beam search’ technique to efficiently scan the most promising movement sequences without evaluating every possible path. At each step, the VLM assesses generated scenes, discarding less useful views and expanding on the most promising ones. This loop continues for several steps, building a collection of simulated perspectives tailored to answer the original spatial question.

![Spatial reasoning with MindJourney](https://www.microsoft.com/en-us/research/wp-content/uploads/2025/08/MindJourney-fig1.jpg)

### Example Workflow

1. **Receive a spatial query** (e.g., "Where is the kitchen if I move here?")
2. **World model generates possible views** for agent movements.
3. **VLM filters the generated images** for informative perspectives.
4. **Spatial beam search prioritizes and expands** the best options.
5. **Iterative process** continues until the system compiles sufficient evidence to answer the question.

![MindJourney pipeline](https://www.microsoft.com/en-us/research/wp-content/uploads/2025/08/MindJourney_pipeline_1400x788.jpg)

## Results and Applications

- **Performance**: MindJourney increased VLM accuracy by 8% on the Spatial Aptitude Training (SAT) benchmark compared to baseline.
- **Integration**: Works with existing pre-trained VLMs and trainable world models without additional retraining.
- **Applications**: Potential uses include robotics, smart home devices, and accessibility (e.g., aiding visually impaired users with spatial questions).

## Future Directions

Future work aims to:

- Extend world models to predict not only new viewpoints but also future scene changes.
- Enable agents to actively plan without real-world trial and error, improving safety and efficiency.
- Advance the connection between computer vision and sequential planning.

## Related Content

**Podcast:** [AI Testing and Evaluation: Learnings from Science and Industry](https://www.microsoft.com/en-us/research/story/ai-testing-and-evaluation-learnings-from-science-and-industry/)

Learn how Microsoft leverages cross-disciplinary insights to enhance AI testing and evaluation as part of responsible AI development.

---

For further details, read the research paper: [MindJourney: Test-time Scaling with World Models for Spatial Reasoning](https://www.microsoft.com/en-us/research/publication/mindjourney-test-time-scaling-with-world-models-for-spatial-reasoning/)

This post appeared first on "Microsoft News". [Read the entire article here](https://www.microsoft.com/en-us/research/blog/mindjourney-enables-ai-to-explore-simulated-3d-worlds-to-improve-spatial-interpretation/)
