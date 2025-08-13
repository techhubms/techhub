Continuous delivery is a hot topic in the current world of software
development, especially for web applications. Most teams and companies I
know of are starting to experiment with it or are in the process of
implementing continuous delivery. In my role as consultant I get to see
a lot of different companies and their ways of working. A lot of
companies have some automated testing and automated deployments to
certain environments in place. However, when you look at mobile
application projects, you often don't find any sign of continuous
delivery.

Setting up some basic principles of continuous delivery for mobile isn't
that hard, although there are some extra hurdles for mobile applications
compared to web applications, for example the deployment process towards
app stores. The first steps in implementing continuous delivery could
consist of setting up continuous integration with automated builds and
unit tests running at every code check-in. Another step could be to set
up automated deployments towards private stores (with tools such as
Hockeyapp), so your test team always has a new, daily version of the app
on their test devices. This article is not about setting up the basics
of continuous delivery which we call continuous delivery 1.0. Instead,
it is aimed at creating a new mindset about continuous delivery. Welcome
to the world of continuous delivery 3.0.

# What is continuous delivery 3.0?

Continuous delivery 3.0 is a term my Xpirit colleague Rene van
Osnabrugge came up with to define the next level of continuous delivery.
It's hard to say exactly what continuous delivery 1.0 or 2.0 is.
However, what we mean to say with CD 3.0 is that when you really want to
get to the next level of continuous delivery, you have to rethink
certain parts of your software delivery pipeline instead of just putting
some automated deployments in place. This article covers three areas you
might want to rethink when implementing continuous delivery for mobile
apps.

![](./media/image1.png)

## 

# Rethink testing

Every developer will agree that proper automated testing is important in
all software development projects. However, it may be even more
important in mobile app projects than in web projects. On the web it's
possible to implement a fast update in the application on your web
servers, but with native mobile apps you'll have to go through app store
submissions and reviews, and this can take up to several days. What's
more, competition within the app stores is huge and it's easy for users
to just open the store and download the app from your competitor if your
app contains bugs or doesn't work properly. If you lose users on a
mobile platform, it's really hard to get them back. They are also very
likely to leave 1-star ratings and never return after switching to a
competitor.

## Automate everything

To be able to maintain high quality levels at every moment in time,
automated tests are key. Mike Cohn developed the concept of the agile
test pyramid which describes how to create a properly balanced set of
tests. The key point of the test pyramid is that you need to create a
large foundation of unit tests that can run fast, as well as a much
lower number of end-to-end test cases that run through the GUI.

![](./media/image2.png)


Structure your application so you can mock or stub all internal and
external dependencies in your code and build unit tests to cover all
code paths within your solution. The unit tests form a base of tests
that you should be able to run as often as possible. This is why they
need to be fast, and with fast I mean being able to run hundreds of
tests within a few seconds. If these kinds of tests take too long,
people will stop running them and the goal of these tests is lost.

In addition to the unit tests there is a set of component tests that
connect several units together, as well as integration tests and API
tests which test all server side functionality of your mobile apps.

Setting up a clear API layer is key for your architecture and this
splits up testing responsibility between your mobile app layer and
back-end layer of the application. The APIs form a contract of
communication between these two layers and if you test the APIs by
themselves, it should be possible to release them separately. By having
a good API stub framework, the automated mobile app tests only have to
communicate to these stubs instead running tests from the UI to the
back-end system.

The mobile device landscape involves an enormous amount of complexity
with different types of operating systems, OS versions, screen sizes,
processor and memory differences. Manual testing of all these device
combinations is impossible. Solutions such as Xamarin Test Cloud can
help you by providing a cloud farm of thousands of devices, running all
possible combinations of operating systems and versions. Tools like this
integrate with your continuous integration builds, so you can run these
tests with every automated build.

These methods of automated testing allow you to focus your manual
testing efforts on edge cases which are hard to automate. Think of
scenarios like receiving a phone call while running your app, or
suddenly losing reception.

## Automating your current tests is not enough

While automating your test effort is a good start, it isn't actually
rethinking your test strategy. Rethinking your test strategy means that
you need to make a shift to focus on quality earlier in the software
development process instead of only reducing the test effort.

This means that the role of your testers is going to change. There is no
room for testers who write test scripts based on specifications or who
try to break your application by monkey testing.

![](./media/image3.png)


Making a shift to an earlier phase of your development process is
important in order to have high-quality code at every moment of the
cycle. Instead of writing test scripts, testers should be helping the
business with writing testable requirements that can be used in
automated test scripts. Popular ways to do this are ATDD (Acceptance
Test Driven Development) or BDD (Behavior Driven Development). Below is
an example of a BDD specification written in the Gherkin language. Tools
such as Specflow can turn these specifications into automated tests. It
is possible to combine Specflow tests and the Xamarin test cloud to have
your requirements tested automatically on thousands of devices.

