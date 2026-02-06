---
external_url: https://techcommunity.microsoft.com/t5/educator-developer-blog/how-microsoft-semantic-kernel-transforms-proven-workflows-into/ba-p/4434731
title: How Microsoft Semantic Kernel Transforms Proven Workflows into Intelligent Agents
author: Abdulhamid_Onawole
feed_name: Microsoft Tech Community
date: 2025-08-13 07:00:00 +00:00
tags:
- Agent Architecture
- AI Orchestration
- Business Intelligence
- Business Logic
- Code Execution
- Data Analysis
- Email Automation
- Function Decorators
- Kernel Functions
- LLM Integration
- Matplotlib
- Microsoft Semantic Kernel
- Natural Language Processing
- OpenAI Integration
- Pandas
- Prompt Engineering
- Python
- Skill Plugins
- Workflow Automation
- AI
- Community
- .NET
section_names:
- ai
- dotnet
primary_section: ai
---
Abdulhamid_Onawole demonstrates how developers can leverage Microsoft Semantic Kernel to intelligently orchestrate existing business logic, preserving workflow reliability while adding natural language control.<!--excerpt_end-->

# How Microsoft Semantic Kernel Transforms Proven Workflows into Intelligent Agents

Many developers struggle with the disconnect between natural language prompts and real code execution. While standard LLMs (like OpenAI's ChatGPT) are excellent at generating responses, they can't directly operate on your existing systems, APIs, or business logic. The result is often a tangled web of manual orchestration and unreliable outcomes.

Microsoft Semantic Kernel (SK) addresses this gap by acting as an AI-driven orchestration layer. Instead of replacing your code with unpredictable AI generations, SK makes your proven Python (or .NET) functions discoverable and automatable by AI—without giving up control or reliability.

## Why Not Just Use an LLM Directly?

- LLMs can produce inconsistent or irrelevant outputs
- Business-specific logic and domain expertise are easily lost
- Manual workflows become more complicated, not less

## Semantic Kernel’s Approach

SK exposes your own code and workflows to large language models as callable functions ("skills"). Instead of asking an LLM to improvise, SK lets the AI select and organize your trusted logic in response to natural language instructions, increasing automation *without sacrificing accuracy*.

## Real-World Example: Kemi’s Analysis Workflow

Meet Kemi, a data analyst with a finely tuned Python script for sales charts and reports. When she tried using a generic AI model (like ChatGPT) for automation, she lost the reliability and focus of her workflow. By integrating SK:

- Her core functions (`get_sales_summary`, `create_basic_chart`) become AI-accessible
- The orchestration moves from manual step-by-step execution to natural language requests ("Email me the latest sales charts and metrics")
- She gains new features (like emailing reports), all managed through the same intelligent agent

### Code Highlights

<details>
  <summary>Registering Functions as Kernel Skills</summary>

```python
from semantic_kernel.functions import kernel_function
from typing import Annotated

@kernel_function(description="Get sales performance summary with total sales, averages, and trends", name="get_sales_summary")
def get_sales_summary(self) -> Annotated[str, "Sales summary with key metrics"]:
    # Business logic preserved
    total_sales = self.sales_data['sales'].sum()
    avg_daily_sales = self.sales_data['sales'].mean()
    return f"Total: ${total_sales:,}, Daily Avg: ${avg_daily_sales:.2f}"
```

</details>

<details>
  <summary>Enhancing Chart Generation With Parameters</summary>

```python
@kernel_function(description="Create and save a sales performance chart visualization", name="create_sales_chart")
def create_sales_chart(self, chart_type: Annotated[str, "Type of chart: 'trend', 'regional', or 'product'"] = "trend") -> Annotated[str, "Confirmation that chart was created"]:
    # Supports versatile chart types
    plt.figure(figsize=(12, 8))
    if chart_type == "trend":
        plt.plot(self.sales_data['date'], self.sales_data['sales'], marker='o')
        plt.title('Sales Trend Over Time', fontsize=16)
    # ... additional chart logic
```

</details>

### Bringing It All Together

To create an intelligent agent, Kemi wires up Semantic Kernel with her OpenAI credentials and skill plugins:

```python
from semantic_kernel import Kernel
from semantic_kernel.connectors.ai.open_ai import OpenAIChatCompletion
from dotenv import load_dotenv

def setup_agent():
    load_dotenv()
    kernel = Kernel()
    kernel.add_service(OpenAIChatCompletion(service_id="business_agent", api_key=os.getenv("OPENAI_API_KEY"), ai_model_id="gpt-4o-mini"))
    kernel.add_plugin(SmartBusinessPlugin(), plugin_name="business")
    # ...setup function orchestration as shown above
```

With the agent class, end users (and developers) can issue natural language prompts:

```python
agent = SmartBusinessAgent()
response = await agent.process_request(
   "Analyze our sales performance, create relevant charts, and email the full report to sarah@company.com"
)

# SK orchestrates all necessary steps using Kemi's original, reliable functions
```

## How to Get Started

1. **Clone the Example Repo**  
   `git clone https://github.com/your-username/semantic-kernel-business-agent`  
   `cd semantic-kernel-business-agent && pip install -r requirements.txt`
2. **Set up your OpenAI API key** using `.env`
3. **Run original scripts** and compare with the SK-powered agent
4. **Try natural language prompts** (see suggestions in the article)

## Next Steps

- Identify and register your own key functions as skills
- Build an agent class using Semantic Kernel in your favorite language
- Extend with additional capabilities (e.g., Slack notifications, dashboard updates)
- Leverage official [Semantic Kernel documentation](https://learn.microsoft.com/en-us/semantic-kernel/overview/)

**Bottom line:** Semantic Kernel lets you keep your business logic, extend it with AI, and orchestrate end-to-end workflows with greater reliability and control.

---

*Article by Abdulhamid_Onawole. For detailed code, troubleshooting, and expansion ideas, visit the full public repository linked above.*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/educator-developer-blog/how-microsoft-semantic-kernel-transforms-proven-workflows-into/ba-p/4434731)
