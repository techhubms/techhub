# From Concept to Creation: Developing Intelligent AI Agents for Modern IT Solutions
## Theory - What you should know!
### Introduction
Imagine having an intelligent system that independently analyzes problems, makes decisions, and continuously improves - exactly the capability provided by modern AI agents. This next generation of automation not only surpasses traditional workflows but opens entirely new avenues in IT operations, support, and beyond.  
In this article, we will explore how AI agents work, what technologies power them, and how to build your own AI agent with Semantic Kernel. Step by step, we’ll create an intelligent system that interacts with Large Language Models (LLMs) and flexibly approaches problems with self-developed solutions.
Ready to dive into the world of AI agents? Then, let's get started!

Artificial intelligence has made great strides. Unlike traditional systems with fixed rules, modern AI agents adapt dynamically, learn from experience, and make decisions independently. These systems solve complex tasks by learning from interactions.

In this article, we will examine the core concepts behind AI agents, trace their evolution from traditional workflow engines to adaptive, autonomous systems, and guide you step by step through building your own AI agent using the Semantic Kernel.

### Timeline and development
#### Early 2000s: Traditional workflow engines
In the early 2000s, automation was largely dominated by workflow engines such as Nintex, Windows Workflow Foundation, IBM WebSphere Process Server, and BizTalk Server. These tools enabled companies to automate business processes without in-depth programming for the first time.   

**Features:**
- Workflow engines were characterized by rigid, predefined logic and decision trees.
- Developers implemented automation using if-then-else conditions and rule-based configurations.
- The focus was on repeatable, structured tasks with clearly defined results.
- Systems were not very adaptable and required extensive reconfigurations during process changes.

While these tools increased efficiency in structured environments, they were limited in processing unstructured data and adapting to dynamic business needs.

#### The Mid-2010s: The Rise of AI and Machine Learning
While rule-based systems dominated in the early 2000s, a new era of data-driven approaches began in the 2010s. With the addition of machine learning, the focus shifted to more automated decision-making. This led to more flexible and intelligent automation solutions based on patterns and predictions.

**Important Developments:**
- **Adoption of predictive analytics:** Systems began to use data patterns to make predictions and reduce manual intervention. (SAS Advanced Analytics, IBM SPSS Modeler, RapidMiner)
- **The transition from rule-based to learning-based models:** AI applications analyzed historical data and adapted to changing conditions (TensorFlow (2015), Microsoft Azure Machine Learning (2015), H2O.ai)
- **Early applications:** fraud detection, recommender systems, and process optimization. (PayPal Fraud Detection, Amazon Recommendation Engine, Google RankBrain (2015))

These changes laid the foundation for more intelligent automation, but AI was still primarily used to complement rule-based workflows rather than as a standalone decision-maker. (UiPath, Automation Anywhere, Blue Prism)

#### Early 2020s: Emergence of Generative AI
At the end of the 2020s, generative AI emerged with transformer models – a special machine learning architecture – and its most prominent model type, GPT, demonstrated remarkable abilities in understanding and generating natural language. Initially, results were modest, but rapidly evolved to highly convincing outputs. With the ability to create human-like texts and provide contextual responses, AI was no longer seen merely as a supportive tool but as a decision-making authority in its own right.

**Key features:**
- AI systems became more context-aware and could understand and generate human-like texts.
- Generative AI sparked an interest in intelligent automation that went beyond traditional workflows.
- Applications expanded to include chatbots, virtual assistants, and AI-powered content creation.

These advances have laid the foundation for AI agents – autonomous systems that make decisions, learn from interactions, and continuously improve.

#### Present: AI Agents and Agent-Based Systems
With the continued advances in AI, we are now on the cusp of agent-based technology – autonomous entities that sense their environment, make decisions, and act to achieve specific goals.

### Description of AI agents

