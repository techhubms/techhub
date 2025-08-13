# How to teach programming to blind children

By Reinier van Maanen & Marc Duiker

## Introduction

In the Netherlands, there are approximately 3300 children who are
visually impaired or blind. Within this group, and especially children
between the ages of 7 and 10, there is a lack of appropriate material or
methods to teach them programming. Block-based programming, often used
by children without visual disabilities of this age group, is not
accessible for visually impaired children. Common text-based languages,
such as Python or Javascript, are still too difficult for this age
group.

We felt that these children should also be able to learn to code and not
be treated any differently than the rest. To fix this, we are
collaborating with researchers from LIACS of Leiden University[^1]
within their "Inclusive programming education" project. In this project,
the researchers are looking at what materials children of elementary
school-age can use to learn to program in the classroom.

This article focuses on the process we went through, where we are now
and what our future plans are for this project.

## How we got the idea

More than a year ago we were both in Oslo having some drinks together.
Reinier was there for the NDC conference, and Marc was working on-site
at a client. We discussed our work and that we would both like to
promote social responsibility efforts within Xpirit. We just didn't know
exactly which form it would take. Reinier attended the talk "How to
teach programming and other things?" by Felienne[^2] at NDC, and that
provided us with some inspiration.

## Goal

Our goal is to create a fun and educative experience for kids with and
without visual impairment. The inclusivity is important because visually
impaired children are often mixed in with non-visually impaired
children. According to the researchers from LIACS, both teachers and
children are asking for inclusive teaching materials.

## How we started

At Xpirit, we have a couple of innovation days each year. We can use
these days for anything we like as long as we share what we have learned
so that the entire team can benefit from it. Some colleagues investigate
the latest version of a development framework while others contribute to
an open-source project of their liking. In the morning, we start with a
'stand-up' format to announce the topics and create teams. The teams
spend nearly a full day on the topics and later in the afternoon they
give short demos on what they achieved & learned (and how many yaks have
been shaved[^3]).

We started the project with a brainstorm session on how we could achieve
our goal. Here are the mind maps from the brainstorm:

![Figure 2: Defining our
goal](./media/image1.png)


Figure 1: Defining our goal

![Figure 3: Exploring game
ideas](./media/image2.png)


Figure 2: Exploring game ideas

After two brainstorm sessions, we started with the idea of making a
physical board game with playing cards, containing both text and
braille, which are used to build a programming sequence. Although it was
fun to think about programming concepts in the form of playing cards and
laying them out in a structure to form a small program, it has some
significant drawbacks. The first one is that it's limiting the kind of
programs you can 'write' with these cards. Secondly, and this was the
biggest problem, there was no right way of quickly determining whether
the children had placed the cards in the correct sequence. The correct
answers could be presented in a separate document (again with braille),
but we found that the feedback cycle from putting down cards and
verifying the output was not convenient.

Parallel to prototyping the physical game, we also tried to create a
quick mockup in Unity to simulate the game. However, since we didn't
have much experience with this framework, it took us too long to do
quick simulations of the game so we abandoned the idea.

We realized that, in order to have decent progress, we should stick to
the tools and techniques we already know, or that are close to our
abilities. So we decided to create a digital game instead of a physical
one.

## Web-based text adventure

When we started to think about a computer game we arrived very quickly
at a web-based game, a web-based text adventure to be more specific. Our
reasons for liking this solution so much are:

1.  The web and web browsers have good support for screen readers used
    by visually impaired people to have the text read aloud. This means
    that the format of the game has to be text-based.
2.  A web-based game can be played on any device, so schools do not have
    to invest in special or extra hardware. Lots of schools use laptops
    or Chromebooks these days and our game runs perfectly on these.
3.  We can easily add more content and add additional features to a
    solution hosted in the cloud.

The advantage of making a digital game over a physical one is that we
now have better control over the gameplay. We can guide the children
through the game, provide help when needed and verify their input.

