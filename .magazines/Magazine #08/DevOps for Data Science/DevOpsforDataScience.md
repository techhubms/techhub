# DevOps for Data Science

*Rob Bos & Kees Verhaar*

As a reader of this magazine, you'll be familiar with the concept of
DevOps: closing the gap between all disciplines involved in software
engineering, and enabling continuous delivery of value to your end
users. This sounds simple enough, yet it proves to be very hard in
practice. In a typical software delivery environment, there are many
moving parts which all need to work together -- organizationally as well
as technically -- to be effective.

To make matters worse, modern applications include Machine Learning or
Artificial Intelligence components. These require a particular skillset,
typically embodied in a Data Scientist. They use tools unfamiliar to the
typical .NET developer and follow a development cycle that differs from
what a .NET developer is used to.

In this article, we will explore the DevOps process for an app that
includes an Artificial Intelligence model. In the next issue of XPRT
magazine, we will implement this process in a real-world example.

# Build -- Measure -- Learn for Application Development

A typical DevOps process entails three significant parts: Build, Measure
& Learn, which comprises the typical DevOps cycle as shown in Figure 1.

![](./media/image1.png)


Figure 1: The DevOps cycle (source:
https://innovationorigins.com/nl/startups-op-zoek-naar-een-prototype-perfect-kan-de-vijand-zijn-van-goed-genoeg/build-measure-learn/)

In "Build", we develop our application: we gather requirements, we
translate them into code, we compile, deploy, test, and we release it to
production. Then, we "measure": is our application running? Is it
performing the way we expect? How are our users using the app? Finally,
we evaluate our measurements and extract "learnings" from it. How can we
improve user experience? What should be the next feature we work on? Do
we need to work on stability? We feed these learnings back to the
beginning of our loop (the "Build" part), and we continuously repeat
this cycle to improve steadily.

In a typical .NET world, the process to implement this cycle looks
similar to what is shown in Figure 2.

![](./media/image2.png)

Figure 2: Implementation of the typical DevOps cycle for application
development

Now, what happens if we want to infuse a little Artificial Intelligence
(AI) into our application?

# Build -- Measure -- Learn: The Data Science way

To understand what is required to incorporate AI into our application,
we must first understand the development cycle of an AI model. In
"traditional" application development, you write code and an app comes
out. In Data Science, this works slightly different. The result of the
work of a Data Scientist is a model. This model has inputs and outputs,
depending on what the model was built for. The model could be designed
to detect anomalies in a continuous stream of data (for example to
detect impending server outages based on operational metrics) or to
recognize faces in a photograph. It could be anything. Three factors are
deterministic for a model:

-   Training Features: a set of variables that are generated from the
    raw data and are used to train the model.

-   Model structure: for instance a linear regression model, a decision
    tree model, or a random forest model.

-   Hyperparameters of the model: for example, for a random forest
    model, how many trees, the maximum depth of each tree, and the
    minimum number of observations in each leaf node. Or for an
    artificial neural network model: the number of hidden layers, how
    many hidden nodes, the activation function, learning rate, and
    random seed.

These three factors are determined and tuned until the model satisfies
its required performance. A typical Data Science workflow is shown in
Figure 3.

![](./media/image3.png)


Figure 3: A typical Data Science workflow

# Joining both worlds

When both workflows are combined, we arrive at a process that is similar
to the process shown in Figure 4.

![](./media/image4.png)

Figure 4: Implementation of the typical DevOps cycle for application
development combined with Data Science

The key component that ties the world of the app developer and the Data
Scientist together is a Model Management Service, which helps track
model versions, performance, and deployments. Let's go over three
critical DevOps pillars to see what should be considered there:
Traceability, Automation, and Feedback.

## Traceability

As we have seen before, three factors are deterministic for a model:
training features, model structure, and the hyperparameters used. The
first step is to determine these factors and then tune them until the
model has the performance (high enough accuracy and low enough errors)
to satisfy your criteria.

Because the steps during the modeling phase have a profound impact on
the accuracy of the model, it becomes essential to have traceability of
design choices. If you can't provide a clear overview of the history of
the model, it will become a black box that makes some predictions
without a way to validate its choices. This can have real repercussions
when the model's prediction is being used to make critical decisions.
Think for example of a healthcare situation, where you are predicting
the probability that a patient is suffering from a particular disease,
based on a set of symptoms. This could lead to a life or death decision
and makes it very clear why humans still need to verify predictions and
conclusions. To do so effectively, they need the entire context.

### Training features

As an example, say that you are predicting the necessity of a hospital
in a city and you decide to calculate that necessity based on the
average age of the population in the city and the average traveling time
they would have to the hospital. You split the age data into 70%
training data and 30% validation data. By doing this for the entire set
of residents, you didn't account for the fact that the necessity of a
hospital is strongly correlated to the average age of residents in a
specific area. The city in our example happens to have a very distinct
set of age groups living in a particular area. The city can be split
into three different areas:

-   Area 1 has 10.000 residents, all in the age group of 30-40

-   Area 2 has 20.000 residents, all in the age group of 20-30

-   Area 3 has 30.000 residents, all in the age group of 40-50

You can see that by randomly splitting the full data set and using that
data for training a model, you will skew the prediction, as half of the
dataset actually has an age group of 40-50.

This example shows how easy it is to get a bias into your model by
choosing to use the full dataset and forgetting to check the grouping of
the features in the dataset. These decisions are usually made during a
data discovery phase where you search for the properties of the dataset
that are relevant for training the model. By making sure you have the
setup of the model in source control with descriptive commit messages,
you can keep track of the reasoning behind the choices you had to make
with that dataset. This also allows you to set up checks for the dataset
that you can later reuse to reevaluate those choices when new data is
available. By doing so, you open up the black box, and you obtain
visibility in the decisions and underlying reasons for them.

### Model structure

By having the code of the model available in source control, you can
also experiment with different algorithms and document their accuracy on
the dataset you are using. You can record the outcomes and include them
with the code you use in the final model. If you set up this process in
the right way, you can use its documentation for "release notes" that
contain the full research steps and reasoning behind the choices leading
to this version of the model.

### Hyperparameters of the model

Hyperparameters of a model are the settings you use during the training
phase of the model creation. Take a decision tree algorithm for example.
A regular tree will loop through the data and decide how much a specific
feature will impact the desired outcome. See Figure 5 for a
visualization of the steps taken to determine the income of a person,
based on the features we fed to the algorithm.

![](./media/image5.png)


Figure 5: Example of a decision tree

For this algorithm, you can change the number of trees to use, the
maximum depth of each tree and the minimum number of observations in
each leaf node. Changing these settings can have a significant impact on
the model, and on the time it will take to train the model. It is
paramount to keep track of the parameters that were used for training
and the impact they had on the resulting model. You need to store the
values and the outcome in a separate store, where you can link the
settings, the outcome, precision, loss values, etc. to the code used to
determine this.

## Automation

Automation is critical when employing DevOps. It fosters speed, greater
accuracy, consistency, reliability, and increases the number of
deliveries. When thinking of automating steps in building AI models, we
should consider two principal parts. The first part involves the
automation of preparing training data, training the model and evaluating
the model's performance. The second part consists of the automation of
integrating the model into your application and deploying this. The
first part is implemented in a Machine Learning (ML) pipeline, while the
second part is performed in a DevOps pipeline. The ML pipeline is the
domain of the Data Scientist, while the DevOps pipeline bridges the gap
between the Data Scientist and the app developers.

The nature of building an ML model sets some specific requirements for
automation tooling in this space.

-   Compute: an ML pipeline consists of computational steps. A lot of
    these steps (especially the steps of preparing data and training the
    model) are very computationally intensive. Automation tooling
    should, therefore, make it easy to distribute execution of these
    steps across large clusters of machines, so that execution time
    stays within acceptable time constraints.

-   Tools: building an ML model requires specific toolkits and
    frameworks, most of which are very different from what we are used
    to in .NET application development. Python, TensorFlow and
    SciKit-learn are just a few examples of what a Data Scientist uses
    daily, while a .NET developer might not be so familiar with these.
    Automation tooling for ML models should seamlessly work with these
    tools, to make it easy for a Data Scientist to use without having to
    learn an entirely different toolset.

-   Traceability: when automating steps in the model development
    process, you'll most likely produce a lot more model versions.
    Traceability will become more important than ever, letting you know
    which inputs led to which model output, and allowing you to decide
    (or automate) which model version should be deployed to production.
    Automation tools should offer seamless integration with other tools
    used in your development process so that traceability is guaranteed.

When selecting automation tools for automating your ML pipeline, you
should carefully consider the above-mentioned three factors. When
targeting the Azure platform, the Azure Machine Learning Service[^1] is
the obvious choice. In our next magazine, we'll show you how to create
an ML pipeline using this service.

For bridging the gap between the Data Scientist and the app developer we
need a DevOps pipeline. This will be very similar to what we are used to
from a .NET development world, except for the fact that it will gain one
extra responsibility: integrating the correct ML model version into the
application. For this, the DevOps pipeline will need to interface with
the model store. The model store (part of the Model Management Service
in Figure 4) contains all model versions along with the metadata
describing (amongst others) model performance. With defined criteria
(e.g. "model with greatest accuracy") the DevOps pipeline can select the
correct model from the model store and integrate and deploy it.

## Feedback

Implementing the feedback loop for Data Science looks a lot like
implementing that loop for application development: you want to see how
the model performs in the real world, evaluate it with the prior
assumption and adjust it when necessary. Let's consider three examples:
operational information, new training data, and reinforcement learning.

### Operational information

Monitoring operational information answers questions like: how well are
the predictions you have made followed? Or, how many times is that
prediction correct? And thus, how many times is that prediction not
correct? You can observe this by logging the prediction made with all of
its contexts and linking this to the action that was taken based on the
prediction.

### New training data

You also need to send new data that is available in the system through
the model training. Trends can always change over time, especially if
you have a prediction that immediately influences decisions. If you
tried to predict the demand for a product on the Monday of a specific
week and based on that prediction your company delivers less of that
product, it can very well happen that you find that your prediction had
an impact on the number of sales for that product on that day. Of
course, this is a self-fulfilling prophecy, since you cannot sell what
you do not have.

To enable re-evaluation of your model you need to have a way to send in
more data through your model, with all the necessary data preparation
steps automatically executed. From there you can measure how much better
or worse the performance of the model is compared to the previous
dataset. It could very well be that you need to verify whether
previously made assumptions and decisions are still valid.

### Reinforcement training

With reinforcement training, you enable the end-users of the prediction
to give feedback about it. They can indicate whether your prediction was
correct or not, and how they determined this. By sending the new and
updated "label" (the value that you are trying to predict) on that same
information back into the build-measure-learn loop, you provide the
algorithms with more information so it can adjust if necessary.

# OK, so now what?

By now, you should have an idea of what it takes to incorporate Data
Science model development into your DevOps cycle. Data Science is
different from application development: it requires a specific skill
set, model development requires a different process, and Data Scientists
use different tools. However, the things that are important in DevOps
are just as applicable to Data Science as they are to application
development. We have shown the considerations that come into play here
as well as a general direction on how to solve them.

The next step is to figure out how to implement this. What tools and
techniques do we need to create a DevOps setup for an AI infused app? In
the next issue of XPRT magazine, we will show you exactly that, so stay
tuned!

## 

[^1]: https://azure.microsoft.com/en-us/services/machine-learning-service/
