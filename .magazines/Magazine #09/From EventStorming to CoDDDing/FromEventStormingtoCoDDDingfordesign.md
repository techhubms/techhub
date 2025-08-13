We live in a world of constant change. Today we generate more data than
we can consume[^1], and it contributes to the constant changes that we
observe. Within the IT industry, we have the ambition to create flexible
and reliable solutions that can cope with the demand for changes. We
created frameworks, new programming languages, and abstracted the
management of hardware with the cloud offering. Our industry provides
tools and techniques that allow organizations to achieve the promised
land of delivering business value to match the changing world. This is
why we see many companies make a move towards microservices for mostly
the same reasons; creating smaller deployable units, and to achieve a
shorter and quicker feedback cycle. Moreover, many companies see the
need for Domain-Driven Design to be able to do it.

\-\-\-\-\-\-\-\-\-\--

However, what we observe is that we still see the same code is written
as follows:

![](./media/image6.png)

On the one hand, it looks like a proper domain model, but if we look
closer, the code is actually an anti-pattern described by Martin Fowler
in 2003 as the Anemic Domain Model[^2]. In his words, "The fundamental
horror of this anti-pattern is that it's so contrary to the basic idea
of object-oriented design; which is to combine data and process
together. (\...) What's worse, many people think that anemic objects are
real objects, and thus completely miss the point of what object-oriented
design is all about".

What is the reason that we are still using this anti-pattern to date in
complex environments? We won't argue that in simple domains an anemic
domain model works fine, only most of the software we write is intended
for more complex models. One of the well-known reasons for it to happen
is all the ORM examples shown on the internet. Heck, we even fell for
that trap ourselves when we started developing. Only we also have
another theory, and it has to do with the way we communicate together,
it is the way we do social-technical software engineering.

The common belief is that people will collaborate in open spaces, where
the ideas can flow between the different persons involved in the
software creation process. Thus, organizations invested in the creation
of wide-open spaces, hoping the teams will deliver more value. What we
observed is the opposite outcome; the communication between teams and
team members decreased due to the physical workplace conditions, given
that the open spaces produce high levels of noise. Usually, people buy
expensive noise-cancelling headphones and use virtual communication such
as ticketing systems or instant messaging to communicate.

The issue with the way we communicate nowadays is that we are all
subject to cognitive biases that screw up our solutions. One that has
the most effect in these forms of communication is the confirmation
bias, where we focus on information that only confirms existing
preconceptions. Language plays an important role here. Any group or
tribe has its own lingo, used to describe their concepts and processes.
As developers, we create code to streamline the processes, trying to
capture the concepts. However, if we don't listen carefully to the
language, we will miss the essence of the concepts, leading to a
mismatch between what the business expert thinks, and the actual code
that is going to production.

## Why Domain-Driven Design?

Domain-Driven Design (DDD) is a holistic approach to software
development. Eric Evans coined the DDD term in his book, back in 2003;
it addresses the difficulties software teams have in building software
autonomously. Multiple teams design and build complex solutions as
monolithic software that usually employs only one model that aims to
solve different problems. He described the ambiguity between the
language that the business speaks and the language coded in the model.
Such ambiguity causes confusion and entanglement within and between
teams.

To solve this problem, Eric created the concept of a Bounded Context, a
pattern to divide software based on a model of consistent language.
Within the Bounded Context, we create a shared language through
conversations between business specialists and software people; this
becomes the Ubiquitous Language. We focus on a language that concisely
describes the situation within the domain. Instead of one canonical
language for the entire business, we create several Bounded Contexts,
each with their specific language and model.

Within a Bounded Context, one team can take ownership of its model and
increase its autonomy as team members develop software. They can test in
isolation since teams have a clear vision of who their customers are and
can receive their feedback metrics. Studies[^3] by Nicole Forsgren
Ph.D., Jez Humble, and Gene Kim have shown that the strongest predictors
of continuous delivery performance and successful organizational scaling
are loosely coupled teams, enabled by loosely coupled software
architecture. It makes the Bounded Context a fundamental pattern if you
want to accelerate!

