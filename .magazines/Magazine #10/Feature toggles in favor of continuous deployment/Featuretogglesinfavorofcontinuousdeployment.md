# Why you want feature toggles and what type of toggle to use

Feature toggles or feature flags are techniques for hiding feature
implementations from your customer until you want your customer to
experience your new feature. This technique helps to overcome all kinds
of challenges during your software development. There are many different
types of feature toggles and different ways to implement them. Depending
on whether you build your own toggles or use a framework, a number of
toggles will be provided out of the box. Many frameworks also support
your custom implementation of a feature toggle. However, before using
and building your own toggles, it is essential to understand a number of
possible scenarios and when to use a particular toggle. This article
will provide insight into a variety of toggles and how to choose the
most suitable toggle for your requirements.

## Feature toggle categories

Feature toggles come in different shapes and sizes, and the
implementation can be anything you want it to be. You can categorize
your toggle in one of the following categories, each of which has its
distinct purpose and criteria;

-   **Release**, this toggle can change per release and usually has a
    life span of days to weeks. Once the code is deployed, you don't
    want to flip it because it might activate unwished code or
    unexpected behavior.

-   **Operations**, this toggle can be anywhere from short- to
    long-lived, often managed by an operations group in order to have a
    way to control software in a tested and controlled manner.

-   **Experiment**, this toggle is short-lived, and due to the nature of
    experiments, you don't want it to be activated too long because it
    will distort the results of your experiment with other code changes.

-   **Permission**, you\'re probably already using this toggle to
    provide access to a cohort or targeted user to use a closed-down
    part of your software. The life span can be anywhere from short- to
    long-lived.

It is important to understand what you want to do with your toggle and
what category it belongs to. This will help you to understand what you
are implementing and why you are implementing a specific piece of code.

The following overview shows the various categories with examples of
some of the most frequently used implementation types.

![A screenshot of a cell phone Description automatically
generated](./media/image1.png)


## Separating deployment from release

Although you are in full control of everything required to develop your
application in an ideal situation, the reality is that quite often
you're dependent on other teams, products, or schedules. The way we
manage this traditionally is to have meetings and agreements with all
teams and products involved. We agree upon a plan to deliver a product
to our customers when everything is finished, tested, and 100% ok.
Despite the fact that this looks great on paper with some helpful "Gantt
charts\", it could not be further from the truth. Not only are we humans
bad in planning complex work, but our environment is also changing
continuously, and we might have time to develop this new feature now,
but in two weeks' time something else is likely to be more important.
All this time, the constraints you have with other teams, products, and
schedules need to be managed, and this time cannot be used to develop
more new and exciting features.

By separating the deployment of features from exposure to your
customers, you can mitigate the dependencies between the technical part
and the business part; i.e. between the technical phase of deploying and
shipping the product, and the business phase of exposing and releasing
new features. By separating these phases, you enable yourself to keep
developing and deploy functionality independently from other teams or
products because it is hidden from your end-users.

The functionality of an ON/OFF toggle is a typical operation toggle that
gives you the ability to enable or disable the feature, and would be a
simple way to implement. While developing your functionality, you can
use the ALWAYS-OFF toggle from the release category because you don't
want your feature to be activated by accident. But when your part of the
work is finished, use a more dynamic toggle to activate it when the
business is ready for it.

## Code branch management

When you work on features, there can be quite a time gap between start
and finish. During the development phase we are often tempted to create
a separate branch that allows us to develop in isolation. Branching
protects you from changes by others or other teams because they 'won't
reflect into your branch until you update it. However, the biggest
pitfall is that you are also delaying yourself from receiving feedback
on your changes. You especially want constant feedback on big and
complex changes that take a lot of time. And you certainly don't want to
wait until the end, when you finish your significant changes and realize
the product has changed too much to easily integrate it back into the
master version.

Using feature flags allows you to wrap the new application functionality
that is under development. Such functionality is "hidden" by default.
You can safely ship the feature, even with the unfinished work, because
it will stay dormant during production. By using this approach, called
"dark launch," you can release all your code at the end of each
development cycle. You no longer need to maintain any code branch across
multiple cycles because of the feature taking more than one cycle to be
completed. Just keep working on the master following trunk-based
development together with your team.

A complex branching strategy is often over-engineered with only a few
people that truly understand it. Prevent merging issues and follow the
continuous integration principle by continuing to work on the master
branch instead of hiding your work in a separate branch. Use an ALWAYS
ON/OFF toggle until you are finished so you can keep in sync with your
colleagues and enable fast peer feedback by exposing your changes.

## Testing in production

When you develop a new feature, you want to shorten the feedback cycle
from your customer as fast as possible. Knowing that you are building
the right thing right is an advantage you get with short development
cycles and fast customer feedback. You can start to experiment and
validate the hypothesis you think your customers want.

