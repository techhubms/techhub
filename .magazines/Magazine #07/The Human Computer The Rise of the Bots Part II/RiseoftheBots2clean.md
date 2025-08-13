# Making your Bot more human

We hope you enjoyed Part 1 of this article, in which we discussed how to
create a Bot that can understand and process natural language. Now,
imagine using your Bot to order your family\'s favorite pizzas. But
instead of you having to type everything, your Bot would listen to each
of your family members and recognize them by their voice. It would then
look up what their favorite pizza is and place the order. Sounds
difficult? It isn\'t! You can build this using Azure Cognitive Services.
In this article we\'ll show you how to get started.

 

# Azure Speaker Recognition API

 

## Speaker verification

The Azure Speaker Recognition API is part of Azure\'s Cognitive
Services. It is a cloud-based platform that helps you identify people by
their voices. You can use it to authenticate users, instead of using
usernames and passwords. Also, you can use it as a factor inside a
multi-factor authentication (MFA) process. This feature is called
\'speaker verification\'.

## Speaker identification

The platform can also be used to identify the 'current speaker' during a
conversation with multiple speakers, a feature that is useful if you
need to display information about speakers during a conversation. For
example, during a television interview, you could use this API to
display differently colored subtitles based on who is talking. This
feature is called \'speaker identification\', which we'll discuss in
this article.

## Profiles, enrollment and recognition

The API works by creating speaker profiles and enrolling them. This
means training the recognizer by uploading audio fragments containing a
speaker\'s voice. Once enrolled, the speaker can be identified in a
different audio fragment. The identification process uses an
asynchronous model; you upload an audio stream and start periodically
querying for identification results. This way multiple speakers can be
identified during a conversation. 

# ![Home \> SpeakerRecognition - Keys SpeakerRecognition - Keys Cognitive Services p Search (Ctrl 4/) Overview Activity log Access control (l AM) Tags Diagnose and solve problems RESOURCE MANAGEMENT Keys Regenerate Keyl Regenerate Key2 Notice: It may take up to 10 minutes for the newly (re)generated keys to take effect. NAME SpeakerRecognition KEY 1 4e4bf6de01b2451b2451b2451b21b245 KEY 2 4e4bf6de01 b2451 b2451 b2451 b21 b245 ](./media/image1.png)Getting started

To help you get started building a Bot you'll need to prepare your
development environment first. After installing the required tools,
we'll show you how to set up your Bot project. Next, we'll introduce you
to the various Cognitive Services and how to interact with them from the
Bot.

