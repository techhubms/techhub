---
external_url: https://www.reddit.com/r/MachineLearning/comments/1miev16/p_from_business_processes_to_gnn_for_next/
title: Structuring a GNN-Based Next Activity Prediction for Business Process Mining
author: Street_Car_1297
feed_name: Reddit Machine Learning
date: 2025-08-05 17:05:11 +00:00
tags:
- Batch Processing
- Business Process Analysis
- CSV
- GCN
- GNN Training
- Graph Neural Networks
- Graph Representation
- MachineLearning
- Model Architecture
- Next Activity Prediction
- Node Classification
- Prefix Graph
- Process Instance
- Process Mining
- Python
- Machine Learning
- Community
section_names:
- ml
primary_section: ml
---
Street_Car_1297 seeks community input on designing a GNN solution for next activity prediction in business process mining, sharing project structure, dataset specifics, and open modeling questions.<!--excerpt_end-->

# Structuring a GNN-Based Next Activity Prediction for Business Process Mining

*Posted by Street_Car_1297*

This post discusses the early stages of a project aimed at using Graph Neural Networks (GNNs) to predict the next event in business process data, specifically helpdesk support traces stored in CSV files.

## Project and Data Overview

- **Goal:** Build a model for next activity prediction at the node level using graph representations of business processes.
- **Input Data:** A CSV representing process traces—each trace becomes a graph (a Directly-Follows Graph) where nodes represent events or activities.
- **Dataset Size:** 4,580 separate process instances (graphs) with an average of 7 nodes each and 15 total activity types (labels).

## Core Modeling Questions

1. **Model Architecture**: Considering a 3-layer Graph Convolutional Network (GCN), but open to community suggestions for architectures better suited to sequence-based node prediction within process graphs.
2. **Handling Multiple Instances**: Unsure whether to treat 4,580 traces as separate graphs for training or to merge them into a single large graph while preserving per-node instance information. Also seeking advice on batching strategies for GNNs with many small graphs versus constructing a global graph for training.

## Additional Context

- The use case involves learning to predict the next activity in what is essentially a sequence, but modeled as a graph structure to capture more complex dependencies than standard sequence models.
- The author notes a lack of practical examples or published studies directly matching this scenario, highlighting both the novelty and challenge of the task.

## Community Collaboration

- Street_Car_1297 encourages feedback, especially from those experienced in GNNs or process data, looking for recommendations on model structure, data batching, and best practices for graph-based activity prediction.

---

**Key Takeaways:**

- Focused on the technical modeling of business process data using GNNs
- Explicitly mentions challenges around architecture and multi-instance data handling
- Targeted at practitioners interested in advanced machine learning techniques for process mining

This post appeared first on "Reddit Machine Learning". [Read the entire article here](https://www.reddit.com/r/MachineLearning/comments/1miev16/p_from_business_processes_to_gnn_for_next/)
