# A recipe for high-quality releases

Shipping applications to a production site, continuously, is becoming
more common every day. Deployment pipelines make automatic installation
of software onto a site possible. The next step? Releasing value
increments continuously and ***safely***.

Quality assurance is an important part of continuous delivery.
Installing the software on production is one thing. Making sure it does
what it is supposed to do, is another. We prefer validating that
automatically, within a couple of minutes. It is possible to create a
release pipeline that validates all functional requirements of the
software, fast, and in a way that convinces non-IT stakeholders.

### The deployment checklist

At some point in time, somebody decides to go live. The users of the
application can now start using new features. Going live shouldn't cause
any problems. Validating some of the following will ensure that:

-   All the domain's business rules. Have they been implemented
    correctly?

-   The integration of the software in the IT landscape. Do all the
    applications that depend on it, and all other applications it
    depends on, still work?

-   Infrastructure. Does the application have sufficient permission to
    do what it is supposed to do? Are all SSL certificates still valid?
    Has the application been configured correctly?

-   Non-functionals. Like security and performance.

Validating that can't take longer than a couple of minutes. And that's
challenging. Especially because there are a lot of business rules to
validate, and usually validating business rules requires integration.
Too much integration makes it impossible to validate all business rules
in less than five minutes.

### The target audience of a unit test

"Testing is an information, intelligence or evidence gathering activity
performed on behalf of stakeholders to support
decision-making." --- Paul Gerrard.

Depending on how the unit test has been written, it gathers a different
type of information. It either manifests in a meaningless green bulb, or
it provides information about what functionality has proven to be
implemented in the software.

"Unit tests are tests, written by developers, for developers, and they
are fast." --- Bob Martin

Unit tests test the if-statements of the code. Some developers write
them before they write the code, others do so afterwards.

There are different styles of unit testing. Some developers say a "unit"
is a single class. Others say a "unit" may as well be a combination of
classes. The "smaller" the unit, the more technical information the
tests provide. As a unit becomes "bigger", and the subject under test is
a combination of more components, the unit tests tend to provide more
functional information. These two schools of unit testing are known as
inside-out and outside-in, or London and Chicago style.

Depending on the type of information the development team needs, a
different style of unit testing applies. Unit tests only provide the
developer with very detailed information about small parts of the
system. They assist the development team in judging the fitness of
low-level components of the system.

### Unit tests are not enough

Considering the target audience of unit tests. The development team
should have (some of) the information they require to support their
decision to go live. Unfortunately, usually, the development team is
only one of many stakeholders in a project. What information do other
stakeholders need? And in what format?

Installing the software in a test environment and having stakeholders
validate their requested changes there, slows down the deployment
process. Such a scenario indicates manual work, too. Manual work is
expensive and time-consuming. Also, this scenario means regression needs
to be tested manually. That slows down the deployment process even more!

A proper, manual regression test easily takes days or weeks. All
functional requirements of the complete system are validated in such a
test. Depending on the desired release frequency, automating that
process can save lots of time and money.

Whether or not automating regression tests *does* save time and money,
depends on the amount of effort spent to create the automated regression
tests, the amount of maintenance they require over time, and how fast
they run. Testability is a software-architectural driver. If
releasability is a requirement, proper software architecture supports
easy regression testing.

### Separate business rules from infrastructure

A common misperception is that integration should be tested at the API
level. Invoking a REST endpoint carries out a command to the database
and back. This results in tedious, slow tests that require a lot of
maintenance. They require databases to be in a given state,
configuration to be correct, other systems to be up, and so forth. Such
a test has too many responsibilities. It validates too many different
things, implicitly and explicitly, and as a result, it is slow.

Invoking an endpoint to validate whether the business rules have been
implemented correctly, proves more than intended. It proves a correctly
configured API, for example. Otherwise, the endpoint would not have been
available. And proving that the API has been configured correctly over
and over again for every business rule is redundant and time-consuming.

The Single Responsibility Principle also applies to tests. It is the key
to a fast test suite. Make a test responsible for validating only a
single thing. Craft the source code accordingly. Creating a fast,
functional test is particularly easy when infrastructural concerns and
business rules haven't been mixed. Alistair Cockburn's Ports and
adapters architecture (also known as Hexagonal architecture)
demonstrates one of many ways to properly separate these concerns.

### Separate technical test code from functional test cases

Continuous delivery and deployment pipelines are words managers or
clients should not need to understand. They probably care about the
(strategic) goals of their business and how the new versions of their
software help them achieve those goals. Showing what the development
team has changed in the software, and how they have mitigated any risks
associated with that, can help to gain the stakeholders' confidence. It
sounds like a lot of work, but that should be fairly easy with the
proper integration tests in place.

