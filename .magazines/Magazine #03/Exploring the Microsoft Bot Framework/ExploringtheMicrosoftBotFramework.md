# Exploring the Microsoft Bot Framework

At the Microsoft Build Conference in 2016, Microsoft announced something
they call "Conversations as a platform". This stated human interaction
and machine learning as the next computing interface.

![](./media/image1.jpeg)
The goal is to turn a conversation into a more
powerful tool that is contextually rich while improving productivity. As
Satya Nadella said, the human language is the new user interface.
Microsoft is investing substantially in this area with Cortana,
Cognitive services and Bot framework. In the end Microsoft sees a
convergence from messaging with people to messaging with services.

The convergence towards conversations starts with the Cortana
Intelligence Suite, which consists of 3 main parts.

-   Bot Framework. We will explore this framework in more detail in this
    article.

-   Cognitive Services. For example, Language Understanding Intelligent
    Service (LUIS)

-   Machine Learning. Which is the basis for the language understanding
    of LUIS.

## Bot Framework

One of the main building blocks for this next computing interface are
bots. Microsoft announced the bot framework to help developers build
rich bots with easy integration to many channels and other services. The
bot framework consists of three main parts, as seen in the following
diagram.

![](./media/image2.png)

### Bot builder SDK

The Bot Builder SDK is an open source SDK that provides all the building
blocks you need to develop your Bot using .NET, NodeJS. It also consists
of a chat bot emulator and Visual Studio templates. The emulator makes
it easy to test your bot while it is being developed, but in the end the
bot is just an API. You could also use postman or any other tool to test
your bot.

### Developer Portal

![](./media/image3.png)
The developer portal is the place where you
can register your Bot and connect it to a wide range of communication
channels. Here you get a step-by-step guide on how to add more channels
to your bot. The Skype and Web Chat channels are added by default when
you first register your bot in the developer portal.

### 

### 

### 

### 

### 

### Bot directory

When you are done building your Bot you can publish your Bot to the Bot
Directory, which contains different Bots which are already published and
are ready for use. When your Bot is submitted to the Bot directory it
usually takes a few days to get through the validation process. After
that other users can add your bot to their channels.

## Building a Bot

When you first create a new Bot in Visual Studio, the project should
look very familiar if you previously developed a Web API. There is a
Controllers folder and a MessagesController which handles the requests
coming into the bot. The *Post* method gets an Activity class which
describes the message/event the bot receives. The *HandleSystemMessage*
method allows you to handle different Bot events such as users entering
the conversations or leaving.

### Dialog

A dialog is a class with state and methods that use Bot Builder-defined
types to manage interactions. The Bot Framework manages conversations by
sending JSON messages through the Bot Connector and to/from the user and
bot. These messages hold the state of an ongoing conversation. To
facilitate managing state with the Bot Framework, your dialog type must
be serializable. To support this, the code is decorated with the
\[Serializable\] attribute. Additionally, the class contains fields,
representing the state of the conversation. The Bot framework dialog
manages all the state for you if you follow this simple guidance.

When implementing a dialog, you simply chain interactions and guide the
user through the conversation one step at a time. You have to implement
the Start method for your dialog and from there you can chain methods to
guide the user to the end of the dialog. A code snippet from a dialog is
seen below.

![](./media/image4.png)

### FormFlow

FormFlow is a Bot Builder SDK library that lets you declare the type of
information you need and then it does the bulk of the work of managing
the conversation and moving the user from question to question
automatically. In Dialogs, you have the responsibility of writing the
methods to prompt the user and collect the results, but FormFlow
simplifies the entire process. Another benefit of FormFlow is that it
automatically formats questions to be more readable and it tries to
process partial answers if it can.

When rebuilding the Xpirit FeedbackBot to use FormFlow instead of
Dialogs the whole implementation is an entity with the fields you wish
to gather from the user. So instead of specifying the flow yourself, the
Bot Framework handles that for you.

![](./media/image5.png)

So while you have to build the dialog yourself using Dialogs, the
FormFlow does that for you and you only have to focus on the information
that you want to gather. There are a lot of additional options available
to customize how the FormFlow method renders things, as you can see in
the above code sample with the \[Prompt\] attribute.

## Conclusion

Microsoft has made it very easy to get started with building and
exploring the bot world. However, there are also a lot of things you can
add to your bot when you start combining the Bot framework and LUIS or
other cognitive services. Microsoft has even made it very easy to use
LUIS (Language Understanding Intelligent Service) from the Bot Framework
by providing base classes and attributes to integrate LUIS into your
Bot. As seen in the code sample it is very easy to include LUIS into
your Bot dialog.

![](./media/image6.png)

All the sources for the bot build in this article are available on
Github. <https://github.com/cvs79/XpiritRatingBot>
