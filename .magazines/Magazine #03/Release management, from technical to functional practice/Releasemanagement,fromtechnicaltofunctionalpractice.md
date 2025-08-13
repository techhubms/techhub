# Release management, from technical to functional practice

Traditionally deployment and release of software have been synonymous to
each other. As a result, the IT department is in charge of the time of
release, and decides when the business can release a feature to the
public. Separating the concerns of release and deployment allows you to
improve the performance of continuous delivery pipelines and also
empower the business to release features when they want to, without the
involvement of IT. In this article I will show how you can apply the
concept of separation of concerns in your continuous delivery strategy,
specifically targeted at the notion of releasing and deploying your
software to production. This enables faster deployment cycles and
empowers the business to release features without the need to involve
IT.

## The difference between deploy and release

Let's start with the definition of a software release and what
deployment entails. Releasing software or features is defined as
exposing the software or feature to the end-user of the system. So this
is the first moment the software or feature becomes available to the
end-user. Deploying software includes installing the software on an
environment (one or more machines in some kind of coherent
configuration) to make it potentially accessible to the end-users. Based
on these definitions you already can see why this has been something
that has been done hand in hand at the same moment in time, since it all
has to do with exposing new software to the end-user. But there is a
subtle difference. Deployment is the activity of installing the software
on an environment and release is the fact that the end-user can access
it. So we can deploy our software without releasing features and we can
release our features without deploying the software. It is possible to
separate these two activities and carry them out at different moments,
and this provides us with a huge number of advantages. To mention a few:

-   We can deploy at any moment in time, since it does not imply
    exposing new things to end-users, thus enabling deployments without
    requiring business approval. They are not impacted in any way by our
    deployments, so why would we need their approval? This enables
    continuous delivery for our teams without disrupting the business
    with our deployments.

-   We can validate whether the new deployment shows the same behavior
    after installation and before exposing new functionality.

-   We can determine whether the new software -- which has new features
    but that are not yet visible to end-users -- has different
    performance or stability characteristics that might need fixing
    before we expose it to the end-user. This gives us the ability to
    remediate issues before they are experienced by end-users, and thus
    give them more confidence in one-time-right delivery.

-   We can empower the business units by enabling them to toggle
    features on or off at their leisure and when they require it,
    without the involvement of IT.

## Feature toggles as a fundamental piece of the puzzle

![](./media/image1.emf)
How can we realize this separation of
concerns? We need to have a way of making the exposure of features
independent of installing the software in the production environment.
The key to this is the concept of Feature toggles. Feature toggles are
also known as Feature flippers, Feature flags, Feature switches,
Conditional features, etc. A feature toggle is very simple in concept.
It is a mechanism to turn a feature on or off, independent of the
installation procedure. New features only become available when they are
switched on. Application logic will check for the status of a feature
switch and then decide whether to offer the feature or not. A simple
"if" statement that evaluates the feature toggle status is often enough
to accomplish this. The real challenge lies in managing a set of feature
toggles and how you want to expose the feature in the future. You have
multiple options when it comes to how you are going to pick the group of
end-users that will see a feature behind a switch. You can define an
all-or-nothing switch, or you can define a switch that only shows a
feature to a particular set of customers, for instance a group that you
have pre-selected as your very special customer group.

The use of feature toggles has been around for quite some time, but only
recently has it gained in popularity. Instead of avoiding deployment of
features that are not part of the release, we now consider it good
practice to deploy both old, current and new features.

Of course, you could build your own sophisticated framework around
feature toggles to read the status from configuration files or a
database. But you might want to take a look at what is already available
in open source that you could use in your product.

A quick search returns a lot of frameworks, from which I picked a few
that I have found useful in various projects. I also added the open
source license of the software in the table below ([See side
note]{.mark}).

  ------------------------------------------------------------------------------------
  Framework Name       License        Link
  -------------------- -------------- ------------------------------------------------
  NFeature             GPL            https://github.com/benaston/NFeature

  Feature Toggle       Apache 2.0     https://github.com/jason-roberts/FeatureToggle

  Feature Switcher     Apache 2.0     https://github.com/mexx/FeatureSwitcher

  FlipIt               Apache 2.0     https://github.com/timscott/flipit
  ------------------------------------------------------------------------------------

[Text for side note]{.mark} \*\*\*\*\*\*\*\*\*\*\*\*\>

