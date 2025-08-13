# 99% of your apps and sites are not your code

Your own 1% is under source control, but are you keeping taps on all of
the libraries you import each time you do a dotnet restore or npm
install?

Over the last years there has been an increase in reported supply chain
attacks. Attacks where the attacker isn't trying to get access to your
source control repositories, but that of one of the many projects you
depend on. A bitcoin wallet was compromised and sent wallet keys to a
third-party domain[^1] through a nodejs package that changed ownership.
Credit card details for thousands of users were intercepted through the
chat client embedded in the same pages that handled transactions[^2].
And it's not limited to websites and JavaScript apps. Asus had their
laptop update tools compromised, causing specific targets to download,
and install additional packages as part of driver updates[^3].

The same dangers lurk for .NET developers. You may be asking: "how does
it work, and how does it affect me?"

A supply chain attacks occurs when someone infiltrates your systems via
a third-party service or dependency to exploit a vulnerability in a
system. Typically, attackers try to insert malicious code into official
downloads and installers of trusted third-party service providers or in
dependencies used by developers. Once organizations start using these
services, they are automatically exposed to the embedded malware too.
Usually, the attackers are after access to source code or sensitive
data, and they can access that by finding the weakest link in the
software supply chain without ever having to go near their target's
servers. One of the advantages for the attackers is that with one piece
of malicious code in a dependency, they can target many organizations at
once. On top of that it is often difficult for organizations to detect
these attacks, since they depend on many third-party services and
dependencies.

That is all interesting, but that won't happen to you, right? Well, as
it turns out, it might not be as difficult for hackers to insert some
malicious code into your project as you think. Here's a small scenario:
imagine you are a .NET developer within an organization, and your team
is responsible for an application handling sensitive information. You
want to focus on the business logic of your application instead of
reinventing the wheel for every bit of code you need, so you use NuGet
as a package manager. It helps you re-use code from other developers to
solve some of your tasks, that way you can spend your time on your
application's specific logic.

While this is a common practice, using somebody else's code means that
you need to find a way to trust it. Do you always know what is in the
packages you consume? What if one of the many dependencies you use in
your project is infected with malicious code? What would be the
consequences? And how would you detect this at all?

# How can this happen?

It isn't hard to be presented a different package when restoring
packages across machines. This is the default behavior for most package
managers, including NuGet. When you restore packages, it will try to
find the versions you're after and will do a best effort attempt to
resolve issues[^4].

An example from one of the open source projects we maintain

There are a few cases in which NuGet may not be able to get the same
package graph with every restore across machines. Most of these cases
happen when consumers or repositories do not follow NuGet best
practices[^5]:

1.  **nuget.config mismatch**: This may lead to an inconsistent set of
    package repositories (or sources) across restores. Based on the
    packages' version availability on these repositories, NuGet may end
    up resolving to different versions of the packages upon restore.

2.  **Intermediate versions**: A missing version of the package,
    matching PackageReference version requirements, is published:

    -   Day 1: If you specified \<PackageReference
        Include=\"My.Sample.Lib\" Version=\"4.0.0\"/\> but the versions
        available on the NuGet repositories were 4.1.0, 4.2.0 and 4.3.0,
        NuGet resolves to **4.1.0** because it is the nearest minimum
        version.

    -   Day 2: Version 4.0.0 gets published. NuGet now restores
        version **4.0.0** because it is an exact match.

3.  **Package deletion**: Though nuget.org does not allow package
    deletions, not all package repositories have this constraint.
    Deletion of a package version results in NuGet finding the best
    match when it cannot resolve to the deleted version.

4.  **Floating versions**: When you use floating versions
    like \<PackageReference Include=\"My.Sample.Lib\"
    Version=\"4.\*\"/\>, you might get different versions after new
    versions are available. While the intention here is to float to the
    latest version on every restore of packages, there are scenarios
    where users require the graph to be locked to a certain latest
    version and float to a later version, if available, only upon an
    explicit gesture.

5.  **Package content mismatch**: If the same package (id and version)
    is present with different content across repositories, then NuGet
    cannot ensure the same package (with the same content hash) gets
    resolved every time. It also does not warn or error out in these
    cases.

6.  **Cache poisoning**: NuGet will check the local package cache before
    checking configured package feeds (unless \--no-cache is specified).
    These will be used in case of an exact version match. If you are
    using a proxy feed (such as Azure Artefacts), an attacker with
    access to the feed (or an upstream feed) could publish a specific
    version to that feed which will be used instead of the one you are
    expecting.

# More and more re-use

If we would only depend on a few dependencies and if they would only
change once in a very long while, it wouldn't be hard to manually review
the changes. If you had access to the sources. And in that case, you
could copy all your dependencies to a manually curated feed.