## How do we create a shared language between business and IT?

"A picture says more than a thousand words" the saying goes.
Conversations about complexity usually happen in a meeting room setting,
with everyone sitting around the same table, watching a screen while
being in a discussion. Most of the apparent communication in these
meetings is through words. Studies have shown that the human brain
processes images faster than words, and remembers visualization better
than speech. It means that when we have a conversation about complexity
that is more visual, the general level of ideas, decisions, and
productivity will increase.

  -----------------------------------------------------------------------
  ![](./media/image1.png)
  
  -----------------------------------------------------------------------
  [Copyright EventStorming.com]{.mark}

  -----------------------------------------------------------------------

It is good to know that we can use EventStorming for lots of approaches
like discovering our business architecture and finding our software
delivery flow. In this article, we will describe EventStorming for
software design. When using EventStorming, it's always crucial to do a
chaotic exploration and enforcing the timeline. For software design, it
means that we need to embrace ambiguity. This process occurs between the
problem space and the solution space. The problem space is our world as
we perceive it, and it is the space of the business architecture, where
it is independent of the software, and the language is fluid. On the
other side, the solution space is the solution for the problem at hand,
the world as we designed it through software architecture where we
design models for creating software.

EventStorming for software design is a technique that iterates between
problem space and solution space. As described, we embrace ambiguity
when people present their perspective on the problem. From that point,
the language is refined, and we make the explicit implicit. It means
that during an EventStorming session, the facilitator needs to be able
to steer between the problem and solution space.

## We had an EventStorming session, now what?

  -----------------------------------------------------------------------
  ![](./media/image2.jpg)
  
  -----------------------------------------------------------------------
  Example outcome of an EventStorming session on the cinema domain

  -----------------------------------------------------------------------

\<AS QUOTE\> EventStorming is about merging the people and splitting the
software using bounded contexts. - Alberto Brandolini \</AS QUOTE\>

EventStorming gives us clues about how to design our bounded context. A
bounded context is where we create a model for a purpose, and the
language stays consistent --- the first clue we can find in looking at
the people. Different people have different needs, and we probably need
to create different bounded contexts for that --- the second clue
resides in policies. Policies are reactions to domain events. In its
essence, it means that if a domain event happened, we need to do X.
Policies are always a good conversation starter. Policies are usually
containers of more insights and information, and it is here that the
communication between concepts takes place --- the third clue you can
find is in the language. We need to listen to the language spoken and
what concept is meant by it. We want to create a consistent language in
a bounded context. That consistent language then turns into the
ubiquitous language used only in that bounded context.

Only when you believe that you have enough information and scenarios,
you should leave the problem space behind and dive into the solution
space and start modeling. It is of vital importance to make this
decision consciously and explicitly. Eric Evans, after his seminal book
(known as the Blue Book), created a good guideline for deliberate
discovery, with his Model Exploration Whirlpool[^4].

## How do we design the bounded context model?

A vital facilitator skill is to be able to listen and filter the
information. During an EventStorming session, people will use concrete
examples to explain the business rules at hand. When it happens, as a
facilitator, you can distil it. Our technique is to write down the
examples as they appear. You can use post-its or index cards. The
collected examples are a valuable source of information, and from it, we
can start to do Example Mapping either during or after our EventStorming
session. Crossing these two techniques will push the group to generate
more insights into the domain, allowing the development teams to
decrease the assumptions, leading to better models. The examples laid
down during the session are used to drive and validate the behavior of
the models. Another important aspect is the domain concepts. Every time
the group stops to discuss the meaning of a specific domain concept it
takes the time to write it down. This information is crucial, given that
it will provide clarity when creating the models. Also, it works as
documentation, and it is up to the group to persist on having it in a
more durable format.![](./media/image3.jpg)