Another great advantage of making a digital game is that we can separate
the content from the gameplay. So we're making a game engine with a
generic web interface (it's all text-based) which can run different
adventure stories.

### The Story

An adventure game needs a story. We created a very small one just to
prove the game engine works. Our intention is that new stories can be
added by non-technical people. The story is a very basic escape room
situation in which the user needs to find a key to unlock the door. The
user needs to type in commands such as: *Open the cupboard* and *pick up
the key*.

## How we made it

The solution, named Louise after one of the technologies used, consists
of a back-end with the chatbot and a front-end for exposing the chatbot
over the web. Both the chatbot and the front-end are deployed to Azure.
Besides these three things, we're also going to talk about LUIS, as that
is an important part of the solution.

### Back-end project

This project makes use of three major technologies:

-   Microsoft Bot Framework[^4]
-   Microsoft Language Understanding Intelligent Service (LUIS)[^5]
-   Bing SpellChecker[^6].

The Microsoft Bot Framework makes creating chatbots easy. It's a pretty
standard ASP.NET Core application bundled with a bunch of NuGet packages
like 'Microsoft.Bot.Builder'. This package contains the 'IBot'
interface, which you implement by creating the 'OnTurnAsync' method:

    public async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken)
    {
        ...

        if (turnContext.Activity.Type == ActivityTypes.Message)
        {
            var dialogContext = await _dialogSet.CreateContextAsync(turnContext, cancellationToken);

            if (dialogContext.ActiveDialog != null)
            {
                await dialogContext.ContinueDialogAsync(cancellationToken);
            }
            else
            {
                await dialogContext.BeginDialogAsync("StoryPrompt", "1", cancellationToken);
                var replyMessage = _story.ToMessageForFirstScene();
                await turnContext.SendActivityAsync(replyMessage, cancellationToken);
            }
        }

        ...
    }

This method is invoked with every incoming activity with the bot. The
ITurnContext interface passed in provides access to information about
the current activity and the text provided by the user. A few of the
possible activities are 'Message', 'Typing' or 'EndOfConversation'. The
interface also allows you to respond with another activity, for
instance, a reply message.

In this particular instance, when a new message comes in, we use the
'DialogSet' which we initialized in the constructor. A DialogSet is a
collection of dialogs implemented in this bot. A dialog is a structure
which the framework uses to guide the person interacting with the bot to
the goal. We make use of a 'WaterfallDialog' and a 'TextPrompt'. The
text prompt is a simple prompt for text to the user with built-in
validation. Other prompts are number prompt, choice prompt and many
others. The text prompt isn't used directly but is referenced by the
waterfall dialog. A waterfall dialog is just a sequence of steps, in our
case a practically unlimited amount of the text prompt mentioned
earlier. This is the basis for our game engine.

![Figure 4: Waterfall
Dialog](./media/image3.png)


Figure 3: Waterfall Dialog

When a new message comes in, we check whether we already have an active
dialog, and if not, we begin the dialog 'StoryPrompt', which is the id
of our 'WaterfallDialog'. If we begin a new dialog, we also immediately
send a text message (with or without an audio attachment) for the first
scene of the story.

![Figure 5: First scene in the
story](./media/image4.png)


Figure 4: First scene in the story

At this point, we're in the first step of the waterfall dialog and the
user is in the first text prompt. For any messages coming in afterward,
we detect that there is already an active dialog and continue with that
and process the answer given in the text prompt. At that point the
custom validation step of the text prompt kicks in. A couple of things
happen there: The Bing spellchecker corrects any mistakes in the input,
and the corrected input is passed into LUIS, which is Microsoft's
Language Understanding Intelligent Service. With LUIS we try and figure
out what the intent of the user is. What is he/she trying to do? More on
LUIS and some key concepts later.

Creating a recognizer (another concept of the Bot framework) for LUIS,
with Bing spellchecking, is easy to do:

    private IRecognizer CreateLuisRecognizer()
    {
        var luisAppId = Configuration.GetSection("LuisAppId").Value;
        var luisEndpointKey = Configuration.GetSection("LuisEndpointKey").Value;
        var luisEndpoint = Configuration.GetSection("LuisEndpoint").Value;

        var app = new LuisApplication(luisAppId, luisEndpointKey, luisEndpoint);
        var options = new LuisPredictionOptions()
        {
            BingSpellCheckSubscriptionKey = Configuration.GetSection("LuisBingSpellCheckSubscriptionKey").Value,
            SpellCheck = true,
            Log = true
        };

        return new LuisRecognizer(app, options, true);
    }

