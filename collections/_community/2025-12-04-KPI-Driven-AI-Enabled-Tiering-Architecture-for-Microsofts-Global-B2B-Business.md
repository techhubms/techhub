---
external_url: https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/architecting-the-next-generation-customer-tiering-system/ba-p/4475326
title: KPI-Driven, AI-Enabled Tiering Architecture for Microsoft’s Global B2B Business
author: BonnieAo
feed_name: Microsoft Tech Community
date: 2025-12-04 21:00:36 +00:00
tags:
- Azure Stream Analytics
- Azure Synapse Analytics
- Business Analytics
- CatBoost
- Clustering
- Composite Score Optimization
- Dynamic Tiering
- FastAPI
- Gurobi
- Hybrid Segmentation
- KPI Segmentation
- LLMs
- Microsoft MCAPS
- OpenAI API
- Optimization
- OR Tools
- Pyomo
- Sales Planning
- Ward Clustering
- XGBoost
section_names:
- ai
- azure
- ml
---
Bonnie Ao and the UCLA Anderson MSBA team present a Microsoft-sponsored capstone on designing an AI-driven, KPI-optimized customer tiering framework, covering Azure-based analytics, ML, and operational constraints.<!--excerpt_end-->

# KPI-Driven, AI-Enabled Tiering Architecture for Microsoft’s Global B2B Business

**Authors:** Sailing Ni, Joy Yu, Peng Yang, Richard Sie, Yifei Wang  
**Affiliation:** UCLA Anderson School of Management, MSBA Class of December 2025  
**Project Leads:** Juhi Singh, Bonnie Ao (Microsoft MCAPS AI Transformation Office)

**Abstract**

Microsoft’s global B2B software business relies on customer tiering to guide sales coverage, investment, and strategic planning. Traditionally, these tiers have been defined by static rules and manual heuristics that lack statistical rigor, operational flexibility, and alignment with business KPIs. This capstone project, sponsored by Microsoft, engineered a scalable, interpretable, and KPI-driven segmentation architecture using Microsoft Azure’s analytics stack, AI/ML techniques, and LLM-powered interactive tooling.

## Legacy Limitations

- Tiers do not consistently represent customer growth or revenue importance.
- Business KPIs (TPA, TCI, SFI) are not optimized or systematically enforced.
- Imbalanced tier distributions arise from ad-hoc rules and manual adjustments.
- Coverage planning is disconnected from actual data structure and customer dynamics.

## Five-Layer Solution Architecture

1. **Natural Segmentation (Unsupervised Clustering)**: Used clustering algorithms (Ward, Weighted Ward, K-Medoids, K-Means, HDBSCAN) to reveal the intrinsic customer base structure. Ward (K=4) with 'Policy v2' was selected for statistical coherence and business alignment.
2. **Pure KPI-Based Tiering**: Built theoretical upper-bound models that assign tiers solely on KPIs. These showed maximal improvement but disrupted operational continuity—valuable for benchmarking, impractical for deployment.
3. **Hybrid KPI-Aware Segmentation (Recommended)**: Developed a hybrid model blending Ward clustering with KPI optimization (Composite Score = 0.35×TPA + 0.35×SFI + 0.30×[TCI_PI + TCI_REV]), plus business constraints (+1/–1 movement rule, stable distribution). Only ~5% of customers were impacted. The hybrid model offers interpretability (learnable rules/decision tree) and operational feasibility.
4. **Dynamic Tier Diagnostics**: Applied ML models (CatBoost, XGBoost, Neural Nets, MLR) to diagnose tier progressions over time. Found that tier transitions are rare and policy-driven, making ML forecasts unreliable for change but useful for surfacing risks and unusual behavior.
5. **Optimization & Resource Allocation**: Prototyped coverage planning using mixed-integer programming (MIP), Gurobi, OR-Tools, and Pyomo. Simulations validated that segmentation could drive real workload balancing and whitespace prioritization under Microsoft’s operational rules.

## LLM & Backend Integration

- Built a conversational tiering assistant using FastAPI (deployed on Render) and OpenAI LLMs to help business users articulate strategic priorities and dynamically update tier assignments through accessible natural language prompts.
- The backend validates, parses, and applies LLM outputs for real-time, schema-compliant optimization.

## Metrics & Success Criteria

- **Tier Potential Alignment (TPA):** Rank correlation between assigned tiers and account growth potential.
- **Tier Compactness Index (TCI):** Within-tier homogeneity (potential/revenue).
- **Strategic Focus Index (SFI):** Revenue share from top strategic tiers.
- **Composite Score:** Unified benchmark for optimizing all models.

## Key Technical Insights

- **Clustering algorithms:** Ward hierarchical merges yielded the best business-aligned segmentation. Weighted variants improved KPIs but hurt geometric stability.
- **Hybrid optimization:** Local search with strict movement constraints balanced KPI uplift and operational feasibility.
- **Dynamic diagnostics:** ML models excel at surfacing outlier or risk accounts, not predicting tier moves.
- **LLM explainability:** Natural-language interfaces facilitate business-driven modeling, versatile for analyst and sales teams.
- **Azure Analytics stack:** Core data processing and modeling pipelines run on Azure Synapse Analytics and Azure Stream Analytics.
- **Open-source optimization:** Gurobi, Pyomo, and OR-Tools enable flexible scenario planning under business constraints.

## Limitations & Future Development

- Full optimization engine not yet ready for deployment (due to time and operational data limits).
- Dynamic modeling best suited for diagnostics, not forecasting tier changes.
- Framework can extend with behavioral signals, market features, and refined tier-level KPIs.

## Conclusion & Recommendations

This architecture transforms Microsoft’s tiering approach from static, heuristic assignments to a scalable, AI-powered KPI framework. Benefits include:

- Data-driven investment, coverage, and resource planning.
- Operationally stable segmentation with explainable movement constraints.
- Real-time adjustment via LLM-based assistant and Azure cloud workflows.
- Technical blueprint for continuous improvement toward Microsoft’s AI strategy and global growth.

> [DEMO: Microsoft x UCLA Anderson – AI-Driven KPI Segmentation Project (LLM demo)](https://youtu.be/5F8M3T-bMwA)

For details, see full methodology and architecture visuals in referenced Capstone materials.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/architecting-the-next-generation-customer-tiering-system/ba-p/4475326)
