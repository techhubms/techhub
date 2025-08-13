# The Human Computer: Rise of the Bots

# Introduction

Imagine walking into your home after a hard day of work and just asking
"OK Jarvis, can you please order me a pizza Pepperoni from Domino's for
dinner? Also, please tell my boss that I'm taking the day off tomorrow.
And now, let's watch the awesome Bruce Willis in Die Hard 4".

This article will show you how you can build your own intelligent
"Jarvis[^1]". Of course, you could buy something readily available like
an Amazon Echo or a Google Home, but where's the fun in that? Why buy
them, when you can build something yourself?

Seems impossible? It's not. You probably have an app on your phone to
order a pizza. Telling your boss you're taking the day off is a matter
of writing a quick e-mail. And Die Hard 4 is available on Netflix. All
these services and technologies are available today. It's just a matter
of how we interact with them.

# Concepts

## Bots

In today's world, there is an ever increasing amount of technology to
serve us. Smarter devices, linked to more and more services which are
deeply integrated into our lives. What would you do without your
smartphone and the services it unlocks for you? And what about your
smart TV, your smart fridge and your smart home?

Electronic services have become an integral part of our lives, and we
need new ways of interacting with all this technology. Interaction must
become more similar to how we interact with other humans. Technologies
like Artificial Intelligence enable this, because machines can now
understand what we mean.

Bots will become a vital part of this new way of interaction. You
communicate with a bot in the same way you would communicate with
another human being. For example through e-mail, chat, text messages or
speech.

In this article we will show you how you can create a simple bot that
understands human language. We will enable the bot to control the lights
in our home. Our bot is created by using the Azure Bot Service and the
Bot Framework.

## The Azure Bot Service

We're using the Azure Bot Service to build our bot. This provides an
easy way to connect your bot to different "channels". A "channel" is a
way for a user to interact with our bot. There are quite a few channels
available right out of the box, such as Skype, Facebook, Teams, Slack,
SMS, Cortana, and others. By leveraging the bot service, the actual
implementation of our bot is basically reduced to implementation of a
web API, and the bot service takes care of the rest.

## The Microsoft Bot Framework 

A conversation between the bot and a user is structured around
"Dialogs". For example, you could have a conversation to order a pizza,
send an e-mail message, start a movie on Netflix, or switch on the
lights in a certain room. The Microsoft Bot Framework[^2] offers an easy
way to implement these dialogs, and includes an emulator to test your
bot locally.

## Language understanding

To make our bot understand human language we'll use Language
Understanding (LUIS)[^3]. LUIS allows us to define an "intent". An
intent is an action that a user can perform. These intents are linked to
methods in our web API implementation. LUIS will learn which intent the
user has by interpreting what the user says. This is called an
"utterance". For example, utterances like "I would like to switch off
the lights" and "Turn the lights off" will both refer to the intent of
"LightsOff".

At first, you will have to give LUIS some example utterances for each
intent that you define. As the usage of your bot increases, there will
be more utterances for each intent. Initially, you will have to tell
LUIS which intent is meant for each utterance. But, as LUIS learns, it
will get smarter and will automatically start to understand the correct
intent for new utterances that it encounters. You can get started with
LUIS by checking the walkthrough on the Microsoft site.[^4]

But what if your commands may apply to different contexts? For example,
what if you only want to turn off the lights in the living room, or in
the kitchen? For this, you can use the concept of "entities". Entities
allow commands to be reused in multiple contexts. In this case, an
utterance could be "Please turn off the lights in the kitchen". When
first encountered, you would tell LUIS that the intent for this
utterance is "LightsOff", and that it contains the entity named "room",
with value "kitchen". You can even have multiple entities within a
single utterance.

  -------------------------------------------------------------------------
  Utterance                        Intent      Entity name Entity value
  -------------------------------- ----------- ----------- ----------------
  "Please turn off the lights in   LightsOff   Room        Kitchen
  the kitchen"                                             

  "Please turn on the lights in    LightsOn    Room        Bathroom
  the bathroom"                                            

  "Please turn off the lights"     LightsOff   \-          \-
  -------------------------------------------------------------------------

# ![](./media/image1.png)

Figure : LUIS powered bot

# Implementation

Looking at the internals of a bot application as shown in Figure 1, you
will recognize elements from a real-life conversation. A dialog between
a bot and a human happens within a context. Individual messages are
called activities. The bot talks to LUIS for language understanding. The
input from the human is passed to LUIS and the discovered intents and
entities are returned.