Feature flags allow you to grant early access to new functionality in
production. For example, you can limit the access to only development
team members or some internal beta testers. This technique is called
"Ring-based deployment," and provides users with the full-fidelity
production experience instead of a simulated or partial one in a test
environment. It gives you the much needed and fast feedback without the
need for a production-like environment. After all, this is always a lot
of trouble to set up and maintain, especially when you are handling
personal data and need to be GDPR-compliant, which is a struggle many
organizations are facing these days.

Use a PERMISSION TOGGLE implementation to combine the newly developed
feature with your claims system to grant specific groups access to the
functionality being developed quickly. First set it for your development
team and later extend it to early adapters.

## Flighting

After weeks of developing, testing, and validating our test
environments, the time finally arrives to release the world-changing
feature to customers. To obtain full exposure, your marketing team sent
an email to your customers so they can all try out your new feature.
This scenario is not uncommon in the industry. However, all too often
the result consists of unresponsive webpages or customers facing long
queuing or no experience at all. Correct estimations of the number of
customers hitting your new feature and the required underlying resources
are hard to make. And we 'don't want to show up on the news with
negative publicity.

By using a flighting mechanism, you can incrementally roll out new
functionality to your end-customers. Start by targeting a small
percentage of your user population and gradually increase that
percentage over time, after you have gained more confidence in the
implementation and the use of your feature. When something goes wrong,
only a small part of your customer base would be affected, and you can
monitor the capacity of your resources closely while ramping up the
number of customers.

A typical flighting lifecycle starts as an ALWAYS OFF toggle. When the
feature is complete and ready to be exposed to your customers, you can
either use a PERMISSION toggle, if you want to control who gains access,
or a PERCENTAGE toggle which you can ramp from 5% to 100% in as many
steps as you feel comfortable.

A BIG BANG can turn out painful!\
Continuous Deployments ease the transition.

## Instant kill switch

Your feature is available to your customers, and suddenly an unexpected
behavior arises that is costing the enterprise money every second it is
enabled. The error could come from a part you control or a dependency
upon another service. For example, a payment service that you are
requesting for different payment options from a specific bank is
returning status 500 and giving your customers no option to pay as a
result.

When you are dependent upon externally controlled services, it is
advisable to think about how you want your application to react when
that service is down or behaving unexpectedly. After all, they are just
another product like your own, and mistakes and unwanted responses will
occur. But even when your application is entirely under your control,
you want to think about how you want your application to react in case
of errors. For example, when the authentication service of Netflix is
down, they grant access to everyone instead of blocking everyone. From
their perspective, they made a mistake, and the customer should not have
to pay for it, which could also lose them business and reputation.

Although the toggle implementation is just a simple ON/OFF toggle, the
mindset and duration of that toggle are different compared to the
"Separate Deployment from Release." This toggle is an operational toggle
intended to be controlled manually when production needs it. A potent
addition to this toggle is to combine it with a circuit breaker
strategy. ![](./media/image2.png)
\
Whenever a dependency returns an unexpected response within a timeframe,
the circuit breaker will trigger, i.e. toggle the feature toggle. Doing
this will prevent your application from being affected by errors from
the external service, and your customers will enjoy your service even
though the service will probably show a certain level of degraded
performance or lesser functionality, but to prevent it from being
offline. In addition, you can give the remote service some breathing
space to recover or start-up additional instances without being hammered
constantly by your application. Every X-amount of time, the circuit
breaker will try one or two requests to check whether the external
service is recovered. When the result is positive, the circuit breaker
will close, meaning the toggle will flip again, and all traffic will
flow to the external service, once again enabling full functionality for
your customers.

## Selective activation

Think of a scenario in which you are developing a new feature for every
web browser. A custom implementation is needed and your feature is
dependent upon that implementation to work correctly. You prioritized
Chrome to be the most important and want to expose it to be enabled, but
only for customers with the correct version of Chrome.

The focus on one implementation allows you to receive production
feedback fast and quickly for the part of your feature that is finished.
Moreover, you can expand the list of supported browsers in the future.
You could also use this to inform your customers of the browsers that
you support with additional functionality.

Implementation-wise this toggle will validate whether the 'visitor's web
browser version occurs in a list of values stored in the feature toggle.

## Life-span

While there are all kinds of toggles, you can use it in different
scenarios. The life-span can vary from short-lived to very long-lived.
One thing that all toggles have in common is the way you implement the
toggle and that you eventually need to remove the toggle.

"Savvy teams view their Feature Toggles as inventory, which comes with a
cost, and work to keep their inventory as low as possible."\
Martin Fowler

![](./media/image3.png)


We start with our original code, and we decide that we need a feature
toggle. Next, we create the toggle, we think about where to place it on
the correct level and implement it as an always off toggle. Now,
everything is set up to hide the development of your new feature, and
you can start building it. Eventually, you can change your ALWAYS OFF
toggle to become an ON/OFF toggle and flip the switch so people can
experience your new feature. When your feature is running the way it
should run, and it has not been turned off recently, you have your
ALWAYS ON toggle, and now you can start removing the old feature
resulting in the removal of your feature toggle.