This dependency is injected into our bot and we just call the method
'RecognizeAsync', passing in the turn context which provides access to
the answer given by the user. The recognizer result, containing the
intent of the user and how sure LUIS was about that being the correct
intent, is then passed into a new class we wrote, the IntentHandler.
That class contains most of our "business logic", dealing with all the
possible scenarios. Because we felt it was quite difficult to properly
unit test the bot framework code, we decided to keep the framework code
separate from our code as much as possible, which is probably good
practice in any case.

The final code of our custom validation looks like this:

    public async Task<bool> CustomPromptValidatorAsync(PromptValidatorContext<string> promptContext, CancellationToken cancellationToken)
    {
        var turnContext = promptContext.Context;
        var recognizerResult = await _luisRecognizer.RecognizeAsync(turnContext, cancellationToken);
        var intentHandler = new IntentHandler(_accessors, _story, turnContext, recognizerResult);
        var result = await intentHandler.Handle(recognizerResult.GetTopScoringIntent(), cancellationToken);

        return result;
    }

If the intent of the user matches with a possible intent as defined in
the story, the story continues with a new scene and a corresponding new
message is sent to the user. If the intent wasn't clear, or LUIS wasn't
sure enough of the intent, we also sent a new message, asking the user
to be more specific.

The last major part of implementing a chatbot is registering the
implementation in the Startup.cs:

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddBot(sp => new LouiseBot(), options =>
        {
            if (Env.IsProduction())
            {
                var appId = Configuration.GetSection("MicrosoftAppId").Value;
                var appPassword = Configuration.GetSection("MicrosoftAppPassword").Value;
                options.CredentialProvider = new SimpleCredentialProvider(appId, appPassword);
            }

            ILogger logger = LoggerFactory.CreateLogger<LouiseBot>();

            options.OnTurnError = async (context, exception) =>
            {
                logger.LogError($"Exception caught: {exception}");

                var message = "Sorry, it looks like something went wrong.";
                if(Env.IsDevelopment())
                {
                    message += " Exception: " + exception;
                }

                await context.SendActivityAsync(message);
            };

            IStorage dataStore = new MemoryStorage();
            var conversationState = new ConversationState(dataStore);
            options.State.Add(conversationState);
        });
    }

Next up is the stories project, which contains all the code we have to
support the various stories of the text-based adventure.

### Stories project

The content and flow of the game are captured in a story file in
markdown format. A story file consists of several scenes, with IDs,
descriptions, sounds and possible actions that map to LUIS intents and
entities.

Here's an example of a scene in a story file:

    ## [2.2, investigate the window]

    You walk towards the window. You feel thick metal bars are placed in front of it. Outside a bird flies past. No, you can't exit the room here.

    Choose what you're going to do now:
    [- investigate, Investigate,,, 2]
    [- investigate the door, InvestigateObject, object, Door, 2.1]
    [- investigate the cupboard, InvestigateObject, object, Cupboard, 2.3]

    Audio [crow.mp3]

Although markdown works well for us now because it is quick to edit, we
realize we need a more structured way to persist the story to make this
more scalable and manageable. We are going to move to a cloud-based data
store and an API to manage the content.

The bottom part of the story file contains instructions for LUIS in
LUDown format[^7]. We use a script to extract this part, and convert it
to JSON which can be imported in the LUIS management portal.

### Front-end project

The chatbot is exposed through a website, which is pretty easy to do. We
added a web project with a single Razor page, containing just the
following code which integrates with the Web Chat channel of our bot
(more on that in the Azure section).

    @page
    @model IndexModel
    @{
        ViewData["Title"] = "An adventure!";
    }

        <div class="text-center">
            <p>Hi, say something in the chat-window below to start an awesome adventure!</p>
            <iframe src='https://webchat.botframework.com/embed/MviLouiseBot?s=SECRET' style='min-width: 400px; width: 100%; min-height: 500px;'></iframe>
        </div>

The result is a simple chat, which works pretty well on all the devices
and browsers that we tried. Autoplaying of audio files doesn't work for
all browsers, but no major issues. One thing that is still on our todo
list is checking the compatibility of the chat with well-known screen
readers, which is a pretty major thing for blind kids, but our primary
focus was to first get a working prototype; UI can always easily be
changed if needed. This has been something we've been struggling with
during the entire process: we're not used to developing for the visually
impaired, constantly using words as "you see this or that", or wanting
to use colors, images and many more that's just not possible. It's been
a great learning experience!

