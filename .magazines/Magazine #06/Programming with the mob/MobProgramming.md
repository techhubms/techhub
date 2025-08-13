There is a team in San Diego that started coding together with all the
team members working on one computer. It sounds a bit crazy. So why did
they do this? It grew out of the desire to maximize learning, sharing
knowledge and innovation; That and a lot of experimentation. Management
didn't care about the chosen approach; the only thing that counted were
the results and those looked good. The team got more work done with
fewer bugs in production by writing solid code. They coined it Mob
Programming. After reading this article you will know the foundational
ideas behind the concept, its advantages and how to get started to make
this work. **We think it is a valuable addition to adopting a DevOps
culture and way of working.**

## What is mob programming?

You could regard Mob Programming as pair programming on steroids. Pair
programming has its origins in the Extreme Programming philosophy: it
encourages two developers working behind the same computer, working on
the same code. While doing so, they both have a clear role. One is
behind the keyboard writing the code while the other verifies what is
written and is thinking ahead about the next steps to take. They
regularly exchange these roles.

Mob programming takes this discipline a step further. The whole team
works on the same thing, at the same time, in the same space, and at the
same computer. So, you could compare it to pair-programming, but
collaboration now includes the entire team instead of two persons[^1].
In the end, all the work is about delivering code to create done
software.

![](./media/image1.png)


Figure 1: Definition of Mob Programming (source: Mob Programming, Woody
Zuill)

### Foundational ideas

The advantages mentioned are key ingredients to culture and sharing,
which are elements of the CALMS (Culture, Automation, Lean, Measurement
and Sharing) principles of DevOps. Moreover, the foundational ideas
behind Mob Programming are in line with the DevOps philosophy:

**The people doing the work can best figure out how to do that work.**

One of the characteristics of a DevOps team is autonomy. In order to
collaborate effectively, the team members need to be able to make
decisions and apply changes without complicated decision-making
processes. This requires trust, changing the way risk is managed, and
creating an environment that is free of a fear of failure, i.e. an
environment that encourages fast learning. Mob programming sets the
stage for this environment under the condition that management gives the
team the autonomy and aligns teams by setting shared goals.

![](./media/image2.png)


Figure 2: Alignment enables autonomy (source: Henrik Kniberg)

**We can get a lot of benefit out of studying and practicing together**

Mob Programming promotes sharing of knowledge between people and
providing each other with feedback, with the intention to continuously
improve. Another important value that is accomplished is collective
ownership, as opposed to individual ownership of work. Instead of
relying on everyone on the team knowing everything, which will never
happen, it\'s great for the team to know everything as a group. If the
team feels a sense of collective ownership this group approach will
work.

**Pay attention to what's working, and look for ways to "Turn up the
good"**

A continuous improvement mindset is a key DevOps soft skill[^2]. One out
of three improvement ideas will be worth holding on to. The other two
will be discarded. Finding out in which category an idea falls is the
trick and is done by executing Plan, Do, Check and Act cycles. Mob
programming will help in achieving improvements by means of working as a
team and have an active discussion about it. The reason for this is that
any impediments immediately affect the team when working together. So,
these topics will need to be discussed and solved on the spot for the
team to be able to proceed. This is something that would not easily be
done without this type of collaboration.

**Getting good at getting good results from retrospectives is
important**

As with the previous foundational idea, the reflection by the team on
the way collaboration is done and how work is being delivered, is a
daily, continuously occurring feedback loop. Since the team is
constantly together they optimize the opportunity to act on any issues
that may occur while they work together. The entire team understands the
context of the possible improvement action, they were all present, and
it allows the team to act immediately. This fits well with the concept
of left-shifting, binging the opportunity for feedback and improvements
forward. If the team is practicing Scrum, it also ensures that the
retrospective can be used for tackling larger topics.

### Driver-navigator model

The main work of programming is "thinking, describing, discussing, and
steering" what we are developing. The Navigators are doing this part
while the Driver is doing the translation or transcription of the ideas
into code (see [cross]{.mark} [ref]{.mark}). The driver role is rotated
at frequent intervals. ![](./media/image3.png)