Unit tests have proven to be tough to explain to clients. A common
practice is to separate (important) test cases from test code. Use
Behavior Driven Development (BDD) to do that. Write down the
specifications of the software in the Gherkin format and discuss them
with the stakeholders. Use BDD frameworks like SpecFlow to implement
these test cases and run them in the deployment pipeline during every
release. And use plugins like Pickles to generate reports about the
team's test efforts, automatically and without any effort.

### Take chain testing to the next level

Loads of unit and integration tests provide clarity on the quality of
the software. When these tests pass, they indicate that the business
rules have been implemented correctly. But there's still the matter of
integrating them into an environment of other systems. Most likely the
system depends on other systems and other systems depend on it.

Chain tests are extremely time-consuming and expensive. Opening a
front-end, going through a couple of scenarios, and validating what
appears in other systems, proves that systems integrate correctly. This
requires the entire application landscape to be up and running, to be
configured correctly, and to be in a given state.

Conceptually, when a user clicks a button, a command is carried out by a
system. This system creates other commands and queries, in a given
format, which it sends to other systems.

Any system can produce a variety of queries and commands to other
systems. They're mocked and asserted upon in the tests. Sharing these
mocks and assertions, allows other teams to use them as input for their
tests. They can validate the ability to process them, continuously,
without having to install any software in any environment. This concept,
Consumer-Driven Contracts, allows delayed execution of chain tests in a
deployment pipeline and locally. It provides fast feedback and reduces
the need for chain testing.

### Does it run at all?

It's good to know the business rules have been implemented correctly and
that the application integrates into the environment. But that's rather
useless if the application doesn't start. 

#### Epic fail.. All tests pass... But production is broken...

Regardless of all validations mentioned earlier, there's still a chance
the application isn't going to work. Faulty configuration, permissions,
and dependencies can cause the application to break. 

That's easily validated by invoking the most important endpoints and by
asserting it doesn't return any unexpected errors. 

Unfortunately, invoking some endpoints will cause mutations in data.
Everybody knows mutating production data with tests should be avoided.
And running these tests in any environment but the production
environment doesn't make sense either. It proves the test environments
are fine, while we want to check whether the production environment is
working properly. 

Hence the need of a representational test environment. An environment
that's pretty much equal to the production environment. Running these
tests there makes the test-results conclusive enough. 

### Look for a sign of life

Having a production-like environment, in which all critical parts of the
application seem to be operational, makes it likely that the deployment
on the production environment will be successful.

There shouldn't be a big difference between a production-like
environment and the production environment itself, but there must be
some difference... After all, it's production-like, and not the
production site itself. 

Execute a simple smoke test after installing the software onto the
production site. A trivial one, too. Invoke some GET on an endpoint and
assert a 200 OK.

### Choose carefully

Base the effort spent on testing on the risk that is involved. Not all
changes are equally risky. Faults in the software have a different
impact. In some cases, it makes a lot of sense to spend more money on
testing. In some cases, it doesn't. It depends. Use a probability impact
matrix to determine the effort that applies:

![](./media/image1.png)


A probability-impact matrix helps to estimate the risk

Testing software takes time. And clients want their features at some
point in time. Sometimes pushback is appropriate, and clients should
wait just a little longer. Use the probability impact matrix to decide
when to push back and when to take time for testing. Estimate the
probability first: How likely is it that this change will cause issues?
Then estimate the impact: How much money, reputation, or any other thing
that matters to the company, is lost if it does? The more risk involved,
the more testing effort is appropriate.

### Summary

Martin Fowler's Practical Testing pyramid shows a different type of
automated tests. The top level of the pyramid refers to assertions made
on the entire application as a whole. That's the highest possible level
of integration. The bottom of the pyramid refers to assertions made on
parts of the system in the highest possible level of isolation: Unit
tests.

Unit tests are tests written by developers, for developers. They provide
developers the information they need to determine the health of the
low-level components of the system. 

Usually, the other stakeholders can't judge the health of the system by
looking at the results of unit tests. They need functional, readable
test cases to make that call. And they need to make assertions on
combinations of components. Separate functional test cases from
technical test code, and make the test cases available to the
stakeholders. Isolate the combinations of components that implement
business rules from infrastructure to keep the tests fast. 

Nonetheless, configuration, permissions and external dependencies can
still cause an application to break. Run contract tests, end-to-end
tests, and smoke tests in the release pipeline to make sure that it
doesn't happen.

Don't get carried away. Base the test effort, per story, on a
probability impact matrix to make sure the effort spent is reasonable,
compared to the risks involved.
