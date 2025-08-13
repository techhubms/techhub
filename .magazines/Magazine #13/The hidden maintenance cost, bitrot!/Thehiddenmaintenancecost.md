## [The hidden maintenance cost, bitrot!](https://fluentbytes.com/?p=9224)

Sometimes it is very hard for customers to understand the hidden costs
involved when you build custom software. By a hidden cost, I mean a
phenomenon that apparently is back in our industry called **BitRot**.

**What is BitRot?**

Back in the day, BitRot was caused by the fact that the magnetic media
we used to store our computer programs sometimes lost their magnetic
information, causing problems reading the data back. With the industry
moving to new ways to store data, like solid-state drives, the problem
is still there but not predominantly visible anymore. We have algorithms
that store our data in such a way that the lost data can be recovered,
and from an end-user perspective, BitRot seems to have vanished
completely.

![A picture containing text, screenshot Description automatically
generated](./media/image1.png)
[Data degradation --
Wikipedia](https://en.wikipedia.org/wiki/Data_degradation)

**Bitrot redefined**

Although the original problem has gone more or less away, we are now
confronted with a completely new way that bitrot is coming back in our
industry. You might disagree with me about reusing the name BitRot for
something different, but in essence, the problem we face manifests
itself in the same way. If we don't touch the software, we write for
even a few weeks, the software deteriorates!

Let me explain what is going on here.

BitRot today is the issue when we don't touch our software for a few
days or weeks, and the software deteriorates caused by a number of
sources. Let me share a few of the issues you will face when building
software today.

**New known vulnerabilities**

A known vulnerability is some weakness found in the software you wrote
or in any software you used to build your application or website that
someone can exploit. You might think, why would my software all of a
sudden become exploitable while I haven't touched it? This is caused by
the fact that attackers become smarter each day. They find new
innovative ways to exploit software. Since the software that is written
often contains tons of code not only written by your own company but
also open source components, the chance of your software becoming
vulnerable itself is a significant risk. This does not immediately mean
your software will be exploited as well, but the likelihood that your
software becomes exploitable will increase almost every day. This is
something you need to keep track of and you need to make updates or
changes to your software to keep up with the current state of the
industry. The number of vulnerabilities found is also increasing all the
time. You can see how fast this is picking up in the following graph the
NVD publishes[^1]. I added a capture of the chart that shows how many
known vulnerabilities are found and that the rate of finding them is
constantly increasing.

![](./media/image2.png)
[NVD -- CVSS Severity Distribution Over
Time
(nist.gov)](https://nvd.nist.gov/general/visualizations/vulnerability-visualizations/cvss-severity-distribution-over-time)

**Updates of used frameworks or packages**

Your software is rarely built from scratch. To build software, you will
use other software components all the time. Depending on the technology
you use to write your programs, you use NuGet packages (.NET), Maven
packages (Java), Node Packages (Node/Web Development), RubyGems (Ruby
development), etc. These packages are built and maintained by others
and, coming back to the previous topic, they also need to maintain their
software to keep it free from known vulnerabilities. Also, they want to
provide new capabilities and features constantly. This implies those
packages will get new versions all the time, and keeping up with those
more recent versions is not to be taken lightly.

Let's assume you build a simple Hello World web application using .NET 6
and React. You can see the screenshot of what I did to create the
application in Visual Studio 2022:\
![Graphical user interface, application Description automatically
generated](./media/image3.png)
New ASP.NET with React project

This will result in an astonishing set of **1,487 **Dependencies from
the NodeJS ecosystem and 15 more from the .NET ecosystem. Starting with
a clean template (I updated everything before I created the application
in Visual Studio), this already resulted in **23 Known
Vulnerabilities** of which 9 are at the level of High!

![Graphical user interface, text, application, email Description
automatically generated](./media/image4.png)
Analysis of new project with Dependabot (GitHub)

**Updates on compilers and tooling**

Then there are the dependencies on Visual Studio 2022, which has updates
at least once every month. We took a dependency on the .NET framework
that is updated at least 1x per year and every two years has a new
stable release that is supported for a maximum of 3 years. Not to
mention all the small updates that might come to fix bugs and
vulnerabilities. And finally, we took a dependency on the NodeJS
toolset, which is also updated multiple times a year. These tools also
tend to make breaking changes. You must constantly keep them up to date
because they can also contain known vulnerabilities that might
compromise your development environment!

**Newer versions of the languages and frameworks**

Finally, there are also dependencies on the languages we use. In this
example, I used React which is Javascript/typescript based, and C#10 for
the .NET codebase. C# is updated on a yearly cycle, and if you look at
the versions of Node, then you see you need to update this every six
months:

![Table Description automatically
generated](./media/image5.png)
[Releases \| Node.js
(nodejs.org)](https://nodejs.org/en/about/releases/)

Those language and framework updates can be significant if you look at
the amount of work involved to actually use the new capabilities. Not
using them still makes your codebase deteriorate from a maintainability
perspective since the industry is moving on with new ways of utilizing
the language and framework features. New team members on a project will
have a hard time adapting old programming styles and inefficiencies if
you only ensure it compiles.

**How can we mitigate this issue?**

First of all, the most important thing is that you allocate time to keep
things clean. This means allocating a certain budget of time to keep
your packages up to date and spending time updating to newer minor or
major versions of packages that come available. There is no silver
bullet in solving this issue, but taking the time for it is one step in
the right direction.

Secondly, you can automate some of this when you are using, e.g., GitHub
as a DevOps platform. Here you have the option to enable a feature
called Dependabot that will inform you that your packages are out of
date or contain known vulnerabilities. When it knows the vulnerability
can be mitigated with a new package version, it will even create a pull
request for you. You still need to review it and approve the PR, so it
becomes part of the main codebase. There are also tools that can
integrate with most DevOps platforms to manage automated updates for you
e.g. Renovate[^2].

And then the final check is, when do I take the dependency? Be very
aware of the fact that taking the dependency adds a maintenance cost.
Building something yourself will also incur significant costs, but it
should be a decision you make and not just a default you accept all the
time. Be very aware of the tradeoff and that a dependency incurs a cost
as well.

**Conclusion**

The software you build is in a constant state of decay, and you must
allocate a significant portion of time to keep things evergreen and up
to date. Waiting for your updates will cost you significantly more time
than updating constantly. The adage "if it hurts, do it more often"
applies here and makes the software delivery cadence more predictable
and more secure. So make updating packages, frameworks, and languages
part of the standard maintenance cycle!

The real challenge lies in making our customers aware of this problem
and finding ways to make them aware that they need to maintain software.
You can not leave software untouched for a few weeks since, in the
meanwhile, the software becomes outdated and vulnerable. And last but
not least it will become more complicated each day to make it up to date
again.

[^1]: <https://nvd.nist.gov/general/visualizations/vulnerability-visualizations/cvss-severity-distribution-over-time>

[^2]: https://docs.renovatebot.com/