Figure 3: Driver-Navigator collaboration pattern (source:
mobprogramming.org, Woody Zuill)

When the team grows in experience and maturity, a mobber may will learn
to take on one or more (temporary) roles (see [cross ref]{.mark}). For
example, one may act as a Researcher who and quickly collects
information during the development process in order to support the
navigator. Or Automationists who focus on options for automating
repetitive tasks. If the Driver needs to implement a certain design
pattern but doesn't know how to implement it from the top of his head,
the researcher could go and find out on a separate laptop, and then take
over as navigator. For more information on these roles, have a look at
the Mob Programming Role Playing Game on
Github[^3].![](./media/image4.png)


[SIDE COLUMN/CALL OUT]{.mark}

+-----------------------------------------------------------------------+
| Small experiments with Mob Programming                                |
|                                                                       |
| In the Professional Scrum Developer[^4] course we always touch on     |
| pair programming. In                                                  |
|                                                                       |
| certain cases the class benefits from experiencing mob programming    |
| and we might introduce the concept after the first sprint.            |
|                                                                       |
| Usually the first experience with Mob Programming is pretty close to  |
| total chaos. Everyone needs to understand their role. There is no     |
| agreement on what is and what isn\'t socially acceptable behavior and |
| often it causes all kinds of hidden issues to surface. This initial   |
| experience can feel frustrating or unproductive. A short              |
| retrospective at the end of the experience can help bring these       |
| frustrations to the forefront and can help remove them. Often these   |
| frustrations are happening in the team in their normal way of         |
| working, [but they\'re never bad enough to discuss and resolve        |
| them.]{.mark}                                                         |
|                                                                       |
| Examples of issues surfacing after one hour of Mob Programming:       |
|                                                                       |
| -   Coding styles between team members are quite different, making it |
|     hard to switch driver positions; normally this isn\'t an issue as |
|     developers would own the code from start to finish.               |
|                                                                       |
| -   Getting used to giving and receiving feedback. Many teams         |
|     practice code reviews after a significant portion of the work is  |
|     done. This reviews the end-product, but not the creative process  |
|     that led to that result. Pair programming already greatly         |
|     increases the amount of feedback, and thus the opportunity to     |
|     learn. With Mob Programming the number of eyes and insights is    |
|     multiplied even further. People need a safe environment in which  |
|     they can deal with what may feel like working under a magnifying  |
|     glass.                                                            |
|                                                                       |
| At the same time a number of things were very helpful. One hour of    |
| working this way led to new insights, which were turned into          |
| improvements that would otherwise be unknown:                         |
|                                                                       |
| -   Because the entire team was working on the same code, no time was |
|     spent on merging code. In the previous exercise the team had lost |
|     precious time trying to get two features to cooperate properly.   |
|                                                                       |
| -   Many tips and tricks were shared. Short-cut keys to common        |
|     actions, alternative solutions for code structures, and tips for  |
|     better testability were shared.                                   |
|                                                                       |
| -   Everyone felt comfortable maintaining or having to bugfix the     |
|     code they had created together.                                   |
|                                                                       |
| -   There was far less need for coordination and status sharing.      |
|     While they still practiced a daily scrum, it focused immediately  |
|     on the impact, on the goal and other higher-level concerns. No    |
|     sharing of minute details and progress.                           |
|                                                                       |
| At the end the team felt that working this way would probably cause   |
| them to be dead tired before lunch; and that they\'d deliver a lot    |
| more than they are currently used to.                                 |
+=======================================================================+
+-----------------------------------------------------------------------+

## Advantages for a DevOps transformation

Several advantages can be mentioned that all support and speed up your
DevOps transformation.

### Strengthens Lean thinking

Mob programming brings a lot of focus to the work. The whole team is
working collectively on the same thing, expediting the delivery and
working towards single piece flow. The ruthless focus removes the need
for branching and merging, and other practices that silently consume a
lot of time or add overhead in order to facilitate people working in
parallel.