Short note on OSS licenses

When you use open source code in your software, you need to be aware of
the license under which the software is published. Roughly speaking,
there are three categories of licenses:

**Strong Copy Left or so-called "viral" licenses**

This includes licenses such as GPLv2 and GPLv3. These licenses involve
the requirement that you must pass on all the *[same]{.underline}*
rights you received if you redistribute the covered program. This
applies *[not only]{.underline}* to the original program, *[but
also]{.underline}* to any modifications and additions. The GPLv2 states:
"work based on the program", but the term "work based on the program" is
vague whereas this is of key importance. It is not clear whether this
means just derivative works of the original program or something much
broader. So this is a tricky license type that can get you into trouble
and potentially requires you to open-source your commercial software,
including any potential patents that are part of the software!

**Downstream or "weak copyleft licenses"**

This includes licenses such as LGPL. The general requirement of these
licenses is that, if the covered code is distributed, the same code must
be provided downstream under the same license terms. Unlike Strong
copyleft licenses, this mandate generally does not extend to
improvements or additions to the covered code. The LGPLv2 (the "lesser"
or "library" GPL) is classified as both a downstream and a copyleft
license. The prevailing wisdom is whether or not the license is "viral"
depends on how the covered library is linked to any proprietary code.

**Attribution**

This includes licenses such as BSD, MS-PL, MIT and Apache. These
licenses are very basic and allow any kind of downstream use, including
use in a commercial product, as long as the code contains appropriate
attributions to the upstream authors.

**Be aware!**

Carefully evaluate which license you allow for your development and
please always check this, since a dependency on open source, for
instance a nuget package, is created in a second, but the consequences
can be huge!

[End Text for side note]{.mark} \*\*\*\*\*\*\*\*\*\*\*\*\>

## How can you implement the feature toggles?

You can incorporate a feature toggle framework in just a few simple
steps. To explain in more detail how you can do this, I chose the
framework "Feature Toggle" as an example. All other frameworks in the
table are implemented in similar ways and each has its own unique set of
capabilities. I chose "Feature Toggle" since it has out-of-the-box
support for a database-backed feature toggle that works well in
distributed systems that can run on multiple different machines.

A feature toggle for a typical ASP.NET MVC application is implemented by
adding the nuget package to your project. You could create a specific
folder that contains all feature toggles, which makes it easier to
locate them during maintenance. Next, create a class that inherits from
the base class **SqlFeatureToggle** as in code sample 1.

public class HomePagefeatureToggle : SqlFeatureToggle {}

The next step consists of adding some configuration to the web.config
file, where we configure the feature toggle framework. It includes a
connection string to the database and a query for each feature toggle to
determine whether the feature is turned on or off. To do so, add a
section to the config file as shown in code sample 2.

\<appSettings\>

 \<add key=\"FeatureToggle.HomePagefeatureToggle.ConnectionStringName\" 

value=\"MusicStoreEntities\"/\>

 \<add key=\"FeatureToggle.HomePagefeatureToggle.SqlStatement\" 

value=\"Select status from featuretoggles where name 

=\'HomePagefeatureToggle\'\"/\>  

\</appSettings\>

This allows you to evaluate the feature toggle anywhere in the code.
Just create an instance of the feature toggle class and then access the
property FeatureEnabled to check the feature status. The query per
feature gives the flexibility to store this information in any table of
the database. Code sample 3 shows how to change the title of a web page
based on the feature toggle.

\<div id=\"header\"\>

 @{

 
var featureToggle = new MvcMusicStore.Featuretoggles.HomePagefeatureToggle();

  if (featureToggle.FeatureEnabled)

  {

  \<h1\>

\<a href=\"/\"\>ASP.NET MVC MUSIC STORE-With feature toggle in action\</a\>

\</h1\>

  }

  else

  {

  \<h1\>\<a href=\"/\"\>ASP.NET MVC MUSIC STORE\</a\>\</h1\>

  }

 }

\</div\> 

As you can see, implementing these feature toggles is not rocket
science. Now let's look at different release strategies and what we need
to add in order to make the idea fully come to life.

## Release strategies