But we don't live in that world anymore.

![Image](./media/image1.jpeg)
As Snyk.io's Liran Tal[^6] puts it:
vulnerabilities can occur anywhere, but you only have full control over
a small piece.

When you create a new Visual Studio 2019 (16.2.2) React.js Web
Application project, you end up with 15214 Nodejs packages (686 with
known security issues) and 284 NuGet packages (18 with known security
issues. If any of them is compromised, you may be adding them to your
project the next time you run npm install or dotnet restore.\
Or worse, your local development machine may be fine, but the build
server may be fetching all the latest versions. This is especially the
case when you use the Azure Pipelines Hosted Pool, since every build
uses a fresh image with very few packages pre-cached.

What we need is a way to store all our dependent packages in source
control in an efficient manner, preferably without having to store all
the contents of the packages in source control. Now, while that may
sound like a contradiction, it isn't. Instead of storing all package
contents and that of all their dependencies, use what npm, NuGet and
yarn do. These tools all store the name, exact version, and a hash of
the package contents for all packages in the dependency tree in a file.
This file is called a lock file, and by committing this lock file to
your version control repository, you ensure that:

1.  Your build server (and your colleagues) will use exactly the same
    packages you used on your development machine.

2.  You keep an auditable log of all the changes to your dependency
    tree.

3.  You can inspect all changes to the dependencies prior to committing,
    or as part of the pull-request review process.

# Generate lock files for .NET solutions

Your .NET projects won't generate lock files by default. You must also
upgrade your project to use the new \<PackageReference\> format. Then
you can instruct the build process to generate the lock file through a
command line parameter:

Generate the lock file through dotnet:

Generate the lock file through msbuild:

You can also add a Property to your project files to generate lock files
on every restore:

*Note: This behavior is different from npm and yarn, which automatically
generate the lock files each time you restore your dependencies.*

NuGet will now store a packages.lock.json alongside every project. The
file contains all the dependencies, their exact versions, how the
dependency was introduced, and a hash of the package contents:

Commit these files to your source control repository to store the exact
dependencies along your other source files.

# Restore from the lock file in your CI solution

What we want NuGet to do, is to download the exact same packages we used
on our development system. Just storing your dependencies in source
control isn't enough. One of the first steps of your CI process is
likely dotnet restore and unless we do something about it, this will
just download a new set of dependencies and then overwrite the lock
file.

Instead, we should tell NuGet to restore the exact packages specified in
the lock file. And again, this can be done through a command line
parameter or an msbuild property.

To restore in locked mode using dotnet:

Restore in locked mode using msbuild:

To ensure the Continuous Integration server uses locked mode by default,
you can also set this property in the project file:

You're all set, your .NET projects will now restore to a predictable set
of dependencies each time you build it, or the build will fail.

Each time you restore locally, you'll see exactly which packages have
been updated and you can inspect their contents on your development
machine:

![](./media/image2.png)

Restoring against a different .NET Core version may cause different
package contents with the same version. This will be detected and fails
your build.

# Impact on build times

You may be wondering what the impact on restore times will be when
turning this feature on. On the development machine restores will take
longer, because the lock file must be generated and the hash for the
package contents must be calculated.

On the build server it's less clear-cut. The time to resolve package
versions and calculate the dependency tree is reduced to the time it
takes to just load the lock file. This may save a lot of time. On the
other hand, verifying the package contents will add some time. In our
tests, the average times to run the build on Azure Pipelines were faster
with the locked mode turned on.

# Hands-on: Try the Global DevOps Bootcamp 2019 challenge

The Global DevOps Bootcamp 2019 featured a Supply Chain Attack
challenge[^7] that lets you experience the effects of a supply chain
attack. As part of the hands-on lab you get to generate npm and NuGet
package lock files, adapt the build process to perform locked restores,
and add a scanner to your build process to detect known vulnerabilities
in your dependencies. By applying these techniques, you will be able to
take control over what you ship to your customers every time you deploy
your latest changes.

Jesse Houwing

Sofie Wisse

[^1]: <https://github.com/bitpay/copay/issues/9346>

[^2]: <https://security.ticketmaster.ie/>

[^3]: <https://securelist.com/operation-shadowhammer/89992/>

[^4]: <https://docs.microsoft.com/en-us/nuget/concepts/dependency-resolution>

[^5]: <https://devblogs.microsoft.com/nuget/Enable-repeatable-package-restores-using-a-lock-file/>

[^6]: <https://twitter.com/liran_tal/status/1067775376229834754>

[^7]: <https://www.gdbc-challenges.com/Challenges/ChallengeDetails/VULNPACKAGE>