### Dialog

Now it's time to get your hands dirty and write some code. In this
chapter we will create a bot that is able to process LUIS intents. The
bot will understand commands like "Turn the lights on in the kitchen".
To get started, first create a new ASP.NET MVC web project, targeting
.NET framework 4.6.1 or higher. Next, add the following Nuget packages:
'Microsoft.Bot.Builder' and 'Microsoft.Bot.Connector'.

To add the bot behavior, add a class named "RootDialog" that inherits
from LuisDialog\<T\>. Mapping LUIS intents to methods on this dialog is
done by using a LuisIntentAttribute.

In Figure 2 you can see code that maps a LUIS intent named "LightsOn" to
a dialog method named 'LightsOn'. The method uses three parameters:

-   The context is used to describe the context of a conversation
    between the bot and the end-user. We use this to send a message back
    to the end-user to request more information, or to send back
    confirmation that the command was properly handled by the bot.

-   An activity represents a message in a conversation; it will not be
    used in our code.

-   Parameter result describes what LUIS learned from the user input.
    For instance, any entities that were detected in the user input. In
    our code, we will check for an entity that describes the room in
    which to turn on the lights.

Now that you understand what the parameters are for, let's add some
behavior to the method. This is shown in Figure 3.

We start off by checking whether LUIS was able to detect a room entity.
If so, it will be used as the room in which to turn on the lights. We
notify the end-user, and wait for the next interaction. If no room
entity was detected, we will ask for the location to be selected from a
list of all known rooms. We specify method 'LightsOnWhere' to handle the
users' answer. When this method is called, we know that the end-user
intends to have the lights turned on, and that he has just specified the
room. The implementation is displayed in Figure 4. Note that this method
does not have an attribute, as this is not the start of a conversation,
but rather the end.

The implementation of 'ToggleLightState' depends on the type of Home
Automation software you are running. So, for now, we will leave this
empty.

Next, add the constructor to the dialog. The constructor takes arguments
that are used to authenticate the bot to the LUIS service. In a
production situation, you would put these credentials in a secure store,
like in an Azure Key Vault. However, for this walkthrough we will keep
them in a configuration file.

The modelID value can be found on the LUIS portal, on the 'Settings'
page as **Application ID**. The value for subscriptionKey is visible on
the 'Publish' page under the label Key String. The sample code for the
Dialog constructor is in Figure 5.

### Controller

Our bot dialog is now complete. We will now create code that hosts the
dialog inside a Web Api to enable interaction with the bot. To do this,
we add a class named 'MessagesController' that inherits from
'ApiController'. The controller needs to respond to input, sent by
users. In a Web Api, this is usually done by exposing an Action that
handles POST requests. The implementation is shown in Figure 6. If the
incoming activity is a message, it is passed to the dialog we created
earlier. The bot framework will ensure that the proper dialog method is
called, based on the information that LUIS detects in the posted
message.

Your bot can now be tested by running it inside the Bot Framework
Emulator as shown in Figure 7. Run your web site locally and note the
URL at which it is running. By default, this will be
<http://localhost:2584/api/messages>. Incoming calls will be directed to
the controller we have just created. Open the emulator, and enter the
URL and press 'Connect' to connect the emulator to your local bot. The
emulator can translate user input to "Activity" data and send it to the
bot. It can also interpret responses by the bot and present those to the
user.

Test your bot by typing "Turn the lights on in the kitchen". If LUIS was
able to interpret, the dialog method marked with the corresponding LUIS
intent "LightsOn" will be invoked.

# ![](./media/image2.png)Conclusion & next steps

We've shown you how to use the Microsoft Bot Service and the Bot
Framework to create a simple bot that understands natural language and
can switch the lights in our home.

So, what's next? In the next issue of XPRT we will make our bot more
human. By adding speaker recognition your bot will recognize you when
you talk to it. And we will go one step further, and show you how to
make your bot sensitive to human moods. A true Jarvis experience!

[^1]: http://marvel-movies.wikia.com/wiki/J.A.R.V.I.S.

[^2]: https://docs.microsoft.com/en-us/bot-framework/dotnet/bot-builder-dotnet-overview

[^3]: https://www.luis.ai/home

[^4]: https://docs.microsoft.com/en-us/azure/cognitive-services/luis/luis-csharp-tutorial-build-bot-framework-sample
