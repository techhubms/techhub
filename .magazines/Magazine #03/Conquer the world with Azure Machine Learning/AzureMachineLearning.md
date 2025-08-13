# Conquer the world with Azure Machine Learning

*What a great idea! You've decided to convert your brick and mortar
store into an online supermarket. You'll create intelligent business
processes that will make shopping easy. They will allow you to run the
web shop efficiently! You'll make Amazon and Ali Baba jealous. Let\'s
investigate how using Azure Machine Learning can help you.*

 

## Creatures of habit

Most people regularly buy the same items. For example, imagine a
customer who regularly buys some cheese. It would be nice if the system
would advise him to put it in his shopping cart. But he probably won\'t
need cheese during each visit to the shop, only when he has run out. So
the system should be smart enough to figure out if he needs it. If you
repeat this process for all of his regular products, you can produce an
automatically generated grocery list. Which leads to one-click shopping!
Now how\'s that for a time saver?

### Out of cheese?

![](./media/image1.jpeg)
Azure Machine Learning (ML) provides a way
to answer this question. First you need to identify the type of problem
you need to solve. Your problem has two possible outcomes: either your
customer will buy cheese, or he won't. Because the possible outcomes are
limited to just two categories here, this problem describes what is
called a *Binary Classification*. Fortunately Azure ML provides some
great out-of-the-box classification algorithms to solve these kinds of
problems. You use a binary classification algorithm to make predictions
based on past data. For instance, the fact that a certain customer buys
cheese every week will stand out as a pattern. Azure ML can detect this
pattern and predict whether the customer will buy some cheese when he's
in your web shop.

 

## Been there, done that

So far we've determined that it\'s likely that your customer buys cheese
regularly. He\'ll probably get bored eating the same kind over and over
again. Wouldn't it be nice if your system could recommend some excellent
alternative brands?

### What else have you got?

Azure ML also has a way to answer this question. Again you start by
identifying the type of problem you need to solve. This problem has many
outcomes; there are many types of cheese your customer might like. The
goal is to select just one or two items he'll like best and recommend
those. Finding similar products in a catalog can be done using a
*Clustering* algorithm. You can get an answer to the question by looking
at possible ratings of previous purchases by that user, and also by
seeing what similar customers (same age, gender, etc.) prefer. The
Matchbox Recommender algorithm uses both approaches, making it an
excellent choice to use in your Azure ML model.

 

## Stuck in traffic

![](./media/image2.jpeg)
Let's pivot from the customers into
logistics.

You can only make a good profit if you don\'t waste money. Your company
should always deliver orders in the most efficient manner. Predicting
heavy traffic can help you do that. So when there is a great deal of
traffic on the highway, you'll want to take an alternative route. You
can make an informed decision based on traffic information combined with
intelligent processing.  

### What's the best route?

In order to compute the optimal delivery route, you'll need to determine
the delay if you take the highway. Again we first need to identify the
type of problem we need to solve. The delay itself can be any value
(minutes, hours), so classification won't help us here. Predicting such
a numeric requires a *Regression* algorithm. You can use your knowledge
about past deliveries, combined with real-time traffic data as inputs.
The delay can be compared to the extra time it will take to use
alternative routes.

## Sold out

![](./media/image3.jpeg)
Efficient planning of inventory will help
reduce waste. You don\'t want to keep too many perishable goods in
stock. Everything past its best-before date will become waste. You need
to know how many items of each product you\'re likely to sell tomorrow.
In order to be able to do this, you'll need to have information on what
you have sold in the past. Based on information from products that were
sold in the past, your system can predict what will be sold tomorrow.
The more information you put into the model, the more accurate the
predictions will become. For instance, if your data set contains the
date at which ice cream was sold, Azure ML will be able to determine
that more stock is needed in summer. It would be helpful if you were
notified about possible mistakes while ordering new stock. So when you
are about to order only ten boxes of ice cream instead of the required
100, the system should warn you about it.

### I can't let you do that Dave

Predicting the quantity of ice cream that will be sold also requires a
*Regression* algorithm. Using information from the registers as input,
your system will be able to make accurate predictions about future
sales. An algorithm that detects instances of you breaking a pattern is
an *Anomaly Detection* algorithm. Azure ML provides two of them. You can
use them to monitor planned orders and detect possible user errors.

## How to get started

If you want to try using Azure ML, you can go to the portal at
<https://studio.azureml.net> and sign up for a free trial. In the
portal, you start by creating a workspace. In that workspace you add
projects and experiments. For developers, this set-up is somewhat
similar to having a team project in which you add solutions and
projects. Once you've created an experiment, you can drag and drop
components into it and start connecting them in order to create a
Machine Learning model. An example of this is shown in Figure 1.

## Heads up

Adding some components to a model is easy; the difficult part is finding
out if your model can actually predict the correct things. Optimizing
your model starts with cleaning the input data set. Missing values can
lead to incorrect patterns. Providing too much information can lead to
poor ![](./media/image4.png)
performance. Fortunately, Azure ML provides a number
of tools to analyze the accuracy of your model. You can even compare the
performance of multiple algorithms. Interpreting the results does
require some knowledge about statistics. A Data Scientist can help you
clean your data sets and optimize your models.

## To sum up

We've discussed a number of concepts found in Machine Learning. All
examples were based on finding patterns in existing data, in order to
predict something about the future. You can't predict your future
without knowing your past. Machine Learning is no longer something only
large companies use, it has become a commodity. It is quite simple to
add some cleverness to your application using Azure ML. By using some
trial and error you can quickly create some working predictive models.
However, finetuning them still requires the help of an expert.

 
