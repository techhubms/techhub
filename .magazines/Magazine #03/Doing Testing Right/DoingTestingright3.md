**Doing Testing Right and at the Speed of Light**

*Viktor Clerc and Erik Swets*

*Most organizations think they are ready for the future when they engage
in agile software development practices and ALM. However, if these
organizations do not change their traditional, after-the-fact approach
towards testing, they will not be able to deliver their desired value at
the speed of light. This article describes a different approach towards
testing, presents steps to properly introduce testing and test
automation, and shows how the Microsoft eco system can be employed to
achieve this: integration with all the relevant test tools by
standardizing their output formats. We exemplify this by using various
concrete examples obtained along the way*.

**Wake up!**

IT organizations that still feature an after-the-fact approach to
testing need to radically change their software testing approach if they
are to stay in business. New ideas, Minimum Viable Products, apps, and
novel integrations are what businesses should be built on -- without any
compromise on quality. In an era in which acceleration of the delivery
cycles is necessary to shorten the time to market, traditional testing
is one of the last bastions that needs to be conquered. Testing these
ideas, MVPs, apps, and integrations as quickly and often as possible
becomes a mere necessity: the sooner we can invalidate assumptions, the
sooner organizations can come up with the right, game-changing
innovations. Organizations simply cannot survive without it.

Testing is a unique discipline which requires a clear, quality-oriented
mindset. Traditionally, test activities are performed by dedicated teams
of test professionals. In turn, these test professionals will commence
testing the software only once it is built. As such, test activities
typically are performed after a sprint's end. Often they are
labor-intensive, and in the case of findings, rework is injected into
subsequent sprints. This negatively impacts the velocity of the project
and can ultimately lead to the project's grinding halt: what is built in
two weeks' time, often takes two months (or more) to be put into
production. This needs to stop. Organizations should be able to deliver
high-quality software to production as often as possible and as quickly
as possible, so there really is no use in separate testing activities
with late feedback cycles.

In short, it's time to do it differently. Testing and checking should
commence before the production software is built and should rely on
close collaboration between the whole team. It's a rather clear adagio:
"we specify, develop, and test our functionality together, and we
deliver software that is production-ready as soon as it's ready."

**A plea for a straightforward test strategy**

Some of the most important questions related to testing include: "What
do we want to test? And why?". If the rationale for testing is clearly
defined, different test types can be logically derived; a focus on
integration testing, performance testing, or resilience testing. As a
next step, we define a current and a desired state on how these tests
are actually performed (automated or not), where the testing takes place
(see sidebar on the agile test automation pyramid), what technologies
are used and by whom (the test-oriented team member, or
development-oriented team member).

**Sidebar** *Mike Cohn\'s agile test automation pyramid*

![Test%20Pyramid.png](./media/image1.png)


The pyramid defines several layers on which tests can be automated. Unit
tests are the lowest-level tests, counterparts of designated source code
functions. Automated end to end tests should (only) focus on end-to-end
integration scenarios rather than testing individual functionality.
Likewise, Component & integration tests serve to test functionality of
the application, typically exposed through services, without having to
rely on the user interface of the application.

