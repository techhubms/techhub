## Introduction

One of the most frequently used buzzwords in IT is definitely DevOps.
Many organizations are adopting DevOps practices to deploy faster,
better and cheaper releases of functionality into production. As John
Willis and Damon Edwards described the core of the DevOps movement in
2010: DevOps is all about breaking down the silos between teams and to
have a culture of automation, measurement and sharing within the teams.
In this article we'll show you how the implementation of ChatOps
accelerates the DevOps movement within your organization.

## Why ChatOps?

You probably recognize a situation in which you have to do some work
that is normally done by a colleague and you don't have a clue how to
fix it. Wouldn't it be nice if you could see how he had solved that task
in the past? Or wouldn't it be great if he decided to automate the task
so that you are able to execute it instantly without knowing about the
inner details?

Another situation you probably recognize is a situation in which you are
asked to join a war room meeting with the management team because of a
high-priority production incident. During this session the management
team has to decide about a solution without knowing about the finer
details. Your job is to provide them with the most up-to-date
information. But wouldn't it be more efficient and effective to work on
solving the issue in a central place instead of talking about the
incident in a meeting? Wouldn't it be better that management already
knows about the finer details, because they can continuously obtain the
latest insights by just looking at the latest conversations in the chat
room?

ChatOps addresses the above challenges by aligning the work at hand with
the context of conversations. Or like others say, doing most of your
work from the same chat room as where you have your conversations.
Whether you communicate with your team members or have to perform an
operational task, you do it all from the comfort of a single command
line interface. Instead of collecting and cross-referencing the
necessary information from different tools and sources to perform a job,
you just type a command into chat that provides you with all the
information you need. All interactions with external systems and your
team members are logged and initiated in a central place. If I want to
deploy code, I type a command into chat and don't have to know about the
tool that executes the deployment. If I want to get reports about the
velocity of our teams, I type a command into chat. If I want to restore
a database backup on the production environment, I type a command into
chat. I do it all from the comfort and safety of a centralized chat
room, which automatically logs my activities and makes them visible to
the rest of the organization.

Many DevOps implementations face challenges in embracing the DevOps
culture of automation, measurement, and sharing. How to ensure an
automation-first mindset and behavior? How to embed the knowledge
sharing part within the teams and even across different teams? How to
ensure that teams stay transparent about all things that happen? ChatOps
is an accelerator for making such a cultural shift by enforcing work
execution via automated commands, by leveraging the power of automation
to facilitate knowledge sharing, and by providing a self-service
reporting environment via chat.

## What is ChatOps?

The core of ChatOps is that we as a team automate our knowledge (by
means of bots), and make that knowledge available as a command in our
chat room to prevent our team from being dependent on the institutional
knowledge of team members. Following the movement of ChatOps means that
you automate as much relevant work and knowledge as possible. All that
information should be accessible from the centralized team chat. Because
this team chat is also used to communicate about work, we'll get an
end-to-end trace log about all actions and communication that happened
to complete a given job.

On a day-to-day basis we extend the set of automated commands, so that
others are able to benefit from it. To force ourselves to keep
automating those commands, we follow one single procedure within the
team: the only way to know whether a daily task or insight is automated,
is that you have to type a command into the team chat. At the moment you
expect a given task to be automated, but it appears that this this is
not the case, the whole team can see that you expected a task or insight
to be automated. If this happens, the one central rule of ChatOps
applies: "If something isn't automated, you have to automate it!".

## Benefits

There are multiple benefits of applying ChatOps. The following list
contains a couple of elaborated examples:

-   **Increased collaboration:** by centralizing all interaction with
    your systems and team in one central command line interface, you get
    the benefit of increased collaboration within your team. Everyone
    within your team can see the communication that happened within your
    chat room and this ensures that team members are more involved.

-   **Instant information:** ChatOps speeds up the decision-making
    process. Instead of having to collect information from different
    systems and persons, ChatOps makes it possible to obtain all that
    information instantly by entering a single command into your chat
    room.

-   **Knowledge sharing:** especially in case of distributed teams,
    ChatOps helps in sharing knowledge and aligning communication. Each
    team member can see the full history of the chat and has to speak
    the same language within the chat. Every single command is logged,
    so everyone knows what happened on a given server or environment.
    This ensures that everyone is on the same page in communication and
    language, and also ensures that new team members can join a
    conversation very quickly. Because common tasks are automated,
    everyone within the team is able to execute a given task without
    having to know about the finer technical implementation.

## How to get started?

Getting your team up and running with ChatOps takes an investment to get
started. You are going to get a return on investment when the tasks you
do by hand are automated. Start small to get a feeling of what's smart
to do and what's not. Most importantly, don't invent it all yourself, be
a lazy developer by using a package that does the integration to your
chat platform. Building it yourself is fun, but not very efficient.
There are frameworks that can integrate with your favorite chat
environment, so pick a framework that uses a programming language that
is familiar to your team. Anyone should be able to contribute to your
tools and automation. If it is easy, it will become part of the normal
work and day-to-day tasks.

Building your first ChatBot can be done in a few hours, assuming you
already have a shared chat workspace that is capable of bot integration.
In a few hours you should be able to send a simple command from chat to
your chatbot, which then does something with your application. For
example: when using Slack, you can integrate with slash commands, web
hooks, or a bot. A bot is a good choice for ChatOps. For the Slack
integration there are already frameworks that do all the plumbing for
you. You only have to implement the actual code that integrates with
your application. Select a bot framework that fits your development
stack best.

![](./media/image1.png)

When you have the chatbot up and running, the next step is to select
functions you want to perform from chat. Keep it simple by making small
functions. Learn what is working and what isn't. Start by writing a
short description of what to call the new command. You can discuss this
description in the team to see if it is going to help you. Some things
you can start with:

-   Check logs

-   Restart the application

-   Report application status

-   Start a batch process

-   Perform application test integration

-   Start a deployment

-   Add/remove a user

-   Upgrade the database

-   Check database version

-   Automate a tool you are using.

Do the implementation and show it in the sprint review. Remember to keep
it small and score the value it adds for the team. Manage it in PBIs on
your backlog to get the right priority. The end goal is to keep your
team productive. By automating recurrent day-to-day work you have more
time for adding real value to the product you are making.

## Conclusion

We've seen that ChatOps offers a great set of benefits such as increased
collaboration, efficiency, knowledge sharing, and out-of-the-box
logging. ChatOps can also be used to break down barriers between teams.
In our day-to-day job we've seen great implementations of ChatOps where
the friction between the security, compliance and development teams was
taken away because of self-service infrastructure deployments via chat.
So don't hesitate, and accelerate your DevOps implementation by adding
ChatOps to the mix!