From this point, we start to create object models. It is useful for the
domain experts to join the session because we will further explore our
language and (most probably) change the Ubiquitous Language. As a
starter, we begin by putting down the domain concepts captured during
the session and start making relationships between them. At that point,
we link the business rules to the objects which will enforce that rule.
A tip: we are still discovering, and laying down post-its; it is normal
to make mistakes, and we invite you to create several potential models
for the same problem. Running the examples captured before against the
different object models, we can evaluate the solution, and make
conscious decisions (trade-offs). Once we have the feeling that we
explored enough and fine-tuned the language and concepts, we can start
designing our models with the Model-Driven Design building blocks. At
this point, the development team does not need the domain experts.

## Building blocks of Model-Driven Design

When Eric Evans wrote the Blue Book, the prevalent programming language
paradigm was object-oriented. However, it doesn't mean the building
block is object-oriented. If you want to know how to do it in a
functional language (F# in .NET world), we recommend reading Scott
Wlaschin book "Domain Modeling Made Functional". We model in an
interactive way, and we don't follow a clear path. In this section we
will describe some key heuristics used in our decision making process.

From the building blocks of Model-Driven Design described by Eric Evans,
we will use a subset of it: Value Object, Entity, Aggregate, Aggregate
Root and Domain Service. After his first book, Eric wrote a reference
book, bundling all the patterns[^5].

  -----------------------------------------------------------------------
  ![](./media/image4.png)
  
  -----------------------------------------------------------------------
  Example of a value object

  ![](./media/image5.png)
  

  Example of an aggregate root
  -----------------------------------------------------------------------

Value Objects are simple because they are stateless, so one of our
heuristics is trying to design everything as a Value Object. We can only
do this if we only care about the reference and logic of the object.
When its identity rather than its attributes distinguish an object, we
need to use the Entity building block; this is another heuristic that
you can use. Whenever we need to protect our business invariant over
several objects, we cluster these into an Aggregate. We choose one
entity to be the Aggregate Root and allow external objects to hold a
reference to the root only. Some concepts from the domain aren't natural
to model as objects, and for this, we wrap the logic as a Domain
Service. This last heuristic is essential, given that not everything
needs to be an object (in the end people are developing in an
object-oriented paradigm), and you need to balance the trade-offs. You
can find the code at
[[https://github.com/joaoasrosa/xpirit-magazine-from-eventstorming-to-coddding/]{.underline}](https://github.com/joaoasrosa/xpirit-magazine-from-eventstorming-to-coddding/).

## In conclusion\...

The problem with engineering teams is never the technical knowledge; it
is the domain knowledge. Domain-Driven Design and microservices won't
help you if we don't start to collaborate with the domain experts
regularly and create a shared mindset. To do that, we need to find more
straightforward and more accessible tools to collaborate. EventStorming
is a simple tool to learn that can quickly give us enough knowledge as
well as a shared team mindset. Quoting Alberto Brandolini "Software
development is about learning, working code is a side effect". We need
just enough upfront design for our software, so that we can decrease the
assumptions and increase the value delivered to the end-user. It is the
cornerstone of an Agile mindset, with which we need to inspect, learn
and adapt.

[^1]: [[https://www.weforum.org/agenda/2019/04/how-much-data-is-generated-each-day-cf4bddf29f/]{.underline}](https://www.weforum.org/agenda/2019/04/how-much-data-is-generated-each-day-cf4bddf29f/)

[^2]: [[https://martinfowler.com/bliki/AnemicDomainModel.html]{.underline}](https://martinfowler.com/bliki/AnemicDomainModel.html)

[^3]: Accelerate: The Science of Lean Software and DevOps: Building and
    Scaling High Performing Technology Organizations - Nicole Forsgren
    Ph.D., Jez Humble, and Gene Kim ISBN-13: 978-1942788331

[^4]: [[http://domainlanguage.com/ddd/whirlpool/attachment/ddd_model_exploration_whirlpool-2/]{.underline}](http://domainlanguage.com/ddd/whirlpool/attachment/ddd_model_exploration_whirlpool-2/)

[^5]: [[http://domainlanguage.com/ddd/reference/]{.underline}](http://domainlanguage.com/ddd/reference/)