### Azure

The Azure side of things isn't too complicated either: the bot itself is
deployed to an App Service. At that point, you could probably interact
with the deployed endpoint directly (we didn't try), but the easy way to
integrate your bot is by creating a "Bot Channels Registration". You
just specify the name of your bot, its messaging endpoint (URL of your
App Service + "/api/messages") and choose a pricing tier (free or
standard). You can link to Application Insights as well if you want some
analytics for your bot, which gives you a couple of nice diagrams
showing the number of users, user retention, amount of activities
separated by channel.

![Figure 6: Bot Channel
Analytics](./media/image5.png)


Figure 5: Bot Channel Analytics

These channels are your integration possibilities which you create
inside of the Bot Channels Registration and there are a lot of options:
Web Chat, Slack, Cortana, Teams, Telegram, Facebook, Email, Direct Line
(custom integrations) and more. As we are exposing the bot with a
website, we created a Web Chat channel. We also experimented with Slack,
which works fine as well but isn't well suited for our use case as it's
not easy enough to access.

Another thing you can do with the Bot Channel Registration is testing
your bot with the "Test in Web Chat" option, which gives you a nice
built-in chat in the Azure Portal to test whether things are working. Of
course you don't want to deploy every time and while we mentioned
earlier that unit testing was a bit difficult, there is another way to
test your bot: The Bot Framework Emulator[^8], a tool provided by
Microsoft to test and debug your bot locally.

### LUIS

So what are these intents and entities we've mentioned earlier? They are
concepts from LUIS, the Language Understanding Intelligent Service from
Microsoft. With LUIS you can add natural language understanding
capabilities to your application. It is used frequently when creating a
chatbot interface.

Intents are configurable goals you want your users to achieve. Usually,
there are many different ways to describe one intent and your end-users
will use different phrases to achieve the same goal. LUIS helps to
understand the user input and translate it to an intent you can use in
your application.

Let's take this example intent: *Examine the door*. Alternatives to this
include: *Look at the door*, *Investigate the door* and *Check out the
door*. They all have the same meaning.

![Figure 7: LUIS Intents in the
portal](./media/image6.png)


Figure 6: LUIS Intents in the portal

Entities are objects which play a role in achieving the goal. In the
above example, *door* is the entity. You can use custom Entities based
on a certain type (Simple, Hierarchy or List) or use a prebuilt one,
such as Age, Money or Temperature.

Once the intents and entities and have been imported from the story file
we train and test the LUIS model using the portal. We found we always
need to provide more examples for LUIS to understand the many variations
we humans use in describing our goals and actions. Once we're satisfied
with the level of understanding, we will publish the model as a service
so it can be used with the bot framework.

## Field testing, future work & sponsorship

At the start of the new school season, the researchers of LIACS will use
our solution in a real classroom so children with and without visual
impairment can test it.

In parallel, we will focus on features on our backlog such as:

-   Sharing a completed story with family and friends.
-   Multiplayer support so children can work together.
-   A portal for teachers to
    -   manage story content
    -   analyze user input to help improve the story
    -   monitor story progress for an entire classroom.

We've made great progress, but there's still lots to be done. We're
still looking for sponsors in order to keep working on this amazing
project outside the innovation days. Please contact us if you want to
support this project in any way!

[^1]: LIACS website: https://liacs.leidenuniv.nl/

[^2]: NDC talk by Felienne: https://www.youtube.com/watch?v=Ygk9CCRWOJs

[^3]: Yak Shaving: https://en.wiktionary.org/wiki/yak_shaving

[^4]: Microsoft Bot framework: https://dev.botframework.com/

[^5]: Microsoft LUIS: https://eu.luis.ai/home

[^6]: Bing SpellChecker:
    https://docs.microsoft.com/en-us/azure/cognitive-services/bing-spell-check/

[^7]: LUDown:
    https://github.com/microsoft/botbuilder-tools/tree/master/packages/ludown

[^8]: Microsoft Bot Framework Emulator:
    https://github.com/microsoft/BotFramework-Emulator