Unlike traditional workflows, AI agents can process **unstructured data** and make complex decisions. More advanced agents, can also **learn from interactions**, by continuously improving their performance and decisions. Those AI agents are **goal-oriented** and dynamically adapt their behavior to new circumstances. They use advanced techniques such as **reinforcement learning**, **self-supervised learning**, and **deep neural networks** to optimize their decision-making ability. An agent framework by BMW (https://arxiv.org/abs/2406.20041) or a solution like MADDOX from Peerox GmbH in Germany are examples of self-learning and adaptive systems.

AI agents represent a paradigm shift — from **static automation** to adaptive, **self-improving systems** that can operate in dynamic, real-world environments (*see Figure 1 below*).
![Figure 1 AI Ops Agents – A group of AI agents that address problems on the local system](./images/AIAgents-figure-1.drawio.svg)   
*Figure 1: AI Ops Agents – A group of AI agents that address problems on the local system*

Although the definition of "agent" may vary, agent-based systems (often called agentic or multi-agent systems) empirically demonstrate scalability by orchestrating specialized groups of AI agents (e.g., for network analysis) within a decentralized, modular framework — allowing additional agents to be integrated seamlessly without a complete redesign of the overall system. (*see Figure 2*).

For example, while a single AI agent might only answer the question, "Why is my network so slow?", an agentic system leverages the capabilities of entire groups of AI agents (covering network, application monitoring, ticketing systems, etc.) to solve more complex problems.

![Agentic System](./images/AIAgents-figure-2.drawio.svg)

*Figure 2: Agentic System*

### Essential AI Technologies for AI Agents
AI agents leverage key artificial intelligence technologies to analyze data, understand speech, and process visual information. Machine learning, a subset of artificial intelligence, enables AI agents to recognize data patterns and make predictions or decisions based on accumulated experience. This gives rise to the following specializations:

- **Neural Networks and Deep Learning**   
Neural networks inspired by brain structures help AI agents understand complex data. Deep learning leverages multi-layered networks for tasks such as image and speech recognition.
- **Natural Language Processing (NLP)**   
NLP enables communication in human language, which is essential for chatbots, virtual assistants, and automated systems. It underpins large language models – research in this domain has led to their creation. Examples include sentiment detection, intent recognition, or entity detection applied either within or on top of LLMs.
- **Computer Vision**   
AI agents interpret visual data and make decisions that are useful in robotics, quality control, and face recognition.
- **Reinforcement Learning**   
AI agents learn optimal decision-making through reward feedback systems, particularly in dynamic environments.
- **Transformer Models and LLMs**   
Transformer models, a neural network architecture, enable efficient text processing and generation. They form the basis for LLMs, which are trained on vast amounts of textual data, enabling advanced language processing, comprehension, and generation.


## Practice - Create Your Own AI Agent!
Now that we have explored the evolution and core concepts of AI agents – from rule-based systems to autonomous, self-improving entities – it’s time to put theory into action.

In this practical section, we will build a working AI agent using Microsoft’s Semantic Kernel. This framework acts as a bridge between your application and powerful Large Language Models (LLMs), making it easy to integrate intelligent behavior into your software.

Step by step, we will walk through setting up a .NET project, integrating Semantic Kernel, and creating a team of specialized agents that can analyze and resolve IT issues autonomously. Whether you’re building a smart assistant, an AI Ops tool, or just experimenting with agent-based systems, this guide will give you the foundation you need to get started.

Let’s dive in and create your first intelligent AI agent.

### Brief Introduction to Semantic Kernel  
Semantic Kernel is a framework that acts as middleware between applications and LLMs. It provides an abstraction layer that allows developers to easily integrate AI capabilities into their software. Thanks to its modular structure, different AI models and plugins can be flexibly combined.

#### Key features
- **Easy integration:** Integrates seamlessly with existing .NET projects.
- **Modularity:** Allows flexible adaptation of AI components to specific requirements.
- **Scalability:** Supports deployment in large environments and distributed systems.

Developers can use Semantic Kernel to create powerful AI agents and extend their applications with intelligent features.

Several frameworks exist for developing AI agents, including LangChain/Graph, AutoGen / AG2, and Semantic Kernel. In this article, we will use Semantic Kernel due to its flexible and modular approach for integrating large language models (LLMs) into applications. At the time of writing, the Agent Framework within Semantic Kernel is still in preview but is under active development by both Microsoft and the open-source community – ensuring continuous improvements and long-term support.

Semantic Kernel is particularly well-suited for building LLM-based AI agents because of its strong focus on language model orchestration and plugin extensibility. It supports multiple programming languages, including C#, Python, and Java, making it accessible to a wide range of developers.


### Step-by-Step Guide: Integrating the Semantic Kernel with the Agent Framework
We begin by setting up a new project, adding the necessary dependencies, and configuring access to an LLM. By the end of this section, you will have a working base for your AI agent.

In the following steps, we will:
- Create a new .NET Project
- Install the Semantic Kernel NuGet packages
- Build a simple AI agent
- Evolve it into a team of agents

#### Preconditions
- **Technical knowledge:** Experience with C#, .NET and Terminal commands
- **Access to an LLM, such as:**
    - an (Azure) OpenAI GPT model
    - a locally hosted Ollama model


#### Start with an example: "AI Ops"
We are planning an analysis system for AI Ops that uses generative AI to automate the resolution of IT tickets. The goal is for AI agents to analyze and solve problem descriptions. These agents will need access to the network, CPU, and memory. Our team will consist of an analysis agent and an evaluation agent. The system should be capable of independently finding problems and proposing expert-level solutions (see Figure 1 AI Ops Agents).

#### Create a New .NET Project:  
`dotnet new console -o MyAIAgents`  
`cd MyAIAgents`

#### Installing Semantic Kernel
To work with Semantic Kernel Framework, add the following Nuget packages:    
```bash 
dotnet add package ...
```
- `Microsoft.SemanticKernel`  
- `Microsoft.SemanticKernel.Agents.abstractions ↩`   
                    `--version 1.35.0-alpha`
- `Microsoft.SemanticKernel.Agenten.core ↩`   
                    `--version 1.35.0-alpha`
- `Microsoft.SemanticKernel.agenten.openai ↩`   
                    `--version 1.35.0-alpha`

**Note**:  
*Some of these packages are still in preview, and version numbers are subject to change. Please refer to the official Semantic Kernel documentation if you are using a newer version.*

#### Access to an LLM  
To create an AI agent, you need access to an LLM. We mentioned three common options here:
- **Azure OpenAI:** Create an Azure OpenAI service in the Azure portal and note the API key.
- **OpenAI API:** Register on the OpenAI website and generate an API key.
- **Local models:** Use Ollama for a locally hosted model.

For this project, we use the Azure OpenAI Service. OpenAI's API requires only minor changes in the code, as both approaches are implemented almost identically in Semantic Kernel.

#### Set Environment Variables   
We typically store configuration details such as API keys in environment variables. This approach simplifies moving the project to different environments, including Docker. (A version using standard .NET settings is also available in our GitHub repository.)

Create a file called `.env` in the project directory and add:
```bash
AZURE_OPENAI_API_KEY=<your API Key>
AZURE_OPENAI_ENDPOINT=<your endpoint>
AZURE_OPENAI_MODEL_ID=<your deployment model>
```
Note: The deployment model is the name of the model you've deployed to Azure OpenAI.  
![Deployment in Azure Foundry](./images/ModelDeployment-screenshot0.png)   
*Figure 3: Model deployment name in Azure Foundry*


Install the `DotNetEnv` package to load the environment variables in your code:  

`dotnet add package DotNetEnv`  

*Note: Also create a `.gitignore` file and add .env to avoid storing sensitive information in your Git repository.*

After adding Semantic Kernel to our .NET project, we can create a simple AI agent that interacts with an LLM. Let's start writing code.   


#### Initializing Semantic Kernel
We next adjust Program.cs by:
- Building the Kernel 
- Setting up Semantic Kernel for connection to an LLM


**Program.cs**
```csharp
class Program
{
    static async Task Main(string[] args)
    {
        //Alternative to appsettings.json..:
        DotNetEnv.Env.Load();  //load the Env Vars
        
        //Initializing the kernel
        var builder = Kernel.CreateBuilder();
        Builder.AddAzureOpenAIChatCompletion(
        Environment.GetEnvironmentVariable("AZURE_OPENAI_MODEL_ID"),
        Environment.GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT"),
        Environment.GetEnvironmentVariable("AZURE_OPENAI_API_KEY"));
        var kernel = builder.Build();
                
        //Creating a clone to provide a network capability
        var networkKernel = kernel.Clone();
        var pluginNetwork = KernelPluginFactory.CreateFromType<NetworkMonitor>();
        networkKernel.Plugins.Add(pluginNetwork);

        //Second kernel with host monitoring capability
        var hostMetricsKernel = kernel.Clone();
        var pluginHost = KernelPluginFactory.CreateFromType<HostMetrics>();
        hostMetricsKernel.Plugins.Add(pluginHost);
        // Logging and housekeeping omitted for brevity
    }
}
```

The method `DotNetEnv.Env.Load()` loads environment variables from the .env file and makes them available in the program, which is especially useful for managing API keys without embedding them in code.
First, the `KernelBuilder` is created. With `AddAzureOpenAIChatCompletion`, the kernel is configured to interact with the appropriate LLM. For those opting for OpenAI's API, use `builder.AddOpenAIChatCompletion` instead.

We clone the kernel to add specialized functionality through plugins, such as network checks or host metrics. Cloning allows for a separation of responsibilities and prevents agents from calling functions they should have no access to. An agent can only use the plugins present in the kernel to perform its intended tasks.
In our example, we have an agent that can perform network checks:

```csharp
KernelPluginFactory.CreateFromType<NetworkMonitor>();
networkKernel.Plugins.Add(pluginNetwork);
```
Below is an excerpt from the plugin class to illustrate its implementation.

**NetworkMonitor class**
```csharp
[KernelFunction()]
[Description("Returns information about the network adapters")]
public static List<string> adapterInfos()
{
    List<string> adapterInfo = new List<string>();
    foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
    {
        string info = $"Name: {ni.Name}, type: {ni.NetworkInterfaceType}, status: {ni.OperationalStatus}";
        adapterInfos.Add(info);
    }
    return adapterInfos;
}
```
Semantic Kernel leverages method decorators, such as `KernelFunction` and `Description`, to expose functions to the LLM while preventing direct system access. In this way, when the LLM requires information on network adapters, it simply triggers the adapterInfos() function, which securely returns the data.


#### Our first agent
Let’s define our first AI agent – a network analyst.
```csharp
ChatCompletionAgent agentNetworkChecker = new()
{
    Name = nameNetworkChecker,
    Instructions =
    """
    **Role:** You are a Network Checker with over 10 years of experience. You specialize in diagnosing network connectivity issues.

    <… (shortened) The prompt defines the role of the agent and provides instructions on how to analyze network problems>
    """,
    Kernel = networkKernel,
    Arguments = new KernelArguments(new AzureOpenAIPromptExecutionSettings() 
                                    {FunctionChoiceBehavior = FunctionChoiceBehavior.Auto() }
                                    ),
    LoggerFactory = loggerFactory
};
```
Each agent is defined by a name, a prompt (instructions), and a kernel. The prompt instructs the LLM regarding its role, and the arguments allow it to decide whether to call a plugin function or produce a direct response. In this case, the agent is configured with network-specific capabilities via the network kernel.

Next, create a ChatHistory object to manage the conversation.
```csharp
ChatHistory chat = [];

//Add the user message
chat.Add(new ChatMessageContent(AuthorRole.User, "<user input>"));

//Generate the agent response(s)
await foreach (ChatMessageContent response in agentNetworkChecker.InvokeAsync(chat))
{
  //Process the answer(s)...
}
```

#### An Agent System
While a single AI agent is useful, their full potential is realized when multiple agents work together. In our system, various specialized agents collaborate to handle complex tasks. In addition to the network agent, you can define agents for CPU/storage checks, analysis, and resolution:
```csharp
ChatCompletionAgent agentNetworkChecker = ... ; //checks the network
ChatCompletionAgent agentCommonChecker = ... ;  //checks CPU, storage, ...
ChatCompletionAgent agentAnalyst = ... ;        //Analyzes agent results
ChatCompletionAgent agentResolver = ... ;       //evaluates the analysis result
```
To coordinate these agents effectively, we define an `AgentGroupChat` structure. This setup controls which agent speaks next and when the analysis ends, utilizing a `SelectionStrategy` and a `TerminationStrategy`.

```csharp
AgentGroupChat groupChat = 
new(agentAnalyst, agentNetworkChecker, agentResolver, agentCommonChecker)
{
    ExecutionSettings = new()
    {
        SelectionStrategy = new KernelFunctionSelectionStrategy(selectionFct, kernel)
        {
            InitialAgent = agentAnalyst,
            HistoryReducer = new ChatHistoryTruncationReducer(1),
            HistoryVariableName = "lastmessage",
            return the response from the current agent/or its name
            ResultParser = (result) =>{
                var selection = result. GetValue<string>() ?? agentAnalyst.Name;
                return selection;
            }
        },
        TerminationStrategy = new KernelFunctionTerminationStrategy( terminationFct, kernel)
        {
            agents = new[] { agentResolver },
            HistoryVariableName = "lastmessage",
            HistoryReducer = new ChatHistoryTruncationReducer(1),last message
            MaximumIterations = 10, // maximum 10 rounds 
            ResultParser = (recall) => {
                //check agent response for presence of the word "YES" 
                var result = recall.GetValue<string>();
                var containesYes = result?.ToLower().Contains("yes");
                return containsYes ?? false;
            }
        }
    }
};
```
Our `SelectionStrategy` determines which agent responds next based on the previous message; the `TerminationStrategy` defines when to end the dialogue, such as when a "YES" response is detected or after a maximum number of rounds.

Following we write a `SelectionStrategy` template, orchestrating agents by their subsequent responses.
```csharp
KernelFunction selectionFct = AgentGroupChat.CreatePromptFunctionForStrategy(
$$$"""
**Task:** Determine which agent should act next based on the last response. Respond with **only** the agent's name from the list below. Do not add any explanations or extra text.

**Agents:**

- {{{nameAnalyst}}} 
- {{{nameNetworkChecker}}}
- {{{nameCommonChecker}}}
- {{{nameResolver}}}

**Selection Rules:**

- **If the last response is from the user:** Choose **{{{nameAnalyst}}} **.
- **If the last response is from {{{nameAnalyst}}} and they are requesting network details:** Choose **{{{nameNetworkChecker}}}**.
- **If the last response is from {{{nameAnalyst}}} and they are requesting non-network system checks:** Choose **{{{nameCommonChecker}}}**.
- **If the last response is from {{{nameAnalyst}}} and they have provided an analysis report:** Choose **{{{nameResolver}}}**.
- **If the last response is from {{{nameNetworkChecker}}} or {{{nameCommonChecker}}}:** Choose **{{{{nameAnalyst}}} }**.
- **Never select the same agent who provided the last response.**

**Last Response:**

{{$lastmessage}}
""",
safeParameterNames: "lastmessage"
);
```
This template clearly outlines which agent should respond based on the content of the last message. The placeholders like `{{{nameAnalyst}}}` are replaced at runtime with the actual agent names. The variable `lastmessage` is made available via the `HistoryVariableName` property, ensuring only the relevant message is used for agent selection.
  
The `TerminationStrategy` is managed similarly to the `SelectionStrategy`, determining the chat ending. 

Finally, we initiate the group conversation by adding a user message and processing responses asynchronously.
```csharp
groupChat.AddChatMessage(new ChatMessageContent(AuthorRole.User, <User Input Message>));
await foreach (ChatMessageContent response in groupChat. InvokeAsync())
{
    Console.WriteLine($"{response. AuthorName.ToUpperInvariant()}:\n{response. Content}");
}
```
This setup allows us to embed our AI agents in support chatbots, automated incident management systems, or internal analysis pipelines. For a complete, interactive implementation, please refer to our GitHub repository at [https://github.com/totosan/tutorial-aiagents](https://github.com/totosan/tutorial-aiagents).

## Conclusion
AI agents are more than just a technological innovation – they transform the way we approach automation and decision-making. With Semantic Kernel, we can build intelligent systems that autonomously search for solutions to complex problems. As development in this area continues to progress rapidly, new models, enhanced orchestration mechanisms, and optimized agent interactions will further expand the realm of possibilities.

To explore the full implementation – including an interactive loop that allows you to chat directly with your AI agents – please visit our GitHub repository: https://github.com/totosan/tutorial-aiagents.   
This is an excellent starting point for experimenting, extending, and building our own intelligent agent systems.  

<img src="./images/GitHub_totosan_qr.png" width=200/>

*by Thomas Tomow (CTO/COO @ Xebia MS Germany)*