+-----------------------------------------------------------------------+
| Feature: Login                                                        |
|                                                                       |
|   In order to access my account                                       |
|                                                                       |
|   As a user of the website                                            |
|                                                                       |
|   I want to log into the website                                      |
|                                                                       |
| Scenario: Logging in with valid credentials                           |
|                                                                       |
|   Given I am at the login page                                        |
|                                                                       |
|   When I fill in the following form                                   |
|                                                                       |
|   \| field \| value \|                                                |
|                                                                       |
|   \| Username \| xtrumanx \|                                          |
|                                                                       |
|   \| Password \| P@55w0Rd \|                                          |
|                                                                       |
|   And I click the login button                                        |
|                                                                       |
|   Then I should be at the home page                                   |
+=======================================================================+
+-----------------------------------------------------------------------+

Combining these kinds of tests with the automated unit tests create a
stable level of quality right from the start of the development process
up to the first release, as well as future releases.

# Rethink releases

If the quality of your code is measurably high at all times, it should
be possible to deploy the application at regular intervals. However,
deploying is totally different from releasing. For more details, please
read the article by Marcel de Vries on separating deployments and
releases, also published in this magazine. Mobile apps have a number of
extra difficulties regarding releases. When you create a public app in
the stores you'll have to go through the official review cycles by
Apple, Google and Microsoft, and this can take up to several days. This
makes quality in your app even more important because there is no way to
quickly release bug fixes as they have to go through the same review
cycle.

## Public beta releases

A lot of companies I visit use tools to distribute the apps that are in
development to their testers. The most common tool for this job is
Hockeyapp. Hockeyapp works great for test app distribution and is easy
to integrate into automated build or release tools such as VSTS. When
you're rethinking releases you will end up trying to release as often as
possible and also make early releases available to end-users. Hockeyapp
can support such beta releases. However, it is not designed for large
public beta tests because managing large sets of users takes a lot of
manual effort. Apple, Google and Microsoft all have features for
releasing beta apps to groups of users but they all involve a number of
disadvantages: Apple still requires you to go through a review cycle
that can take several days and a beta release only has a lifetime of 60
days.

Another option would be to publish two apps to the stores, one with a
beta tag and one without. This option is valid for Android where you
could do early releases to the beta version in the store and use the app
without a beta tag for a more formal release at a slower pace. However,
this is not an option for iOS because Apple forbids adding beta versions
of apps to iTunes. So is there any other way to add beta functionality
to releases that are released to the store?

## Dark Launching & Feature toggles

Feature toggles are an option that allows you to integrate features into
your app without enabling them immediately when you technically release
an app. When you create feature toggles, you start with adding a
configuration option to enable or disable a new feature without
implementing the feature in your code. You should be able to release the
app as long as the configuration remains disabled. Now you can implement
the feature and switch the feature on as soon as it is ready to use.

If you can control the switches of the app from a central location, this
can be extended even further by letting your business users decide which
features will be enabled. There are several tools in the market that
help you with the management of these feature toggles to be able to dark
launch your app's features. Many of these tools also have features that
allow you to enable new features to only a small set of users in order
to see the impact of the feature before releasing it to everyone.

# Rethink analytics

Having health monitoring in your apps seems obvious. There are several
tools to implement this and every customer I visit seems to have some
basic health monitoring installed. Rethinking analytics is thinking
about the extra value analytics can offer you.

## Functional insights

The first step in rethinking analytics is to start using the analytics
features to only see health checks. It can be really useful to see which
pages are used most, which features are used most often, or the keywords
your users are using for searching within your app. Most mobile
analytics tools are able to do this and it can give you valuable
insights that will enable you to improve your app.

An even more advanced scenario would be that you set up your analytics
in such a way that it measures differences between certain functionality
and automatically switches feature toggles when errors occur, or that it
measures decreased use of certain features after enabling a particular
feature toggle.

Data is driving more and more decisions and that's why you never want to
throw away any data. The same goes for your analytics data. Combining
the functional analytics of, for instance, the moments when your app is
used most frequently, the most popular features and other things that
can be traced with your company's big data can lead to new insights.
Machine learning is becoming available for almost everyone and finding
correlations and making predictions is becoming easier and easier. Find
ways to do a continuous export of your analytics data to your big data
environment and get insights that you never thought of before.

# Conclusion

Software quality is even more important in mobile projects than on the
web because of the releases through the store, the wide diversity of
devices and competition of other apps. Setting up a good continuous
delivery process will increase your overall software quality and will
enable you to release more frequently and with less risk.

Taking the first steps should be easy. There are lots of guides for
setting up continuous integration builds, running automated tests and
doing automated deployments to tools, for instance Hockeyapp. On my blog
<http://mobilefirstcloudfirst.net> I've written a couple of guides on
how to set up automated builds, tests and distribution to Hockeyapp for
your mobile apps using VSTS to get you started with continuous delivery
1.0. Hopefully this article has inspired you to think a bit further than
just these basic steps and will help you to evaluate your current
development lifecycle with new ideas on how to rethink your testing,
releasing and analytics steps to move more to continuous delivery 3.0.