First, you will need to get access to the Speaker Recognition API. Go to
[[https://azure.microsoft.com/en-us/services/cognitive-services/speaker-recognition/]{.underline}](https://azure.microsoft.com/en-us/services/cognitive-services/speaker-recognition/)
and sign up. You can opt for a free seven-day trial period or use your
Azure Subscription. Once the resource is deployed, navigate to the
resource on the Azure Portal to grab one of your account keys, as
displayed in Figure 1. You\'ll need this key to access the APIs later.

 

 

## Bot Emulator

Also, please install the Bot Emulator v4, which can be found here:
[[https://github.com/Microsoft/BotFramework-Emulator/releases]{.underline}](https://github.com/Microsoft/BotFramework-Emulator/releases)

![Bot Framework Emulator (V4 PREVIEW) - HaBot File Edit View Help BOT
EXPLORER ENDPOINT O HaBot TRANSCRIPT EXPLORER Welcome Start Over D Live
Chat (HaBot) X Save Transcript As.. What do you want to do? Bot at AM
Manage Profiles Recognize Speaker Type your message\... INSPECTOR hello
user Emulator listening on http://localhost : 31635 \[e8: 34: 28\] ngrok
listening on https://es1226c4.ngrok.io \[e8: 34: 28\] ngrok traffic
inspector : http://127.ø.e.1:aeae
](./media/image2.png)
The Bot Emulator can be used to test your
Bot during development. It will allow you to connect to a Bot and
interact with it. The new \'V4 preview\' version of the emulator (Figure
2) looks a little nicer and will eventually offer more functionality
than the emulator we discussed in the previous article.

## Visual Studio 2017 

 

The code used in this article was created by using the Microsoft Bot
project template, which you can find here:
[[https://marketplace.visualstudio.com/items?itemName=BotBuilder.botbuilderv4]{.underline}](https://marketplace.visualstudio.com/items?itemName=BotBuilder.botbuilderv4)

The SDK template allows you to get started quickly by scaffolding a
Visual Studio solution for you. After installing it, restart Visual
Studio, and create a new project of the type \'Bot Builder Echo Bot\'.
You can use this project template as a quick way to get started. There
is a great GitHub repository with loads of useful sample projects,
explaining many Bot features. You can find it here:
[[https://github.com/Microsoft/botbuilder-dotnet/tree/master/samples-final]{.underline}](https://github.com/Microsoft/botbuilder-dotnet/tree/master/samples-final)

 

At this time, both the SDK and the emulator are in
[preview]{.underline}, which means you should not deploy this version
into a production environment yet.

 

To access the Speaker Recognition API in a simple manner, add a NuGet
package named \"Microsoft.ProjectOxford.SpeakerRecognition\". The class
\'SpeakerIdentificationServiceClient\' from this package helps you
interact with the Speaker Recognition speaker identification APIs in
Azure. Note that there is a different class named
\'SpeakerVerificationServiceClient\' that can be used for speaker
verification. It works in a very similar way.

Some of the code that uses the identification client was inspired by the
sample code from Microsoft in this GitHub repository:
<https://github.com/Microsoft/Cognitive-SpeakerRecognition-Windows>

## 

## Code

Now it's time to write some code.

 

The dialogs we need to make the Bot interact with users are contained in
a DialogSet.

Our Bot has the following elements:

1.  **Main menu dialog**

> This is the entry point of the conversation. The Bot will ask the user
> to choose between 'managing profiles' and 'recognizing speakers'.

2.  **Profile management dialog**

> This is the set of dialogs used to view, create, enrol or delete
> speaker profiles. These options will be displayed in the same way as
> the Main dialog, using buttons.

3.  **Speaker recognition dialog**

> In this dialog the user can upload an audio fragment to be analyzed
> for speaker identification by the speaker identification API.

 

### Main menu

 

The main menu uses the code displayed in Figure 3. The menu options are
passed as a list of string.

 

     

A dialog with a Bot usually involves multiple steps: display
information, ask for input, and process input. This is modeled by adding
an array of WaterfallStep\[\].Every delegate in this array represents a
round-trip between the Bot and the user, e.g. requesting user input and
processing the response.

The first step of the dialog uses the ChoicePrompt to display two
buttons in the Bot Emulator. The user must choose one of the options.
The call to dc.Prompt is used to display the information and buttons,
and prompt for input. The second step in the dialog will be invoked
after the user clicks one of the buttons. The user input is retrieved,
and a new dialog is started. This is done by calling dc.Replace and
passing in the new dialog\'s identifier.

The profile management dialog in our Bot has a very similar set-up.

 

### Speaker Profiles

To recognize speakers, you must first create a speaker profile. After
creating the profile, you can enrol the profile by uploading some audio
fragments that contain the speaker's voice. The speaker recognition
service will learn to recognize the speaker based on the audio
fragments. In our Azure environment, we have created two speaker
profiles. We have uploaded two audio fragments for each profile to enrol
them, so we can test speaker recognition later. You can find these audio
fragments in the 'samples' folder. **Note that in your environment, the
speaker profile identifiers will be different.**

The speaker profile management dialog will use the
SpeakerIdentificationServiceClient to call the speaker identification
APIs. Once this dialog is in place, we can start using the enrolled
profiles for speaker identification.

### 

### Speaker recognition

The speaker recognition dialog will request the user to upload an
attachment. The simplified code is displayed in Figure 4. The user is
requested to upload an audio file as an attachment. You do this by using
the type AttachmentPrompt.

To identify speakers, we can use the Bot emulator. You can send an
attachment containing a .wav file. The Bot will forward the audio file
to the speaker identification API for analysis. During analysis,
whenever a speaker is recognized, the Bot will notify the user. You can
see an example of this in Figure 5.

 

![Bot Framework Emulator (V4 PREVIEW) - HaBot File Edit View Help BOT
EXPLORER ENDPOINT O HaBot TRANSCRIPT EXPLORER \[3 Welcome Start Over
Live Chat (HaBot) X Save Transcript As\... Analyzing your voice\...
Recognized you, confidence \'High\'. Recognized you, confidence
\'High\'. Recognized you, confidence \'High\'. Recognized other profile
\'4ac7dda3-56a5-45cc-8bda Recognized other profile
\'4ac7dda3-56a5-45cc-8bda -1183899bf4bf\' -1183899bf4bf\' INSPECTOR menu
iHigh\' •High\'. Recognized other profile \'4ac7dda3 \'High\'. type your
message\... -56a5-45cc-8bda-1183899bf4bf\' , confidence , confidence ,
confidence - \> message Main POST 2ØØ conversation: message What do you
want to do? POST 2ØØ directline.p, : se:13\] \> message Recognize
Speaker POST 2øø conversatiom message Please upload a .wav file POST 2ØØ
directline.p, \> message audio/wav POST 2øø conversation: message
Analyzing your ](./media/image3.png)


Figure 5: Speaker recognition

 

After profile enrolment, your Bot can recognize you and your family
members by their voice. Now let's bring it up one level and see how your
Bot can detect what you are saying, to make an even better determination
about what your favorite pizza is, based on your mood!

# Making your Bot understand what's being said

Now that your bot recognizes your voice, the next step would be to let
it understand what is being said. When combined with the speaker
recognition, we can tell our bot to "Order my favorite pizza". Your bot
will know who is speaking and what's being said! To convert speech to
text (STT) we will use the Bing Speech recognition API.

![](./media/image5.jpeg)

# Bing Speech recognition

The Bing speech recognition service allows us to convert audio to text.
It is a cloud-based service that extracts text from an audio file. It
works well, but there are few configuration options. If you would like
more advanced features, such as custom speaker style recognition, then
you should have a look at the "Custom Speech API" which is part of
Cognitive Services. Note that this service is currently in preview. For
our purposes, the Bing Speech recognition works fine though.

## Rest API and streaming audio

The speech recognition API offers a choice between using a REST API and
streaming the audio in real-time by using web sockets. Using the REST
API enables us to send our .wav file to the service and get the result
back in text format.

## Recognition modes

When using the API, we have different recognition modes that we can use.
Each of them serves a different use-case.

1.  Interactive mode

> This is a short request to a computer. The user expects the computer
> to do something as a result of what's being said.

2.  Conversation mode

> This mode allows you to recognize two or more people that are in
> conversation. It can translate all that's being said, and can also
> recognize slang and informal speech.

3.  Dictation mode

> A human citing to a computer. Typically, long utterances, like taking
> notes.

In our case we would like to order pizza. We know that we are talking to
a bot, so our utterance will probably be something like "Hey Jarvis, can
you order a pizza Tonno for me?". This is a perfect match with
'interactive mode', so we will go with that.

## An educated guess

When the Speech API is not entirely sure what's being said on the audio
that we are sending, it will make an educated guess and returns a list
of "N-Best" values in different formats. When you think about this, it
is not very different then what humans do. We also often make
assumptions when we do not fully understand something. The most likely
meaning will have the highest confidence level. You can see an example
in Figure 7.

## Code

Before we start, make sure you register a new Bing Speech service on
Azure, install the Microsoft Bing speech Nuget package
(<https://www.nuget.org/packages/Microsoft.Bing.Speech>) and get the key
as you did earlier in this article. The steps that we now need to take
are as follows:

1.  Create a new STT dialog mode

2.  Upload a .wav file

3.  Send the file to the API and return the text

Now that we have explained the basic concepts and everything is in
place, let's start coding! First let's add a new dialog to our bot.

### Speech to text dialog

We can re-use the earlier steps and adjust it a bit since we don't need
to know now who is speaking. We just want to know what the text is, and
we want the selected .wav file to analyze (Figure 8).

### Sending the request

Now that we have the file we can analyze it and upload it to the Bing
Speech API. We will use the 'SpeechClient' for this. The SpeechClient
can optimize the result for the collector of the audio by using
metadata. The code in Figure 9 shows you how to do this.

### Getting the response

Once the request is completed we will get a response in the method that
we defined when setting up the response. Remember that we get an N-Best
result list, sorted on confidence. For our bot it is enough to get the
first result from the list and get the pretty "DisplayText", as you can
see in 10.

# Adding sentiment analysis

Our bot is getting smarter and smarter! We now have a bot that
recognizes your voice and we can convert the audio fragments to text.
Obviously, we are not there yet. We don't understand the emotions of
speech yet. Based on your mood, your choice of pizza may vary!

To add this, you can use yet another one of Azure's Cognitive services:
the Text Analytics service. You can find out how it works here:
<https://azure.microsoft.com/en-us/services/cognitive-services/text-analytics>.
This service can be used for a multitude of things, including sentiment
analysis. The API takes text as input and returns either a score between
zero and one as a result. Scores close to one mean positive sentiments
(like happiness), scores close to zero indicate negative sentiments
(like anger). Calling this API is easiest when using the Nuget package
found here:
<https://www.nuget.org/packages/Microsoft.Azure.CognitiveServices.Language.TextAnalytics/2.1.0-preview>.
Note that at the time of writing, this package is in preview. You can
use the method 'SentimentAsync' on class 'TextAnalyticsClient' to upload
text for analysis.

# Final thoughts

We hope this article inspired you to build your own bot and that it
showed the power of the Azure services that we've used. The source code
of the bot is available on GitHub: <https://github.com/XpiritBV/HaBot>
so you can take a peek and build on what we have built. Feel free to
submit a PR if you found an issue, or if you created functionality that
extends the existing bot!
