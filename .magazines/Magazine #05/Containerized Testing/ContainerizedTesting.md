# Containerized Testing

Many organizations nowadays are implementing continuous delivery
practices to accelerate their time to market. An important part of
continuous delivery is automated testing. However, a lot of companies
are still struggling with how to do this in an effective way. Although a
lot of test automation practices have appeared over time, the effort and
time required for testing is still a bottleneck for many organizations.
In this article you'll learn how the power of containerization can be
leveraged to shorten your feedback cycles, reduce testing effort, and
accelerate your time to market.

## How automated testing is done nowadays

Containerized testing is a new practice within the space of automated
testing. Before we look at what containerized testing has to offer in
addition to other test automation practices, let's briefly look at
commonly used practices nowadays.

![](./media/image1.png)


Figure 1 - Common test automation practices

A lot of current practices have to do with the implementation of your
tests. One well-known practice in the domain of test automation is the
Test Automation Pyramid[^1]. This pyramid describes that you implement
your automated tests at the lowest pyramid level possible. The lower the
level in the pyramid, the more autonomous, granular and maintainable
your test cases are, and the faster your test execution. Other
successful practices related to test implementation include the Page
Object Pattern and Screenplay Pattern for creating maintainable UI
tests.

In addition to implementation-related practices, process-related
practices also appeared to be very suitable for automated testing. For
instance, Acceptance Test Driven Development (ATDD) enforces development
teams to create automated acceptance tests before actually coding the
functionality. To deal with tests that can't be automated, as the Test
Matrix[^2] tells us, the practice of "Testing in production" was
introduced to shorten time to market by executing those tests in
production. For example, instead of executing Usability Tests as part of
the delivery process, A/B testing can be used to execute this type of
test in production.

Thanks to the above-mentioned practices, a lot of organizations are able
to reduce the time required to deliver new functionalities into
production. But if we look at the time, effort and costs that are needed
to set up and manage a testing infrastructure, it is remarkable to see
that only a few practices exist in this area, for example DTAP. Looking
at the operational costs and realizing that the testing infrastructure
is idle most of the time, wouldn't it be better to have the testing
infrastructure on demand? Luckily, we found a solution that can help us
fill this gap. We call it containerized testing.

## The shift to containerized testing

Many of you may already have discovered the power of containerization
for your applications. Think of benefits such as scalability, freedom in
hosting, immutable images, etc. So why not leverage the same benefits
for our testing infrastructure. Wouldn't it be much easier to just set
up your test infrastructure only when you need it?

### What is containerized testing?

If we look at how a testing infrastructure is set up today, you will
most likely have one or more dedicated, pre-provisioned test
environments to support the execution of different types of automated
tests.

Typically each test environment can only run specific types of tests.
For example, performance tests are often executed on an acceptance test
environment. Test or acceptance environments are often also shared
across teams, which means that you will have to wait for that
environment to become available. An example environment for running
integration tests could look like the environment shown in figure 2.

![](./media/image2.png)


Figure 2 -- Traditional setup of test infrastructure for a test
environment

Figure 2 shows a fictitious system that is being tested. This system
consists of a registration service, an email service, and a user
service. The test agent is responsible for executing the automated
tests, replacing an external dependency with a stub, and preloading a
database used by the user service. Depending on the type of test, the
database could be preloaded with test data to support different
scenarios.

With containerized testing we do the same as we do with containerization
of our applications, i.e. the environment becomes part of the test
deployment. For every test we set up the required containers and
configure the required environment. This is also known as
configuration-as-code and infrastructure-as-code. By doing this, test
environments are no longer a physical thing, but they become blueprints
containing the various containerized components that we need to put
together to execute a specific type of test. This concept of blueprints
is referred to as environment-as-code.

For example, you will have different environment blueprints for
security, performance, integration, and end-to-end testing. The nice
thing about this approach is that we can set up a given environment
on-demand, depending on the type of test we want to execute. Figure 3
shows an example of a containerized test environment, the same
environment as we had in figure 2. However, it uses containers for the
application under test and all other test infrastructure required to run
the tests.

![](./media/image3.png)


Figure 3 - Test environments in a containerized testing world

Being able to set up isolated environments means that there is no real
need for Development, Test, Acceptance and Production environments
(DTAP) anymore. Instead, we can think in stages and use the quality
gates within each stage to assess the quality level of our application
and its components. In that case, we might actually say that DTAP is
dead. Using the concept of environment-as-code, we can simply define an
environment for a certain quality gate, for example performance test,
set up that environment, and execute the required tests.