Encourages sharing and changing the culture

The culture of sharing, by exchanging knowledge, adopting continuous
learning and breaking down barriers by forming cohesive teams, is
implicitly carried out.

Focusing on delivering the same piece of code makes everybody learn
simultaneously about the delivery process and the way a feature is
implemented. This strengthens the software craftsmanship culture that is
required to excel. Team members learn about things that are out of their
comfort zone or specialism. This will lead to building up
cross-functional, or t-shaped, team member profiles.

If an impediment pops-up it will be discussed immediately (and removed
if possible), because it is out-in-the-open and the whole team suffers
from it at that very moment. Working together on the same goal gives a
shared purpose and strengthens team cohesion, leading to a culture of
trust. Last but not least, these are important ingredients for employee
and job satisfaction.

Altogether this is fostering the DevOps culture.

### Increases productivity and quality 

Critics say that this way of software development will be slower, and
will take more time than working in a more traditional way. However,
research has proven that peer-review and code-inspection is one of the
most powerful ways of detecting bugs early. You are shifting bug
detection to the left of the software delivery process. This could be
even more powerful than automatic testing. Shifting left increases
productivity and quality, because early detection leads to less rework.

When you also take into account the time that you need to achieve
sharing and changing culture when everyone works in a traditional way,
there is no reason to assume that Mob Programming may be less efficient
than any another approach.

## Getting Started

When asked how to get a team motivated to start Mob programming, Woody
Zuill answered that the goal should not be to do Mob Programming. That
they had never set out to do Mob Programming. They wanted to maximize
their learning, knowledge sharing and innovation. By ruthlessly striving
to achieve these goals through endless experimentation, their way of
working eventually led to Mob programming as we know it now. His advice
was: If you want people to do Mob Programming, get them sold on the
goal. Then again: **who wouldn\'t want to work in an innovative
environment where everyone is helping each other learn and where you can
experiment safely?**

Like many other practices and techniques, it\'s hard to accept that when
you start, you may not get all of the promised benefits immediately. As
a team you may need to get together much more than in the past. Issues
will surface and you may need help from a skilled facilitator to get
through the first hurdles.

To get started you will need (see [cross]{.mark} [ref]{.mark}):

-   [A team that wants to try!]{.mark}

-   A big screen or projector

-   A workstation or laptop to work on

-   One or more keyboards and mice (people have their preferences)

-   A flipchart or whiteboard

-   A goal

-   The safety to try new things.

![](./media/image5.png)

Figure 5: Basic workspace setup to get started (source: Mob Programming,
Woody Zuill)

## We challenge you

Applying Mob Programming can speed up your DevOps adoption process. It
encourages sharing and makes you adopt your culture on the spot. You can
choose to apply it rigorously with an Extreme Programming mindset, but
if this is too ambitious or a step too far, you can also do small
experiments. Working like this for a few hours, like we did in training,
will already give you a lot of insights. We would like to challenge you
to have the courage to start experimenting and we look forward to
hearing your thoughts.

## References

*Mob Programming: A whole team approach, Woody Zuill and Kevin Meadows*

*Mob Programming website <http://mobprogramming.org>*

*Agile Alliance
<https://www.agilealliance.org/glossary/mob-programming>*

*A game for exploring the development practice of mob programming
<https://github.com/willemlarsen/mobprogrammingrpg>*

[^1]: Woody Zuill: Mob programming: A whole team approach:
    https://www.youtube.com/watch?v=SHOVVnRB4h0

[^2]: DASA Competence Framework
    (https://www.devopsagileskills.org/resources/document/white-paper-the-need-for-new-skills-dasa-devops-competence-framework/)

[^3]: Mob Programming Role Playing Game:
    https://github.com/willemlarsen/mobprogrammingrpg

[^4]: Scrum.org Professional Scrum Development with .NET:
    https://training.xebia.com/microsoft/professional-scrum-developer-net
