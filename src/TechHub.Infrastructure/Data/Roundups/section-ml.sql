-- Section-specific roundup content: ml
-- Generated: 2026-05-19 11:07
-- Heading aliases matched: Machine Learning, ML
-- Updates backfilled duplicates so each roundup only contains ml content.
-- Usage: psql -U techhub -d techhub -f section-ml.sql

-- SKIP 2025-07-07 : no 'Machine Learning/ML' block found in source (weekly-ai-roundup-2025-07-07)

-- SKIP 2025-07-14 : no 'Machine Learning/ML' block found in source (weekly-ai-roundup-2025-07-14)

-- SKIP 2025-07-21 : no 'Machine Learning/ML' block found in source (weekly-ai-roundup-2025-07-21)

-- SKIP 2025-07-28 : no 'Machine Learning/ML' block found in source (weekly-ai-roundup-2025-07-28)

-- SKIP 2025-08-04 : no 'Machine Learning/ML' block found in source (weekly-ai-roundup-2025-08-04)

-- SKIP 2025-08-11 : no 'Machine Learning/ML' block found in source (weekly-ai-roundup-2025-08-11)

-- weekly-ml-roundup-2025-08-18  (2025-08-18)
UPDATE content_items
SET  content      = 'Machine learning updates this week include expanded cloud support for open models, step-by-step LLM deployment, scalable optimizers, and upgraded analytics tools. Microsoft introduced more open-source, cloud-native, and production-friendly options, while new tools like Dion are making large model training more efficient. Companies are highlighting useful deployment strategies and tuning guidance so teams can deliver quality ML systems with less friction.

<!--excerpt_end-->

## ML

### Cloud-Native LLM Deployment and Optimization

A new, comprehensive guide walks through deploying OpenAI’s GPT-OSS-20B model on Azure Kubernetes Service (AKS) with KAITO and vLLM, using managed GPUs for scalable and reproducible inference. The tutorial covers everything from setting up clusters to benchmarking, making it easier for teams to roll out open LLMs in Azure environments.