One should strive for automating at the lowest level possible ('Unit
tests'), since this enables quick feedback cycles, detailed feedback
information (\"the bug occurs at line 42 of the Customer Service
module"), and the corresponding technical ecosystem between test code
and production code.

**Start right, and start now!**

Every organization can start with moving towards a sound agile testing
and test automation strategy. Organizations that use the Microsoft stack
can immediately implement the right tooling in order to speed up their
feedback cycle.

Welcome to Visual Studio Team Services and Team Foundation Server (TFS)
*cum suis*. New required functionality (*user stories)* is defined
within TFS as Product Backlog Items (see Figure 1), each of them broken
down into one or more tasks. Proper operation of the functionality needs
thorough testing. As such, one or more test cases can be linked to a
product backlog item. The test cases are managed within Microsoft Test
Manager (MTM). Within MTM, these test cases do not pose any restrictions
as to how the test case is executed (manual or automated) and with
regard to the technical ecosystem; the test case merely serves as a
description of the test activities.

![TFS%20Work%20Item%20Definitions.png](./media/image2.png)


Figure Work Item definitions in Team Foundation Server

Traditionally, testing using a TFS-/MTM-based setup as outlined above
occurs by running the application and (often) manually performing the
steps of the test cases that are attached to a specific work item. This
is a suboptimal approach because of the distance between testing and
coding -- both temporal distance (first we need to develop code, only
then can we perform the testing activities) and technical distance (test
(plan)s and test results reside in a separate system, namely Microsoft
Test Manager). Furthermore, testing occurs within big chunks (complete
sprint results as the unit of test) rather than intermediate feedback on
partly-implemented Product Backlog Items. This makes it more difficult
to address test findings (find the needle in the haystack), which again
delays the feedback cycle.

This approach, which is often encountered in many organizations,
requires a significant speed-up in order to achieve the true merits of
continuous delivery (integration and testing). So, how can we use
readily available solutions to perform proper testing as quickly and as
often as possible? Let's take the next steps to reap the full benefits.

**Taking the next steps**

1.  Automate your tests

Automate existing test cases within TFS, or develop integration adapters
and attach some assembly to the test cases in Microsoft Test Manager.
This can be used to link Protractor (or similar) test code for an
AngularJS front end (cf. *Xpirit Magazine #2*, p. 40-46). Currently,
only MSTest-based test cases can be associated, and increase of this
coverage may not be expected. Microsoft díd announce a new capability to
link test results that were generated during a build run with any
arbitrary adapter, to requirements, for instance user stories[^1].
Linking test results to test *cases* seems like an obvious next step --
this feature is requested.

2.  Truly bring your tests to the code

We can take this even one step further. Why not make the test case
itself reside within the source code -- right next to the production
code? This enables tests to be run as soon as code is developed --
possibly even before: Gherkin-style specifications (*given -- when --
then*) can be used to enable the business to speed up refinements and
clearly describe the acceptance criteria for each product backlog item.
This notation also helps to achieve common understanding between all
stakeholders of the required functionality. Below, we share an example
of shopping cart functionality that verifies the total article count.

Given I am logged in

When I add a product to my shopping cart

Then the cart total is updated

The Gherkin specifications are transferred to source code once
development commences, and glue code is developed to link the test case
and code to the production code. Both the production code and test code
are maintained by the team. All code is part of the same source code
repository, nicely placed together.

![](./media/image3.png)


![](./media/image4.png)


As soon as parts of the functionality are tested and committed, all
tests can be executed automatically. Issues are spotted as quickly as
possible, because the right tests are run as part of your pipeline at
each commit. As a result, lengthy analysis steps to find the issues are
reduced to quickly spotting the issue, actually fixing it, and observing
a green test.

3.  Test and develop as one in an integrated pipeline

Within the VSTS/TFS Build and release, we can construct a pipeline where
*all* the tests reside. Regardless of the technological choices for our
applications' platforms (Angular, iOS, C#, Java), we enable this
pipeline to drive all development and test activities. This allows us to
write test code in the same technical context as the production code
(i.e. we use Protractor for Angular tests, and nUnit for C# unit tests),
because we simply integrate the test results based on their standardized
output formats (such as *xUnit, jUnit or NUnit*); this prevents the
community from having to write (and maintain) adapters. Even more
importantly, using standardized test output formats allows us to
integrate all automated tests in a heterogeneous and partially
non-Microsoft ecosystem. There is an open standards world out there
whose benefits can readily be reaped.

![](./media/image5.png)


Figure Running tests within a pipeline

**Implementing your test strategy in the pipeline**

The build pipeline acts as continuous integration and allows us to chain
all kinds of tasks so that production-ready software is achieved. This
includes development and test tasks. The following section describes how
some of the types of tests from the agile test automation pyramid (see
sidebar) can be implemented within a pipeline concept.

-   **Unit tests** are performed during development, prior to committing
    changes and in the build pipeline, prior to building the component
    consisting of the various units.

-   **Integration tests** test specific integrations between separate
    components. These tests can happen internally and externally.
    Internal integration tests may consist of test routing within a
    service. External integration tests may test compatibility with data
    structures in code and database schema, or how an application
    handles unavailability of a database.

-   **Business-facing component tests** test the user interaction with
    one or more parts of the system.

> [Implementation (simplified)]{.underline}
>
> describe(\'shopping cart filter\', function(): void {
>
> it(\'finds on partial cart ref\', function() {
>
> var result: any\[\];
>
> result = \$filter(\'cartFilter\')(cartItems, \'World of Warcraft\');
>
> expect(result.length).toBe(1);
>
> });
>
> });

These tests can also be nicely specified using the earlier-described
Gherkin format. If we develop an AngularJS-based front end, we can
describe the front-end tests in JavaScript using Protractor, and we
simply consume the results and integrate them in TFS. Suppose we want to
run cross-browser tests ("does our software function properly in various
device/OS/browser combinations?"). Then we can also use a Selenium grid
to spawn the different combinations, which means we are able to
interpret the results within our pipeline.

The test results from each of the test tasks in the build pipeline are
examined to determine whether the software is of sufficient quality to
promote to a next phase. Only software that is properly unit-tested can
be offered for component-testing or integration-testing, allowing these
tests to focus entirely on the component or the integration between
components, rather than on the low-level functionality. We should now
have that one covered. As such, the build pipeline is a clear
implementation of the above-mentioned agile test automation pyramid and
test strategy. Table 1 shows a specific example of the test strategy,
namely the implementation of the various test types. By including a
current (actual) and end state (desired), test improvements can be
managed and measured promptly.

+----------+--------------+------+----+--------+-----------+--------+
| Test     | Description  | Cur  | E  | Dev    | Respo     | Mainly |
| type     |              | rent | nd | eloped | nsibility | per    |
|          |              | s    | s  | in     |           | formed |
|          |              | tate | ta |        |           | by     |
|          |              |      | te |        |           |        |
+==========+==============+======+====+========+===========+========+
| Unit     | Tests the    | Code | Co | C#,    | Team      | Deve   |
|          | smallest     |      | de | Type   |           | lopers |
|          | pieces of    |      |    | script |           |        |
|          | testable     |      |    |        |           |        |
|          | code         |      |    |        |           |        |
|          |              |      |    |        |           |        |
|          | -   C#       |      |    |        |           |        |
|          |     (RESTful |      |    |        |           |        |
|          |              |      |    |        |           |        |
|          |    services) |      |    |        |           |        |
|          |              |      |    |        |           |        |
|          | -            |      |    |        |           |        |
|          |   Typescript |      |    |        |           |        |
|          |              |      |    |        |           |        |
|          |  (front-end) |      |    |        |           |        |
|          |              |      |    |        |           |        |
|          | -   RDBMS    |      |    |        |           |        |
|          |     (if      |      |    |        |           |        |
|          |     needed)  |      |    |        |           |        |
+----------+--------------+------+----+--------+-----------+--------+
| Int      | Tests the    | N/A  | Co | C#     | Team      | Deve   |
| egration | interaction  | (no  | de |        |           | lopers |
|          | between      | te   |    |        |           |        |
|          | components   | sts) |    |        |           |        |
|          | (service     |      |    |        |           |        |
|          | layer        |      |    |        |           |        |
|          | database     |      |    |        |           |        |
|          | layer, or    |      |    |        |           |        |
|          | front-end    |      |    |        |           |        |
|          | layer        |      |    |        |           |        |
|          | service      |      |    |        |           |        |
|          | layer)       |      |    |        |           |        |
+----------+--------------+------+----+--------+-----------+--------+
| C        | Tests        | N/A  | Co | C# OR  | Team      | Te     |
| omponent | independent  |      | de | Type   |           | sters, |
|          | components   |      | (B | script |           | Deve   |
|          | of a larger  |      | DD |        |           | lopers |
|          | system,      |      | f  |        |           |        |
|          | e.g.:        |      | or |        |           |        |
|          |              |      | v  |        |           |        |
|          | -   API      |      | ie |        |           |        |
|          |     (RESTful |      | w) |        |           |        |
|          |     service) |      |    |        |           |        |
|          |              |      |    |        |           |        |
|          | -   The view |      |    |        |           |        |
|          |              |      |    |        |           |        |
|          |  (front-end) |      |    |        |           |        |
+----------+--------------+------+----+--------+-----------+--------+
| En       | Test whether | Ma   | M  | C# OR  | Team      | T      |
| d-to-end | an entire    | nual | TM | Type   |           | esters |
|          | integrated   | (u   |  + | script |           |        |
|          | system meets | sing | B  |        |           |        |
|          | its business | MTM) | DD |        |           |        |
|          | goal         |      |    |        |           |        |
+----------+--------------+------+----+--------+-----------+--------+
| Exp      | Manual       | Ma   | M  | Manual | Team      | T      |
| loratory | testing      | nual | TM | test   |           | esters |
|          | involving    | (u   |    |        |           |        |
|          | continuous   | sing |    |        |           |        |
|          | learning     | MTM) |    |        |           |        |
|          | based on     |      |    |        |           |        |
|          | application  |      |    |        |           |        |
|          | feedback     |      |    |        |           |        |
+----------+--------------+------+----+--------+-----------+--------+

: Table Overview of test types, where tests are implemented (current
state and end state), the programming language, and responsibilities.

If we have a pipeline using TFS and VSTS Build that runs all tests for
the various test forms as quickly and as often as possible, we can
constantly interpret the results and make the right go-live decisions.
Possibly, this can be split across multiple pipelines: a "build
pipeline" for unit tests and a "release pipeline" for end-to-end tests.
Ultimately, we know that we've built the right product right.

**Summary**

Organizations should place their testing activities at the center of
their development efforts. Only then can development teams obtain the
feedback they deserve and need to deliver high-quality software.
VSTS/TFS Build and Release enables organizations to incorporate the
automated tests to cover the required test forms. What's more, it allows
for seamless integration with test tools from other ecosystems by
standardizing test output formats. This allows all technologies to be
injected into the pipeline, which offers new possibilities to test apps
and speed up time to market.

[^1]: Please see the following blogpost for more details:
    <http://xpir.it/mag3-testing1>
