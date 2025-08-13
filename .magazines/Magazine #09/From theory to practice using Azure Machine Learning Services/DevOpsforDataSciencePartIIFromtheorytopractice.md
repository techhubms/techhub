# DevOps for Data Science Part II

From theory to practice using Azure Machine Learning Services

*Rob Bos & Kees Verhaar*

In the previous issue of XPRT magazine[^1] we discussed the DevOps
process for a Data Science team. We explained how the general principles
in DevOps are used when developing Artificial Intelligence or Machine
Learning models that can be used in a multitude of applications. We
arrived at the DevOps cycle as shown in Figure 1, in which the DevOps
pipeline joins the worlds of Data Scientists and App developers
together. In this article we'll explore how to set up each of the steps
in this workflow using components available in Azure Machine Learning
Services and Python scripts.

![](./media/image1.png)


Figure 1: Implementation of the typical DevOps cycle for application
development combined with Data Science

## Data Scientist IDE

It all starts with the Integrated Development Environment. All teams,
and data science teams in particular, have their own way of doing things
that they have cultivated and tweaked through time. The tools in Azure
have been built with this in mind, and support a model of 'bring your
own': whether it is your own datastore, compute power or source control
methods: Azure Machine Learning usually supports them. You can choose to
use Jupyter Notebooks, Data Bricks Clusters, or your Python scripts. The
available solutions range from "everything on your own laptop" to "fully
SaaS Jupyter notebooks" and a lot of options in between.

For example:

-   Local computer: install all the tools you want yourself, you are
    responsible for maintaining everything yourself, including backups,
    etc.

-   Data Science VM: essentially the same as your local computer, except
    everything is pre-installed, with the most common libraries already
    installed. This also is an easy and repeatable method for deploying
    machines in Azure with all the tools installed. When you are done
    with your project or analysis run, you can safely delete them.

-   Python scripts: to ensure you are completely independent of a
    specific runtime environment.

-   Azure Notebooks: hosted Jupyter notebooks, where you do not have to
    update the runtime environment.

These options enable you to create a process that matches your own
workflow. In the rest of this article we will use Python scripts to show
you an example workflow and how that integrates with Azure ML Services.

## Model management

Data Science Model development is essentially a three-stage process, as
shown in Figure 2: you prepare training data, then use that to build and
train your model, and finally deploy it to production. Each stage has
its own iteration cycle and results in components that can be used in
the next stage.

![](./media/image2.png)


Figure 2: A typical Data Science workflow

Azure ML Services takes a key role in this process. It provides a place
to manage the compute resources available to your team, automate your
model training process, track model versions and results, and manage
deployments.

### Creating a workspace

Before you can use Azure ML services, you'll need a workspace and
connect to it from your Python code. The workspace can be seen as a
project or collection of everything you need in the development process
of a model. You can use Azure's role-based access control (RBAC) to
share the workspace with your team.

![](./media/image3.png)


Figure 3: An Azure ML service workspace in the Azure portal

To create a workspace from Python, you import the Azure ML SDK and write
a couple of lines of Python:

import os

from azureml.core import Workspace

\# Load the parameters we need to create a workspace