- [Deploying OpenAI’s GPT-OSS-20B on Azure AKS with KAITO and vLLM](https://techcommunity.microsoft.com/t5/ai-machine-learning-blog/deploying-openai-s-first-open-source-model-on-azure-aks-with/ba-p/4444234)

### Innovations in Data Lake Interoperability

Microsoft Fabric''s OneLake now lets you access Delta Lake tables as Apache Iceberg format using Apache XTable. This enables analytics engines such as Spark, Trino, or Snowflake to work with lake data without ETL or duplication, advancing Microsoft’s vision for a more flexible, open lakehouse platform.

- [How Microsoft OneLake Seamlessly Provides Apache Iceberg Support for All Fabric Data](https://blog.fabric.microsoft.com/en-US/blog/how-to-access-your-microsoft-fabric-tables-in-apache-iceberg-format/)

### Advances in Distributed Optimization for AI Model Training

Microsoft Research introduced Dion, a distributed optimizer for training massive models like LLaMA-3 405B. Dion leverages orthonormal updates to make optimizer steps up to 10x faster while preserving accuracy, and works well with distributed training frameworks such as FSDP2 and tensor parallelism.

- [Microsoft Releases Dion: A New Scalable Optimizer for Training AI Models](https://www.microsoft.com/en-us/research/blog/dion-the-distributed-orthonormal-update-revolution-is-here/)

### Practical Data Engineering and Analytics Platform Enhancements

A deep dive into the Spark UI offers practical advice for improving job run times, fixing data skew and joins, and spotting garbage collection issues—especially for Databricks users seeking to move past trial-and-error tuning.

- [A Deep Dive into Spark UI for Job Optimization](https://techcommunity.microsoft.com/t5/microsoft-mission-critical-blog/a-deep-dive-into-spark-ui-for-job-optimization/ba-p/4442229)

Microsoft Fabric''s Copy Job feature now supports table-level incremental resets, automatic destination table creation, and JSON files—streamlining ETL pipeline deployment by reducing manual steps.

- [Enhancements to Microsoft Fabric Copy Job: Reset Incremental Copy, Auto Table Creation, and JSON Support](https://blog.fabric.microsoft.com/en-US/blog/simplifying-data-ingestion-with-copy-job-reset-incremental-copy-auto-table-creation-and-json-format-support/)

Azure Essentials Show featured Databricks, highlighting unified analytics, ML lifecycle support, and integration across the Azure platform—useful for developers building new skills for Azure ML environments.

- [Supercharge Data and AI Innovation with Azure Databricks](/ai/videos/supercharge-data-and-ai-innovation-with-azure-databricks)

### Enterprise ML Transformation and Modern DataOps

A case study from Adastra and Heritage Grocers Group illustrates how Microsoft Fabric and Azure OpenAI unified post-acquisition data, powered predictive analytics, and rolled out a working system in just six months, showing real benefits from a modern, cloud-based ML setup.

- [How Adastra Used Microsoft Fabric and Azure OpenAI Service to Transform Heritage Grocers Group’s Data Analytics](https://techcommunity.microsoft.com/t5/partner-news/partner-case-study-adastra/ba-p/4442288)

### Other ML News

Excel’s 40th anniversary content showcases its transformation into a capable platform for analytics and ML, including expanded modeling support, Power BI linkage, and deeper connection to Microsoft Fabric.

- [Excel at 40 Week 1: Days 1–3](https://techcommunity.microsoft.com/t5/excel/excel-at-40-week-1-days-1-3/m-p/4443674#M254078)',
     excerpt      = 'Machine learning updates this week include expanded cloud support for open models, step-by-step LLM deployment, scalable optimizers, and upgraded analytics tools. Microsoft introduced more open-source, cloud-native, and production-friendly options, while new tools like Dion are making large model training more efficient. Companies are highlighting useful deployment strategies and tuning guidance so teams can deliver quality ML systems with less friction.',
     content_hash = md5('Machine learning updates this week include expanded cloud support for open models, step-by-step LLM deployment, scalable optimizers, and upgraded analytics tools. Microsoft introduced more open-source, cloud-native, and production-friendly options, while new tools like Dion are making large model training more efficient. Companies are highlighting useful deployment strategies and tuning guidance so teams can deliver quality ML systems with less friction.

<!--excerpt_end-->

## ML

### Cloud-Native LLM Deployment and Optimization

A new, comprehensive guide walks through deploying OpenAI’s GPT-OSS-20B model on Azure Kubernetes Service (AKS) with KAITO and vLLM, using managed GPUs for scalable and reproducible inference. The tutorial covers everything from setting up clusters to benchmarking, making it easier for teams to roll out open LLMs in Azure environments.

- [Deploying OpenAI’s GPT-OSS-20B on Azure AKS with KAITO and vLLM](https://techcommunity.microsoft.com/t5/ai-machine-learning-blog/deploying-openai-s-first-open-source-model-on-azure-aks-with/ba-p/4444234)

### Innovations in Data Lake Interoperability

Microsoft Fabric''s OneLake now lets you access Delta Lake tables as Apache Iceberg format using Apache XTable. This enables analytics engines such as Spark, Trino, or Snowflake to work with lake data without ETL or duplication, advancing Microsoft’s vision for a more flexible, open lakehouse platform.

- [How Microsoft OneLake Seamlessly Provides Apache Iceberg Support for All Fabric Data](https://blog.fabric.microsoft.com/en-US/blog/how-to-access-your-microsoft-fabric-tables-in-apache-iceberg-format/)

### Advances in Distributed Optimization for AI Model Training

Microsoft Research introduced Dion, a distributed optimizer for training massive models like LLaMA-3 405B. Dion leverages orthonormal updates to make optimizer steps up to 10x faster while preserving accuracy, and works well with distributed training frameworks such as FSDP2 and tensor parallelism.

- [Microsoft Releases Dion: A New Scalable Optimizer for Training AI Models](https://www.microsoft.com/en-us/research/blog/dion-the-distributed-orthonormal-update-revolution-is-here/)

### Practical Data Engineering and Analytics Platform Enhancements

A deep dive into the Spark UI offers practical advice for improving job run times, fixing data skew and joins, and spotting garbage collection issues—especially for Databricks users seeking to move past trial-and-error tuning.

- [A Deep Dive into Spark UI for Job Optimization](https://techcommunity.microsoft.com/t5/microsoft-mission-critical-blog/a-deep-dive-into-spark-ui-for-job-optimization/ba-p/4442229)

Microsoft Fabric''s Copy Job feature now supports table-level incremental resets, automatic destination table creation, and JSON files—streamlining ETL pipeline deployment by reducing manual steps.

- [Enhancements to Microsoft Fabric Copy Job: Reset Incremental Copy, Auto Table Creation, and JSON Support](https://blog.fabric.microsoft.com/en-US/blog/simplifying-data-ingestion-with-copy-job-reset-incremental-copy-auto-table-creation-and-json-format-support/)

Azure Essentials Show featured Databricks, highlighting unified analytics, ML lifecycle support, and integration across the Azure platform—useful for developers building new skills for Azure ML environments.

- [Supercharge Data and AI Innovation with Azure Databricks](/ai/videos/supercharge-data-and-ai-innovation-with-azure-databricks)

### Enterprise ML Transformation and Modern DataOps

A case study from Adastra and Heritage Grocers Group illustrates how Microsoft Fabric and Azure OpenAI unified post-acquisition data, powered predictive analytics, and rolled out a working system in just six months, showing real benefits from a modern, cloud-based ML setup.

- [How Adastra Used Microsoft Fabric and Azure OpenAI Service to Transform Heritage Grocers Group’s Data Analytics](https://techcommunity.microsoft.com/t5/partner-news/partner-case-study-adastra/ba-p/4442288)

### Other ML News

Excel’s 40th anniversary content showcases its transformation into a capable platform for analytics and ML, including expanded modeling support, Power BI linkage, and deeper connection to Microsoft Fabric.

- [Excel at 40 Week 1: Days 1–3](https://techcommunity.microsoft.com/t5/excel/excel-at-40-week-1-days-1-3/m-p/4443674#M254078)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'ml'
  AND slug                 = 'weekly-ml-roundup-2025-08-18';

-- weekly-ml-roundup-2025-08-25  (2025-08-25)
UPDATE content_items
SET  content      = 'This week’s machine learning news focuses on practical improvements to scaling model training and agent development. Developers now have guides for optimizing pretraining at scale, new research on agent reasoning, and updates to data engineering workflows that simplify testing and iteration. The overall direction continues to support efficient ML development and deployment.

<!--excerpt_end-->

## ML

### Large-Scale AI Pretraining Optimization on Azure ND GB200 v6

Building on last week’s discussions around cloud-based model training, this week’s benchmarking research provides detailed recommendations for optimizing the pretraining of Llama3 8B models on Azure ND GB200 v6. The study covers adjustments to tensor, pipeline, context, and data parallelism, repeating last week’s strategies for deploying scalable workloads using Azure AKS and vLLM. Benchmarking batch sizes and numerical precision modes, the authors recommend specific parameters for the best throughput: tensor parallelism 1, pipeline parallelism 2, context parallelism 1, and micro batch size 4.

All scripts are shared for reproducibility via the Azure AI Benchmarking Guide, supporting transparent scaling and tuning for teams running production ML on large clusters.

- [Optimizing Large-Scale AI Performance with Pretraining Validation on a Single Azure ND GB200 v6](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/optimizing-large-scale-ai-performance-with-pretraining/ba-p/4445273)

### Feature Updates: Enhanced AI Capability and Developer Workflow

Following on recent analytics and optimizer updates, MindJourney—developed by Microsoft Research—improves spatial reasoning for agents in dynamic, simulated environments. Integrating a pretrained world model and spatial beam search, MindJourney improves agent navigation and accuracy by 8% without requiring agent retraining, with clear uses in robotics, simulation, and accessibility development.

Microsoft Fabric’s new “Develop mode” for User Data Functions now provides a safe editor for testing Python logic before production deployment. This is a direct response to calls for safer, more controlled custom code testing in platforms like Spark, Databricks, and Fabric, and only requires a library update to enable.

- [MindJourney: AI Agents Navigate and Reason in Simulated 3D Worlds](https://www.microsoft.com/en-us/research/blog/mindjourney-enables-ai-to-explore-simulated-3d-worlds-to-improve-spatial-interpretation/)
- [Test and Validate Functions with Develop Mode in Fabric User Data Functions](https://blog.fabric.microsoft.com/en-US/blog/test-and-validate-your-functions-with-develop-mode-in-fabric-user-data-functions-preview/)',
     excerpt      = 'This week’s machine learning news focuses on practical improvements to scaling model training and agent development. Developers now have guides for optimizing pretraining at scale, new research on agent reasoning, and updates to data engineering workflows that simplify testing and iteration. The overall direction continues to support efficient ML development and deployment.',
     content_hash = md5('This week’s machine learning news focuses on practical improvements to scaling model training and agent development. Developers now have guides for optimizing pretraining at scale, new research on agent reasoning, and updates to data engineering workflows that simplify testing and iteration. The overall direction continues to support efficient ML development and deployment.

<!--excerpt_end-->

## ML

### Large-Scale AI Pretraining Optimization on Azure ND GB200 v6

Building on last week’s discussions around cloud-based model training, this week’s benchmarking research provides detailed recommendations for optimizing the pretraining of Llama3 8B models on Azure ND GB200 v6. The study covers adjustments to tensor, pipeline, context, and data parallelism, repeating last week’s strategies for deploying scalable workloads using Azure AKS and vLLM. Benchmarking batch sizes and numerical precision modes, the authors recommend specific parameters for the best throughput: tensor parallelism 1, pipeline parallelism 2, context parallelism 1, and micro batch size 4.

All scripts are shared for reproducibility via the Azure AI Benchmarking Guide, supporting transparent scaling and tuning for teams running production ML on large clusters.

- [Optimizing Large-Scale AI Performance with Pretraining Validation on a Single Azure ND GB200 v6](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/optimizing-large-scale-ai-performance-with-pretraining/ba-p/4445273)

### Feature Updates: Enhanced AI Capability and Developer Workflow

Following on recent analytics and optimizer updates, MindJourney—developed by Microsoft Research—improves spatial reasoning for agents in dynamic, simulated environments. Integrating a pretrained world model and spatial beam search, MindJourney improves agent navigation and accuracy by 8% without requiring agent retraining, with clear uses in robotics, simulation, and accessibility development.

Microsoft Fabric’s new “Develop mode” for User Data Functions now provides a safe editor for testing Python logic before production deployment. This is a direct response to calls for safer, more controlled custom code testing in platforms like Spark, Databricks, and Fabric, and only requires a library update to enable.

- [MindJourney: AI Agents Navigate and Reason in Simulated 3D Worlds](https://www.microsoft.com/en-us/research/blog/mindjourney-enables-ai-to-explore-simulated-3d-worlds-to-improve-spatial-interpretation/)
- [Test and Validate Functions with Develop Mode in Fabric User Data Functions](https://blog.fabric.microsoft.com/en-US/blog/test-and-validate-your-functions-with-develop-mode-in-fabric-user-data-functions-preview/)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'ml'
  AND slug                 = 'weekly-ml-roundup-2025-08-25';

-- weekly-ml-roundup-2025-09-01  (2025-09-01)
UPDATE content_items
SET  content      = 'Machine learning updates focus on better LLM inference performance, improvements to cloud productivity, and clear guidance for teams deploying large-scale solutions. Insights include Azure GPU benchmarking for model throughput, real-world diagnostics, and new analytics features in Microsoft Fabric.

<!--excerpt_end-->

## ML

### Llama 3.1 8B and DeepSeek R1: Azure GPU Inference Analysis

Following earlier coverage on LLM pretraining optimizations, this week’s benchmarks examine Meta’s Llama 3.1 8B and DeepSeek R1 using Azure ND-H100-v5 GPUs and vLLM. The analysis shows how optimizations like quantization and parallel processing yield throughput improvements of over 38%, and includes comparisons across Azure ND-series hardware for speed, cost, and scalability. DeepSeek R1 is effective for complex tasks, but slower and less cost-efficient than lighter models—helping teams choose the right model for their needs.

- [Benchmarking Llama 3.1 8B AI Inference on Azure ND-H100-v5 with vLLM](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/performance-of-llama-3-1-8b-ai-inference-using-vllm-on-nd-h100/ba-p/4448355)
- [Benchmarking Llama 3.1 8B Inference with vLLM on Azure GPU and CPU VMs](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/inference-performance-of-llama-3-1-8b-using-vllm-across-various/ba-p/4448420)
- [Performance Analysis: DeepSeek R1 Inference with vLLM on Azure ND-H100-v5](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/performance-analysis-of-deepseek-r1-ai-inference-using-vllm-on/ba-p/4449351)

### Productivity and Monitoring Advances in Microsoft Fabric

Microsoft Fabric now offers Fabric Notebooks with direct Pandas DataFrame handling via Apache Arrow, boosting workflow speed and memory efficiency. Monitoring and troubleshooting advances include improved mapping, granular log filtering, and execution snapshots for Spark workloads. The new JobInsight library provides diagnostics and historical analysis, automating insight generation for analytics pipelines.

- [Enhancing Fabric Notebooks: Native Pandas DataFrame Support in User Data Functions](https://blog.fabric.microsoft.com/en-US/blog/now-in-fabric-notebook-udf-integration-with-native-support-for-pandas-dataframes-and-series-via-apache-arrow/)
- [Enhanced Monitoring for Spark High Concurrency Workloads in Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/enhanced-monitoring-for-spark-high-concurrency-workloads-in-microsoft-fabric/)
- [Gain Deeper Insights into Spark Jobs with JobInsight in Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/gain-deeper-insights-into-spark-jobs-with-jobinsight-in-microsoft-fabric/)

### Practical Fabric Data Engineering: Materialized Lake Views, Community Best Practices

Guides showcase effective Fabric pipeline operations, spotlighting Materialized Lake Views for syncing Azure SQL to OneLake and detailing layered data transformations. Tutorials from Microsoft MVPs and Super Users cover dynamic masking, Power BI, REST admin, Pandas analysis, and efficient transformation patterns, with tips for troubleshooting and certification.

- [Mastering Declarative Data Transformations with Materialized Lake Views](https://blog.fabric.microsoft.com/en-US/blog/mastering-declarative-data-transformations-with-materialized-lake-views/)
- [Fabric Influencers Spotlight: August 2025 Edition](https://blog.fabric.microsoft.com/en-US/blog/fabric-influencers-spotlight-august-2025/)',
     excerpt      = 'Machine learning updates focus on better LLM inference performance, improvements to cloud productivity, and clear guidance for teams deploying large-scale solutions. Insights include Azure GPU benchmarking for model throughput, real-world diagnostics, and new analytics features in Microsoft Fabric.',
     content_hash = md5('Machine learning updates focus on better LLM inference performance, improvements to cloud productivity, and clear guidance for teams deploying large-scale solutions. Insights include Azure GPU benchmarking for model throughput, real-world diagnostics, and new analytics features in Microsoft Fabric.

<!--excerpt_end-->

## ML

### Llama 3.1 8B and DeepSeek R1: Azure GPU Inference Analysis

Following earlier coverage on LLM pretraining optimizations, this week’s benchmarks examine Meta’s Llama 3.1 8B and DeepSeek R1 using Azure ND-H100-v5 GPUs and vLLM. The analysis shows how optimizations like quantization and parallel processing yield throughput improvements of over 38%, and includes comparisons across Azure ND-series hardware for speed, cost, and scalability. DeepSeek R1 is effective for complex tasks, but slower and less cost-efficient than lighter models—helping teams choose the right model for their needs.

- [Benchmarking Llama 3.1 8B AI Inference on Azure ND-H100-v5 with vLLM](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/performance-of-llama-3-1-8b-ai-inference-using-vllm-on-nd-h100/ba-p/4448355)
- [Benchmarking Llama 3.1 8B Inference with vLLM on Azure GPU and CPU VMs](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/inference-performance-of-llama-3-1-8b-using-vllm-across-various/ba-p/4448420)
- [Performance Analysis: DeepSeek R1 Inference with vLLM on Azure ND-H100-v5](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/performance-analysis-of-deepseek-r1-ai-inference-using-vllm-on/ba-p/4449351)

### Productivity and Monitoring Advances in Microsoft Fabric

Microsoft Fabric now offers Fabric Notebooks with direct Pandas DataFrame handling via Apache Arrow, boosting workflow speed and memory efficiency. Monitoring and troubleshooting advances include improved mapping, granular log filtering, and execution snapshots for Spark workloads. The new JobInsight library provides diagnostics and historical analysis, automating insight generation for analytics pipelines.

- [Enhancing Fabric Notebooks: Native Pandas DataFrame Support in User Data Functions](https://blog.fabric.microsoft.com/en-US/blog/now-in-fabric-notebook-udf-integration-with-native-support-for-pandas-dataframes-and-series-via-apache-arrow/)
- [Enhanced Monitoring for Spark High Concurrency Workloads in Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/enhanced-monitoring-for-spark-high-concurrency-workloads-in-microsoft-fabric/)
- [Gain Deeper Insights into Spark Jobs with JobInsight in Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/gain-deeper-insights-into-spark-jobs-with-jobinsight-in-microsoft-fabric/)

### Practical Fabric Data Engineering: Materialized Lake Views, Community Best Practices

Guides showcase effective Fabric pipeline operations, spotlighting Materialized Lake Views for syncing Azure SQL to OneLake and detailing layered data transformations. Tutorials from Microsoft MVPs and Super Users cover dynamic masking, Power BI, REST admin, Pandas analysis, and efficient transformation patterns, with tips for troubleshooting and certification.

- [Mastering Declarative Data Transformations with Materialized Lake Views](https://blog.fabric.microsoft.com/en-US/blog/mastering-declarative-data-transformations-with-materialized-lake-views/)
- [Fabric Influencers Spotlight: August 2025 Edition](https://blog.fabric.microsoft.com/en-US/blog/fabric-influencers-spotlight-august-2025/)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'ml'
  AND slug                 = 'weekly-ml-roundup-2025-09-01';

-- SKIP 2025-09-08 : no 'Machine Learning/ML' block found in source (weekly-ai-roundup-2025-09-08)

-- weekly-ml-roundup-2025-09-15  (2025-09-15)
UPDATE content_items
SET  content      = 'Machine learning developments focus on benchmarking language models using Azure AI hardware, with guides enabling reproducible testing for demanding workloads. Extending last week''s coverage of benchmarking standards, practical instructions offer clear steps for real-world large-model testing.

<!--excerpt_end-->

## ML

### Benchmarking Llama 2 70B and Llama 3.1 405B on Azure ND GB200 v6

A comprehensive guide explains benchmarking Llama 2 70B and Llama 3.1 405B models with MLPerf Inference v5.1 on Azure ND GB200 v6 VMs running NVIDIA Grace CPUs and Blackwell B200 GPUs. Detailed steps include VM setup, organizing data, repo cloning, and prepping the environment. Results show Llama 2 70B at 52,000 tokens/sec and Llama 3.1 405B at 847 tokens/sec on a single VM, matching global performance. Sample configurations and MLPerf orchestration enable repeatable evaluations for both research and production.

These outcomes reinforce transparent processes and standards-based evaluation highlighted last week.

- [Benchmarking Llama 2 70B and 405B Models on Azure ND GB200 v6 with MLPerf Inference v5.1](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/a-quick-guide-to-benchmarking-ai-models-on-azure-llama-405b-and/ba-p/4452192)',
     excerpt      = 'Machine learning developments focus on benchmarking language models using Azure AI hardware, with guides enabling reproducible testing for demanding workloads. Extending last week''s coverage of benchmarking standards, practical instructions offer clear steps for real-world large-model testing.',
     content_hash = md5('Machine learning developments focus on benchmarking language models using Azure AI hardware, with guides enabling reproducible testing for demanding workloads. Extending last week''s coverage of benchmarking standards, practical instructions offer clear steps for real-world large-model testing.

<!--excerpt_end-->

## ML

### Benchmarking Llama 2 70B and Llama 3.1 405B on Azure ND GB200 v6

A comprehensive guide explains benchmarking Llama 2 70B and Llama 3.1 405B models with MLPerf Inference v5.1 on Azure ND GB200 v6 VMs running NVIDIA Grace CPUs and Blackwell B200 GPUs. Detailed steps include VM setup, organizing data, repo cloning, and prepping the environment. Results show Llama 2 70B at 52,000 tokens/sec and Llama 3.1 405B at 847 tokens/sec on a single VM, matching global performance. Sample configurations and MLPerf orchestration enable repeatable evaluations for both research and production.

These outcomes reinforce transparent processes and standards-based evaluation highlighted last week.

- [Benchmarking Llama 2 70B and 405B Models on Azure ND GB200 v6 with MLPerf Inference v5.1](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/a-quick-guide-to-benchmarking-ai-models-on-azure-llama-405b-and/ba-p/4452192)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'ml'
  AND slug                 = 'weekly-ml-roundup-2025-09-15';

-- weekly-ml-roundup-2025-09-22  (2025-09-22)
UPDATE content_items
SET  content      = 'Updates in machine learning center on making advanced AI and quantum computing more accessible, with new resources for beginners and ongoing research projects. Initiatives aim to build practical skills and support foundational research across various ML fields.

<!--excerpt_end-->

## ML

### Community-Driven AI Learning for Data Science and ML

Building on last week’s transparency in benchmarking, the focus now is on accessible ML learning for newcomers, featuring Discord sessions using Microsoft’s Data Science and ML for Beginners path. Participants take part in activities using Copilot, Python, Jupyter, and VS Code Data Wrangler, integrating basic knowledge into AI projects.

Live office hours and collaborative peer groups encourage knowledge exchange, matching last week’s benchmarking theme. Prompt cards, notebooks, and hands-on practice now extend to more early-career users, broadening ML engagement.

- [Practical Ways to Use AI in Your Data Science and ML Journey](https://techcommunity.microsoft.com/t5/microsoft-developer-community/practical-ways-to-use-ai-in-your-data-science-and-ml-journey/ba/p/4454764)

### Microsoft Quantum Computing Research Expansion

Following last week’s Azure ND GB200 v6 hardware benchmarking for ML, Microsoft started a quantum research partnership with the University of Maryland, covering hardware/software co-design, benchmarking standards, and error correction. The Microsoft Quantum platform targets reproducible validation and bridging public-private research, reflecting previous ML workflow improvements.

This collaboration paves the way for new programming standards and validation models, continuing the drive for transparent benchmarking from last week.

- [Microsoft Opens Quantum Research Center with Maryland Partnership](https://blogs.microsoft.com/on-the-issues/2025/09/17/our-new-collaboration-with-maryland-will-accelerate-scalable-quantum-computing/)',
     excerpt      = 'Updates in machine learning center on making advanced AI and quantum computing more accessible, with new resources for beginners and ongoing research projects. Initiatives aim to build practical skills and support foundational research across various ML fields.',
     content_hash = md5('Updates in machine learning center on making advanced AI and quantum computing more accessible, with new resources for beginners and ongoing research projects. Initiatives aim to build practical skills and support foundational research across various ML fields.

<!--excerpt_end-->

## ML

### Community-Driven AI Learning for Data Science and ML

Building on last week’s transparency in benchmarking, the focus now is on accessible ML learning for newcomers, featuring Discord sessions using Microsoft’s Data Science and ML for Beginners path. Participants take part in activities using Copilot, Python, Jupyter, and VS Code Data Wrangler, integrating basic knowledge into AI projects.

Live office hours and collaborative peer groups encourage knowledge exchange, matching last week’s benchmarking theme. Prompt cards, notebooks, and hands-on practice now extend to more early-career users, broadening ML engagement.

- [Practical Ways to Use AI in Your Data Science and ML Journey](https://techcommunity.microsoft.com/t5/microsoft-developer-community/practical-ways-to-use-ai-in-your-data-science-and-ml-journey/ba/p/4454764)

### Microsoft Quantum Computing Research Expansion

Following last week’s Azure ND GB200 v6 hardware benchmarking for ML, Microsoft started a quantum research partnership with the University of Maryland, covering hardware/software co-design, benchmarking standards, and error correction. The Microsoft Quantum platform targets reproducible validation and bridging public-private research, reflecting previous ML workflow improvements.

This collaboration paves the way for new programming standards and validation models, continuing the drive for transparent benchmarking from last week.

- [Microsoft Opens Quantum Research Center with Maryland Partnership](https://blogs.microsoft.com/on-the-issues/2025/09/17/our-new-collaboration-with-maryland-will-accelerate-scalable-quantum-computing/)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'ml'
  AND slug                 = 'weekly-ml-roundup-2025-09-22';

-- weekly-ml-roundup-2025-09-29  (2025-09-29)
UPDATE content_items
SET  content      = 'Machine learning updates this week focus on analytics scale, architecture maturity, and observability—especially in Microsoft Fabric’s Spark environment. New diagnostics and APIs offer developers more control, with an ongoing emphasis on collaborative production ML and best operational practices.

<!--excerpt_end-->

## ML

### Microsoft Fabric Spark Observability and Integration

A new preview for Fabric Spark Applications Comparison lets users visually assess up to four Spark app runs, supporting easier identification of performance issues. This builds on Spark Run Series Analysis, now generally available for grouping job runs and finding anomalies. Monitoring APIs provide real-time insight and automation capabilities for scaling ML operations. Features like Spark Advisor, skew diagnostics, and allocation reporting strengthen automated observability for teams.

User Data Functions, now generally available, enable custom Python logic in Fabric SQL, Lakehouse, Warehouses, and Power BI, encouraging wider reuse and easier integration. The VS Code extension and async data processing further improve developer workflow.

- [Microsoft Fabric Spark Applications Comparison Feature (Preview)](https://blog.fabric.microsoft.com/en-US/blog/public-preview-announcement-fabric-spark-applications-comparison/)
- [Fabric Spark Run Series Analysis: Enterprise-Scale Observability for Microsoft Fabric Spark Jobs](https://blog.fabric.microsoft.com/en-US/blog/fabric-spark-run-series-analysis-generally-available/)
- [Fabric Spark Monitoring APIs Now Generally Available](https://blog.fabric.microsoft.com/en-US/blog/general-availability-announcement-fabric-spark-monitoring-apis/)
- [Fabric User Data Functions Now Generally Available](https://blog.fabric.microsoft.com/en-US/blog/announcing-fabric-user-data-functions-now-in-general-availability/)

### Evolving MLOps Architectures and Operational Practices

Ongoing best practices encourage the shift from ad-hoc ML deployment to modular, automated workflows with versioning, CI/CD, lifecycle management, and monitoring—with tools like Kafka, Spark Streaming, Feast, MLflow, and Kubernetes as central components. The focus is on continuous delivery, drift detection, and strong governance within practical ML lifecycle management.

Community discussions around MLOps support collaborative learning, with events, podcasts, and networking driving shared expertise in real-world deployment, governance, and technical debt management.

- [MLOps Architectures: Building Scalable AI Systems](https://dellenny.com/mlops-architectures-building-scalable-ai-systems/)
- [MLOps at Scale: How Community Is Driving AI Into Production](https://devops.com/mlops-at-scale-how-community-is-driving-ai-into-production/)',
     excerpt      = 'Machine learning updates this week focus on analytics scale, architecture maturity, and observability—especially in Microsoft Fabric’s Spark environment. New diagnostics and APIs offer developers more control, with an ongoing emphasis on collaborative production ML and best operational practices.',
     content_hash = md5('Machine learning updates this week focus on analytics scale, architecture maturity, and observability—especially in Microsoft Fabric’s Spark environment. New diagnostics and APIs offer developers more control, with an ongoing emphasis on collaborative production ML and best operational practices.

<!--excerpt_end-->

## ML

### Microsoft Fabric Spark Observability and Integration

A new preview for Fabric Spark Applications Comparison lets users visually assess up to four Spark app runs, supporting easier identification of performance issues. This builds on Spark Run Series Analysis, now generally available for grouping job runs and finding anomalies. Monitoring APIs provide real-time insight and automation capabilities for scaling ML operations. Features like Spark Advisor, skew diagnostics, and allocation reporting strengthen automated observability for teams.

User Data Functions, now generally available, enable custom Python logic in Fabric SQL, Lakehouse, Warehouses, and Power BI, encouraging wider reuse and easier integration. The VS Code extension and async data processing further improve developer workflow.

- [Microsoft Fabric Spark Applications Comparison Feature (Preview)](https://blog.fabric.microsoft.com/en-US/blog/public-preview-announcement-fabric-spark-applications-comparison/)
- [Fabric Spark Run Series Analysis: Enterprise-Scale Observability for Microsoft Fabric Spark Jobs](https://blog.fabric.microsoft.com/en-US/blog/fabric-spark-run-series-analysis-generally-available/)
- [Fabric Spark Monitoring APIs Now Generally Available](https://blog.fabric.microsoft.com/en-US/blog/general-availability-announcement-fabric-spark-monitoring-apis/)
- [Fabric User Data Functions Now Generally Available](https://blog.fabric.microsoft.com/en-US/blog/announcing-fabric-user-data-functions-now-in-general-availability/)

### Evolving MLOps Architectures and Operational Practices

Ongoing best practices encourage the shift from ad-hoc ML deployment to modular, automated workflows with versioning, CI/CD, lifecycle management, and monitoring—with tools like Kafka, Spark Streaming, Feast, MLflow, and Kubernetes as central components. The focus is on continuous delivery, drift detection, and strong governance within practical ML lifecycle management.

Community discussions around MLOps support collaborative learning, with events, podcasts, and networking driving shared expertise in real-world deployment, governance, and technical debt management.

- [MLOps Architectures: Building Scalable AI Systems](https://dellenny.com/mlops-architectures-building-scalable-ai-systems/)
- [MLOps at Scale: How Community Is Driving AI Into Production](https://devops.com/mlops-at-scale-how-community-is-driving-ai-into-production/)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'ml'
  AND slug                 = 'weekly-ml-roundup-2025-09-29';

-- weekly-ml-roundup-2025-10-06  (2025-10-06)
UPDATE content_items
SET  content      = 'This week’s updates in machine learning focus on deeper integrations with the Microsoft Fabric ecosystem: faster, more affordable data transformations, enhanced multitasking, and streamlined collaboration. Broader connectivity between cloud and analytics platforms and stronger tools for experimentation mark a focus on flexible, interactive enterprise workflows.

<!--excerpt_end-->

## ML

### Dataflow Gen2 in Microsoft Fabric: Performance, Integration, and Developer Experience

Dataflow Gen2 in Microsoft Fabric gains a new pricing model, helping organizations manage ETL costs for jobs of all sizes. The Modern Query Evaluation Service speeds up parallel queries for lower expenses and shorter runtimes, advancing last week’s troubleshooting features like Spark monitoring APIs.

Real-time analytics and previews allow faster iteration on transformation logic, with outputs now targeting Fabric Lakehouse, Azure Data Lake Gen2, SharePoint (CSV), Snowflake (preview), and OneLake Catalog management—matching the trend of multi-environment integration.

Copilot now enables natural language transformation and ingestion, contributing to collaborative machine learning themes. Migration from Gen1 is supported by dedicated tools. Permission management, schema controls, and hybrid architecture improvements continue the previous focus on operational governance.

- [Announcing Dataflow Gen2 Enhancements in Microsoft Fabric: Faster, Smarter, and More Affordable Data Transformations](https://blog.fabric.microsoft.com/en-US/blog/unlocking-the-next-generation-of-data-transformations-with-dataflow-gen2-fabcon-europe-2025-announcements/)
- [Enhancements to Dataflow Gen2 in Microsoft Fabric: Expanded Destinations and Schema Support](https://blog.fabric.microsoft.com/en-US/blog/new-dataflow-gen2-data-destinations-and-experience-improvements/)

### AI-Powered Data Transformation and Developer Tools

Fabric Data Wrangler now supports fast AI-driven text summarization, translation, and sentiment analysis through PROSE suggestions and live previews. Copilot prompts generate custom transformation code and feedback, minimizing manual coding for complex datasets. Conversion between pandas and PySpark further scales projects, while documentation and guides support adoption of these new workflows.

- [Accelerate Data Transformation with AI in Data Wrangler](https://blog.fabric.microsoft.com/en-US/blog/accelerate-data-transformation-with-ai-functions-in-data-wrangler/)

### Multitasking and Workflow Improvements in Microsoft Fabric

Fabric’s updated horizontal tabs permit working on multiple items, along with workspace color coding and numbering to prevent errors and reduce context switching. The Object Explorer and higher concurrent item limits cater to users who need advanced multitasking—building on recent improvements for async processing and VS Code extension integration. These features are specific to Fabric.

- [Supercharge Your Workflow: New Multitasking Features Coming to Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/supercharge-your-workflow-new-multitasking-features-coming-to-fabric/)

### Experimentation Analytics with Statsig in Microsoft Fabric

Statsig Experimentation Analytics in Fabric provides tools for running and analyzing A/B/n tests on OneLake data, using frequentist statistics and near real-time metrics via Statsig’s Explorer. Instant results allow rapid update cycles, and Power BI integration assists visual review of experiments. Structured workflows help teams validate ML models, continuing last week’s focus on practical MLOps processes.

- [Experimentation Analytics with Statsig in Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/27219/)',
     excerpt      = 'This week’s updates in machine learning focus on deeper integrations with the Microsoft Fabric ecosystem: faster, more affordable data transformations, enhanced multitasking, and streamlined collaboration. Broader connectivity between cloud and analytics platforms and stronger tools for experimentation mark a focus on flexible, interactive enterprise workflows.',
     content_hash = md5('This week’s updates in machine learning focus on deeper integrations with the Microsoft Fabric ecosystem: faster, more affordable data transformations, enhanced multitasking, and streamlined collaboration. Broader connectivity between cloud and analytics platforms and stronger tools for experimentation mark a focus on flexible, interactive enterprise workflows.

<!--excerpt_end-->

## ML

### Dataflow Gen2 in Microsoft Fabric: Performance, Integration, and Developer Experience

Dataflow Gen2 in Microsoft Fabric gains a new pricing model, helping organizations manage ETL costs for jobs of all sizes. The Modern Query Evaluation Service speeds up parallel queries for lower expenses and shorter runtimes, advancing last week’s troubleshooting features like Spark monitoring APIs.

Real-time analytics and previews allow faster iteration on transformation logic, with outputs now targeting Fabric Lakehouse, Azure Data Lake Gen2, SharePoint (CSV), Snowflake (preview), and OneLake Catalog management—matching the trend of multi-environment integration.

Copilot now enables natural language transformation and ingestion, contributing to collaborative machine learning themes. Migration from Gen1 is supported by dedicated tools. Permission management, schema controls, and hybrid architecture improvements continue the previous focus on operational governance.

- [Announcing Dataflow Gen2 Enhancements in Microsoft Fabric: Faster, Smarter, and More Affordable Data Transformations](https://blog.fabric.microsoft.com/en-US/blog/unlocking-the-next-generation-of-data-transformations-with-dataflow-gen2-fabcon-europe-2025-announcements/)
- [Enhancements to Dataflow Gen2 in Microsoft Fabric: Expanded Destinations and Schema Support](https://blog.fabric.microsoft.com/en-US/blog/new-dataflow-gen2-data-destinations-and-experience-improvements/)

### AI-Powered Data Transformation and Developer Tools

Fabric Data Wrangler now supports fast AI-driven text summarization, translation, and sentiment analysis through PROSE suggestions and live previews. Copilot prompts generate custom transformation code and feedback, minimizing manual coding for complex datasets. Conversion between pandas and PySpark further scales projects, while documentation and guides support adoption of these new workflows.

- [Accelerate Data Transformation with AI in Data Wrangler](https://blog.fabric.microsoft.com/en-US/blog/accelerate-data-transformation-with-ai-functions-in-data-wrangler/)

### Multitasking and Workflow Improvements in Microsoft Fabric

Fabric’s updated horizontal tabs permit working on multiple items, along with workspace color coding and numbering to prevent errors and reduce context switching. The Object Explorer and higher concurrent item limits cater to users who need advanced multitasking—building on recent improvements for async processing and VS Code extension integration. These features are specific to Fabric.

- [Supercharge Your Workflow: New Multitasking Features Coming to Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/supercharge-your-workflow-new-multitasking-features-coming-to-fabric/)

### Experimentation Analytics with Statsig in Microsoft Fabric

Statsig Experimentation Analytics in Fabric provides tools for running and analyzing A/B/n tests on OneLake data, using frequentist statistics and near real-time metrics via Statsig’s Explorer. Instant results allow rapid update cycles, and Power BI integration assists visual review of experiments. Structured workflows help teams validate ML models, continuing last week’s focus on practical MLOps processes.

- [Experimentation Analytics with Statsig in Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/27219/)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'ml'
  AND slug                 = 'weekly-ml-roundup-2025-10-06';

-- weekly-ml-roundup-2025-10-13  (2025-10-13)
UPDATE content_items
SET  content      = 'Machine learning updates this week focused on streamlined data engineering, automation, security, and developer skill-building resources across platforms. Microsoft Fabric’s toolset expanded Spark and SQL integrations. New documentation and security improvements, connectors, and educational material help developers working on analytics, edge AI, and automated data workflows.

<!--excerpt_end-->

## ML

### Microsoft Fabric Ecosystem: Streamlined Data Connectivity and Automation

Building on last week’s Dataflow Gen2 governance, Microsoft Fabric offers a Spark Connector for SQL Databases in preview, improving Spark workloads with direct access to Azure SQL, Managed Instance, and Fabric SQL. This simplifies ETL and ML for PySpark and Scala, continuing support for secure, enterprise standards.

OPENROWSET now lets users set named sources and relative file paths, replacing GUIDs for clear SQL and easier troubleshooting, furthering recent operational efficiency.

Service Principal support in Semantic Link enables scalable, secure automation of pipelines—continuing previous enhancements in permission and identity management. Azure AD managed identities and Key Vaults support role-based data jobs through the "sempy.fabric" package.

- [Fabric Spark Connector for SQL Databases Now Available (Preview)](https://blog.fabric.microsoft.com/en-US/blog/spark-connector-for-sql-databases-preview/)
- [Service Principal Support in Semantic Link: Enabling Scalable, Secure Automation](https://blog.fabric.microsoft.com/en-US/blog/service-principal-support-in-semantic-link-enabling-scalable-secure-automation/)
- [Simplifying File Access in OPENROWSET: Data Sources and Relative Paths (Preview)](https://blog.fabric.microsoft.com/en-US/blog/simplifying-file-access-in-openrowset-using-data-sources-and-relative-paths-preview/)

### Learning and Developer Enablement: Gen AI and Edge AI Resources

Expanded developer support includes a nine-part YouTube series from Pamela Fox, covering generative AI, prompt engineering, RAG, agent frameworks, and live code demonstrations with OpenAI SDK and Azure AI Search—following last week’s collaborative engagement and analytics in Fabric ML.

A new edge AI curriculum covers Windows AI PC and hardware deployment with ONNX Runtime, DirectML, and Olive, advancing last week’s hybrid architecture support. Practical samples address IoT and automation scenarios for NPUs and Azure connections.

- [Level Up Your Python Gen AI Skills: Nine-Part YouTube Stream Series](https://techcommunity.microsoft.com/t5/microsoft-developer-community/level-up-your-python-gen-ai-skills-from-our-free-nine-part/ba-p/4459464)
- [Building Smarter Edge AI with Windows AI PCs: The Edge AI for Beginners Curriculum](https://techcommunity.microsoft.com/t5/microsoft-developer-community/from-cloud-to-chip-building-smarter-ai-at-the-edge-with-windows/ba/p/4459582)

### The Emergence of Automated Data Modeling and Warehouse Modernization

Flow.BI’s AI-powered data modeling adopts metadata-driven automation, supporting model generation, relationship inference, multilingual metadata, mesh configuration, and robust security. This continues last week’s focus on metadata management, helping modernize data warehousing for organizations adapting their architecture.

- [Defining the Raw Data Vault with Artificial Intelligence: Automating Data Warehouse Modeling](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/defining-the-raw-data-vault-with-artificial-intelligence/ba-p/4453557)',
     excerpt      = 'Machine learning updates this week focused on streamlined data engineering, automation, security, and developer skill-building resources across platforms. Microsoft Fabric’s toolset expanded Spark and SQL integrations. New documentation and security improvements, connectors, and educational material help developers working on analytics, edge AI, and automated data workflows.',
     content_hash = md5('Machine learning updates this week focused on streamlined data engineering, automation, security, and developer skill-building resources across platforms. Microsoft Fabric’s toolset expanded Spark and SQL integrations. New documentation and security improvements, connectors, and educational material help developers working on analytics, edge AI, and automated data workflows.

<!--excerpt_end-->

## ML

### Microsoft Fabric Ecosystem: Streamlined Data Connectivity and Automation

Building on last week’s Dataflow Gen2 governance, Microsoft Fabric offers a Spark Connector for SQL Databases in preview, improving Spark workloads with direct access to Azure SQL, Managed Instance, and Fabric SQL. This simplifies ETL and ML for PySpark and Scala, continuing support for secure, enterprise standards.

OPENROWSET now lets users set named sources and relative file paths, replacing GUIDs for clear SQL and easier troubleshooting, furthering recent operational efficiency.

Service Principal support in Semantic Link enables scalable, secure automation of pipelines—continuing previous enhancements in permission and identity management. Azure AD managed identities and Key Vaults support role-based data jobs through the "sempy.fabric" package.

- [Fabric Spark Connector for SQL Databases Now Available (Preview)](https://blog.fabric.microsoft.com/en-US/blog/spark-connector-for-sql-databases-preview/)
- [Service Principal Support in Semantic Link: Enabling Scalable, Secure Automation](https://blog.fabric.microsoft.com/en-US/blog/service-principal-support-in-semantic-link-enabling-scalable-secure-automation/)
- [Simplifying File Access in OPENROWSET: Data Sources and Relative Paths (Preview)](https://blog.fabric.microsoft.com/en-US/blog/simplifying-file-access-in-openrowset-using-data-sources-and-relative-paths-preview/)

### Learning and Developer Enablement: Gen AI and Edge AI Resources

Expanded developer support includes a nine-part YouTube series from Pamela Fox, covering generative AI, prompt engineering, RAG, agent frameworks, and live code demonstrations with OpenAI SDK and Azure AI Search—following last week’s collaborative engagement and analytics in Fabric ML.

A new edge AI curriculum covers Windows AI PC and hardware deployment with ONNX Runtime, DirectML, and Olive, advancing last week’s hybrid architecture support. Practical samples address IoT and automation scenarios for NPUs and Azure connections.

- [Level Up Your Python Gen AI Skills: Nine-Part YouTube Stream Series](https://techcommunity.microsoft.com/t5/microsoft-developer-community/level-up-your-python-gen-ai-skills-from-our-free-nine-part/ba-p/4459464)
- [Building Smarter Edge AI with Windows AI PCs: The Edge AI for Beginners Curriculum](https://techcommunity.microsoft.com/t5/microsoft-developer-community/from-cloud-to-chip-building-smarter-ai-at-the-edge-with-windows/ba/p/4459582)

### The Emergence of Automated Data Modeling and Warehouse Modernization

Flow.BI’s AI-powered data modeling adopts metadata-driven automation, supporting model generation, relationship inference, multilingual metadata, mesh configuration, and robust security. This continues last week’s focus on metadata management, helping modernize data warehousing for organizations adapting their architecture.

- [Defining the Raw Data Vault with Artificial Intelligence: Automating Data Warehouse Modeling](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/defining-the-raw-data-vault-with-artificial-intelligence/ba-p/4453557)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'ml'
  AND slug                 = 'weekly-ml-roundup-2025-10-13';

-- weekly-ml-roundup-2025-10-20  (2025-10-20)
UPDATE content_items
SET  content      = 'Recent ML updates target smoother data engineering and greater Azure integration, making performance and reliability improvements for lakehouse and machine learning frameworks common in big data workflows.

<!--excerpt_end-->

## ML

### Microsoft Fabric Spark: Adaptive File Size Management for Delta Tables

Fabric Spark introduces adaptive file size management, automatically choosing optimal Delta table file sizes based on telemetry data. This automation streamlines ELT and analytics tasks, resulting in up to 2.8 times faster file compaction and 1.6 times TPC-DS performance improvements. Settings update automatically as workloads shift, but developers can tailor configurations to suit specific needs.

Benefits also include improved data skipping, reduced file rewrite costs, and increased processing parallelism, all supporting secure and flexible solutions.

- [Adaptive Target File Size Management in Fabric Spark](https://blog.fabric.microsoft.com/en-US/blog/adaptive-target-file-size-management-in-fabric-spark/)

### Azure Data Lake Integrations: adlfs Python Library Improvements

The adlfs Python library receives speed improvements through parallel block uploads and smaller chunk defaults, helping users avoid timeouts on geo-distributed systems and supporting more secure data pipelines.

Frameworks like Dask, Pandas, Ray, PyTorch, and PyIceberg work seamlessly with these updates, which include easier authentication and continued fsspec compatibility, supporting efficient integration for modern data and AI workflows.

- [Easily Connect AI Workloads to Azure Blob Storage with adlfs](https://devblogs.microsoft.com/azure-sdk/easily-connect-ai-workloads-to-azure-blob-storage-with-adlfs/)',
     excerpt      = 'Recent ML updates target smoother data engineering and greater Azure integration, making performance and reliability improvements for lakehouse and machine learning frameworks common in big data workflows.',
     content_hash = md5('Recent ML updates target smoother data engineering and greater Azure integration, making performance and reliability improvements for lakehouse and machine learning frameworks common in big data workflows.

<!--excerpt_end-->

## ML

### Microsoft Fabric Spark: Adaptive File Size Management for Delta Tables

Fabric Spark introduces adaptive file size management, automatically choosing optimal Delta table file sizes based on telemetry data. This automation streamlines ELT and analytics tasks, resulting in up to 2.8 times faster file compaction and 1.6 times TPC-DS performance improvements. Settings update automatically as workloads shift, but developers can tailor configurations to suit specific needs.

Benefits also include improved data skipping, reduced file rewrite costs, and increased processing parallelism, all supporting secure and flexible solutions.

- [Adaptive Target File Size Management in Fabric Spark](https://blog.fabric.microsoft.com/en-US/blog/adaptive-target-file-size-management-in-fabric-spark/)

### Azure Data Lake Integrations: adlfs Python Library Improvements

The adlfs Python library receives speed improvements through parallel block uploads and smaller chunk defaults, helping users avoid timeouts on geo-distributed systems and supporting more secure data pipelines.

Frameworks like Dask, Pandas, Ray, PyTorch, and PyIceberg work seamlessly with these updates, which include easier authentication and continued fsspec compatibility, supporting efficient integration for modern data and AI workflows.

- [Easily Connect AI Workloads to Azure Blob Storage with adlfs](https://devblogs.microsoft.com/azure-sdk/easily-connect-ai-workloads-to-azure-blob-storage-with-adlfs/)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'ml'
  AND slug                 = 'weekly-ml-roundup-2025-10-20';

-- weekly-ml-roundup-2025-10-27  (2025-10-27)
UPDATE content_items
SET  content      = 'Recent progress in machine learning underlines improvements in model fine-tuning and deployment, emphasizing vision-language models (VLMs) for image classification on Azure AI Foundry. Developers are given clearer steps for achieving better accuracy, controlling costs, and supporting production deployments.

<!--excerpt_end-->

## ML

### Fine-Tuning GPT-4o Vision-Language Models on Azure AI Foundry

A new guide covers fine-tuning GPT-4o for image classification (using Stanford Dogs), continuing last week’s push for performance and usability in ML stacks. The tutorial covers data formatting in Azure JSONL, using Batch Inference API for large workloads (with higher latency and reduced cost), and connects to past automation topics drawn from Microsoft Fabric.

Instructions include using the Vision Fine-Tuning API to adapt GPT-4o for breed identification. The inclusion of public code samples and templates supports research and encourages wider use, echoing Azure ML’s focus on analytics and efficiency. Demonstrated results improved accuracy from 61.67% (CNN) to 82.67% for a fine-tuned model, with a detailed breakdown of cost and latency to help with deployment planning.

Production guidance centers around Azure’s security and scalability, detailing parameter adjustment, throughput, and best practices. Open-source code and Azure documentation make this a practical resource for ML engineers.

- [Fine-Tuning GPT-4o for Image Classification on Azure AI Foundry: A Practical Guide](https://devblogs.microsoft.com/foundry/a-developers-guide-to-fine-tuning-gpt-4o-for-image-classification-on-azure-ai-foundry/)',
     excerpt      = 'Recent progress in machine learning underlines improvements in model fine-tuning and deployment, emphasizing vision-language models (VLMs) for image classification on Azure AI Foundry. Developers are given clearer steps for achieving better accuracy, controlling costs, and supporting production deployments.',
     content_hash = md5('Recent progress in machine learning underlines improvements in model fine-tuning and deployment, emphasizing vision-language models (VLMs) for image classification on Azure AI Foundry. Developers are given clearer steps for achieving better accuracy, controlling costs, and supporting production deployments.

<!--excerpt_end-->

## ML

### Fine-Tuning GPT-4o Vision-Language Models on Azure AI Foundry

A new guide covers fine-tuning GPT-4o for image classification (using Stanford Dogs), continuing last week’s push for performance and usability in ML stacks. The tutorial covers data formatting in Azure JSONL, using Batch Inference API for large workloads (with higher latency and reduced cost), and connects to past automation topics drawn from Microsoft Fabric.

Instructions include using the Vision Fine-Tuning API to adapt GPT-4o for breed identification. The inclusion of public code samples and templates supports research and encourages wider use, echoing Azure ML’s focus on analytics and efficiency. Demonstrated results improved accuracy from 61.67% (CNN) to 82.67% for a fine-tuned model, with a detailed breakdown of cost and latency to help with deployment planning.

Production guidance centers around Azure’s security and scalability, detailing parameter adjustment, throughput, and best practices. Open-source code and Azure documentation make this a practical resource for ML engineers.

- [Fine-Tuning GPT-4o for Image Classification on Azure AI Foundry: A Practical Guide](https://devblogs.microsoft.com/foundry/a-developers-guide-to-fine-tuning-gpt-4o-for-image-classification-on-azure-ai-foundry/)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'ml'
  AND slug                 = 'weekly-ml-roundup-2025-10-27';

-- weekly-ml-roundup-2025-11-03  (2025-11-03)
UPDATE content_items
SET  content      = 'In ML news this week, attention shifted to practical integration of vector search features into enterprise databases. This continues the trend of enabling smarter semantic search and similarity-based retrieval within everyday database operations, building on recent progress in GPT-4o fine-tuning and retrieval pipelines for Azure AI Foundry.

<!--excerpt_end-->

## ML

### Vector Search in SQL Server 2025 and Azure SQL

SQL Server 2025 now offers built-in vector search, highlighted in the Data Exposed: MVP Edition walkthrough led by Joseph D’Antoni. The guide demonstrates setting up vector storage, retrieval, and using new Transact-SQL functions for semantic and similarity search, supporting a range of AI workloads.

Resources include setup walkthroughs, query examples, and architectural details for Azure SQL, continuing the availability of public samples and deployment tools for vector-aware development.

- [Implementing Vector Search in Your Application with SQL Server 2025](/ai/videos/implementing-vector-search-in-your-application-with-sql-server-2025)',
     excerpt      = 'In ML news this week, attention shifted to practical integration of vector search features into enterprise databases. This continues the trend of enabling smarter semantic search and similarity-based retrieval within everyday database operations, building on recent progress in GPT-4o fine-tuning and retrieval pipelines for Azure AI Foundry.',
     content_hash = md5('In ML news this week, attention shifted to practical integration of vector search features into enterprise databases. This continues the trend of enabling smarter semantic search and similarity-based retrieval within everyday database operations, building on recent progress in GPT-4o fine-tuning and retrieval pipelines for Azure AI Foundry.

<!--excerpt_end-->

## ML

### Vector Search in SQL Server 2025 and Azure SQL

SQL Server 2025 now offers built-in vector search, highlighted in the Data Exposed: MVP Edition walkthrough led by Joseph D’Antoni. The guide demonstrates setting up vector storage, retrieval, and using new Transact-SQL functions for semantic and similarity search, supporting a range of AI workloads.

Resources include setup walkthroughs, query examples, and architectural details for Azure SQL, continuing the availability of public samples and deployment tools for vector-aware development.

- [Implementing Vector Search in Your Application with SQL Server 2025](/ai/videos/implementing-vector-search-in-your-application-with-sql-server-2025)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'ml'
  AND slug                 = 'weekly-ml-roundup-2025-11-03';

-- weekly-ml-roundup-2025-11-10  (2025-11-10)
UPDATE content_items
SET  content      = 'Recent advancements in machine learning include new hardware performance benchmarks, updates to distributed computing platforms, practical AI workflow guides, improvements in geospatial analytics tools, and the introduction of a new open-source platform for agent-based market simulation. These updates provide concrete help for teams deploying large-scale ML and modernizing practices.

<!--excerpt_end-->

## ML

### Azure ML Infrastructure and Hardware Optimization

Azure''s ND GB300 v6 virtual machines, equipped with Blackwell GPUs, achieved over 1 million tokens/sec on Llama2 70B inference, surpassing the performance of previous ND GB200 v6 and DGX H100 models. Technical documentation outlines stack improvements such as 2.5x GEMM TFLOPS, 7.37TB/s bandwidth, and multi-VM orchestration, offering reproducible benchmarking scripts and advice for optimizing large language model (LLM) inference on Azure.

- [Azure ND GB300 v6: Achieving Over 1 Million Tokens/sec on Llama2 70B Inference](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/breaking-the-million-token-barrier-the-technical-achievement-of/ba/p/4466080)

### Distributed Python AI with Ray on Azure

Microsoft and Anyscale introduced managed Ray support on Azure Kubernetes Service, featuring Azure Monitor, Entra ID, and Blob Storage integration. Python developers can now deploy distributed ML tasks securely and scale resources easily, without deep Kubernetes expertise. Key features include RayTurbo, simple cluster deployment, and compliance/security within customer subscriptions—streamlining the path from prototype to production.

- [Powering Distributed AI and ML Workloads at Scale with Azure and Anyscale](https://devblogs.microsoft.com/all-things-azure/powering-distributed-aiml-at-scale-with-azure-and-anyscale/)

### Practical AI Workflows: Tutorials and Educational Initiatives

The Spanish-language ''Python + IA'' series offers nine practical sessions on building and deploying GenAI apps, addressing LLMs, RAG, agent engineering, and risk mitigation with code samples and community support on Azure and GitHub. The Cozy Kitchen guide demonstrates intelligent agent engineering with Azure AI Foundry, focusing on modular workflow design, persistence, GitHub integration, and advanced tuning.

- [Recapitulación de la Serie Python + IA: Técnicas, Modelos y Recursos](https://techcommunity.microsoft.com/t5/microsoft-developer-community/python-ia-resumen-y-recursos/ba-p/4465152)
- [From Building to Fine-Tuning: Coding Agents that Optimize AI Workflows](/ai/videos/from-building-to-fine-tuning-coding-agents-that-optimize-ai-workflows)

### Microsoft Fabric Data Services: Spatial Analytics, Workflow Automation, and Data Skills Development

ArcGIS GeoAnalytics is generally available for Fabric Spark users, enabling robust spatial data automation and visualization. Fabric Data Days, a global workshop event, now provides training and competitions for data engineers and scientists. Updates to Fabric introduce decoupled semantic models and API-driven workflow management, improving model lifecycle flexibility.

- [ArcGIS GeoAnalytics for Microsoft Fabric Spark (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/arcgis-geoanalytics-for-microsoft-fabric-spark-generally-available/)
- [Advance your career in Data & AI with Microsoft Fabric Data Days](https://blog.fabric.microsoft.com/en-US/blog/advance-your-career-in-data-ai-with-microsoft-fabric-data-days/)
- [Decoupling Default Semantic Models for Existing Items in Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/decoupling-default-semantic-models-for-existing-in-microsoft-fabric/)

### Open-Source Platforms and Agent-Based Market Simulation

Microsoft’s open-source Magentic Marketplace provides a modular system for agent-based market simulation. It includes REST APIs, customizable agent and market primitives, visualizations, and research summaries. Resources such as source code, datasets, and experiment templates are available for developers and researchers to study transparency and resilience in agent-based systems.

- [Magentic Marketplace: Open-Source Simulation for Agentic Markets Research](https://www.microsoft.com/en-us/research/blog/magentic-marketplace-an-open-source-simulation-environment-for-studying-agentic-markets/)',
     excerpt      = 'Recent advancements in machine learning include new hardware performance benchmarks, updates to distributed computing platforms, practical AI workflow guides, improvements in geospatial analytics tools, and the introduction of a new open-source platform for agent-based market simulation. These updates provide concrete help for teams deploying large-scale ML and modernizing practices.',
     content_hash = md5('Recent advancements in machine learning include new hardware performance benchmarks, updates to distributed computing platforms, practical AI workflow guides, improvements in geospatial analytics tools, and the introduction of a new open-source platform for agent-based market simulation. These updates provide concrete help for teams deploying large-scale ML and modernizing practices.

<!--excerpt_end-->

## ML

### Azure ML Infrastructure and Hardware Optimization

Azure''s ND GB300 v6 virtual machines, equipped with Blackwell GPUs, achieved over 1 million tokens/sec on Llama2 70B inference, surpassing the performance of previous ND GB200 v6 and DGX H100 models. Technical documentation outlines stack improvements such as 2.5x GEMM TFLOPS, 7.37TB/s bandwidth, and multi-VM orchestration, offering reproducible benchmarking scripts and advice for optimizing large language model (LLM) inference on Azure.

- [Azure ND GB300 v6: Achieving Over 1 Million Tokens/sec on Llama2 70B Inference](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/breaking-the-million-token-barrier-the-technical-achievement-of/ba/p/4466080)

### Distributed Python AI with Ray on Azure

Microsoft and Anyscale introduced managed Ray support on Azure Kubernetes Service, featuring Azure Monitor, Entra ID, and Blob Storage integration. Python developers can now deploy distributed ML tasks securely and scale resources easily, without deep Kubernetes expertise. Key features include RayTurbo, simple cluster deployment, and compliance/security within customer subscriptions—streamlining the path from prototype to production.

- [Powering Distributed AI and ML Workloads at Scale with Azure and Anyscale](https://devblogs.microsoft.com/all-things-azure/powering-distributed-aiml-at-scale-with-azure-and-anyscale/)

### Practical AI Workflows: Tutorials and Educational Initiatives

The Spanish-language ''Python + IA'' series offers nine practical sessions on building and deploying GenAI apps, addressing LLMs, RAG, agent engineering, and risk mitigation with code samples and community support on Azure and GitHub. The Cozy Kitchen guide demonstrates intelligent agent engineering with Azure AI Foundry, focusing on modular workflow design, persistence, GitHub integration, and advanced tuning.

- [Recapitulación de la Serie Python + IA: Técnicas, Modelos y Recursos](https://techcommunity.microsoft.com/t5/microsoft-developer-community/python-ia-resumen-y-recursos/ba-p/4465152)
- [From Building to Fine-Tuning: Coding Agents that Optimize AI Workflows](/ai/videos/from-building-to-fine-tuning-coding-agents-that-optimize-ai-workflows)

### Microsoft Fabric Data Services: Spatial Analytics, Workflow Automation, and Data Skills Development

ArcGIS GeoAnalytics is generally available for Fabric Spark users, enabling robust spatial data automation and visualization. Fabric Data Days, a global workshop event, now provides training and competitions for data engineers and scientists. Updates to Fabric introduce decoupled semantic models and API-driven workflow management, improving model lifecycle flexibility.

- [ArcGIS GeoAnalytics for Microsoft Fabric Spark (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/arcgis-geoanalytics-for-microsoft-fabric-spark-generally-available/)
- [Advance your career in Data & AI with Microsoft Fabric Data Days](https://blog.fabric.microsoft.com/en-US/blog/advance-your-career-in-data-ai-with-microsoft-fabric-data-days/)
- [Decoupling Default Semantic Models for Existing Items in Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/decoupling-default-semantic-models-for-existing-in-microsoft-fabric/)

### Open-Source Platforms and Agent-Based Market Simulation

Microsoft’s open-source Magentic Marketplace provides a modular system for agent-based market simulation. It includes REST APIs, customizable agent and market primitives, visualizations, and research summaries. Resources such as source code, datasets, and experiment templates are available for developers and researchers to study transparency and resilience in agent-based systems.

- [Magentic Marketplace: Open-Source Simulation for Agentic Markets Research](https://www.microsoft.com/en-us/research/blog/magentic-marketplace-an-open-source-simulation-environment-for-studying-agentic-markets/)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'ml'
  AND slug                 = 'weekly-ml-roundup-2025-11-10';

-- weekly-ml-roundup-2025-11-17  (2025-11-17)
UPDATE content_items
SET  content      = 'Machine learning highlights this week include new open-source models, practical cloud integration examples, and inventive applications for research and cultural projects. Microsoft furthered its open climate research and shared patterns for agent workflows and legacy dataset modernization.

<!--excerpt_end-->

## ML

### Microsoft’s Open-Source Aurora Model for Climate Forecasting

Microsoft debuted the Aurora project to expand access to climate and weather modeling—an open-source foundation trained on broad atmospheric datasets for predicting waves, air quality, and extreme weather. Code, model weights, and pipeline plans are available, making it easier for developers to offer both localized and large-scale forecasts. Built through partnerships including Cambridge’s Rich Turner lab and built atop efforts like SPARROW, Aurora’s public APIs make it a useful resource for energy management, disaster response, and environmental analysis by reducing the technical hurdles for entry.

- [Aurora: Microsoft’s Open-Source AI Model for Weather and Climate Forecasting](https://blogs.microsoft.com/on-the-issues/2025/11/13/the-next-phase-of-aurora-open-and-collaborative-ai-for-weather-and-climate-forecasting/)

### .NET, Aspire, and Redis: Patterns for Intelligent Agentic Workflows

Detailed coverage of .NET Aspire, Redis, and the Microsoft Agent Framework shows how to build robust, scalable agent systems. Redis enables semantic caching, vector storage, and management of session state, aligning with the trend toward persistent, distributed agent architectures. All updates utilize the new features in .NET 10, C# 14, F# 10, and Visual Studio 2026, reinforcing the focus on modular and multi-agent workflow strategies.

- [Build Smarter Agents with Redis, .NET Aspire, and Microsoft Agent Framework](/ai/videos/build-smarter-agents-with-redis-net-aspire-and-microsoft-agent-framework)

### Modernizing Historical Datasets with ML.NET and Azure

ML.NET and Azure CosmosDb are used this week to modernize a 17th-century Italian-English dictionary. Developers leverage current .NET and ML.NET features for processing legacy data—including custom vector embeddings and scalable cloud storage. These updates enable robust semantic search and reliable API endpoints, demonstrating practical uses of Microsoft’s ML tools in both research and preservation settings.

- [Modernizing a 17th Century Italian-English Dictionary with .NET and ML.NET](/azure/videos/modernizing-a-17th-century-italian-english-dictionary-with-net-and-mlnet)',
     excerpt      = 'Machine learning highlights this week include new open-source models, practical cloud integration examples, and inventive applications for research and cultural projects. Microsoft furthered its open climate research and shared patterns for agent workflows and legacy dataset modernization.',
     content_hash = md5('Machine learning highlights this week include new open-source models, practical cloud integration examples, and inventive applications for research and cultural projects. Microsoft furthered its open climate research and shared patterns for agent workflows and legacy dataset modernization.

<!--excerpt_end-->

## ML

### Microsoft’s Open-Source Aurora Model for Climate Forecasting

Microsoft debuted the Aurora project to expand access to climate and weather modeling—an open-source foundation trained on broad atmospheric datasets for predicting waves, air quality, and extreme weather. Code, model weights, and pipeline plans are available, making it easier for developers to offer both localized and large-scale forecasts. Built through partnerships including Cambridge’s Rich Turner lab and built atop efforts like SPARROW, Aurora’s public APIs make it a useful resource for energy management, disaster response, and environmental analysis by reducing the technical hurdles for entry.

- [Aurora: Microsoft’s Open-Source AI Model for Weather and Climate Forecasting](https://blogs.microsoft.com/on-the-issues/2025/11/13/the-next-phase-of-aurora-open-and-collaborative-ai-for-weather-and-climate-forecasting/)

### .NET, Aspire, and Redis: Patterns for Intelligent Agentic Workflows

Detailed coverage of .NET Aspire, Redis, and the Microsoft Agent Framework shows how to build robust, scalable agent systems. Redis enables semantic caching, vector storage, and management of session state, aligning with the trend toward persistent, distributed agent architectures. All updates utilize the new features in .NET 10, C# 14, F# 10, and Visual Studio 2026, reinforcing the focus on modular and multi-agent workflow strategies.

- [Build Smarter Agents with Redis, .NET Aspire, and Microsoft Agent Framework](/ai/videos/build-smarter-agents-with-redis-net-aspire-and-microsoft-agent-framework)

### Modernizing Historical Datasets with ML.NET and Azure

ML.NET and Azure CosmosDb are used this week to modernize a 17th-century Italian-English dictionary. Developers leverage current .NET and ML.NET features for processing legacy data—including custom vector embeddings and scalable cloud storage. These updates enable robust semantic search and reliable API endpoints, demonstrating practical uses of Microsoft’s ML tools in both research and preservation settings.

- [Modernizing a 17th Century Italian-English Dictionary with .NET and ML.NET](/azure/videos/modernizing-a-17th-century-italian-english-dictionary-with-net-and-mlnet)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'ml'
  AND slug                 = 'weekly-ml-roundup-2025-11-17';

-- weekly-ml-roundup-2025-11-24  (2025-11-24)
UPDATE content_items
SET  content      = 'The machine learning focus this week is on more scalable compute, enhanced platform features, and better operational tools from cloud and enterprise providers. Azure rolled out ND GB300 v6 VMs, while Microsoft Fabric announced further improvements in its AI and data engineering offerings. Aspects like data quality, model deployment, and performance optimization remain front and center, reflecting an ongoing move to scalable and high-throughput ML infrastructure.

<!--excerpt_end-->

## ML

### Azure AI Compute and Infrastructure

Azure has released the ND GB300 v6 VMs, which include NVIDIA GB300 NVL72 GPUs, Grace CPUs, and fast InfiniBand networking built for large-scale training and inference. These VMs integrate with Azure CycleCloud, Batch, and AKS, building on existing solutions for orchestrating AI workloads.

The AMLFS 20 (Azure Managed Lustre) SKU delivers bigger namespaces and higher metadata throughput for high-performance workloads, meeting the needs of fast, scalable data access in ML production.

- [Azure ND GB300 v6 Virtual Machines: General Availability and Next-Gen AI Infrastructure](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/azure-nd-gb300-v6-now-generally-available-hyper-optimized-for/ba-p/4469475)
- [Announcing Public Preview of AMLFS 20: New Azure Managed Lustre SKU for AI and HPC](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/announcing-the-public-preview-of-amlfs-20-azure-managed-lustre/ba-p/4470665)

### Model Development, Deployment, and Optimization Tools

Microsoft Foundry and Azure ML are focusing on seamless model development and production deployment, helping teams standardize their ML pipelines and cover scenarios like reinforcement learning and intelligent agent deployment. Sessions and tutorials explore metric evaluation, reliability testing, and parameter tuning for Retrieval-Augmented Generation (RAG) agents.

Windows ML updates show ongoing work to enable local AI inference using ONNX Runtime, supporting privacy and low-latency requirements, following previous guidance for regulated environments.

- [Training and Deploying Reasoning Models with Microsoft Foundry and Azure ML](/ai/videos/training-and-deploying-reasoning-models-with-microsoft-foundry-and-azure-ml)
- [Debugging and Optimizing RAG Agents in Microsoft Foundry](https://devblogs.microsoft.com/foundry/how-to-debug-and-optimize-rag-agents-in-azure-ai-foundry/)
- [Deploying Local AI Models in Enterprise with Windows ML](/ai/videos/deploying-local-ai-models-in-enterprise-with-windows-ml)

### Microsoft Fabric: Enhanced AI and Data Engineering Capabilities

Microsoft Fabric’s latest updates provide more flexible AI integration, with features like ai.embed() (now GA) and support for models from GPT-5, Claude, LLaMA, Azure OpenAI, and AI Foundry. These tools bring AI-powered workflows into common data engineering platforms, facilitating new uses for PySpark, pandas, and hybrid agent workflows.

Updates for event streaming, data clustering, and endpoint management make it easier to unify analytics workloads and speed up real-time processing with KQL/SQL support. dbt Jobs integration expands on recent improvements to data transformation and validation in Fabric.

- [Microsoft Fabric AI Functions: Enhanced Features Now Generally Available](https://blog.fabric.microsoft.com/en-US/blog/29826/)
- [Eventhouse Endpoint Arrives for Microsoft Fabric Data Warehouse](https://blog.fabric.microsoft.com/en-US/blog/introducing-eventhouse-endpoint-for-fabric-data-warehouse-real-time-analytics-unified-architecture/)
- [Announcing Data Clustering in Microsoft Fabric Data Warehouse](https://blog.fabric.microsoft.com/en-US/blog/announcing-data-clustering-in-fabric-data-warehouse-preview/)
- [Integrating dbt Jobs with Microsoft Fabric for Scalable SQL Transformations (Preview)](https://blog.fabric.microsoft.com/en-US/blog/dbt-job-in-microsoft-fabric-ship-trustworthy-sql-models-faster-preview/)

### Data Quality, Analytics, and Platform Integration

Following up on historical dataset modernization, this week’s content provides more strategies for proactive data quality management, supporting cleaner ML pipelines for any cloud setup.

Further coverage shows Azure Databricks and SAP Business Data Cloud links for modern analytics, with stories about Delta Sharing, agent-based automation, and Power BI integrations that help connect disparate data sources and expand AI development.

- [Continuous Data Quality Optimization for Better AI Output](https://dellenny.com/continuous-data-quality-optimization-for-ai-the-essential-guide/)
- [Modern Data Analytics and AI with SAP Databricks on Azure](/ai/videos/modern-data-analytics-and-ai-with-sap-databricks-on-azure)',
     excerpt      = 'The machine learning focus this week is on more scalable compute, enhanced platform features, and better operational tools from cloud and enterprise providers. Azure rolled out ND GB300 v6 VMs, while Microsoft Fabric announced further improvements in its AI and data engineering offerings. Aspects like data quality, model deployment, and performance optimization remain front and center, reflecting an ongoing move to scalable and high-throughput ML infrastructure.',
     content_hash = md5('The machine learning focus this week is on more scalable compute, enhanced platform features, and better operational tools from cloud and enterprise providers. Azure rolled out ND GB300 v6 VMs, while Microsoft Fabric announced further improvements in its AI and data engineering offerings. Aspects like data quality, model deployment, and performance optimization remain front and center, reflecting an ongoing move to scalable and high-throughput ML infrastructure.

<!--excerpt_end-->

## ML

### Azure AI Compute and Infrastructure

Azure has released the ND GB300 v6 VMs, which include NVIDIA GB300 NVL72 GPUs, Grace CPUs, and fast InfiniBand networking built for large-scale training and inference. These VMs integrate with Azure CycleCloud, Batch, and AKS, building on existing solutions for orchestrating AI workloads.

The AMLFS 20 (Azure Managed Lustre) SKU delivers bigger namespaces and higher metadata throughput for high-performance workloads, meeting the needs of fast, scalable data access in ML production.

- [Azure ND GB300 v6 Virtual Machines: General Availability and Next-Gen AI Infrastructure](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/azure-nd-gb300-v6-now-generally-available-hyper-optimized-for/ba-p/4469475)
- [Announcing Public Preview of AMLFS 20: New Azure Managed Lustre SKU for AI and HPC](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/announcing-the-public-preview-of-amlfs-20-azure-managed-lustre/ba-p/4470665)

### Model Development, Deployment, and Optimization Tools

Microsoft Foundry and Azure ML are focusing on seamless model development and production deployment, helping teams standardize their ML pipelines and cover scenarios like reinforcement learning and intelligent agent deployment. Sessions and tutorials explore metric evaluation, reliability testing, and parameter tuning for Retrieval-Augmented Generation (RAG) agents.

Windows ML updates show ongoing work to enable local AI inference using ONNX Runtime, supporting privacy and low-latency requirements, following previous guidance for regulated environments.

- [Training and Deploying Reasoning Models with Microsoft Foundry and Azure ML](/ai/videos/training-and-deploying-reasoning-models-with-microsoft-foundry-and-azure-ml)
- [Debugging and Optimizing RAG Agents in Microsoft Foundry](https://devblogs.microsoft.com/foundry/how-to-debug-and-optimize-rag-agents-in-azure-ai-foundry/)
- [Deploying Local AI Models in Enterprise with Windows ML](/ai/videos/deploying-local-ai-models-in-enterprise-with-windows-ml)

### Microsoft Fabric: Enhanced AI and Data Engineering Capabilities

Microsoft Fabric’s latest updates provide more flexible AI integration, with features like ai.embed() (now GA) and support for models from GPT-5, Claude, LLaMA, Azure OpenAI, and AI Foundry. These tools bring AI-powered workflows into common data engineering platforms, facilitating new uses for PySpark, pandas, and hybrid agent workflows.

Updates for event streaming, data clustering, and endpoint management make it easier to unify analytics workloads and speed up real-time processing with KQL/SQL support. dbt Jobs integration expands on recent improvements to data transformation and validation in Fabric.

- [Microsoft Fabric AI Functions: Enhanced Features Now Generally Available](https://blog.fabric.microsoft.com/en-US/blog/29826/)
- [Eventhouse Endpoint Arrives for Microsoft Fabric Data Warehouse](https://blog.fabric.microsoft.com/en-US/blog/introducing-eventhouse-endpoint-for-fabric-data-warehouse-real-time-analytics-unified-architecture/)
- [Announcing Data Clustering in Microsoft Fabric Data Warehouse](https://blog.fabric.microsoft.com/en-US/blog/announcing-data-clustering-in-fabric-data-warehouse-preview/)
- [Integrating dbt Jobs with Microsoft Fabric for Scalable SQL Transformations (Preview)](https://blog.fabric.microsoft.com/en-US/blog/dbt-job-in-microsoft-fabric-ship-trustworthy-sql-models-faster-preview/)

### Data Quality, Analytics, and Platform Integration

Following up on historical dataset modernization, this week’s content provides more strategies for proactive data quality management, supporting cleaner ML pipelines for any cloud setup.

Further coverage shows Azure Databricks and SAP Business Data Cloud links for modern analytics, with stories about Delta Sharing, agent-based automation, and Power BI integrations that help connect disparate data sources and expand AI development.

- [Continuous Data Quality Optimization for Better AI Output](https://dellenny.com/continuous-data-quality-optimization-for-ai-the-essential-guide/)
- [Modern Data Analytics and AI with SAP Databricks on Azure](/ai/videos/modern-data-analytics-and-ai-with-sap-databricks-on-azure)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'ml'
  AND slug                 = 'weekly-ml-roundup-2025-11-24';

-- weekly-ml-roundup-2025-12-01  (2025-12-01)
UPDATE content_items
SET  content      = 'At Ignite, machine learning updates centered on developer efficiency and fine-tuning at scale. This week’s coverage spotlights unified infrastructure, data platform improvements, and production agent guidance, all reinforcing the pattern toward integrated AI solutions, tuned models, and enterprise scaling within Azure.

<!--excerpt_end-->

## ML

### Microsoft Foundry and AI Agent Fine-Tuning

Furthering last week’s focus on custom workflows and model integration, Microsoft Foundry’s recent session covers all steps for producing and deploying tuned AI for real-world applications. This builds on Microsoft’s goal of making advanced ML techniques more widely available.

The session highlights Azure OpenAI and open-source models, with concrete examples using GPT-5 and O4 Mini. Synthetic data generation from Swagger specs also features heavily, supporting the need for robust training sets. Demos show how multiple agents collaborate to create, test, and improve synthetic data, increasing system reliability and business flexibility.

The ‘Navigator’ scenario illustrates how Foundry-powered agents process millions of contracts per day, underlining measurable benefits for both technical teams and leadership. Covered topics include model selection, API integration, and production deployment strategies, directly supporting earlier work in orchestration and ML.NET. For Azure or local teams, these guides bring ML workflows to greater maturity and scale.

- [AI Fine-Tuning in Microsoft Foundry: Building Production-Ready Agents](/ai/videos/ai-fine-tuning-in-microsoft-foundry-building-production-ready-agents)

### Azure Databricks: Unified Data and AI Ecosystem

Azure Databricks was featured as a unified analytics solution with extended integration in this week’s news. Tutorials cover new agent tools, such as Genie for rapid creation, Knowledge Assistant for management, and Multi-Agent Supervisor for routing—further supporting persistent workflow state and semantic data practices discussed previously.

The Databricks Connector, now improved for Power BI and Microsoft Apps, supports real-time data integration and workflow automation. The update to Databricks’ security tools—highlighted by Unity Catalog—matches the ongoing enterprise push for monitoring and compliance. Demonstrations, like EyeFi, reinforce Databricks’ expanding use in large organizations.

- [Accelerate Data and AI Transformation with Azure Databricks](/ai/videos/accelerate-data-and-ai-transformation-with-azure-databricks)

### Pushing the Boundaries: Azure AI Supercomputing Infrastructure

This week’s coverage dives into Azure’s updated supercomputing resources, with a focus on validating multi-billion parameter models using new GPU hardware (GB200/300, H100), advanced networking, and storage—building on past improvements in compute capacity.

Methodology guides for system inspection, performance tuning, and validation follow last week’s narrative around reliability and Azure’s blend of open source and built-in tooling. New GB300 GPUs expand capacity for growing models, and case studies (such as LLAMA and GRAC 314B) show Azure’s evolving capability for deployment and operations at scale.

- [Pushing Limits of Supercomputing Innovation on Azure AI Infrastructure](/ai/videos/pushing-limits-of-supercomputing-innovation-on-azure-ai-infrastructure)',
     excerpt      = 'At Ignite, machine learning updates centered on developer efficiency and fine-tuning at scale. This week’s coverage spotlights unified infrastructure, data platform improvements, and production agent guidance, all reinforcing the pattern toward integrated AI solutions, tuned models, and enterprise scaling within Azure.',
     content_hash = md5('At Ignite, machine learning updates centered on developer efficiency and fine-tuning at scale. This week’s coverage spotlights unified infrastructure, data platform improvements, and production agent guidance, all reinforcing the pattern toward integrated AI solutions, tuned models, and enterprise scaling within Azure.

<!--excerpt_end-->

## ML

### Microsoft Foundry and AI Agent Fine-Tuning

Furthering last week’s focus on custom workflows and model integration, Microsoft Foundry’s recent session covers all steps for producing and deploying tuned AI for real-world applications. This builds on Microsoft’s goal of making advanced ML techniques more widely available.

The session highlights Azure OpenAI and open-source models, with concrete examples using GPT-5 and O4 Mini. Synthetic data generation from Swagger specs also features heavily, supporting the need for robust training sets. Demos show how multiple agents collaborate to create, test, and improve synthetic data, increasing system reliability and business flexibility.

The ‘Navigator’ scenario illustrates how Foundry-powered agents process millions of contracts per day, underlining measurable benefits for both technical teams and leadership. Covered topics include model selection, API integration, and production deployment strategies, directly supporting earlier work in orchestration and ML.NET. For Azure or local teams, these guides bring ML workflows to greater maturity and scale.

- [AI Fine-Tuning in Microsoft Foundry: Building Production-Ready Agents](/ai/videos/ai-fine-tuning-in-microsoft-foundry-building-production-ready-agents)

### Azure Databricks: Unified Data and AI Ecosystem

Azure Databricks was featured as a unified analytics solution with extended integration in this week’s news. Tutorials cover new agent tools, such as Genie for rapid creation, Knowledge Assistant for management, and Multi-Agent Supervisor for routing—further supporting persistent workflow state and semantic data practices discussed previously.

The Databricks Connector, now improved for Power BI and Microsoft Apps, supports real-time data integration and workflow automation. The update to Databricks’ security tools—highlighted by Unity Catalog—matches the ongoing enterprise push for monitoring and compliance. Demonstrations, like EyeFi, reinforce Databricks’ expanding use in large organizations.

- [Accelerate Data and AI Transformation with Azure Databricks](/ai/videos/accelerate-data-and-ai-transformation-with-azure-databricks)

### Pushing the Boundaries: Azure AI Supercomputing Infrastructure

This week’s coverage dives into Azure’s updated supercomputing resources, with a focus on validating multi-billion parameter models using new GPU hardware (GB200/300, H100), advanced networking, and storage—building on past improvements in compute capacity.

Methodology guides for system inspection, performance tuning, and validation follow last week’s narrative around reliability and Azure’s blend of open source and built-in tooling. New GB300 GPUs expand capacity for growing models, and case studies (such as LLAMA and GRAC 314B) show Azure’s evolving capability for deployment and operations at scale.

- [Pushing Limits of Supercomputing Innovation on Azure AI Infrastructure](/ai/videos/pushing-limits-of-supercomputing-innovation-on-azure-ai-infrastructure)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'ml'
  AND slug                 = 'weekly-ml-roundup-2025-12-01';

-- weekly-ml-roundup-2025-12-08  (2025-12-08)
UPDATE content_items
SET  content      = 'Updates this week cover serverless workspaces, secure pipeline automation, and AI-enabled customer segmentation.

<!--excerpt_end-->

## ML

### Serverless Workspaces in Azure Databricks

Azure Databricks now supports public preview for Serverless workspaces, which remove the need for manual VNet and cluster setup. These changes support persistent data practices and are governed through Unity Catalog, with improvements in budget controls and security. Serverless egress and Private Link options increase compliance, with Python and SQL workflows now supported for scaling secure ML operations.

- [Serverless Workspaces Are Now Available in Azure Databricks](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/serverless-workspaces-are-live-in-azure-databricks/ba-p/4474712)

### Secure Automation for Notebooks in Fabric Data Factory Pipelines

Service Principal and Workspace Identity authentication are now available for running notebooks in Fabric Data Factory pipelines. This change reduces manual configuration, improves reliability, helps centralize identity management, and creates more robust production environments.

- [Run Notebooks in Pipelines with Service Principal or Workspace Identity](https://blog.fabric.microsoft.com/en-US/blog/run-notebooks-in-pipelines-with-service-principal-or-workspace-identity/)

### AI-Enabled Customer Segmentation Architecture

A joint case study from UCLA Anderson and Microsoft details a system for dynamic customer segmentation, helping B2B software businesses better handle resource allocation. The technical solution uses clustering, ML models like CatBoost and XGBoost, and an LLM assistant for workflow transparency. Azure handles orchestration and pipeline reliability for deployment.

- [KPI-Driven, AI-Enabled Tiering Architecture for Microsoft’s Global B2B Business](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/architecting-the-next-generation-customer-tiering-system/ba-p/4475326)',
     excerpt      = 'Updates this week cover serverless workspaces, secure pipeline automation, and AI-enabled customer segmentation.',
     content_hash = md5('Updates this week cover serverless workspaces, secure pipeline automation, and AI-enabled customer segmentation.

<!--excerpt_end-->

## ML

### Serverless Workspaces in Azure Databricks

Azure Databricks now supports public preview for Serverless workspaces, which remove the need for manual VNet and cluster setup. These changes support persistent data practices and are governed through Unity Catalog, with improvements in budget controls and security. Serverless egress and Private Link options increase compliance, with Python and SQL workflows now supported for scaling secure ML operations.

- [Serverless Workspaces Are Now Available in Azure Databricks](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/serverless-workspaces-are-live-in-azure-databricks/ba-p/4474712)

### Secure Automation for Notebooks in Fabric Data Factory Pipelines

Service Principal and Workspace Identity authentication are now available for running notebooks in Fabric Data Factory pipelines. This change reduces manual configuration, improves reliability, helps centralize identity management, and creates more robust production environments.

- [Run Notebooks in Pipelines with Service Principal or Workspace Identity](https://blog.fabric.microsoft.com/en-US/blog/run-notebooks-in-pipelines-with-service-principal-or-workspace-identity/)

### AI-Enabled Customer Segmentation Architecture

A joint case study from UCLA Anderson and Microsoft details a system for dynamic customer segmentation, helping B2B software businesses better handle resource allocation. The technical solution uses clustering, ML models like CatBoost and XGBoost, and an LLM assistant for workflow transparency. Azure handles orchestration and pipeline reliability for deployment.

- [KPI-Driven, AI-Enabled Tiering Architecture for Microsoft’s Global B2B Business](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/architecting-the-next-generation-customer-tiering-system/ba-p/4475326)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'ml'
  AND slug                 = 'weekly-ml-roundup-2025-12-08';

-- weekly-ml-roundup-2025-12-15  (2025-12-15)
UPDATE content_items
SET  content      = 'Updates in ML this week delivered more productive workflows and better data quality for large-scale and enterprise deployments. Microsoft improved ML engineering, pipeline automation, and operational tooling with Spark and agent frameworks.

<!--excerpt_end-->

## ML

### Microsoft Fabric Ecosystem: Engineering, Data Quality, and Automation

This week, Microsoft Fabric improvements target automation and barrier reduction in ML workflows. A step-by-step guide demonstrates how to automate data quality checks for every layer of a Medallion Architecture using Great Expectations for reusable, testable pipelines. The guide also explains how to integrate results with incident response and analytics workflows.

The new Forecasting Service allows for nearly instant Spark notebook startup, building on the recent focus on serverless infrastructure and cost efficiency. Articles this week explain dynamic scheduling and predictive scaling using Azure Cosmos DB and Data Explorer.

Variable Library is now available for Fabric Notebooks, offering centralized management for secrets and configuration, supporting automation and migration across environments.

Update to Fabric Real-Time Intelligence changes how Anomaly Detector is billed—from instance-based to query-based—helping teams monitor usage and control costs more effectively.

- [From Bronze to Gold: Data Quality Strategies for ETL in Microsoft Fabric](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/from-bronze-to-gold-data-quality-strategies-for-etl-in-microsoft/ba-p/4476303)
- [How Microsoft Fabric''s Forecasting Service Makes Spark Notebooks Instant](https://blog.fabric.microsoft.com/en-US/blog/how-fabric-makes-spark-notebook-feel-instant-proactive-resource-provisioning-for-scalable-data-science-data-engineering/)
- [Variable Library Support Now Available in Microsoft Fabric Notebooks](https://blog.fabric.microsoft.com/en-US/blog/variable-library-support-in-notebook-now-generally-available/)
- [Understanding Billing for Anomaly Detector in Microsoft Fabric’s Real-Time Intelligence](https://blog.fabric.microsoft.com/en-US/blog/billing-for-anomaly-detector-in-real-time-intelligence/)

### Reinforcement Learning in AI Agents: Agent Lightning Open Source Release

Microsoft Research Asia has open-sourced Agent Lightning, a framework designed for reinforcement learning (RL) with support for decoupled RL training and agent execution. The platform enables workflow optimization for existing LLMAgent frameworks, supports hierarchical RL for complex tasks, and allows flexible plug-in of new RL algorithms.

Agent Lightning streamlines logging, supports both GPU and CPU, and increases accuracy in a range of scenarios including text-to-SQL, RAG, and multi-agent QA. Continuous learning is planned. Development is underway to offer better prompt optimization and easy RL integration in live AI applications.

- [Agent Lightning: Making AI Agents Smarter Without Rewriting Code](https://www.microsoft.com/en-us/research/blog/agent-lightning-adding-reinforcement-learning-to-ai-agents-without-code-rewrites/)

### AI for Biomedical Workflows: GigaTIME Spatial Proteomics Platform

GigaTIME, a spatial proteomics platform, lets scientists use machine learning on digital slides to measure protein distributions at scale, removing the need for expensive assays. It supports broad analysis and rapid hypothesis generation for cancer research, representing practical ML for biomedical challenges.

- [AI-Powered Spatial Proteomics Platform GigaTIME Accelerates Cancer Discovery](https://www.linkedin.com/posts/satyanadella_ai-generated-population-scale-is-changing-activity-7404189540757831680-VtoO)',
     excerpt      = 'Updates in ML this week delivered more productive workflows and better data quality for large-scale and enterprise deployments. Microsoft improved ML engineering, pipeline automation, and operational tooling with Spark and agent frameworks.',
     content_hash = md5('Updates in ML this week delivered more productive workflows and better data quality for large-scale and enterprise deployments. Microsoft improved ML engineering, pipeline automation, and operational tooling with Spark and agent frameworks.

<!--excerpt_end-->

## ML

### Microsoft Fabric Ecosystem: Engineering, Data Quality, and Automation

This week, Microsoft Fabric improvements target automation and barrier reduction in ML workflows. A step-by-step guide demonstrates how to automate data quality checks for every layer of a Medallion Architecture using Great Expectations for reusable, testable pipelines. The guide also explains how to integrate results with incident response and analytics workflows.

The new Forecasting Service allows for nearly instant Spark notebook startup, building on the recent focus on serverless infrastructure and cost efficiency. Articles this week explain dynamic scheduling and predictive scaling using Azure Cosmos DB and Data Explorer.

Variable Library is now available for Fabric Notebooks, offering centralized management for secrets and configuration, supporting automation and migration across environments.

Update to Fabric Real-Time Intelligence changes how Anomaly Detector is billed—from instance-based to query-based—helping teams monitor usage and control costs more effectively.

- [From Bronze to Gold: Data Quality Strategies for ETL in Microsoft Fabric](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/from-bronze-to-gold-data-quality-strategies-for-etl-in-microsoft/ba-p/4476303)
- [How Microsoft Fabric''s Forecasting Service Makes Spark Notebooks Instant](https://blog.fabric.microsoft.com/en-US/blog/how-fabric-makes-spark-notebook-feel-instant-proactive-resource-provisioning-for-scalable-data-science-data-engineering/)
- [Variable Library Support Now Available in Microsoft Fabric Notebooks](https://blog.fabric.microsoft.com/en-US/blog/variable-library-support-in-notebook-now-generally-available/)
- [Understanding Billing for Anomaly Detector in Microsoft Fabric’s Real-Time Intelligence](https://blog.fabric.microsoft.com/en-US/blog/billing-for-anomaly-detector-in-real-time-intelligence/)

### Reinforcement Learning in AI Agents: Agent Lightning Open Source Release

Microsoft Research Asia has open-sourced Agent Lightning, a framework designed for reinforcement learning (RL) with support for decoupled RL training and agent execution. The platform enables workflow optimization for existing LLMAgent frameworks, supports hierarchical RL for complex tasks, and allows flexible plug-in of new RL algorithms.

Agent Lightning streamlines logging, supports both GPU and CPU, and increases accuracy in a range of scenarios including text-to-SQL, RAG, and multi-agent QA. Continuous learning is planned. Development is underway to offer better prompt optimization and easy RL integration in live AI applications.

- [Agent Lightning: Making AI Agents Smarter Without Rewriting Code](https://www.microsoft.com/en-us/research/blog/agent-lightning-adding-reinforcement-learning-to-ai-agents-without-code-rewrites/)

### AI for Biomedical Workflows: GigaTIME Spatial Proteomics Platform

GigaTIME, a spatial proteomics platform, lets scientists use machine learning on digital slides to measure protein distributions at scale, removing the need for expensive assays. It supports broad analysis and rapid hypothesis generation for cancer research, representing practical ML for biomedical challenges.

- [AI-Powered Spatial Proteomics Platform GigaTIME Accelerates Cancer Discovery](https://www.linkedin.com/posts/satyanadella_ai-generated-population-scale-is-changing-activity-7404189540757831680-VtoO)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'ml'
  AND slug                 = 'weekly-ml-roundup-2025-12-15';

-- weekly-ml-roundup-2025-12-22  (2025-12-22)
UPDATE content_items
SET  content      = 'Fabric Runtime 2.0 debuts with Apache Spark 4.0, while environment library management runs 2.5x faster and Python Spark sessions start 70% quicker.

<!--excerpt_end-->

## ML

### Microsoft Fabric ML Platform Advances

Fabric Runtime 2.0 (experimental) debuts with Apache Spark 4.0 for scalable distributed processing. Additional upgrades include Java 21, Scala 2.13, Python 3.12, and Delta Lake 4.0, aiding migration and analysis speed. The year-end review covers improvements in platform security, migration help, Copilot access, improved SQL/KQL tooling, and consistent DevOps support—summarizing a year centered on usability and developer needs.

- [Fabric Runtime 2.0 Experimental Preview: Scalable Data Engineering with Spark](https://blog.fabric.microsoft.com/en-US/blog/fabric-runtime-2-0-experimental-public-preview/)
- [Microsoft Fabric 2025 Recap: Unified Data and AI Innovations](https://blog.fabric.microsoft.com/en-US/blog/microsoft-fabric-2025-holiday-recap-unified-data-an-ai-innovation/)

### Performance, Reliability, and Security in ML Workflows

Fabric increases productivity with environment library management running up to 2.5 times faster for custom libraries, and Python Spark session startups now completing 70% quicker. New lightweight install modes are inbound for small deployments. Spark job orchestration supports Service Principal and Workspace Identity authentication, reducing reliance on user credentials in production pipelines. Updated documentation simplifies setup and migration.

- [Fabric Environment Library Management Performance Improvements for Developers](https://blog.fabric.microsoft.com/en-US/blog/fabric-environment-library-management-performance-improvement/)
- [Run Spark Job Definitions in Pipelines with Service Principal or Workspace Identity](https://blog.fabric.microsoft.com/en-US/blog/run-spark-job-definitions-in-pipelines-with-service-principal-or-workspace-identity/)

### Evaluation and Best Practices for Azure-based Document AI Pipelines

A practical guide outlines deploying and evaluating document AI workflows with Azure. The resource covers building a ground truth set, technical steps (OCR, labeling, retrieval), error assessment, and performance tuning with continuous monitoring. It includes architecture diagrams and code examples for developers working on enterprise IDP projects.

- [Evaluation Frameworks for Document Pipelines Using Azure AI & Search](https://techcommunity.microsoft.com/t5/azure-architecture-blog/from-large-semi-structured-docs-to-actionable-data-in-depth/ba-p/4474060)',
     excerpt      = 'Fabric Runtime 2.0 debuts with Apache Spark 4.0, while environment library management runs 2.5x faster and Python Spark sessions start 70% quicker.',
     content_hash = md5('Fabric Runtime 2.0 debuts with Apache Spark 4.0, while environment library management runs 2.5x faster and Python Spark sessions start 70% quicker.

<!--excerpt_end-->

## ML

### Microsoft Fabric ML Platform Advances

Fabric Runtime 2.0 (experimental) debuts with Apache Spark 4.0 for scalable distributed processing. Additional upgrades include Java 21, Scala 2.13, Python 3.12, and Delta Lake 4.0, aiding migration and analysis speed. The year-end review covers improvements in platform security, migration help, Copilot access, improved SQL/KQL tooling, and consistent DevOps support—summarizing a year centered on usability and developer needs.

- [Fabric Runtime 2.0 Experimental Preview: Scalable Data Engineering with Spark](https://blog.fabric.microsoft.com/en-US/blog/fabric-runtime-2-0-experimental-public-preview/)
- [Microsoft Fabric 2025 Recap: Unified Data and AI Innovations](https://blog.fabric.microsoft.com/en-US/blog/microsoft-fabric-2025-holiday-recap-unified-data-an-ai-innovation/)

### Performance, Reliability, and Security in ML Workflows

Fabric increases productivity with environment library management running up to 2.5 times faster for custom libraries, and Python Spark session startups now completing 70% quicker. New lightweight install modes are inbound for small deployments. Spark job orchestration supports Service Principal and Workspace Identity authentication, reducing reliance on user credentials in production pipelines. Updated documentation simplifies setup and migration.

- [Fabric Environment Library Management Performance Improvements for Developers](https://blog.fabric.microsoft.com/en-US/blog/fabric-environment-library-management-performance-improvement/)
- [Run Spark Job Definitions in Pipelines with Service Principal or Workspace Identity](https://blog.fabric.microsoft.com/en-US/blog/run-spark-job-definitions-in-pipelines-with-service-principal-or-workspace-identity/)

### Evaluation and Best Practices for Azure-based Document AI Pipelines

A practical guide outlines deploying and evaluating document AI workflows with Azure. The resource covers building a ground truth set, technical steps (OCR, labeling, retrieval), error assessment, and performance tuning with continuous monitoring. It includes architecture diagrams and code examples for developers working on enterprise IDP projects.

- [Evaluation Frameworks for Document Pipelines Using Azure AI & Search](https://techcommunity.microsoft.com/t5/azure-architecture-blog/from-large-semi-structured-docs-to-actionable-data-in-depth/ba-p/4474060)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'ml'
  AND slug                 = 'weekly-ml-roundup-2025-12-22';

-- SKIP 2025-12-29 : no 'Machine Learning/ML' block found in source (weekly-ai-roundup-2025-12-29)

-- SKIP 2026-01-05 : no 'Machine Learning/ML' block found in source (weekly-ai-roundup-2026-01-05)

-- weekly-ml-roundup-2026-01-12  (2026-01-12)
UPDATE content_items
SET  content      = 'Machine learning updates focus on deploying AI securely and at scale. Microsoft adds new local embedding features for semantic search and retrieval augmented generation (RAG) in analytics and expands deep learning applications for autonomous vehicles through the cloud.

<!--excerpt_end-->

## ML

### Local Embedding Generation in Fabric Eventhouse

Microsoft enables text embedding creation in the Kusto Python sandbox within Fabric Eventhouse using Small Language Models (SLMs) such as jina-v2-small and e5-small-v2, via the slm_embeddings_fl() function.

Previously, developers needed Azure OpenAI endpoints for embeddings, which added dependency on remote APIs and could bring latency, cost, and privacy limitations. Now, local inference allows for lower overhead, reduced latency, and simpler compliance—improving scalability and automation for data processing teams.

Documentation provides step-by-step KQL and Python examples for embedding creation, real-time search, and automated processing, supporting efficient, secure AI adoption in Azure environments.

- [Create Embeddings in Fabric Eventhouse with Built-in Small Language Models (SLMs)](https://blog.fabric.microsoft.com/en-US/blog/create-embeddings-in-fabric-eventhouse-with-built-in-small-language-models-slms/)

### Deep Learning for Autonomous Vehicles on Azure

Wayve leverages Azure for distributed training and large-scale deployment of deep learning models in autonomous vehicles, extending advanced ML into connected mobility. Azure''s infrastructure supports big data handling and fast model rollout across GPU and TPU clusters, building on cloud-enabled AI operations for industrial applications.

- [AI that drives change: Wayve rewrites self-driving playbook with deep learning in Azure](https://news.microsoft.com/source/emea/features/ai-that-drives-change-wayve-rewrites-self-driving-playbook-with-deep-learning-in-azure/)',
     excerpt      = 'Machine learning updates focus on deploying AI securely and at scale. Microsoft adds new local embedding features for semantic search and retrieval augmented generation (RAG) in analytics and expands deep learning applications for autonomous vehicles through the cloud.',
     content_hash = md5('Machine learning updates focus on deploying AI securely and at scale. Microsoft adds new local embedding features for semantic search and retrieval augmented generation (RAG) in analytics and expands deep learning applications for autonomous vehicles through the cloud.

<!--excerpt_end-->

## ML

### Local Embedding Generation in Fabric Eventhouse

Microsoft enables text embedding creation in the Kusto Python sandbox within Fabric Eventhouse using Small Language Models (SLMs) such as jina-v2-small and e5-small-v2, via the slm_embeddings_fl() function.

Previously, developers needed Azure OpenAI endpoints for embeddings, which added dependency on remote APIs and could bring latency, cost, and privacy limitations. Now, local inference allows for lower overhead, reduced latency, and simpler compliance—improving scalability and automation for data processing teams.

Documentation provides step-by-step KQL and Python examples for embedding creation, real-time search, and automated processing, supporting efficient, secure AI adoption in Azure environments.

- [Create Embeddings in Fabric Eventhouse with Built-in Small Language Models (SLMs)](https://blog.fabric.microsoft.com/en-US/blog/create-embeddings-in-fabric-eventhouse-with-built-in-small-language-models-slms/)

### Deep Learning for Autonomous Vehicles on Azure

Wayve leverages Azure for distributed training and large-scale deployment of deep learning models in autonomous vehicles, extending advanced ML into connected mobility. Azure''s infrastructure supports big data handling and fast model rollout across GPU and TPU clusters, building on cloud-enabled AI operations for industrial applications.

- [AI that drives change: Wayve rewrites self-driving playbook with deep learning in Azure](https://news.microsoft.com/source/emea/features/ai-that-drives-change-wayve-rewrites-self-driving-playbook-with-deep-learning-in-azure/)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'ml'
  AND slug                 = 'weekly-ml-roundup-2026-01-12';

-- weekly-ml-roundup-2026-01-19  (2026-01-19)
UPDATE content_items
SET  content      = 'This week’s machine learning articles focus on efficient AI-driven workflows for industrial and research teams. There are updates on in-database semantic search, agent frameworks, industrial deployments, and research in the life sciences. Tutorials and case studies provide actionable examples and show practical adoption of advanced ML tools.

<!--excerpt_end-->

## ML

### Building AI Workflows with Microsoft Agent Framework and .NET AI Stack

Building on recent themes of local embeddings and agent-based architectures, Pamela Fox’s livestream series demonstrates using the Microsoft Agent Framework in Python, with coverage of RAG agent skills, modular and reproducible AI deployments, monitoring using OpenTelemetry, and orchestration via Magentic. Evaluation with Azure AI SDK rounds out the workflow.

.NET developers can join the AI Community Standup, which now features hands-on sessions using Semantic Kernel, AI Extensions, and orchestration tools—helping the .NET community move beyond chatbot projects to deeper AI integration.

- [Python + Agents: Livestream Series on Building AI Agents with Microsoft Agent Framework](https://techcommunity.microsoft.com/t5/microsoft-developer-community/join-our-free-livestream-series-on-building-agents-in-python/ba-p/4485731)
- [.NET AI Community Standup: Building AI Apps with the New .NET AI Stack](/ai/videos/net-ai-community-standup-building-ai-apps-with-the-new-net-ai-stack)

### Industrial ML and Scientific Workflows Powered by Azure HPC and Microsoft Discovery

New case studies highlight large-scale machine learning on Azure’s HPC resources, such as Neural Concept’s industrial engineering work with Azure GPUs and storage for AI training in automotive aerodynamics. Benchmarks show efficient model development that parallels what was seen last week in deep learning rollouts.

In drug discovery, Insilico Medicine’s Nach01 model deployed via Microsoft Discovery demonstrates secure, repeatable analytics in the life sciences, drawing on Azure ML’s compliance and deployment features.

- [Neural Concept Sets New Industrial AI Benchmark on Azure HPC for Automotive Aerodynamics](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/scaling-physics-based-digital-twins-neural-concept-on-azure/ba-p/4483403)
- [AI-Native Drug Discovery using Insilico Medicine’s Nach01 Model and Microsoft Discovery](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/ai-native-drug-discovery-using-insilico-medicine-s-nach01-model/ba-p/4484497)

### Expanding Vector Search in Databases: DiskANN in Azure SQL and Fabric SQL

DiskANN now enables large-scale, fast vector search directly inside Azure SQL and Fabric SQL, building on last week’s announcement of local-embedding in the Fabric Eventhouse. This lets teams implement semantic search, classification, and content analysis at the database level for less latency and stronger privacy, without relying on outside APIs.

- [DiskANN: Vector Indexing in Azure SQL and Fabric SQL Explained](/ai/videos/diskann-vector-indexing-in-azure-sql-and-fabric-sql-explained)',
     excerpt      = 'This week’s machine learning articles focus on efficient AI-driven workflows for industrial and research teams. There are updates on in-database semantic search, agent frameworks, industrial deployments, and research in the life sciences. Tutorials and case studies provide actionable examples and show practical adoption of advanced ML tools.',
     content_hash = md5('This week’s machine learning articles focus on efficient AI-driven workflows for industrial and research teams. There are updates on in-database semantic search, agent frameworks, industrial deployments, and research in the life sciences. Tutorials and case studies provide actionable examples and show practical adoption of advanced ML tools.

<!--excerpt_end-->

## ML

### Building AI Workflows with Microsoft Agent Framework and .NET AI Stack

Building on recent themes of local embeddings and agent-based architectures, Pamela Fox’s livestream series demonstrates using the Microsoft Agent Framework in Python, with coverage of RAG agent skills, modular and reproducible AI deployments, monitoring using OpenTelemetry, and orchestration via Magentic. Evaluation with Azure AI SDK rounds out the workflow.

.NET developers can join the AI Community Standup, which now features hands-on sessions using Semantic Kernel, AI Extensions, and orchestration tools—helping the .NET community move beyond chatbot projects to deeper AI integration.

- [Python + Agents: Livestream Series on Building AI Agents with Microsoft Agent Framework](https://techcommunity.microsoft.com/t5/microsoft-developer-community/join-our-free-livestream-series-on-building-agents-in-python/ba-p/4485731)
- [.NET AI Community Standup: Building AI Apps with the New .NET AI Stack](/ai/videos/net-ai-community-standup-building-ai-apps-with-the-new-net-ai-stack)

### Industrial ML and Scientific Workflows Powered by Azure HPC and Microsoft Discovery

New case studies highlight large-scale machine learning on Azure’s HPC resources, such as Neural Concept’s industrial engineering work with Azure GPUs and storage for AI training in automotive aerodynamics. Benchmarks show efficient model development that parallels what was seen last week in deep learning rollouts.

In drug discovery, Insilico Medicine’s Nach01 model deployed via Microsoft Discovery demonstrates secure, repeatable analytics in the life sciences, drawing on Azure ML’s compliance and deployment features.

- [Neural Concept Sets New Industrial AI Benchmark on Azure HPC for Automotive Aerodynamics](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/scaling-physics-based-digital-twins-neural-concept-on-azure/ba-p/4483403)
- [AI-Native Drug Discovery using Insilico Medicine’s Nach01 Model and Microsoft Discovery](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/ai-native-drug-discovery-using-insilico-medicine-s-nach01-model/ba-p/4484497)

### Expanding Vector Search in Databases: DiskANN in Azure SQL and Fabric SQL

DiskANN now enables large-scale, fast vector search directly inside Azure SQL and Fabric SQL, building on last week’s announcement of local-embedding in the Fabric Eventhouse. This lets teams implement semantic search, classification, and content analysis at the database level for less latency and stronger privacy, without relying on outside APIs.

- [DiskANN: Vector Indexing in Azure SQL and Fabric SQL Explained](/ai/videos/diskann-vector-indexing-in-azure-sql-and-fabric-sql-explained)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'ml'
  AND slug                 = 'weekly-ml-roundup-2026-01-19';

-- weekly-ml-roundup-2026-01-26  (2026-01-26)
UPDATE content_items
SET  content      = 'This week’s machine learning updates highlight improvements to developer toolkits, analytics, and applied AI for robotics. Microsoft’s ecosystem releases streamline development, testing, and deployment for a range of ML applications.

<!--excerpt_end-->

## ML

### Microsoft Fabric: Enhanced Data Engineering, Analytics, and Performance

Building on last week’s news about ML in Fabric, these updates provide enhanced security and speed for Spark workloads via Private Endpoints, cost-saving autoscale features, and up to 4x Spark performance improvements with the Native Execution Engine. The GigaOm report recognizes Fabric’s unified feature set and includes new controls for cost, scaled SQL pool management, and additional ML connectors. Serverless processing and new OneLake capabilities support flexible analytics and engineering. Real-Time Dashboards have further speed optimizations, boosting streaming and IoT analytics up to 6x or 10x faster. Updated documentation and ongoing events keep users informed.

- [Securely Scaling Spark Data Engineering in Microsoft Fabric](/ml/videos/securely-scaling-spark-data-engineering-in-microsoft-fabric)
- [Microsoft Fabric Data Warehouse: GigaOm Radar Leader and Outperformer](https://blog.fabric.microsoft.com/en-US/blog/microsoft-fabric-data-warehouse-named-a-leader-and-outperformer-in-gigaom-radar-for-data-warehouses/)
- [Performance Improvements for Microsoft Fabric Real-Time Dashboards](https://blog.fabric.microsoft.com/en-US/blog/faster-smoother-more-delightful-real-time-dashboards-performance-improvements/)

### Physical AI Advances: Microsoft Research’s Rho-alpha Robotics Model

Rho-alpha, from Microsoft Research, applies machine learning beyond data analytics by supporting physical robotics. Its underlying system combines natural language processing, multiple sensors, and controls, and supports continuous learning from user interactions. The platform aligns with earlier discussions around Copilot’s agentic updates and best-practice monitoring. Developers in robotics, manufacturing, and real-time control gain tools as APIs and SDKs are released, showing a unified approach similar to advances in Fabric and .NET AI.

- [Introducing Rho-alpha: Microsoft Research''s Robotics Model for Physical AI](https://www.linkedin.com/posts/satyanadella_introducing-rho-alpha-the-new-robotics-model-activity-7419757666660466688-jSpD)',
     excerpt      = 'This week’s machine learning updates highlight improvements to developer toolkits, analytics, and applied AI for robotics. Microsoft’s ecosystem releases streamline development, testing, and deployment for a range of ML applications.',
     content_hash = md5('This week’s machine learning updates highlight improvements to developer toolkits, analytics, and applied AI for robotics. Microsoft’s ecosystem releases streamline development, testing, and deployment for a range of ML applications.

<!--excerpt_end-->

## ML

### Microsoft Fabric: Enhanced Data Engineering, Analytics, and Performance

Building on last week’s news about ML in Fabric, these updates provide enhanced security and speed for Spark workloads via Private Endpoints, cost-saving autoscale features, and up to 4x Spark performance improvements with the Native Execution Engine. The GigaOm report recognizes Fabric’s unified feature set and includes new controls for cost, scaled SQL pool management, and additional ML connectors. Serverless processing and new OneLake capabilities support flexible analytics and engineering. Real-Time Dashboards have further speed optimizations, boosting streaming and IoT analytics up to 6x or 10x faster. Updated documentation and ongoing events keep users informed.

- [Securely Scaling Spark Data Engineering in Microsoft Fabric](/ml/videos/securely-scaling-spark-data-engineering-in-microsoft-fabric)
- [Microsoft Fabric Data Warehouse: GigaOm Radar Leader and Outperformer](https://blog.fabric.microsoft.com/en-US/blog/microsoft-fabric-data-warehouse-named-a-leader-and-outperformer-in-gigaom-radar-for-data-warehouses/)
- [Performance Improvements for Microsoft Fabric Real-Time Dashboards](https://blog.fabric.microsoft.com/en-US/blog/faster-smoother-more-delightful-real-time-dashboards-performance-improvements/)

### Physical AI Advances: Microsoft Research’s Rho-alpha Robotics Model

Rho-alpha, from Microsoft Research, applies machine learning beyond data analytics by supporting physical robotics. Its underlying system combines natural language processing, multiple sensors, and controls, and supports continuous learning from user interactions. The platform aligns with earlier discussions around Copilot’s agentic updates and best-practice monitoring. Developers in robotics, manufacturing, and real-time control gain tools as APIs and SDKs are released, showing a unified approach similar to advances in Fabric and .NET AI.

- [Introducing Rho-alpha: Microsoft Research''s Robotics Model for Physical AI](https://www.linkedin.com/posts/satyanadella_introducing-rho-alpha-the-new-robotics-model-activity-7419757666660466688-jSpD)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'ml'
  AND slug                 = 'weekly-ml-roundup-2026-01-26';

-- weekly-ml-roundup-2026-02-02  (2026-02-02)
UPDATE content_items
SET  content      = 'This week’s ML updates cover new ways to diagnose RL workflows, agent orchestration patterns, more structured evaluation methods, and enhancements for data engineering with Microsoft Fabric. Advancements are also noted for medical imaging and multimodal RL solutions.

<!--excerpt_end-->

## ML

### Reliability and Diagnostics in Production-Scale Reinforcement Learning

Microsoft engineering teams released guidance on troubleshooting RL agent instability in production. Traditional aggregate metrics often miss rare errors, so this week’s article presents slice-aware diagnostics to flag drift and instability at the per-token level (using log-ratio percentiles and CDF drift analysis). Agent “tail growth” signals increase risk and needs mitigation.

Open-source Post-Training Toolkit features include TRL integration, a CLI, and distributed monitoring for RL systems, enabling more detailed RL debugging in production.

- [Diagnosing Instability in Production-Scale Agent Reinforcement Learning](https://devblogs.microsoft.com/engineering-at-microsoft/diagnosing-instability-in-production-scale-agent-rl/)

### Local-First Agentic Automation and Multi-Agent Orchestration

Local-first, privacy-focused agent pipelines are growing in use. This week’s hands-on guide covers a podcast automation workflow using the Agent Framework, edge-based SLMs (Ollama), and local speech for on-premise orchestration. Modular design examples show search, review, and script generation using real-time observability with DevUI. Complete code and hardware tips are included.

- [Engineering a Local-First Agentic Podcast Studio: A Deep Dive into Multi-Agent Orchestration](https://techcommunity.microsoft.com/t5/microsoft-developer-community/engineering-a-local-first-agentic-podcast-studio-a-deep-dive/ba-p/4488947)

### Streamlined Model Evaluation and Selection with Microsoft Foundry

Model evaluation is now easier using Microsoft Foundry and GitHub Copilot. The step-by-step guide describes forming datasets, running repeatable benchmarks with metrics like F1/METEOR, and analyzing results using the Python SDK and Jupyter. Debugging and documentation guidance are provided, along with pointers to Foundry and Azure AI model leaderboard resources.

- [Evaluating AI Models with Microsoft Foundry and GitHub Copilot](/github-copilot/videos/evaluating-ai-models-with-microsoft-foundry-and-github-copilot)

### Data Engineering and Platform Operations with Microsoft Fabric

Fabric introduces workspace-level surge protection, minimizing the risk of resource spikes by limiting job concurrency and allowing exemptions for important workloads. This complements last week’s resource management changes for better cost control.

The new "Get Data with Cloud Connection" feature in Fabric Notebooks streamlines secure connections to cloud sources and provides code snippets, making developer workflows faster.

On-premises Data Gateway (January 2026 release) improves connectivity between CSV, Fabric, and Power BI, syncing with Power BI Desktop for easier data pipelines.

A new guide explains robust real-time pipelines in Fabric, including strategies for data validation, lag monitoring, network planning, logging, and clear ownership—all aimed at reducing pipeline downtime in complex environments.

- [Workspace-Level Surge Protection in Microsoft Fabric (Preview)](https://blog.fabric.microsoft.com/en-US/blog/surge-protection-gets-smarter-introducing-workspace-level-controls-preview/)
- [Fabric Connection inside Notebook (Preview)](https://blog.fabric.microsoft.com/en-US/blog/32822/)
- [On-premises Data Gateway January 2026 Release Overview](https://blog.fabric.microsoft.com/en-US/blog/on-premises-data-gateway-january-2026-release/)
- [Building a Reliable Real-Time Data Pipeline with Microsoft Fabric](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/building-a-reliable-real-time-data-pipeline-with-microsoft/ba-p/4489534)

### Multimodal Reinforcement Learning Advances in Medical Imaging

The GigaTIME project applies multimodal RL to radiology/pathology report writing, fusing text and image data for clearer, patient-specific documentation generation. Insights cover modeling, simulation, and automation, with actionable examples drawn from Microsoft’s latest research.

- [How AI is Learning to Write Smarter Medical Imaging Reports](https://www.microsoft.com/en-us/research/blog/unirg-scaling-medical-imaging-report-generation-with-multimodal-reinforcement-learning/)',
     excerpt      = 'This week’s ML updates cover new ways to diagnose RL workflows, agent orchestration patterns, more structured evaluation methods, and enhancements for data engineering with Microsoft Fabric. Advancements are also noted for medical imaging and multimodal RL solutions.',
     content_hash = md5('This week’s ML updates cover new ways to diagnose RL workflows, agent orchestration patterns, more structured evaluation methods, and enhancements for data engineering with Microsoft Fabric. Advancements are also noted for medical imaging and multimodal RL solutions.

<!--excerpt_end-->

## ML

### Reliability and Diagnostics in Production-Scale Reinforcement Learning

Microsoft engineering teams released guidance on troubleshooting RL agent instability in production. Traditional aggregate metrics often miss rare errors, so this week’s article presents slice-aware diagnostics to flag drift and instability at the per-token level (using log-ratio percentiles and CDF drift analysis). Agent “tail growth” signals increase risk and needs mitigation.

Open-source Post-Training Toolkit features include TRL integration, a CLI, and distributed monitoring for RL systems, enabling more detailed RL debugging in production.

- [Diagnosing Instability in Production-Scale Agent Reinforcement Learning](https://devblogs.microsoft.com/engineering-at-microsoft/diagnosing-instability-in-production-scale-agent-rl/)

### Local-First Agentic Automation and Multi-Agent Orchestration

Local-first, privacy-focused agent pipelines are growing in use. This week’s hands-on guide covers a podcast automation workflow using the Agent Framework, edge-based SLMs (Ollama), and local speech for on-premise orchestration. Modular design examples show search, review, and script generation using real-time observability with DevUI. Complete code and hardware tips are included.

- [Engineering a Local-First Agentic Podcast Studio: A Deep Dive into Multi-Agent Orchestration](https://techcommunity.microsoft.com/t5/microsoft-developer-community/engineering-a-local-first-agentic-podcast-studio-a-deep-dive/ba-p/4488947)

### Streamlined Model Evaluation and Selection with Microsoft Foundry

Model evaluation is now easier using Microsoft Foundry and GitHub Copilot. The step-by-step guide describes forming datasets, running repeatable benchmarks with metrics like F1/METEOR, and analyzing results using the Python SDK and Jupyter. Debugging and documentation guidance are provided, along with pointers to Foundry and Azure AI model leaderboard resources.

- [Evaluating AI Models with Microsoft Foundry and GitHub Copilot](/github-copilot/videos/evaluating-ai-models-with-microsoft-foundry-and-github-copilot)

### Data Engineering and Platform Operations with Microsoft Fabric

Fabric introduces workspace-level surge protection, minimizing the risk of resource spikes by limiting job concurrency and allowing exemptions for important workloads. This complements last week’s resource management changes for better cost control.

The new "Get Data with Cloud Connection" feature in Fabric Notebooks streamlines secure connections to cloud sources and provides code snippets, making developer workflows faster.

On-premises Data Gateway (January 2026 release) improves connectivity between CSV, Fabric, and Power BI, syncing with Power BI Desktop for easier data pipelines.

A new guide explains robust real-time pipelines in Fabric, including strategies for data validation, lag monitoring, network planning, logging, and clear ownership—all aimed at reducing pipeline downtime in complex environments.

- [Workspace-Level Surge Protection in Microsoft Fabric (Preview)](https://blog.fabric.microsoft.com/en-US/blog/surge-protection-gets-smarter-introducing-workspace-level-controls-preview/)
- [Fabric Connection inside Notebook (Preview)](https://blog.fabric.microsoft.com/en-US/blog/32822/)
- [On-premises Data Gateway January 2026 Release Overview](https://blog.fabric.microsoft.com/en-US/blog/on-premises-data-gateway-january-2026-release/)
- [Building a Reliable Real-Time Data Pipeline with Microsoft Fabric](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/building-a-reliable-real-time-data-pipeline-with-microsoft/ba-p/4489534)

### Multimodal Reinforcement Learning Advances in Medical Imaging

The GigaTIME project applies multimodal RL to radiology/pathology report writing, fusing text and image data for clearer, patient-specific documentation generation. Insights cover modeling, simulation, and automation, with actionable examples drawn from Microsoft’s latest research.

- [How AI is Learning to Write Smarter Medical Imaging Reports](https://www.microsoft.com/en-us/research/blog/unirg-scaling-medical-imaging-report-generation-with-multimodal-reinforcement-learning/)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'ml'
  AND slug                 = 'weekly-ml-roundup-2026-02-02';

-- weekly-ml-roundup-2026-02-09  (2026-02-09)
UPDATE content_items
SET  content      = 'This week’s machine learning news includes more advanced Spark pipeline management, improved real-time analytics, and visual upgrade options in Microsoft Fabric and Azure Synapse.

<!--excerpt_end-->

## ML

### Smarter Pipeline Orchestration in Shared Spark Environments

Admins of Spark workloads can now use a priority-based orchestration approach using job tags like Light/Critical, Medium/High, or Heavy/Best Effort with metadata for optimized job scheduling. This strategy, compatible with Microsoft Fabric and Synapse, supports both fixed and adaptive classification. Copilot-style agents monitor and adjust workload class, reducing human input and increasing stability. Ready-to-use sample notebooks and template tools are available to get started. These changes deepen pipeline control and protection, continuing last week’s surge management efforts.

- [Smart Pipelines Orchestration: Designing Predictable Data Platforms on Shared Spark](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/smart-pipelines-orchestration-designing-predictable-data/ba-p/4491766)

### Real-Time Analytics and ML in Spark Notebooks with Eventstreams Integration

Direct Eventstreams and Spark Notebook integration (preview) in Fabric means instant access to over 30 streaming data sources—like CDC databases or brokers—right from the Real-Time Hub. PySpark code is auto-generated, Entra ID secures access, and one-click imports enable fast prototyping and migration to production. Early community feedback is encouraged. This builds on earlier additions like "Get Data with Cloud Connection," supporting a smooth transition from batching to real-time analytics.

- [Integrating Real-Time Eventstreams with Spark Notebooks in Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/bringing-together-fabric-real-time-intelligence-notebook-and-spark-structured-streaming-preview/)

### Improved Visualization for Real-Time Dashboards: Custom Series Colors

Microsoft Fabric’s dashboard series now features customizable data series colors for any chart, so teams can visually separate operational data for easier monitoring and clarity. Documentation covers usage, supporting ongoing dashboard improvements.

- [Introducing Data Series Colors: Enhanced Visualization Control in Real-Time Dashboards](https://blog.fabric.microsoft.com/en-US/blog/32713/)',
     excerpt      = 'This week’s machine learning news includes more advanced Spark pipeline management, improved real-time analytics, and visual upgrade options in Microsoft Fabric and Azure Synapse.',
     content_hash = md5('This week’s machine learning news includes more advanced Spark pipeline management, improved real-time analytics, and visual upgrade options in Microsoft Fabric and Azure Synapse.

<!--excerpt_end-->

## ML

### Smarter Pipeline Orchestration in Shared Spark Environments

Admins of Spark workloads can now use a priority-based orchestration approach using job tags like Light/Critical, Medium/High, or Heavy/Best Effort with metadata for optimized job scheduling. This strategy, compatible with Microsoft Fabric and Synapse, supports both fixed and adaptive classification. Copilot-style agents monitor and adjust workload class, reducing human input and increasing stability. Ready-to-use sample notebooks and template tools are available to get started. These changes deepen pipeline control and protection, continuing last week’s surge management efforts.

- [Smart Pipelines Orchestration: Designing Predictable Data Platforms on Shared Spark](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/smart-pipelines-orchestration-designing-predictable-data/ba-p/4491766)

### Real-Time Analytics and ML in Spark Notebooks with Eventstreams Integration

Direct Eventstreams and Spark Notebook integration (preview) in Fabric means instant access to over 30 streaming data sources—like CDC databases or brokers—right from the Real-Time Hub. PySpark code is auto-generated, Entra ID secures access, and one-click imports enable fast prototyping and migration to production. Early community feedback is encouraged. This builds on earlier additions like "Get Data with Cloud Connection," supporting a smooth transition from batching to real-time analytics.

- [Integrating Real-Time Eventstreams with Spark Notebooks in Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/bringing-together-fabric-real-time-intelligence-notebook-and-spark-structured-streaming-preview/)

### Improved Visualization for Real-Time Dashboards: Custom Series Colors

Microsoft Fabric’s dashboard series now features customizable data series colors for any chart, so teams can visually separate operational data for easier monitoring and clarity. Documentation covers usage, supporting ongoing dashboard improvements.

- [Introducing Data Series Colors: Enhanced Visualization Control in Real-Time Dashboards](https://blog.fabric.microsoft.com/en-US/blog/32713/)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'ml'
  AND slug                 = 'weekly-ml-roundup-2026-02-09';

-- weekly-ml-roundup-2026-02-16  (2026-02-16)
UPDATE content_items
SET  content      = 'This week’s ML coverage spotlights Microsoft Fabric’s expansion of analytics and machine learning, with practical routes for ML in production, fine-tuning workflows, and automated pipelines. Tools like Semantic Link and Foundry fine-tuning offer easier, AI-driven analytics and operational intelligence.

<!--excerpt_end-->

## ML

### Microsoft Fabric for ML, AI, and Operational Analytics

Microsoft Fabric now better unifies analytics, machine learning, and business reporting.

Semantic Link is now generally available, allowing a shared semantic layer for data engineering, AI, and BI to use common models. It supports semantic model updates directly from notebooks, immediate sync to Power BI, and harmonized workflows. Automation is easier with tighter SQL/Spark orchestration, while community repositories provide reusable patterns.

For IoT and streaming data, Fabric’s operational analytics uses time series dashboards with Kusto, dynamic slicing, anomaly detection, and DirectQuery for live reporting. These updates expand the platform’s ability to handle large-scale, real-time data.

ML workflows in Fabric and Power BI are progressing, letting teams run predictions in dashboards using LightGBM/SMOTE, OneLake-backed data, and MLflow for automation. The Fabric IQ platform provides the foundation for digital twins and ontologies, supporting smarter knowledge and automation development.

- [Supercharge AI, BI, and Data Engineering with Semantic Link in Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/supercharge-ai-bi-and-data-engineering-with-semantic-link-generally-available/)
- [Adaptive Time Series Visualization at Scale with Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/adaptive-time-series-visualization-at-scale-with-microsoft-fabric/)
- [Integrating Machine Learning with Power BI Reports in Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/enrich-power-bi-reports-with-machine-learning-in-microsoft-fabric/)
- [Fabric IQ Overview](/fabric-iq-overview)

### Fine-tuning and Preference Optimization for Large Language Models on Azure

A hands-on guide is available for fine-tuning enterprise LLMs with Microsoft Foundry on Azure, taking models and aligning them for organization-specific requirements and policies. The documentation covers data prep, running training jobs, and benchmarking—applying methods to use cases like PubMed summarization for health and science.

Direct Preference Optimization (DPO) is also explained, detailing how human feedback can steer LLMs toward better outputs. DPO is now in the Foundry SDK, and tutorials include example code, best practices for parameter selection, and links to new documentation.

- [Beyond the Prompt – Why and How to Fine-tune Your Own Models](https://devblogs.microsoft.com/foundry/beyond-the-prompt-why-and-how-to-fine-tune-your-own-models/)
- [DPO Fine-Tuning Using Microsoft Foundry SDK](https://devblogs.microsoft.com/foundry/dpo-fine-tuning-using-microsoft-foundry-sdk/)',
     excerpt      = 'This week’s ML coverage spotlights Microsoft Fabric’s expansion of analytics and machine learning, with practical routes for ML in production, fine-tuning workflows, and automated pipelines. Tools like Semantic Link and Foundry fine-tuning offer easier, AI-driven analytics and operational intelligence.',
     content_hash = md5('This week’s ML coverage spotlights Microsoft Fabric’s expansion of analytics and machine learning, with practical routes for ML in production, fine-tuning workflows, and automated pipelines. Tools like Semantic Link and Foundry fine-tuning offer easier, AI-driven analytics and operational intelligence.

<!--excerpt_end-->

## ML

### Microsoft Fabric for ML, AI, and Operational Analytics

Microsoft Fabric now better unifies analytics, machine learning, and business reporting.

Semantic Link is now generally available, allowing a shared semantic layer for data engineering, AI, and BI to use common models. It supports semantic model updates directly from notebooks, immediate sync to Power BI, and harmonized workflows. Automation is easier with tighter SQL/Spark orchestration, while community repositories provide reusable patterns.

For IoT and streaming data, Fabric’s operational analytics uses time series dashboards with Kusto, dynamic slicing, anomaly detection, and DirectQuery for live reporting. These updates expand the platform’s ability to handle large-scale, real-time data.

ML workflows in Fabric and Power BI are progressing, letting teams run predictions in dashboards using LightGBM/SMOTE, OneLake-backed data, and MLflow for automation. The Fabric IQ platform provides the foundation for digital twins and ontologies, supporting smarter knowledge and automation development.

- [Supercharge AI, BI, and Data Engineering with Semantic Link in Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/supercharge-ai-bi-and-data-engineering-with-semantic-link-generally-available/)
- [Adaptive Time Series Visualization at Scale with Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/adaptive-time-series-visualization-at-scale-with-microsoft-fabric/)
- [Integrating Machine Learning with Power BI Reports in Microsoft Fabric](https://blog.fabric.microsoft.com/en-US/blog/enrich-power-bi-reports-with-machine-learning-in-microsoft-fabric/)
- [Fabric IQ Overview](/fabric-iq-overview)

### Fine-tuning and Preference Optimization for Large Language Models on Azure

A hands-on guide is available for fine-tuning enterprise LLMs with Microsoft Foundry on Azure, taking models and aligning them for organization-specific requirements and policies. The documentation covers data prep, running training jobs, and benchmarking—applying methods to use cases like PubMed summarization for health and science.

Direct Preference Optimization (DPO) is also explained, detailing how human feedback can steer LLMs toward better outputs. DPO is now in the Foundry SDK, and tutorials include example code, best practices for parameter selection, and links to new documentation.

- [Beyond the Prompt – Why and How to Fine-tune Your Own Models](https://devblogs.microsoft.com/foundry/beyond-the-prompt-why-and-how-to-fine-tune-your-own-models/)
- [DPO Fine-Tuning Using Microsoft Foundry SDK](https://devblogs.microsoft.com/foundry/dpo-fine-tuning-using-microsoft-foundry-sdk/)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'ml'
  AND slug                 = 'weekly-ml-roundup-2026-02-16';

-- weekly-ml-roundup-2026-02-23  (2026-02-23)
UPDATE content_items
SET  content      = 'Microsoft releases its preview ODBC driver for Fabric Data Engineering, making it easier to connect enterprise analytics platforms and Spark SQL in Microsoft Fabric. This driver simplifies query capabilities and integrates with analytics and lakehouse solutions.

<!--excerpt_end-->

## ML

### Microsoft ODBC Driver for Fabric Data Engineering Preview

The new driver supports Spark SQL in Fabric Data Engineering, allowing .NET and Python integration for managing and querying data via OneLake. The release is ODBC 3.x compliant and works with Fabric’s security and configuration options, including credentials, tokens, certificates, and CLI authentication. Features such as session reuse, async prefetch, and proxy support enhance automation and real-time analytics

By connecting traditional tools and ML workflows, this driver supports lakehouse queries, remote ML scenarios, and analytics, and is shaped by feedback from early developer uses.

- [Introducing the Microsoft ODBC Driver for Fabric Data Engineering (Preview)](https://blog.fabric.microsoft.com/en-US/blog/microsoft-odbc-driver-for-microsoft-fabric-data-engineering-preview/)',
     excerpt      = 'Microsoft releases its preview ODBC driver for Fabric Data Engineering, making it easier to connect enterprise analytics platforms and Spark SQL in Microsoft Fabric. This driver simplifies query capabilities and integrates with analytics and lakehouse solutions.',
     content_hash = md5('Microsoft releases its preview ODBC driver for Fabric Data Engineering, making it easier to connect enterprise analytics platforms and Spark SQL in Microsoft Fabric. This driver simplifies query capabilities and integrates with analytics and lakehouse solutions.

<!--excerpt_end-->

## ML

### Microsoft ODBC Driver for Fabric Data Engineering Preview

The new driver supports Spark SQL in Fabric Data Engineering, allowing .NET and Python integration for managing and querying data via OneLake. The release is ODBC 3.x compliant and works with Fabric’s security and configuration options, including credentials, tokens, certificates, and CLI authentication. Features such as session reuse, async prefetch, and proxy support enhance automation and real-time analytics

By connecting traditional tools and ML workflows, this driver supports lakehouse queries, remote ML scenarios, and analytics, and is shaped by feedback from early developer uses.

- [Introducing the Microsoft ODBC Driver for Fabric Data Engineering (Preview)](https://blog.fabric.microsoft.com/en-US/blog/microsoft-odbc-driver-for-microsoft-fabric-data-engineering-preview/)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'ml'
  AND slug                 = 'weekly-ml-roundup-2026-02-23';

-- SKIP 2026-03-02 : no DB record for section 'ml'

-- SKIP 2026-03-09 : no DB record for section 'ml'

-- SKIP 2026-03-16 : no 'Machine Learning/ML' block found in source (weekly-ai-roundup-2026-03-16)

-- weekly-ml-roundup-2026-03-23  (2026-03-23)
UPDATE content_items
SET  content      = 'This week''s ML-adjacent data engineering updates were less about model releases and more about tightening pipelines and developer surfaces. Fabric moved Spark and notebook capabilities closer to production usage, and Azure Databricks shared a concrete pattern for consolidating near-real-time ingestion, transformation, and governance into a single Lakeflow workflow.

<!--excerpt_end-->

## ML

### Microsoft Fabric: Lakehouse pipelines, runtimes, and notebook automation

Materialized Lake Views (MLVs) GA is Fabric''s clearest move toward declarative lakehouse transforms without hand-rolled Spark ETL plus separate orchestration. The GA release focuses on refresh behavior and manageability: Fabric expands incremental refresh support across common patterns (aggregations with `GROUP BY`, left outer joins, left semi joins, CTEs) and decides per run whether incremental or full recompute is cheaper based on change volume and estimated cost. With Change Data Feed enabled by default for new MLVs, incremental processing becomes the default rather than another setting. Operationally, multi-schedule support at the lakehouse level lets you define named schedules for subsets of views (hourly gold vs six-hour lower-priority), with Fabric handling dependencies, parallelizing independent views, and centralizing errors; overlapping triggers are skipped if a refresh is already running. "Replace" allows updating an MLV definition in place without drop/recreate, preserving identity/metadata/lineage and avoiding broken dependencies. Data quality constraints get fuller reporting across refresh history, including richer expression-based constraints for PySpark-authored MLVs (multi-column expressions, arithmetic/functions, session-scoped Python UDFs), while PySpark authoring itself is preview; PySpark-authored MLVs still full-refresh for now.

Fabric Runtime 2.0 entered preview as a new baseline for Spark engineering and science workloads, and it is the kind of upgrade that usually requires retesting. It brings Spark 4.0, Delta 4.0, Azure Linux 3.0 (Mariner 3.0), Java 21, Scala 2.13, and Python 3.12, with Spark 4.1 / Delta 4.1 / Python 3.13 planned soon. Because you can enable preview at workspace or environment level, teams can stage rollout: validate connector JARs with Scala 2.13, check Java 21 requirements, and confirm Python wheels/native dependencies before moving to production.

Fabric also added guidance to make Spark work more reproducible. Environments best practices splits workflows into Quick mode (fast publish, installs at session start, good for iteration and testing overrides) and Full mode (3-6 minute publish for a validated snapshot, then 1-3 minute session startup), with a practical middle ground: Full mode plus a custom live pool for reproducibility with ~5s session startup. It also recommends using Resources folder/inline installs only for early-stage or one-off work, then promoting validated dependencies into Full mode for scheduled jobs and shared production runs.

Notebook automation is also becoming a first-class integration surface with Fabric Notebook Public APIs GA. Teams can manage notebooks via REST (create/update/get/list/delete) and execute them through the Fabric Job Scheduler API with parameters, session config, and explicit execution context (environment/lakehouse). Two details matter for CI/CD-style orchestration: service principal auth for unattended automation, and the Run Notebook API returning an exit value (via notebook utilities) so external orchestrators can branch or gate on structured output, not just success/failure. Together, the story is coherent: define transforms declaratively (MLVs), adopt a new Spark/Delta baseline when ready (Runtime 2.0), control dependencies (Environments), and orchestrate notebooks via APIs.

- [Materialized Lake Views in Microsoft Fabric (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/materialized-lake-views-in-microsoft-fabric-generally-available/)
- [Fabric Runtime 2.0 (Preview)](https://blog.fabric.microsoft.com/en-US/blog/fabric-runtime-2-0-preview/)
- [Best Practices for Library Management with Fabric Environments](https://blog.fabric.microsoft.com/en-US/blog/33772/)
- [Fabric Notebook Public APIs (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/fabric-notebook-public-apis-generally-available/)

### Azure Databricks Lakeflow: near-real-time ingestion, SCD transformations, and governance in Delta Lake

Azure Databricks published a detailed walkthrough for collapsing "too many tools" into a Lakeflow-native pipeline, covering ingestion, transformation, orchestration, monitoring, lineage, and Unity Catalog access control. The architecture starts with two Bronze ingestion paths into Delta: application telemetry streamed into Delta via Lakeflow Connect "Zerobus Ingest" over gRPC, and SQL Server CDC ingested incrementally from an on-prem transaction log (assuming ExpressRoute). For telemetry, it includes concrete prerequisites (Unity Catalog + serverless), SQL for creating UC tables (for example, `prod.bronze.telemetry_events`), and service principal grants (`GRANT USE CATALOG/SCHEMA` plus `GRANT MODIFY, SELECT`). It shows deriving a Zerobus endpoint from workspace URL (`<workspace-id>.zerobus.<region>.azuredatabricks.net`) and Python using `databricks-zerobus-ingest-sdk` to stream client-credential auth, define JSON record types, ingest records, and close streams, with targets like sub-5s latency and up to ~100 MB/sec per connection; records become queryable immediately via Unity Catalog.

For SQL Server CDC, the focus is correctness and incremental efficiency: TCP 1433 connectivity, enabling CDC with `sys.sp_cdc_enable_db` / `sys.sp_cdc_enable_table`, plus SQL permissions (CDC read) and Databricks privileges (`CREATE CONNECTION` at metastore, plus destination `USE CATALOG` / `CREATE TABLE`). Setup then uses the Databricks UI (Data Ingestion -> Add Data): configure an ingestion gateway, connection details, select tables, optionally enable SCD Type 2 history per table, map outputs to Bronze tables (`orders_raw`, `customers_raw`), and schedule runs (example every 5 minutes).

Transformations use Lakeflow Spark Declarative Pipelines and a medallion pattern with SQL-defined incremental processing: `CREATE OR REFRESH STREAMING TABLE` for Silver; `APPLY CHANGES INTO` keyed with `SEQUENCE BY updated_at` for SCD Type 1 "latest state" and SCD Type 2 customer history; telemetry data quality constraints with `EXPECT ...` and violating rows dropped. Gold uses `CREATE OR REFRESH MATERIALIZED VIEW` joining orders/customers/telemetry and aggregating metrics (including conditional sums like purchase event counts). Continuous mode keeps it near real time; Unity Catalog registers everything so lineage flows from Gold back to Bronze automatically. Governance details include granting analysts access only to Gold and applying PII masking via UDF (for example, `mask_email`) that reveals full data only for privileged groups, enforced with `ALTER TABLE ... ALTER COLUMN ... SET MASK`.

Orchestration and monitoring use Lakeflow Jobs to chain dependencies (CDC ingestion then transforms) with scheduling and notifications. For day-2 operations, it shows querying system tables like `system.lakeflow.job_run_timeline` for runs, states, and durations. Consumption examples stay inside Databricks (AI/BI Dashboards and Genie NL->SQL) while relying on Unity Catalog permissions, keeping access control and lineage consistent for BI and ML feature preparation off the same Gold layer.

- [Near–Real-Time CDC to Delta Lake for BI and ML with Lakeflow on Azure Databricks](https://techcommunity.microsoft.com/t5/azure-databricks/near-real-time-cdc-to-delta-lake-for-bi-and-ml-with-lakeflow-on/ba-p/4502750)

### Other ML News

Fabric Eventhouse (Real-Time Intelligence) previewed a small but useful KQL workflow update: DB Explorer can browse stored functions, show definitions read-only, and run "Preview results" without manually writing the KQL call (including parameter formatting). Parameter prompts plus a 100-row preview cap make it a quick validation step when iterating on function libraries or reviewing inherited functions before using them in dashboards and reports.

- [''Instantly Run and Preview Functions in Microsoft Fabric Eventhouse: No Code Required''](https://blog.fabric.microsoft.com/en-US/blog/instantly-run-and-preview-functions-in-microsoft-fabric-eventhouse-no-code-required/)',
     excerpt      = 'This week''s ML-adjacent data engineering updates were less about model releases and more about tightening pipelines and developer surfaces. Fabric moved Spark and notebook capabilities closer to production usage, and Azure Databricks shared a concrete pattern for consolidating near-real-time ingestion, transformation, and governance into a single Lakeflow workflow.',
     content_hash = md5('This week''s ML-adjacent data engineering updates were less about model releases and more about tightening pipelines and developer surfaces. Fabric moved Spark and notebook capabilities closer to production usage, and Azure Databricks shared a concrete pattern for consolidating near-real-time ingestion, transformation, and governance into a single Lakeflow workflow.

<!--excerpt_end-->

## ML

### Microsoft Fabric: Lakehouse pipelines, runtimes, and notebook automation

Materialized Lake Views (MLVs) GA is Fabric''s clearest move toward declarative lakehouse transforms without hand-rolled Spark ETL plus separate orchestration. The GA release focuses on refresh behavior and manageability: Fabric expands incremental refresh support across common patterns (aggregations with `GROUP BY`, left outer joins, left semi joins, CTEs) and decides per run whether incremental or full recompute is cheaper based on change volume and estimated cost. With Change Data Feed enabled by default for new MLVs, incremental processing becomes the default rather than another setting. Operationally, multi-schedule support at the lakehouse level lets you define named schedules for subsets of views (hourly gold vs six-hour lower-priority), with Fabric handling dependencies, parallelizing independent views, and centralizing errors; overlapping triggers are skipped if a refresh is already running. "Replace" allows updating an MLV definition in place without drop/recreate, preserving identity/metadata/lineage and avoiding broken dependencies. Data quality constraints get fuller reporting across refresh history, including richer expression-based constraints for PySpark-authored MLVs (multi-column expressions, arithmetic/functions, session-scoped Python UDFs), while PySpark authoring itself is preview; PySpark-authored MLVs still full-refresh for now.

Fabric Runtime 2.0 entered preview as a new baseline for Spark engineering and science workloads, and it is the kind of upgrade that usually requires retesting. It brings Spark 4.0, Delta 4.0, Azure Linux 3.0 (Mariner 3.0), Java 21, Scala 2.13, and Python 3.12, with Spark 4.1 / Delta 4.1 / Python 3.13 planned soon. Because you can enable preview at workspace or environment level, teams can stage rollout: validate connector JARs with Scala 2.13, check Java 21 requirements, and confirm Python wheels/native dependencies before moving to production.

Fabric also added guidance to make Spark work more reproducible. Environments best practices splits workflows into Quick mode (fast publish, installs at session start, good for iteration and testing overrides) and Full mode (3-6 minute publish for a validated snapshot, then 1-3 minute session startup), with a practical middle ground: Full mode plus a custom live pool for reproducibility with ~5s session startup. It also recommends using Resources folder/inline installs only for early-stage or one-off work, then promoting validated dependencies into Full mode for scheduled jobs and shared production runs.

Notebook automation is also becoming a first-class integration surface with Fabric Notebook Public APIs GA. Teams can manage notebooks via REST (create/update/get/list/delete) and execute them through the Fabric Job Scheduler API with parameters, session config, and explicit execution context (environment/lakehouse). Two details matter for CI/CD-style orchestration: service principal auth for unattended automation, and the Run Notebook API returning an exit value (via notebook utilities) so external orchestrators can branch or gate on structured output, not just success/failure. Together, the story is coherent: define transforms declaratively (MLVs), adopt a new Spark/Delta baseline when ready (Runtime 2.0), control dependencies (Environments), and orchestrate notebooks via APIs.

- [Materialized Lake Views in Microsoft Fabric (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/materialized-lake-views-in-microsoft-fabric-generally-available/)
- [Fabric Runtime 2.0 (Preview)](https://blog.fabric.microsoft.com/en-US/blog/fabric-runtime-2-0-preview/)
- [Best Practices for Library Management with Fabric Environments](https://blog.fabric.microsoft.com/en-US/blog/33772/)
- [Fabric Notebook Public APIs (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/fabric-notebook-public-apis-generally-available/)

### Azure Databricks Lakeflow: near-real-time ingestion, SCD transformations, and governance in Delta Lake

Azure Databricks published a detailed walkthrough for collapsing "too many tools" into a Lakeflow-native pipeline, covering ingestion, transformation, orchestration, monitoring, lineage, and Unity Catalog access control. The architecture starts with two Bronze ingestion paths into Delta: application telemetry streamed into Delta via Lakeflow Connect "Zerobus Ingest" over gRPC, and SQL Server CDC ingested incrementally from an on-prem transaction log (assuming ExpressRoute). For telemetry, it includes concrete prerequisites (Unity Catalog + serverless), SQL for creating UC tables (for example, `prod.bronze.telemetry_events`), and service principal grants (`GRANT USE CATALOG/SCHEMA` plus `GRANT MODIFY, SELECT`). It shows deriving a Zerobus endpoint from workspace URL (`<workspace-id>.zerobus.<region>.azuredatabricks.net`) and Python using `databricks-zerobus-ingest-sdk` to stream client-credential auth, define JSON record types, ingest records, and close streams, with targets like sub-5s latency and up to ~100 MB/sec per connection; records become queryable immediately via Unity Catalog.

For SQL Server CDC, the focus is correctness and incremental efficiency: TCP 1433 connectivity, enabling CDC with `sys.sp_cdc_enable_db` / `sys.sp_cdc_enable_table`, plus SQL permissions (CDC read) and Databricks privileges (`CREATE CONNECTION` at metastore, plus destination `USE CATALOG` / `CREATE TABLE`). Setup then uses the Databricks UI (Data Ingestion -> Add Data): configure an ingestion gateway, connection details, select tables, optionally enable SCD Type 2 history per table, map outputs to Bronze tables (`orders_raw`, `customers_raw`), and schedule runs (example every 5 minutes).

Transformations use Lakeflow Spark Declarative Pipelines and a medallion pattern with SQL-defined incremental processing: `CREATE OR REFRESH STREAMING TABLE` for Silver; `APPLY CHANGES INTO` keyed with `SEQUENCE BY updated_at` for SCD Type 1 "latest state" and SCD Type 2 customer history; telemetry data quality constraints with `EXPECT ...` and violating rows dropped. Gold uses `CREATE OR REFRESH MATERIALIZED VIEW` joining orders/customers/telemetry and aggregating metrics (including conditional sums like purchase event counts). Continuous mode keeps it near real time; Unity Catalog registers everything so lineage flows from Gold back to Bronze automatically. Governance details include granting analysts access only to Gold and applying PII masking via UDF (for example, `mask_email`) that reveals full data only for privileged groups, enforced with `ALTER TABLE ... ALTER COLUMN ... SET MASK`.

Orchestration and monitoring use Lakeflow Jobs to chain dependencies (CDC ingestion then transforms) with scheduling and notifications. For day-2 operations, it shows querying system tables like `system.lakeflow.job_run_timeline` for runs, states, and durations. Consumption examples stay inside Databricks (AI/BI Dashboards and Genie NL->SQL) while relying on Unity Catalog permissions, keeping access control and lineage consistent for BI and ML feature preparation off the same Gold layer.

- [Near–Real-Time CDC to Delta Lake for BI and ML with Lakeflow on Azure Databricks](https://techcommunity.microsoft.com/t5/azure-databricks/near-real-time-cdc-to-delta-lake-for-bi-and-ml-with-lakeflow-on/ba-p/4502750)

### Other ML News

Fabric Eventhouse (Real-Time Intelligence) previewed a small but useful KQL workflow update: DB Explorer can browse stored functions, show definitions read-only, and run "Preview results" without manually writing the KQL call (including parameter formatting). Parameter prompts plus a 100-row preview cap make it a quick validation step when iterating on function libraries or reviewing inherited functions before using them in dashboards and reports.

- [''Instantly Run and Preview Functions in Microsoft Fabric Eventhouse: No Code Required''](https://blog.fabric.microsoft.com/en-US/blog/instantly-run-and-preview-functions-in-microsoft-fabric-eventhouse-no-code-required/)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'ml'
  AND slug                 = 'weekly-ml-roundup-2026-03-23';

-- weekly-ml-roundup-2026-03-30  (2026-03-30)
UPDATE content_items
SET  content      = 'This week''s ML-adjacent momentum mostly came through Microsoft Fabric, with updates that make analytics engineering more like a managed product: repeatable transformation workflows (dbt), more event-driven automation (Activator + UDFs), and steadier ingestion mechanics (Copy job upgrades, more connectors, easier troubleshooting). Building on last week''s "pipelines over one-off notebooks" theme (Materialized Lake Views, Environments, Notebook Public APIs), the thread is Fabric turning those building blocks into managed operating surfaces: author in familiar tools, execute in Fabric, and connect actions with less custom glue. Fabric also tightened admin/governance with better workspace organization at scale.

<!--excerpt_end-->

## ML

### Microsoft Fabric’s dbt roadmap: adapters, operational dbt Jobs, and a path to Fusion

Fabric continues treating dbt as a first-class workflow, focusing not just on adapter availability but on correctness for Fabric SQL semantics, materializations, and performance. This mirrors last week''s shift toward declarative transforms via Materialized Lake Views: dbt is another "transformations as code" path, and Fabric is aiming for clean mapping to Warehouse and (soon) Lakehouse execution. Today, the recommendation for SQL-first managed warehouse work is the Fabric Warehouse dbt core adapter; a Fabric Lakehouse dbt core adapter is "coming soon" as GA for running dbt directly on Lakehouse tables in OneLake, aligned with Fabric governance and compute/storage separation.

Operationally, dbt Jobs in Fabric (public preview since December 2025) is positioned as the control plane for scheduling, retries, environment promotion, and observability. This matches last week''s "managed orchestration" focus (Notebook Public APIs + Job Scheduler): less interactive execution, more managed jobs with traceable outputs. Recent additions include public package support, native GitHub support (run jobs from GitHub-hosted dbt projects for CI/CD alignment), and OneLake-based enterprise logging with no size limits (removing the prior 1 MB cap). API support enables automation, and "coming soon" items include dbt Jobs as a Fabric Pipelines activity with parameterization, plus Lakehouse adapter support in dbt Jobs (Warehouse supported today).

Looking ahead, Fabric called out planned dbt Fusion support expected later in calendar Q2 2026, focusing on clean Warehouse/Lakehouse adapter integration and aligned execution metadata/observability as Fusion enters dbt''s runtime story. The net effect is a cohesive path: author in GitHub, execute/schedule in Fabric, centralize logs in OneLake, and adopt Fusion-backed execution later without reworking Warehouse/Lakehouse layouts.

- [dbt + Microsoft Fabric: dbt adapters, dbt Jobs on OneLake, and upcoming dbt Fusion support](https://blog.fabric.microsoft.com/en-US/blog/dbt-microsoft-fabric-a-strategic-investment-in-the-modern-analytics-stack/)

### Fabric Real-Time Intelligence: Activator grows from alerting into action (Teams, Spark, Dataflows, and UDF triggers)

Fabric Activator is expanding from "tell me something happened" to "do something when it happens," adding rule actions to send Microsoft Teams messages and trigger compute/pipeline work: run a Spark job, run a User Data Function (UDF), or run a Dataflow (Dataflows Gen2). This reduces glue code by removing the need for custom listener services that translate events into downstream work, especially when teams want event-driven processing instead of scheduled refresh. It follows last week''s automation direction: after notebooks became easier to run/manage via APIs, Activator now provides an "event -> execution" surface inside Fabric without external schedulers.

Two additions stand out for operational workflows. First, triggering UDFs from Activator creates a direct event-to-function bridge: rules can pass entity IDs, values, and timestamps into code, enabling incidents/runbooks/custom logic without new infrastructure. This pairs with this week''s UDF defaults update: as UDFs become shared primitives invoked by rules, backwards-compatible signatures matter more. Second, Spark job and Dataflow actions can respond to Fabric and Azure Blob Storage events, enabling "data landed, process now" patterns rather than waiting for schedules, similar in spirit to last week''s near-real-time pipeline patterns but implemented through Fabric''s event/action model.

Authoring surfaces broadened too: Warehouse SQL query monitoring rules (Preview) let rules run on ad-hoc or saved query results on a frequency, and Ontology entity rules (Preview) support entity-level conditions. Rule creation is now embedded in Eventstream, and Power BI integration improved so Activator can alert when a new row appears in a table visual in a published report, which helps when dashboards function as queue views.

- [What’s new with Fabric Activator: more connected and capabilities](https://blog.fabric.microsoft.com/en-US/blog/whats-new-with-fabric-activator-more-connected-and-capabilities/)

### Fabric Data Factory: Copy job and connector upgrades for incremental movement, CDC, and cross-cloud destinations

Fabric Data Factory''s Copy job updates targeted ingestion constraints where schemas do not match ideal assumptions. This is Fabric''s version of the "productionize the plumbing" story we touched last week (Databricks Lakeflow simplifying ingestion + CDC + SCD): in Fabric, improvements are landing in Copy job incremental and CDC behavior, which often blocks teams before transformations like MLVs or dbt. Incremental copy is now more flexible in GA with additional watermark types: ROWVERSION, date/datetime (with delayed extraction to reduce missed late updates), and string columns interpreted as datetime. This reduces custom query workarounds while still using built-in state tracking and checkpointing.

CDC replication added three practical updates: Oracle as a CDC source, Fabric Data Warehouse as a CDC sink, and an SCD Type 2 write method in Preview as a simple toggle. The SCD2 option provides history-table semantics (new version rows on updates; soft deletes via expiring current versions), reducing per-table MERGE logic and custom frameworks. It echoes last week''s SCD2-as-first-class capability in Databricks, but here it''s pushed down into ingestion so history tables can be created earlier without bespoke transform code.

Connector and throughput improvements also landed. SharePoint Online File is now GA as source/destination, easing "files in SharePoint" ingestion/publishing. BigQuery, MySQL, and PostgreSQL gained destination write support in Preview for more cross-cloud movement. "Native incremental copy" expanded to more connectors (including RDS variants, ODBC, GCS, SharePoint Lists/Files, Fabric Lakehouse tables/files), and automatic partitioning was introduced to speed large-table loads by parallelizing reads/writes via a selected partition column without manual setup.

- [Incremental copy gets more flexible—New watermark column types in Copy job in Fabric Data Factory (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/incremental-copy-gets-more-flexible-new-watermark-column-types-in-copy-job-in-fabric-data-factory-generally-available/)
- [Richer CDC in Fabric Data Factory Copy job: Oracle source, Fabric Data Warehouse sink, and SCD Type 2 (Preview)](https://blog.fabric.microsoft.com/en-US/blog/simplifying-data-movement-across-multiple-clouds-with-richer-cdc-in-copy-job-in-fabric-data-factory-oracle-source-fabric-data-warehouse-sink-and-scd-type-2-preview/)
- [Outstanding connectivity for data movement in Fabric Data Factory](https://blog.fabric.microsoft.com/en-US/blog/outstanding-connectivity-for-data-movement-in-fabric-data-factory/)

### Other ML News

Fabric''s programmable surfaces got a small but useful update: User Data Functions (UDFs) now support default arguments in Python. Because inputs are JSON-serialized, defaults must be JSON-serializable (strings, numbers, booleans, arrays/lists, objects/dicts, and datetime-like strings, ideally ISO 8601). The guidance also reiterates standard Python practice for mutable defaults (use `None` then assign inside), which helps teams evolve shared UDFs without breaking callers. This pairs with Activator triggering UDFs: defaults allow signature extension without updating every rule immediately.

- [Support for default arguments in Fabric User data functions](https://blog.fabric.microsoft.com/en-US/blog/support-for-default-arguments-in-fabric-user-data-functions/)

Dataflow Gen2 troubleshooting is becoming more self-service. A Preview feature lets admins/support download a per-run diagnostic package from run history after completion. It bundles metadata, structured logs, execution traces, and runtime/environment signals, reducing time spent collecting evidence across views for failed or slow runs. This continues last week''s day-2 manageability thread: as more execution becomes managed and event-driven, diagnostics determine whether failures are quickly explainable.

- [Dataflow Gen2 – Dataflow Diagnostics Download (Preview)](https://blog.fabric.microsoft.com/en-US/blog/dataflow-gen2-dataflow-diagnostics-download-preview/)

Workspace tags are now GA, providing a first-class way to label workspaces (team, project, environment, cost center) and filter them in the workspaces list and OneLake Catalog Explorer. Tags are also exposed via REST APIs (create/apply/remove and included in Get/List Workspaces), supporting automated inventory and governance reporting; Fabric Scanner APIs are expected to include tags later. This complements last week''s API-driven ops push: as teams automate notebook/job lifecycles, programmatic workspace organization helps control sprawl.

- [Find and manage workspaces faster with workspace tags (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/find-and-manage-workspaces-faster-with-workspace-tags-generally-available/)

Fabric Open Mirroring added a GA ERP replication option: the BC2Fab Fabric Workload (Navida) replicates Dynamics 365 Business Central tables into Fabric with incremental change detection and schema evolution handling. The goal is lighter transformation-heavy ingestion and reduced load on production ERP, while enabling querying in Fabric engines and Power BI reporting on OneLake-backed copies. Like last week''s consolidation of ingestion and governance for near-real-time pipelines, it continues moving replication closer to standardized OneLake landing zones so downstream dbt/MLV work can focus on shaping data, not extraction.

- [Integrating Dynamics 365 Business Central with Microsoft Fabric using Open Mirroring with BC2Fab workload (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/integrating-dynamics-365-business-central-with-microsoft-fabric-using-open-mirroring-with-bc2fab-workload-generally-available/)',
     excerpt      = 'This week''s ML-adjacent momentum mostly came through Microsoft Fabric, with updates that make analytics engineering more like a managed product: repeatable transformation workflows (dbt), more event-driven automation (Activator + UDFs), and steadier ingestion mechanics (Copy job upgrades, more connectors, easier troubleshooting). Building on last week''s "pipelines over one-off notebooks" theme (Materialized Lake Views, Environments, Notebook Public APIs), the thread is Fabric turning those building blocks into managed operating surfaces: author in familiar tools, execute in Fabric, and connect actions with less custom glue. Fabric also tightened admin/governance with better workspace organization at scale.',
     content_hash = md5('This week''s ML-adjacent momentum mostly came through Microsoft Fabric, with updates that make analytics engineering more like a managed product: repeatable transformation workflows (dbt), more event-driven automation (Activator + UDFs), and steadier ingestion mechanics (Copy job upgrades, more connectors, easier troubleshooting). Building on last week''s "pipelines over one-off notebooks" theme (Materialized Lake Views, Environments, Notebook Public APIs), the thread is Fabric turning those building blocks into managed operating surfaces: author in familiar tools, execute in Fabric, and connect actions with less custom glue. Fabric also tightened admin/governance with better workspace organization at scale.

<!--excerpt_end-->

## ML

### Microsoft Fabric’s dbt roadmap: adapters, operational dbt Jobs, and a path to Fusion

Fabric continues treating dbt as a first-class workflow, focusing not just on adapter availability but on correctness for Fabric SQL semantics, materializations, and performance. This mirrors last week''s shift toward declarative transforms via Materialized Lake Views: dbt is another "transformations as code" path, and Fabric is aiming for clean mapping to Warehouse and (soon) Lakehouse execution. Today, the recommendation for SQL-first managed warehouse work is the Fabric Warehouse dbt core adapter; a Fabric Lakehouse dbt core adapter is "coming soon" as GA for running dbt directly on Lakehouse tables in OneLake, aligned with Fabric governance and compute/storage separation.

Operationally, dbt Jobs in Fabric (public preview since December 2025) is positioned as the control plane for scheduling, retries, environment promotion, and observability. This matches last week''s "managed orchestration" focus (Notebook Public APIs + Job Scheduler): less interactive execution, more managed jobs with traceable outputs. Recent additions include public package support, native GitHub support (run jobs from GitHub-hosted dbt projects for CI/CD alignment), and OneLake-based enterprise logging with no size limits (removing the prior 1 MB cap). API support enables automation, and "coming soon" items include dbt Jobs as a Fabric Pipelines activity with parameterization, plus Lakehouse adapter support in dbt Jobs (Warehouse supported today).

Looking ahead, Fabric called out planned dbt Fusion support expected later in calendar Q2 2026, focusing on clean Warehouse/Lakehouse adapter integration and aligned execution metadata/observability as Fusion enters dbt''s runtime story. The net effect is a cohesive path: author in GitHub, execute/schedule in Fabric, centralize logs in OneLake, and adopt Fusion-backed execution later without reworking Warehouse/Lakehouse layouts.

- [dbt + Microsoft Fabric: dbt adapters, dbt Jobs on OneLake, and upcoming dbt Fusion support](https://blog.fabric.microsoft.com/en-US/blog/dbt-microsoft-fabric-a-strategic-investment-in-the-modern-analytics-stack/)

### Fabric Real-Time Intelligence: Activator grows from alerting into action (Teams, Spark, Dataflows, and UDF triggers)

Fabric Activator is expanding from "tell me something happened" to "do something when it happens," adding rule actions to send Microsoft Teams messages and trigger compute/pipeline work: run a Spark job, run a User Data Function (UDF), or run a Dataflow (Dataflows Gen2). This reduces glue code by removing the need for custom listener services that translate events into downstream work, especially when teams want event-driven processing instead of scheduled refresh. It follows last week''s automation direction: after notebooks became easier to run/manage via APIs, Activator now provides an "event -> execution" surface inside Fabric without external schedulers.

Two additions stand out for operational workflows. First, triggering UDFs from Activator creates a direct event-to-function bridge: rules can pass entity IDs, values, and timestamps into code, enabling incidents/runbooks/custom logic without new infrastructure. This pairs with this week''s UDF defaults update: as UDFs become shared primitives invoked by rules, backwards-compatible signatures matter more. Second, Spark job and Dataflow actions can respond to Fabric and Azure Blob Storage events, enabling "data landed, process now" patterns rather than waiting for schedules, similar in spirit to last week''s near-real-time pipeline patterns but implemented through Fabric''s event/action model.

Authoring surfaces broadened too: Warehouse SQL query monitoring rules (Preview) let rules run on ad-hoc or saved query results on a frequency, and Ontology entity rules (Preview) support entity-level conditions. Rule creation is now embedded in Eventstream, and Power BI integration improved so Activator can alert when a new row appears in a table visual in a published report, which helps when dashboards function as queue views.

- [What’s new with Fabric Activator: more connected and capabilities](https://blog.fabric.microsoft.com/en-US/blog/whats-new-with-fabric-activator-more-connected-and-capabilities/)

### Fabric Data Factory: Copy job and connector upgrades for incremental movement, CDC, and cross-cloud destinations

Fabric Data Factory''s Copy job updates targeted ingestion constraints where schemas do not match ideal assumptions. This is Fabric''s version of the "productionize the plumbing" story we touched last week (Databricks Lakeflow simplifying ingestion + CDC + SCD): in Fabric, improvements are landing in Copy job incremental and CDC behavior, which often blocks teams before transformations like MLVs or dbt. Incremental copy is now more flexible in GA with additional watermark types: ROWVERSION, date/datetime (with delayed extraction to reduce missed late updates), and string columns interpreted as datetime. This reduces custom query workarounds while still using built-in state tracking and checkpointing.

CDC replication added three practical updates: Oracle as a CDC source, Fabric Data Warehouse as a CDC sink, and an SCD Type 2 write method in Preview as a simple toggle. The SCD2 option provides history-table semantics (new version rows on updates; soft deletes via expiring current versions), reducing per-table MERGE logic and custom frameworks. It echoes last week''s SCD2-as-first-class capability in Databricks, but here it''s pushed down into ingestion so history tables can be created earlier without bespoke transform code.

Connector and throughput improvements also landed. SharePoint Online File is now GA as source/destination, easing "files in SharePoint" ingestion/publishing. BigQuery, MySQL, and PostgreSQL gained destination write support in Preview for more cross-cloud movement. "Native incremental copy" expanded to more connectors (including RDS variants, ODBC, GCS, SharePoint Lists/Files, Fabric Lakehouse tables/files), and automatic partitioning was introduced to speed large-table loads by parallelizing reads/writes via a selected partition column without manual setup.

- [Incremental copy gets more flexible—New watermark column types in Copy job in Fabric Data Factory (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/incremental-copy-gets-more-flexible-new-watermark-column-types-in-copy-job-in-fabric-data-factory-generally-available/)
- [Richer CDC in Fabric Data Factory Copy job: Oracle source, Fabric Data Warehouse sink, and SCD Type 2 (Preview)](https://blog.fabric.microsoft.com/en-US/blog/simplifying-data-movement-across-multiple-clouds-with-richer-cdc-in-copy-job-in-fabric-data-factory-oracle-source-fabric-data-warehouse-sink-and-scd-type-2-preview/)
- [Outstanding connectivity for data movement in Fabric Data Factory](https://blog.fabric.microsoft.com/en-US/blog/outstanding-connectivity-for-data-movement-in-fabric-data-factory/)

### Other ML News

Fabric''s programmable surfaces got a small but useful update: User Data Functions (UDFs) now support default arguments in Python. Because inputs are JSON-serialized, defaults must be JSON-serializable (strings, numbers, booleans, arrays/lists, objects/dicts, and datetime-like strings, ideally ISO 8601). The guidance also reiterates standard Python practice for mutable defaults (use `None` then assign inside), which helps teams evolve shared UDFs without breaking callers. This pairs with Activator triggering UDFs: defaults allow signature extension without updating every rule immediately.

- [Support for default arguments in Fabric User data functions](https://blog.fabric.microsoft.com/en-US/blog/support-for-default-arguments-in-fabric-user-data-functions/)

Dataflow Gen2 troubleshooting is becoming more self-service. A Preview feature lets admins/support download a per-run diagnostic package from run history after completion. It bundles metadata, structured logs, execution traces, and runtime/environment signals, reducing time spent collecting evidence across views for failed or slow runs. This continues last week''s day-2 manageability thread: as more execution becomes managed and event-driven, diagnostics determine whether failures are quickly explainable.

- [Dataflow Gen2 – Dataflow Diagnostics Download (Preview)](https://blog.fabric.microsoft.com/en-US/blog/dataflow-gen2-dataflow-diagnostics-download-preview/)

Workspace tags are now GA, providing a first-class way to label workspaces (team, project, environment, cost center) and filter them in the workspaces list and OneLake Catalog Explorer. Tags are also exposed via REST APIs (create/apply/remove and included in Get/List Workspaces), supporting automated inventory and governance reporting; Fabric Scanner APIs are expected to include tags later. This complements last week''s API-driven ops push: as teams automate notebook/job lifecycles, programmatic workspace organization helps control sprawl.

- [Find and manage workspaces faster with workspace tags (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/find-and-manage-workspaces-faster-with-workspace-tags-generally-available/)

Fabric Open Mirroring added a GA ERP replication option: the BC2Fab Fabric Workload (Navida) replicates Dynamics 365 Business Central tables into Fabric with incremental change detection and schema evolution handling. The goal is lighter transformation-heavy ingestion and reduced load on production ERP, while enabling querying in Fabric engines and Power BI reporting on OneLake-backed copies. Like last week''s consolidation of ingestion and governance for near-real-time pipelines, it continues moving replication closer to standardized OneLake landing zones so downstream dbt/MLV work can focus on shaping data, not extraction.

- [Integrating Dynamics 365 Business Central with Microsoft Fabric using Open Mirroring with BC2Fab workload (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/integrating-dynamics-365-business-central-with-microsoft-fabric-using-open-mirroring-with-bc2fab-workload-generally-available/)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'ml'
  AND slug                 = 'weekly-ml-roundup-2026-03-30';

-- weekly-ml-roundup-2026-04-06  (2026-04-06)
UPDATE content_items
SET  content      = 'This week’s Fabric updates focused on production gaps for data and ML-adjacent workloads: more standard orchestration (especially for Airflow teams) and more day-2 guardrails via alerting and recovery to reduce downtime from failures or deletes. This continues last week’s "managed operating surfaces" thread, where dbt Jobs, Activator-triggered actions, and improved diagnostics emphasized repeatable, observable workflows.

<!--excerpt_end-->

## ML

### Microsoft Fabric orchestration and operations (Airflow, scheduling, recovery)

Fabric Data Factory’s Apache Airflow integration added native operators to run Fabric artifacts directly from Airflow DAGs. Teams can invoke Fabric Notebooks, Spark job definitions, Fabric pipelines, Semantic Models, and user data functions as first-class tasks, with broader coverage including Copy jobs and dbt jobs. This builds on last week’s emphasis on dbt Jobs as a scheduling/observability plane and Copy job improvements for incremental/CDC ingestion, but it now lets existing Airflow standards orchestrate those Fabric primitives without custom glue. It also complements last week’s Activator direction (event -> action inside Fabric) by giving teams another coordination surface when a DAG view is preferred.

Fabric also added a shortcut, "Run Fabric Artifact" in the Airflow job context menu, that inserts the needed code/config to call a Fabric item. This speeds DAG authoring and reduces boilerplate, which matches the recent push to minimize bespoke integration code.

New Apache Airflow job APIs also support platform automation: programmatic management/monitoring/triggering of DAG runs from external services, including event-driven scenarios. This fits teams integrating Fabric orchestration with CI/CD, internal portals, or runbooks, and matches last week’s API-first posture (dbt Jobs APIs, workspace tags via REST, Notebook Public APIs referenced previously). The direction is increasingly "everything is addressable as an API," which supports consistent promotion, scheduling, and monitoring across many workspaces.

Operationally, Fabric improved both "find out fast" and "recover fast." Scheduled job failure notifications are now GA: configure recipients once per item under Schedule settings, and the list applies to all schedules for that item. Failed scheduled runs email error details plus a link to the Monitoring Hub run, and it works for schedules created in the UI or via the Job Scheduler REST API. The limitation is explicit: only scheduled runs trigger emails, not manual runs, so ad-hoc execution still needs separate practices. This extends last week’s day-2 manageability theme by making managed schedules more actionable without constant dashboard watching.

Fabric Data Warehouse also added preview recovery for a dropped warehouse via the workspace Recycle Bin. Within a tenant-set retention window (7-90 days, default 7), a Workspace Admin can restore a warehouse to its pre-delete state, including schemas/data, snapshots, permissions/security settings, and objects like saved queries, views, and stored procedures. For fast-moving environments, this is a cleaner rollback than rebuilding and replaying pipelines, and it pairs with last week’s "productionize the plumbing" theme by reducing blast radius when mistakes happen.

- [Announcing the latest innovations in Fabric Data Factory: Apache Airflow jobs and pipelines](https://blog.fabric.microsoft.com/en-US/blog/announcing-the-latest-innovations-in-fabric-data-factory-apache-airflow-jobs-and-pipelines/)
- [Get notified when scheduled jobs fail in Fabric (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/get-notified-when-scheduled-jobs-fail-in-fabric-generally-available/)
- [Dropped warehouse recovery in Microsoft Fabric (Preview)](https://blog.fabric.microsoft.com/en-US/blog/dropped-warehouse-recovery-in-microsoft-fabric-preview/)',
     excerpt      = 'This week’s Fabric updates focused on production gaps for data and ML-adjacent workloads: more standard orchestration (especially for Airflow teams) and more day-2 guardrails via alerting and recovery to reduce downtime from failures or deletes. This continues last week’s "managed operating surfaces" thread, where dbt Jobs, Activator-triggered actions, and improved diagnostics emphasized repeatable, observable workflows.',
     content_hash = md5('This week’s Fabric updates focused on production gaps for data and ML-adjacent workloads: more standard orchestration (especially for Airflow teams) and more day-2 guardrails via alerting and recovery to reduce downtime from failures or deletes. This continues last week’s "managed operating surfaces" thread, where dbt Jobs, Activator-triggered actions, and improved diagnostics emphasized repeatable, observable workflows.

<!--excerpt_end-->

## ML

### Microsoft Fabric orchestration and operations (Airflow, scheduling, recovery)

Fabric Data Factory’s Apache Airflow integration added native operators to run Fabric artifacts directly from Airflow DAGs. Teams can invoke Fabric Notebooks, Spark job definitions, Fabric pipelines, Semantic Models, and user data functions as first-class tasks, with broader coverage including Copy jobs and dbt jobs. This builds on last week’s emphasis on dbt Jobs as a scheduling/observability plane and Copy job improvements for incremental/CDC ingestion, but it now lets existing Airflow standards orchestrate those Fabric primitives without custom glue. It also complements last week’s Activator direction (event -> action inside Fabric) by giving teams another coordination surface when a DAG view is preferred.

Fabric also added a shortcut, "Run Fabric Artifact" in the Airflow job context menu, that inserts the needed code/config to call a Fabric item. This speeds DAG authoring and reduces boilerplate, which matches the recent push to minimize bespoke integration code.

New Apache Airflow job APIs also support platform automation: programmatic management/monitoring/triggering of DAG runs from external services, including event-driven scenarios. This fits teams integrating Fabric orchestration with CI/CD, internal portals, or runbooks, and matches last week’s API-first posture (dbt Jobs APIs, workspace tags via REST, Notebook Public APIs referenced previously). The direction is increasingly "everything is addressable as an API," which supports consistent promotion, scheduling, and monitoring across many workspaces.

Operationally, Fabric improved both "find out fast" and "recover fast." Scheduled job failure notifications are now GA: configure recipients once per item under Schedule settings, and the list applies to all schedules for that item. Failed scheduled runs email error details plus a link to the Monitoring Hub run, and it works for schedules created in the UI or via the Job Scheduler REST API. The limitation is explicit: only scheduled runs trigger emails, not manual runs, so ad-hoc execution still needs separate practices. This extends last week’s day-2 manageability theme by making managed schedules more actionable without constant dashboard watching.

Fabric Data Warehouse also added preview recovery for a dropped warehouse via the workspace Recycle Bin. Within a tenant-set retention window (7-90 days, default 7), a Workspace Admin can restore a warehouse to its pre-delete state, including schemas/data, snapshots, permissions/security settings, and objects like saved queries, views, and stored procedures. For fast-moving environments, this is a cleaner rollback than rebuilding and replaying pipelines, and it pairs with last week’s "productionize the plumbing" theme by reducing blast radius when mistakes happen.

- [Announcing the latest innovations in Fabric Data Factory: Apache Airflow jobs and pipelines](https://blog.fabric.microsoft.com/en-US/blog/announcing-the-latest-innovations-in-fabric-data-factory-apache-airflow-jobs-and-pipelines/)
- [Get notified when scheduled jobs fail in Fabric (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/get-notified-when-scheduled-jobs-fail-in-fabric-generally-available/)
- [Dropped warehouse recovery in Microsoft Fabric (Preview)](https://blog.fabric.microsoft.com/en-US/blog/dropped-warehouse-recovery-in-microsoft-fabric-preview/)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'ml'
  AND slug                 = 'weekly-ml-roundup-2026-04-06';

-- weekly-ml-roundup-2026-04-13  (2026-04-13)
UPDATE content_items
SET  content      = 'This week''s ML thread was about shipping models and data products with fewer operational surprises. Azure ML plus Azure DevOps guidance went deep on repeatable training-to-serving pipelines and the details that tend to break CI/CD. Fabric continued last week''s "operationalize the platform" momentum, focusing this time on real-time ingestion security and smoother warehouse querying to reduce glue work once systems move past prototype.

<!--excerpt_end-->

## ML

### Azure Machine Learning + Azure DevOps: a repeatable training-to-endpoint MLOps pipeline

An end-to-end template showed how to take a scikit-learn model from local training to reliable serving on Azure ML Managed Online Endpoints using an Azure DevOps multi-stage YAML pipeline. It splits into four stages (DevOps gate -> Train -> Register -> Deploy) so teams can validate and capture metadata early, retrain only when needed, and rerun register/deploy without retraining after transient failures.

In Train, the example standardizes the environment (Python 3.12), pulls data from Azure Blob Storage (CSV/Parquet via `adlfs`/`pyarrow` patterns), and adds basic validation (schema and row counts) before feature engineering and fitting (with `StandardScaler` as the example preprocessing). The output is one serialized artifact: a pickle bundle containing the estimator, fitted preprocessor, expected feature column order, and metadata (timestamps, row counts, scikit-learn version) to prevent silent mismatches and manage pickle compatibility.

Register uses the Azure ML CLI (`az extension add -n ml`, then `az ml model create`) to push the artifact into an Azure ML Registry, using auto version incrementing for re-registers under the same model name. Deploy then creates/updates a Managed Online Endpoint and deploys a specific model version (example: "blue" with all traffic) using `az ml online-endpoint create/show` and `az ml online-deployment create`, and finishes with a smoke test via `az ml online-endpoint invoke` to confirm the endpoint is callable.

It also covers operational details that determine whether this works in a team setting: managed-endpoint scoring script structure (`init()` loading from `AZUREML_MODEL_DIR`, `run()` enforcing feature order, applying the stored scaler, returning predictions), tradeoffs among `pickle`, `joblib`, and ONNX, and a warning on untrusted pickle deserialization. On DevOps/security, it reinforces no secrets in code (env vars/variable groups), managed identity over keys/secrets, least-privilege RBAC, and sample roles (Storage Blob Data Reader, AzureML Registry User, AzureML Data Scientist), plus workload identity federation from Azure DevOps to a user-assigned managed identity. It also flags pitfalls (Windows agent command differences, checkout behaviors, schema mismatches) and suggests extensions like validation gates, batch endpoints, drift monitoring, environment promotion, and blue/green or traffic-splitting.

- [Building an End-to-End MLOps Pipeline: From Training to Managed Endpoints on Azure](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/building-an-end-to-end-mlops-pipeline-from-training-to-managed/ba-p/4509852)

### Microsoft Fabric: real-time ingestion/security upgrades and cleaner warehouse SQL

Fabric''s ML-adjacent updates focused on improving ingestion and querying that feed scoring, enrichment, and analytics, continuing last week''s push to reduce bespoke operations. Last week emphasized orchestration surfaces and recovery. This week targets streaming ingestion (networking, certs, retries, fewer embedded secrets) and warehouse SQL ergonomics to reduce production friction.

In Eventstreams (Q1 2026 recap), ingestion expanded and Spark handoff tightened. New preview connectors include DeltaFlow for converting DB CDC events (inserts/updates/deletes) into structured streams, which reduces manual CDC format/schema/destination work. MQTT enhancements add v3.1 and v3.1.1 support to onboard existing brokers/device fleets without upgrades. Anomaly Detection also appears as a preview source, making anomaly signals first-class streaming inputs for routing/enrichment with telemetry. For teams following last week''s orchestration theme, these broaden what becomes "just another input" into repeatable pipelines, especially where CDC/anomaly feeds must land reliably before downstream scoring/feature updates.

Eventstreams also improved processing integration with Spark Structured Streaming and Fabric Notebooks to reduce setup friction: discover Eventstreams through Real-Time Hub, auto-generate PySpark connection snippets, and reuse shared notebooks within Eventstreams. Operationally, it pushes safer defaults by reducing embedded connection strings/SAS keys and adding notebook auto-retry policies to restart streaming jobs after failures. This fits last week''s "recover fast" theme by adding resilience settings for long-running streams and reducing secret sprawl.

Enterprise connectivity and security also advanced with preview private network ingestion for VNet/on-prem sources using an Azure managed virtual network bridge, supporting VPN, ExpressRoute, peering, private endpoints, and a streaming VNet data gateway experience. Connector security added preview custom CA certificates and mutual TLS (mTLS), with certs stored in Azure Key Vault for centralized rotation. This is called out for Kafka sources including Apache Kafka, Amazon MSK, Confluent Cloud for Apache Kafka, and Confluent Schema Registry. It matches last week''s "platform-managed" posture: connectivity and cert rotation move into managed config and Key Vault-backed rotation rather than custom code.

Separately, Fabric Data Warehouse shipped GA support for T-SQL `ANY_VALUE()` as an aggregate/analytic function, which addresses a common reporting and semantic-layer pain point. It returns an arbitrary representative value per `GROUP BY` group (or window partition) when projected columns are functionally dependent on the grouping key. For example, you can group revenue by `GeographyID` while including `City`, `State`, `Country` without expanding the `GROUP BY`. It is clearer than `MIN()`/`MAX()` workarounds and can reduce unnecessary grouping columns, with the guardrail that it is only valid when values are constant in the group. Paired with last week''s recovery work, it is another everyday production SQL/ops edge being improved.

- [What’s new in Fabric Eventstream: 2026 Q1 Edition](https://blog.fabric.microsoft.com/en-US/blog/whats-new-in-fabric-eventstream-2026-q1-edition/)
- [Use ANY_VALUE() for simpler grouping of results in Fabric Data Warehouse (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/use-any_value-for-simpler-grouping-of-results-in-fabric-data-warehouse-generally-available/)',
     excerpt      = 'This week''s ML thread was about shipping models and data products with fewer operational surprises. Azure ML plus Azure DevOps guidance went deep on repeatable training-to-serving pipelines and the details that tend to break CI/CD. Fabric continued last week''s "operationalize the platform" momentum, focusing this time on real-time ingestion security and smoother warehouse querying to reduce glue work once systems move past prototype.',
     content_hash = md5('This week''s ML thread was about shipping models and data products with fewer operational surprises. Azure ML plus Azure DevOps guidance went deep on repeatable training-to-serving pipelines and the details that tend to break CI/CD. Fabric continued last week''s "operationalize the platform" momentum, focusing this time on real-time ingestion security and smoother warehouse querying to reduce glue work once systems move past prototype.

<!--excerpt_end-->

## ML

### Azure Machine Learning + Azure DevOps: a repeatable training-to-endpoint MLOps pipeline

An end-to-end template showed how to take a scikit-learn model from local training to reliable serving on Azure ML Managed Online Endpoints using an Azure DevOps multi-stage YAML pipeline. It splits into four stages (DevOps gate -> Train -> Register -> Deploy) so teams can validate and capture metadata early, retrain only when needed, and rerun register/deploy without retraining after transient failures.

In Train, the example standardizes the environment (Python 3.12), pulls data from Azure Blob Storage (CSV/Parquet via `adlfs`/`pyarrow` patterns), and adds basic validation (schema and row counts) before feature engineering and fitting (with `StandardScaler` as the example preprocessing). The output is one serialized artifact: a pickle bundle containing the estimator, fitted preprocessor, expected feature column order, and metadata (timestamps, row counts, scikit-learn version) to prevent silent mismatches and manage pickle compatibility.

Register uses the Azure ML CLI (`az extension add -n ml`, then `az ml model create`) to push the artifact into an Azure ML Registry, using auto version incrementing for re-registers under the same model name. Deploy then creates/updates a Managed Online Endpoint and deploys a specific model version (example: "blue" with all traffic) using `az ml online-endpoint create/show` and `az ml online-deployment create`, and finishes with a smoke test via `az ml online-endpoint invoke` to confirm the endpoint is callable.

It also covers operational details that determine whether this works in a team setting: managed-endpoint scoring script structure (`init()` loading from `AZUREML_MODEL_DIR`, `run()` enforcing feature order, applying the stored scaler, returning predictions), tradeoffs among `pickle`, `joblib`, and ONNX, and a warning on untrusted pickle deserialization. On DevOps/security, it reinforces no secrets in code (env vars/variable groups), managed identity over keys/secrets, least-privilege RBAC, and sample roles (Storage Blob Data Reader, AzureML Registry User, AzureML Data Scientist), plus workload identity federation from Azure DevOps to a user-assigned managed identity. It also flags pitfalls (Windows agent command differences, checkout behaviors, schema mismatches) and suggests extensions like validation gates, batch endpoints, drift monitoring, environment promotion, and blue/green or traffic-splitting.

- [Building an End-to-End MLOps Pipeline: From Training to Managed Endpoints on Azure](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/building-an-end-to-end-mlops-pipeline-from-training-to-managed/ba-p/4509852)

### Microsoft Fabric: real-time ingestion/security upgrades and cleaner warehouse SQL

Fabric''s ML-adjacent updates focused on improving ingestion and querying that feed scoring, enrichment, and analytics, continuing last week''s push to reduce bespoke operations. Last week emphasized orchestration surfaces and recovery. This week targets streaming ingestion (networking, certs, retries, fewer embedded secrets) and warehouse SQL ergonomics to reduce production friction.

In Eventstreams (Q1 2026 recap), ingestion expanded and Spark handoff tightened. New preview connectors include DeltaFlow for converting DB CDC events (inserts/updates/deletes) into structured streams, which reduces manual CDC format/schema/destination work. MQTT enhancements add v3.1 and v3.1.1 support to onboard existing brokers/device fleets without upgrades. Anomaly Detection also appears as a preview source, making anomaly signals first-class streaming inputs for routing/enrichment with telemetry. For teams following last week''s orchestration theme, these broaden what becomes "just another input" into repeatable pipelines, especially where CDC/anomaly feeds must land reliably before downstream scoring/feature updates.

Eventstreams also improved processing integration with Spark Structured Streaming and Fabric Notebooks to reduce setup friction: discover Eventstreams through Real-Time Hub, auto-generate PySpark connection snippets, and reuse shared notebooks within Eventstreams. Operationally, it pushes safer defaults by reducing embedded connection strings/SAS keys and adding notebook auto-retry policies to restart streaming jobs after failures. This fits last week''s "recover fast" theme by adding resilience settings for long-running streams and reducing secret sprawl.

Enterprise connectivity and security also advanced with preview private network ingestion for VNet/on-prem sources using an Azure managed virtual network bridge, supporting VPN, ExpressRoute, peering, private endpoints, and a streaming VNet data gateway experience. Connector security added preview custom CA certificates and mutual TLS (mTLS), with certs stored in Azure Key Vault for centralized rotation. This is called out for Kafka sources including Apache Kafka, Amazon MSK, Confluent Cloud for Apache Kafka, and Confluent Schema Registry. It matches last week''s "platform-managed" posture: connectivity and cert rotation move into managed config and Key Vault-backed rotation rather than custom code.

Separately, Fabric Data Warehouse shipped GA support for T-SQL `ANY_VALUE()` as an aggregate/analytic function, which addresses a common reporting and semantic-layer pain point. It returns an arbitrary representative value per `GROUP BY` group (or window partition) when projected columns are functionally dependent on the grouping key. For example, you can group revenue by `GeographyID` while including `City`, `State`, `Country` without expanding the `GROUP BY`. It is clearer than `MIN()`/`MAX()` workarounds and can reduce unnecessary grouping columns, with the guardrail that it is only valid when values are constant in the group. Paired with last week''s recovery work, it is another everyday production SQL/ops edge being improved.

- [What’s new in Fabric Eventstream: 2026 Q1 Edition](https://blog.fabric.microsoft.com/en-US/blog/whats-new-in-fabric-eventstream-2026-q1-edition/)
- [Use ANY_VALUE() for simpler grouping of results in Fabric Data Warehouse (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/use-any_value-for-simpler-grouping-of-results-in-fabric-data-warehouse-generally-available/)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'ml'
  AND slug                 = 'weekly-ml-roundup-2026-04-13';

-- weekly-ml-roundup-2026-04-20  (2026-04-20)
UPDATE content_items
SET  content      = 'This week''s ML-adjacent Fabric updates focused on reducing two workflow frictions: getting local artifacts into OneLake, and moving between SQL, notebooks, and KQL analysis without re-learning each workload UI. Building on last week''s "operationalize the platform" theme (safer ingestion, fewer embedded secrets, smoother Warehouse querying), these changes aim to reduce glue work once teams move beyond prototypes.

<!--excerpt_end-->

## ML

### Microsoft Fabric: lower-friction ingestion and a more consistent analysis surface

OneLake File Explorer is now GA, addressing a common prototyping need: early datasets and artifacts often start on a developer machine (Excel, CSV, Parquet, images, intermediate outputs). With Windows File Explorer integration, OneLake mounts in Explorer so teams can browse by workspace/item and use standard file operations like drag-and-drop to place files where they belong. In the context of last week''s Eventstreams ingestion and security improvements (private networking, Key Vault certs, fewer embedded connection strings), this is a complementary on-ramp: teams can move local artifacts into governed storage without scripts or portal detours. Once in OneLake, data is immediately usable across Fabric experiences (pipelines, notebooks, semantic models) without one-off uploads during iteration.

In preview, Fabric is reducing UI fragmentation with a unified "Analyze data with" entry point across Lakehouse, Data Warehouse, and Eventhouse. This follows last week''s "cleaner warehouse SQL" thread: once data is shared in OneLake, friction often shifts to inconsistent compute and query entry points. Eventhouse Endpoint now appears alongside SQL Endpoint and Notebook options so switching modalities is predictable from the same menu. For Lakehouse and Warehouse, enabling Eventhouse Endpoint provisions an Eventhouse and KQL Database as child artifacts with backend-managed schema sync, which provides a KQL surface over the same data without manual sync or duplication. That matches last week''s push for managed configuration over bespoke integration. Eventhouse also gets the same menu at the database level (next to Share), and notebook launching is standardized so opening from Eventhouse/KQL Database auto-adds the database to the notebook environment for consistent Spark notebook behavior across workloads.

- [Bring your local files to OneLake with OneLake File Explorer (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/bring-your-local-files-to-onelake-with-onelake-file-explorer-generally-available/)
- [Unifying “Analyze data with” analytics across Fabric (Preview)](https://blog.fabric.microsoft.com/en-US/blog/unifying-analyze-data-with-analytics-across-fabric-preview/)',
     excerpt      = 'This week''s ML-adjacent Fabric updates focused on reducing two workflow frictions: getting local artifacts into OneLake, and moving between SQL, notebooks, and KQL analysis without re-learning each workload UI. Building on last week''s "operationalize the platform" theme (safer ingestion, fewer embedded secrets, smoother Warehouse querying), these changes aim to reduce glue work once teams move beyond prototypes.',
     content_hash = md5('This week''s ML-adjacent Fabric updates focused on reducing two workflow frictions: getting local artifacts into OneLake, and moving between SQL, notebooks, and KQL analysis without re-learning each workload UI. Building on last week''s "operationalize the platform" theme (safer ingestion, fewer embedded secrets, smoother Warehouse querying), these changes aim to reduce glue work once teams move beyond prototypes.

<!--excerpt_end-->

## ML

### Microsoft Fabric: lower-friction ingestion and a more consistent analysis surface

OneLake File Explorer is now GA, addressing a common prototyping need: early datasets and artifacts often start on a developer machine (Excel, CSV, Parquet, images, intermediate outputs). With Windows File Explorer integration, OneLake mounts in Explorer so teams can browse by workspace/item and use standard file operations like drag-and-drop to place files where they belong. In the context of last week''s Eventstreams ingestion and security improvements (private networking, Key Vault certs, fewer embedded connection strings), this is a complementary on-ramp: teams can move local artifacts into governed storage without scripts or portal detours. Once in OneLake, data is immediately usable across Fabric experiences (pipelines, notebooks, semantic models) without one-off uploads during iteration.

In preview, Fabric is reducing UI fragmentation with a unified "Analyze data with" entry point across Lakehouse, Data Warehouse, and Eventhouse. This follows last week''s "cleaner warehouse SQL" thread: once data is shared in OneLake, friction often shifts to inconsistent compute and query entry points. Eventhouse Endpoint now appears alongside SQL Endpoint and Notebook options so switching modalities is predictable from the same menu. For Lakehouse and Warehouse, enabling Eventhouse Endpoint provisions an Eventhouse and KQL Database as child artifacts with backend-managed schema sync, which provides a KQL surface over the same data without manual sync or duplication. That matches last week''s push for managed configuration over bespoke integration. Eventhouse also gets the same menu at the database level (next to Share), and notebook launching is standardized so opening from Eventhouse/KQL Database auto-adds the database to the notebook environment for consistent Spark notebook behavior across workloads.

- [Bring your local files to OneLake with OneLake File Explorer (Generally Available)](https://blog.fabric.microsoft.com/en-US/blog/bring-your-local-files-to-onelake-with-onelake-file-explorer-generally-available/)
- [Unifying “Analyze data with” analytics across Fabric (Preview)](https://blog.fabric.microsoft.com/en-US/blog/unifying-analyze-data-with-analytics-across-fabric-preview/)'),
     updated_at   = now()
WHERE collection_name     = 'roundups'
  AND primary_section_name = 'ml'
  AND slug                 = 'weekly-ml-roundup-2026-04-20';

-- SKIP 2026-04-27 : no 'Machine Learning/ML' block found in source (weekly-ai-roundup-2026-04-27)

-- SKIP 2026-05-04 : no 'Machine Learning/ML' block found in source (weekly-ai-roundup-2026-05-04)

-- SKIP 2026-05-11 : no 'Machine Learning/ML' block found in source (weekly-ai-roundup-2026-05-11)