As we pass more and more quality gates, we are building up the trust
required to actually run the application in production. In theory the
order in which we pass the quality gates before production doesn't
really matter. We could even run a few of them in parallel. But it is a
good idea to consider the time tests take to run and what they actually
test. For example, there is no real need to run relatively slow UI tests
if your most basic smoke tests already fail. This might be conceptually
similar to what you would do in your DTAP environments today, but you
are no longer limited by the number of environments available or by the
type of test you can run on a particular environment. This means you can
focus on how to get rapid feedback and how to fail fast.

![](./media/image4.png)


Figure 4 - Stages and quality gates instead of DTAP

### How to get started?

Before you can use the concept of containerized testing, the application
you are testing needs to be containerized. Actually, not only the
application you are testing must be containerized, all other components
required to run the specific test must also be containerized. And you
will require some sort of test agent or test container that executes the
tests for you. For UI testing you will need a container that runs
headless UI tests, and for your test data you will need a database
inside a container with the test data pre-loaded as a snapshot. If you
connect to external services, it is a good idea to have stubs inside
containers that can replace this dependency when you run your tests.
There are many more examples of test components you can come up with,
but the bottom line is that you will have to make sure that everything
you require to run your tests is available in your containerized test
environment.

Now that we have containerized the components of our test environment,
we need to describe the set-up of the various test environments using
the concept of environment-as-code. For example, we can use Docker
Compose to define blueprints of the test environments that will be
deployed in isolation. Docker Compose supports the use of multiple
compose files that complement or overwrite each other. This means that
we can have a main Docker-compose.yml file for our application, and a
Docker-compose.integrationtests.yml file that adds the specialized
testing container(s) and that (re)configures services to connect to the
stubs instead of a real external service. We then tell Docker Compose to
set up an isolated environment using the combined configuration of these
two files. Moreover, we can have multiple combinations of compose files
that configure different types of environments to run different types of
tests.

![](./media/image5.png)


Figure 5 - Using multiple compose files to run your tests

### Benefits

**Partial test environments**

One of the major benefits of containerized environments is the ability
to set up partial environments. A partial environment means that you
only have to include the services and test components required to run a
specific type of test instead of your entire environment, for example
only an API service and a back-end service.

**Isolation**

The containerized testing approach means that you can truly test your
environment in isolation. There is no need to make your services
accessible from the outside, because the agent running your tests is
just another container running in the same environment as your services.
For most types of tests there also is no need for your services to
communicate with the outside world because your services talk to stubs
instead of to the actual external services, and these stubs run in the
same environment as your services. What's more, you don't have to worry
about other running containers interfering with your test environment,
unless you have explicitly configured it that way. Because you run your
test agent within a container, you need a CI/CD orchestration tool (e.g.
VSTS) to start your containerized testing environment. If you make use
of Docker Compose, you can use the Docker compose up command to start
your containers and the actual test execution.

![](./media/image6.png)


Figure 6 - On-demand and parallel test environments

**On-demand parallel test execution**

You can create your test environment exactly when you need it. You are
now able to deploy your isolated environment on any container host and
run multiple types of test in parallel, because you can set up an
environment for each of them. You can now consider running security
tests, performance tests and, for example, UI tests all at once. You
don't have to wait for environments to become available so the overall
execution time of your tests will decrease significantly.

**Reduced costs**

Using the container infrastructure, you can set up as many environments
as you want. There is no real need for dedicated, pre-provisioned
testing environments any longer; you can create a set of containers on a
container host and run specific tests against them. Once you are done
with the tests, you no longer need the environment, so it can be
destroyed. This means that you don't have any pre-provisioned
environments that are idle most of the time. You still do need a
container host, but you can utilize the available resource more
efficiently than you ever could with separate pre-provisioned test
environments.

## Conclusion

There are already many practices available that cover different aspects
of test automation. However, there are only few practices available for
infrastructure when it comes to automated testing. Leveraging the same
benefits that you get from containerizing your application for test
automation allows you to look at infrastructure for test automation in a
whole new way. The concept of a test environment changes from a
pre-provisioned set of servers to an on-demand environment that contains
everything needed to run just that specific type of test. This also
changes the way you think about executing your automated tests. You
don't have to think about DTAP anymore; instead, you can think about
which types of test (quality gates) you need to run in order to build
confidence to move to the next stage. Instead of thinking about which
test has to run in which environment, you can think about which tests
provide the fastest feedback.

Containerized testing is a great opportunity for organizations that are
starting or thinking about containerizing their application stack.
Containerizing your test infrastructure in addition to your application
stack, speeds up your feedback cycles and accelerates your time to
market.

[^1]: [https://xpir.it/](https://www.mountaingoatsoftware.com/blog/the-forgotten-layer-of-the-test-automation-pyramid)xprt5-container-testing1

[^2]: https://xpir.it/xprt-container-testing2