Now that we have separated the release from deployment we can start
thinking about what our options are when it comes to releasing features
to the end-users. The way we distribute the new feature to the end-users
needs to be based on the goal we want to achieve. There is a set of
release strategies you can select from, and based on this, you need to
pick the right implementation of your feature toggle. There are many
strategies you can apply, some requiring you to add capabilities to the
feature toggle framework, while others can be done with out-of-the-box
functionality. Let me describe a few to give you an idea.

### A/B testing

With A/B testing our goal is to test whether certain changes to your
product will yield the result that you want to accomplish. For example,
think about adding a special banner to your website with a deal of the
day in order to increase conversion ratios. We can validate in
production whether this is actually the case by selecting a cohort of
users (a cohort is nothing more than a selected group of targeted users
to which we expose the feature) and then watch whether the change yields
the expected results. In this particular case we need to design our
feature toggle in such a way that we can select the right set of users.
This means that we need a set of criteria based on which we influence
the toggle, or we can implement a toggle that randomizes a percentage of
users to whom we want to expose the feature.

Important to note is that apart from the feature toggle, instrumentation
also needs to be in place. You might want to track this using Google
Analytics. If you are interested in feature usage statistics, then tools
such as Microsoft Application Insights can show how a feature is used.
However, the design of the telemetry we need for our validation needs to
be part of feature development.

This way of releasing features to end-users and testing the effects of
the feature on your product in production is better known as A/B
testing. It has been used in the online marketing space for many years.
Applying this to continuous delivery, our goal will be less targeted
towards marketing, but more towards pipeline optimization and ensuring
that our software budgets are spent wisely on features that are actually
used and loved by our end-users.

### Canary releasing

![](./media/image2.jpeg)
Another release technique is focused on
discovering whether the new functionality we want to expose does not
interfere with the standard use of your application and whether it does
not introduce performance and scalability issues. In this scenario we
build a new feature and include telemetry to measure the impact of the
feature on usability, scalability and performance. Microsoft Application
Insights can track the performance and scale impact. Moreover, by adding
custom telemetry metrics we can also track the usage of the feature and
see if people find the feature and use it as expected. We also need to
allow a specific group of users to use the new feature in a controlled
and highly monitored environment. To do so, you can use traffic
management from Microsoft Azure, or "Testing in Production" when using
Azure Web Apps. We call this technique Canary Releasing. This term comes
from the early days when miners brought a canary with them into the coal
mines. A canary is more sensitive to toxic gases than the miners. If the
canary suddenly died, then the miners knew to get out of the mines as
quickly as possible. The same concept applies here, since we are
validating in production in a controlled environment and we are closely
watching our telemetry data. As soon as we see indicators that show that
things are going wrong on the servers that host the new features, we can
bail out and move all traffic back to the servers with only the current
features.

### Dark launching

The other common way of releasing is by exposing the feature to the
end-user without him noticing. You will enable the complete dataflow,
but not the actual UI. This means that you exercise the complete feature
and closely measure the performance, scale and expected outcomes,
without the end-user noticing this. You can think of this way of
releasing as a headless canary release. You might think, ok so how do I
expose a feature without an end-user noticing? Take two examples: a new
calculation engine for your product or a new feature in which you show
users where they are, based on their IP address. In these cases you can
submit the data that is required for the calculations or location
determination, without showing this to the end-user. You can measure the
costs of compute and the impact on the scale. We only release the UI to
the end-user when the feature shows what we expect and does not have a
big impact and we do so by flipping a second feature toggle. In these
kinds of releases, you typically have multiple switches exposing one
layer in your architecture at a time. Dark launching is typically used
by developers and IT to assess whether new features have significant
impact on scale and performance. This enables them to ensure that when
the business unlocks the feature for the end-user, no IT involvement is
required.

## Conclusion

Applying the concept of feature toggles allows us to create the ability
to separate the concerns of deploying and releasing software. This
empowers IT to deploy software to production any time they want, since
it will not affect the end-users. This is a fundamental step in enabling
continuous delivery and removing waiting times in the delivery pipeline
for deployments. It also empowers the business to define release
moments, since IT does not have to be involved when a feature is
released to end-users. It enables a set of new release strategies that
can help us be more effective in the development of new functionality
while eliminating waste at the core, and to only build features that
end-users like and use. Finally, it also provides more stability in our
deployment process and enables us to be more in control of the whole
design, build, deploy and release process, given we have built in the
required telemetry to track what is going on.