## Monitoring

In addition to the technical implementation, you want to monitor your
application and the usage of your feature toggle. Every time you enable
a feature, you should treat it as a deployment. Have an increased
awareness of exceptions and watch your monitoring closely to identify
unwanted behavior. You want to know whether people are using your new
feature and how many people are using it. If nobody is using your
feature, it will not send any exceptions either.

The absence of errors is not good enough

![A close up of text on a white background Description automatically
generated](./media/image4.png)


In the diagram above, you can see that the initial deployment enabled
our new feature, and we see an increase in the number of users, limited
to a particular control group. Upon finding a bug, we close the feature
down to solve it. We fix the bug and re-deploy it and see the same
control group as an increasing activity. When we feel more confident, we
extend the feature to be available to more customers. Eventually, the
toggle is running in production for a time, and you remove the feature.

# Smells and pitfalls

The opportunities provided by feature toggles make you think that this
must be a silver bullet! Well, not exactly. Feature toggles are a means
to an end and not a goal in itself. By not understanding the use and
complexity of feature toggles, you could do serious harm to your
product. The following section contains an unordered set of smells and
pitfalls that can help you recognize and understand the risky situations
we experienced during our years of development.

**Too many feature toggles**\
It is difficult to explain exactly what number is too many, but this is
a sign that something is not right in your understanding or
implementation. The amount can differ per product, team, and experience.
Keep track of all your toggles and keep the number limited.

**Fine-grained feature toggles\
**Fine-grained toggles give you a lot of control but also give you many
toggle combinations to test alone and in combination. Keep toggles on
entire features and keep them simple. The fewer combinations you need to
validate, the less prone you are to making errors.

**Toggle on technical capabilities instead of business process\
**Feature toggles should be easy to translate to business processes and
capabilities. When you are implementing technical toggles, they should
end up in the hands of your operator, or they should be very
short-lived.

**Same toggle used in multiple places\
**Toggles that are used in multiple places and that don't have having a
single place of entry could indicate that you need to place your toggle
on a higher level, or maybe you need a strategy pattern [^1]to inject
behavior. This helps you to keep a clear overview of where a toggle is
used and reduces the number of code paths.\
![](./media/image5.gif)


**Forgetting to describe what your toggle does\
**A description like: "This toggle routes traffic to the new score
calculator engine, when the toggle is off, the old legacy one will be
used and can cause latency bugs to appear." It will work so much better
than a description like **ft-calc-engine.**

"A good convention is to enable existing or legacy behavior when a
Feature Flag is Off and new or future behavior when it's On."\
Martin Fowler

**Toggles are technical debt\
**All toggles are by nature technical debt and should be treated as
such. They should be removed when they are no longer needed. The removal
of toggles is a continuous part of refactoring your code and the cost
you are paying for using feature toggles.

**Launching blindly\
**Launching blindly is nothing more than throwing your application over
the fence. Even when you deploy, turning a toggle on or off without some
kind of monitoring is irresponsible, and you won't know whether the
deployment succeeded..

**Unseparated Control\
**When you manage your toggles from the same product you are
controlling, toggle management might be unresponsive when a toggle
enables and is harmful to memory or CPU. However, you cannot turn it off
now.

**Long-lived toggles**\
By nature, long-lived toggles present technical debt and should be
removed. The longer a toggle is in your system, the higher the risk of
combining multiple toggles, adding complexity to your code path.

**Re-using a feature toggle\
**A **feature toggle 'shouldn't be re-used**, it's a one-time
implementation with history and auditing. Once it is refactored out, you
should not re-use the name because this would only create confusion

# Conclusion

In this article, I have tried to provide an overview of a variety of
scenarios for applying feature toggling. There are many more scenarios
and they often involve chaos engineering (i.e. the discipline of
experimenting on a software system in production in order to build
confidence in the system's capability to withstand turbulent and
unexpected conditions). Although it is almost a textbook explanation of
how to use feature toggles, I often see this technique misused and thus
it undermines the confidence of the team and product owner. As a result,
they maintain the status quo of releasing once a month or even less
frequently. I deliberately stayed away from the technical
implementation; this depends heavily on the framework you choose to work
with or build your own. If you want to have a starting point, you can
take a look at "[Azure Application Configuration Feature
management](https://docs.microsoft.com/en-us/azure/azure-app-configuration/concept-feature-management)[^2]."
This gives you a number of out-of-the-box toggles, local use of feature
toggles, as well as a good cloud platform to run it for you". The
take-away of this article is that feature toggles are business-driven,
allowing you to separate the use of functionality from its technical
deployment. Using the mantra, "A BIG BANG can turn out painful!
Continuous Deployments ease the transition " Use this to your advantage
and use the 'smells and pitfalls' to help you recognize a hard to
maintain set-up or an error-prone method.

[^1]: <https://en.wikipedia.org/wiki/Strategy_pattern>

[^2]: <https://docs.microsoft.com/en-us/azure/azure-app-configuration/concept-feature-management>