subscription_id = os.getenv(

\"SUBSCRIPTION_ID\", default=\"xprt-1234-xprt-1234\")

resource_group = os.getenv(\"RESOURCE_GROUP\", default=\"mlpipeline\")

workspace_name = os.getenv(\"WORKSPACE_NAME\",
default=\"magazine9-mlpipeline\")

workspace_region = os.getenv(\"WORKSPACE_REGION\",
default=\"westeurope\")

\# Connect to existing workspace or create a new one

try:

ws = Workspace(subscription_id=subscription_id,

resource_group=resource_group, workspace_name=workspace_name)

\# write the details of the workspace to a configuration file to the
notebook library

ws.write_config()

except:

\# Create the workspace using the specified parameters

ws = Workspace.create(name=workspace_name,

subscription_id=subscription_id,

resource_group=resource_group,

location=workspace_region,

create_resource_group=True,

exist_ok=True)

ws.get_details()

\# write the details of the workspace to a configuration file to the
notebook library

ws.write_config()

The code shown above tries to connect to an existing workspace based on
the set environment variables. If the workspace doesn't exist, it will
create it. In both cases, the workspace configuration is written to a
file so it can easily be reused later. The SDK uses this file to connect
to the workspace and is used in all the environments in the same way.
You can include this code in a Python script or run it from a Jupyter
Notebook.

### Managing compute

Preparing data and training a model takes a lot of compute power. The
power of the cloud is available in various options (e.g. DataBricks or
Azure ML Services compute). That enables you to skip doing the heavy
calculations on your development machine and utilize a server in the
cloud. This compute power is available on demand, so you can easily do
periodical reevaluations when the dataset has changed as new data comes
in. Managing your compute targets through Azure ML service allows your
team to share (sometimes costly) compute targets, which of course saves
costs. It also makes it simple to execute computationally highly intense
jobs on large and complex clusters of machines. It also enables you to
scale the compute in and out based on your usage. You can even scale the
compute down to zero if you are not using anything! The scaling is
calculated on a metric like the percentage of CPU or RAM used over a
period of time. This is used for scaling up as well as for scaling down.

Before you can execute jobs on a compute target, you'll need to define
it in your code. The following code segment shows how this is done in
Python, using the Azure ML SDK:

\# Create compute

cpu_cluster_name = \"cpu-cluster\"

\# Verify that the cluster does not exist already

try:

cpu_cluster = ComputeTarget(workspace=ws, name=cpu_cluster_name)

print(\"Found existing cpu-cluster\")

except ComputeTargetException:

print(\"Creating new cpu-cluster\")

\# Specify the configuration for a new Azure Machine Learning Cluster

compute_config =
AmlCompute.provisioning_configuration(vm_size=\"STANDARD_D2_V2\",

min_nodes=0,

max_nodes=4)

\# Create the AML cluster with the specified name and configuration

cpu_cluster = ComputeTarget.create(ws, cpu_cluster_name, compute_config)

\# Wait for the cluster to complete, show the output log

cpu_cluster.wait_for_completion(show_output=True)

As mentioned before, you can connect many types of compute targets to
Azure ML service[^2]. The code as shown above connects to an existing
"Azure Machine Learning Cluster" (which is the simplest form of compute
to create) in the ML Service workspace, or it creates one if it doesn't
exist. When creating an AML cluster, you specify the VM size and the
minimum and maximum number of nodes. A minimum of zero means the cluster
will be shut down after some time of inactivity, which again helps in
reducing cost. It is common practice to start with a maximum of four
nodes and run the experiments to get a feeling for the problem you are
trying to solve, and whether or not it will benefit from more compute
power.

### Data preparation & model training

When you have your IDE and compute targets set up, you can start
building your model. This starts with preparing data and then training
the model. This is an iterative process, which can easily take many
cycles before getting a model that meets your requirements. Azure ML
services provides a place for automating this process, which is
important for repeatability and traceability. This automated process is
captured in a ML pipeline, which consists of multiple steps. For
example, to create a step that executes a Python script on the cluster
we just created:

\# Specify the directory that contains the Python code for this step

source_directory = \'./train\'

print(\'Source directory for the step is {}.\'.format(

os.path.realpath(source_directory)))

\# Specify the script to execute from the source_directory

\# and the compute target for this step (the cluster we just created in
this case)

step1 = PythonScriptStep(name=\"train_step\",

script_name=\"train.py\",

compute_target=cpu_cluster,

source_directory=source_directory,

allow_reuse=True)

By specifying a unique source directory for each step, Azure ML
pipelines will cache the result of the step, so it can be re-used in
subsequent pipeline runs if the files in the source directory haven't
changed. In a similar fashion, you can define pipeline steps to be
executed on, for example Azure DataBricks or Azure Batch. Once all steps
are defined, you combine them into a ML pipeline:

\# list of steps to run

steps = \[step1, step2, step3\]

print(\"Step lists created\")

\# Build the pipeline. All steps will be executed in parallel

pipeline1 = Pipeline(workspace=ws, steps=steps)

print(\"Pipeline is built\")

\# Submit the pipeline to be executed

pipeline_run1 = Experiment(ws, \'Hello_World1\').submit(

pipeline1, regenerate_outputs=False)

print(\"Pipeline is submitted for execution\")

pipeline1.publish(name=\'Hello_World1 pipeline\',

description=\'My very cool pipeline\')

In this case, all steps will be executed in parallel, since they are
independent. If you explicitly specify inputs and outputs of steps, the
Azure ML pipeline will calculate dependencies and execute steps in the
appropriate order.

By publishing the pipeline, it becomes available for your team to view,
edit, and re-run. You can trigger the pipeline from the Azure Portal,
but you can also do this programmatically through a simple REST request:

\# Retrieve an AAD token to authenticate your REST request

auth = InteractiveLoginAuthentication()

aad_token = auth.get_authentication_header()

\# Get the rest endpoint from the pipeline. You can also get this from
the Azure portal

all_pub_pipelines = PublishedPipeline.list(ws)

pipeline_to_start = all_pub_pipelines\[0\]

rest_endpoint1 = pipeline_to_start.endpoint

print(\"You can perform HTTP POST on URL {} to trigger this
pipeline\".format(rest_endpoint1))

\# Start the pipeline. You can optionally specify parameters for the
pipeline as a json body

response = requests.post(rest_endpoint1,

headers=aad_token,

json={\"ExperimentName\": \"My_Pipeline1\",

\"RunSource\": \"SDK\",

\"ParameterAssignments\": {\"pipeline_arg\": 45}})

run_id = response.json()\[\"Id\"\]

print(run_id)

When tuning a model and its parameters, you typically execute your
pipeline many times. The pipeline results are stored in an experiment,
where each iteration of an experiment results in a "run". Azure ML
services tracks these runs and the results in a central place. By
logging the appropriate attributes from the ML pipeline, the Data
Science team can collectively see graphical logged information, like
model performance indicators and duration metrics. They can even see the
version of the data the model was trained with, so they can take
decisions based on that information.

To log a specific value, invoke the start_logging() method on your
experiment and then use the log(...) method to log a metric:

exp = Experiment(workspace=ws, name=\'test_experiment\')

run = exp.start_logging()

run.log(\"test-val\", 10)

By invoking the log() method during training you will get a
visualization of that metric in your Azure ML workspace. In this example
we will show the charts for two values we are logging:

### ![](./media/image4.png)

### Registering a model

When you are happy with your model results, it's time to register it. By
registering your model, you assign a certain status to it. This can be
anything from an Alpha/Beta labeling scheme to a version number.
Published models can then (potentially) be deployed to production. The
model to register is the output of an experiment run. Registering a
model from a run is simple:

run.register_model(\'super cool model\')

More advanced scenarios include automated evaluation of the model, and
depending on the outcome of the evaluation, this can be registered. For
example: only register the model if it performs better than our
currently deployed model. The way to quantify "performs better", is up
to you. It is then trivial to include this step in your ML pipeline,
further automating your process.

## Deployment using an Azure DevOps pipeline

When you have a versioned model that performs at the required level, it
is time to deploy it. A deployed model enables an application to
actually use the model to make predictions with. There are a lot of
options to deploy a model, like hosting it in a webservice or a Docker
container that can be deployed anywhere you want. That choice depends on
the application that will use the model and the requirements you have.
The most common scenario will be to create a Docker container and
deploy, for example in Azure Kubernetes Service (AKS) or Azure Container
Instance(ACI), or even in an Azure App Service!

In this scenario you'll need a scoring script that is executed against
the model. The scoring script accepts inputs, feeds them to the actual
model, and presents the output back to the user. An example of a scoring
script is a simple webserver that accepts a REST request with some input
and sends the model response back in JSON format. The scoring script is
packaged in a Docker container, together with the actual model. There
are various ways to deploy the model from Azure ML Services, but the
most flexible method is to use Azure DevOps. This also opens up options
for integrating with other parts of your application and their
deployment pipelines.

It also has additional benefits in terms of monitoring model
performance, as we'll see later.

Deploying your models with Azure DevOps can be done in multiple ways,
depending on the preferences of the Data Science team:

-   From a Python script with the Azure ML Service and include it in you
    Azure Pipeline.

from azureml.core.model import Model

model = Model.register(workspace=ws, model_path=\"model.pkl\",
model_name=\"model-test\")

-   Using the Azure ML extension in the Azure CLI[^3].

> ![](./media/image5.png)
> 

This also registers the deployment in Azure ML service and annotates it
with the version number and other metadata to provide end-to-end
traceability. Especially in an Enterprise environment it is important to
have logging on what was changed and by whom.

### Machine Learning extension in Azure DevOps

A very helpful Azure DevOps extension is the "Machine Learning"
extension[^4]. This enables you to trigger a release whenever a new
model is registered. The new model is then an artefact to trigger your
release on. It also provides extra tasks to deploy a model to Azure ML
services and to set the optimal values for CPU and memory options for
the Docker container to use, and which are retrieved from the metadata
in Azure ML services.

## Telemetry

In any DevOps culture, monitoring the running software is a key
ingredient to a healthy process. This is also true for Data Science:
once your model is deployed, you want to know how it performs. Without
any monitoring you do not know anything about the data that the model
gets and generates, or when the model needs tuning due to data drift.
You might even be influencing some results with the predictions from the
model! There are two levels of performance that are interesting: model
performance and technical data.

### Model performance

Monitoring model performance over time means you need to gather data on
accuracy and data drift. Data drift occurs when production inputs turn
out to have different characteristics than the data used to train the
model. In order to analyze model accuracy and the amount of data drift
occurring, it is necessary to collect model input and outputs and
analyzing them.

If you have your model deployed in AKS, it's very easy to gather the
required data by just including a few lines of Python in your scoring
script[^5]. Collected data is stored in blob storage. From there you can
retrieve it, and analyze it through e.g. DataBricks or PowerBI.

from azureml.monitoring import ModelDataCollector

global inputs_dc, prediction_dc

inputs_dc = ModelDataCollector(\"best_model\", identifier=\"inputs\",
feature_names=\[\"feat1\", \"feat2\", \"feat3\". \"feat4\", \"feat5\",
\"feat6\"\])

prediction_dc = ModelDataCollector(\"best_model\",
identifier=\"predictions\", feature_names=\[\"prediction1\",
\"prediction2\"\])

data = np.array(data)

result = model.predict(data)

inputs_dc.collect(data) #this call is saving our input data into Azure
Blob

prediction_dc.collect(result) #this call is saving our prediction
results into Azure Blob

### Technical data

Just like any other application or service, it is vital to make sure
your model is always up and running like it should. For this you'll need
to gather performance data, such as response times, any exceptions that
occurred, etc. In Azure ML, this is implemented through Application
Insights. When deploying to AKS from Azure ML, technical data collection
is enabled by setting the appropriate parameter in your deployment
configuration:

aks_config = AksWebservice.deploy_configuration(collect_model_data=True,
enable_app_insights=True)

Data is then automatically gathered in Application Insights, from which
you can analyze it, configure alerts, etc.

# Conclusion

The best options for your Data Science process depend heavily on what
you're trying to achieve and on the skills of the team. We've shown a
very simple setup to show the basics, and much more is possible. We've
also shown how flexible the tooling has become to set up an Azure DevOps
Pipeline using Python, used by most Data Science teams.

[^1]: <https://pages.xpirit.com/magazine8>

[^2]: <https://docs.microsoft.com/en-us/azure/machine-learning/service/concept-compute-target#train>

[^3]: <https://aka.ms/aml-cli>

[^4]: <https://marketplace.visualstudio.com/items?itemName=ms-air-aiagility.vss-services-azureml>

[^5]: <https://docs.microsoft.com/en-us/azure/machine-learning/service/how-to-enable-data-collection#enable-data-collection>
