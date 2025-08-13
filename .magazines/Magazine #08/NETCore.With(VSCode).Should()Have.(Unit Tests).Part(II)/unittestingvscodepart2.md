# .NETCore.With("vsCode").Should().Have("Unit Tests").Part("II").

*by Reinier van Maanen & Marc Duiker*

## Introduction

In our previous article, in XPRT Magazine #7, we showed how to get
started with unit testing .NET Core projects using VS Code (an excellent
code editor, in our opinion). In this follow-up article we'll continue
with:

-   What is new in the latest releases of .NET Core and VS Code related
    to unit testing?
-   How to run unit tests across multiple projects.
-   How to collect test coverage results across multiple projects.

## What's new?

### .NET Core / ASP.NET Core

Among the many updates of .NET Core 2.x and ASP.NET Core, the most
notable change of the last few months related to testing is the
[Microsoft.AspNetCore.Mvc.Testing](https://docs.microsoft.com/en-us/aspnet/core/release-notes/aspnetcore-2.1?view=aspnetcore-2.2#integration-tests)[^1]
package in ASP.NET Core 2.1.

This package streamlines integration test creation and execution and
handles the following tasks:

-   Copies the dependency file (\*.deps) from the tested app into the
    test project's bin folder.
-   Sets the content root to the tested app's project root so that
    static files and pages/views are found when the tests are executed.
-   Provides the WebApplicationFactory class to streamline bootstrapping
    the tested app with TestServer.

*Example usage of the WebApplicationFactory in an integration test:*

    public class BasicTests
        : IClassFixture<WebApplicationFactory<RazorPagesProject.Startup>>
    {
        private readonly HttpClient _client;

        public BasicTests(WebApplicationFactory<RazorPagesProject.Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetHomePage()
        {
            // Act
            var response = await _client.GetAsync("/");

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }
    }

So while the above isn't about 'pure' unit testing, it's still a
valuable addition to your testing arsenal, which enables you to write
integration tests for Razor Pages with minimal effort.

If you want to write end-to-end tests for web apps, then use a tool such
as [Selenium](https://www.seleniumhq.org/)[^2]. Do not use Coded UI
tests for this, as this is
[deprecated](https://docs.microsoft.com/en-us/visualstudio/test/use-ui-automation-to-test-your-code?view=vs-2017)[^3].

### Unit testing Libraries & Frameworks

The mocking framework **NSubstitute** had a major release to version
4.0.0. The breaking change is related to argument matchers, `Arg.Is`,
`Arg.Any` etc., which now use `ref` returns, a C# 7.0 feature. This
change allows proper support for working with `ref` and `out` arguments.

### VS Code extensions

The **.NET Core Test Explorer** extension had [six new
releases](https://github.com/formulahendry/vscode-dotnet-test-explorer/releases)[^4]
since the previous article and is currently at version 0.6.3. It has
been improved to support multiple workspaces and includes numerous bug
fixes.

The **Coverage Gutters** extension had [four new
releases](https://github.com/ryanluker/vscode-coverage-gutters/releases)[^5]
and is now at version 2.3.1. It contains dozens of bug fixes and
performance improvements by making better use of async operations.

**Coverlet**, the cross platform code coverage tool for .NET Core has
had [eight new
releases](https://github.com/tonerdo/coverlet/releases)[^6], including
two major versions, and is now at version 4.1. The changes include
several performance enhancements and a feature to compute cyclomatic
complexity.

## Running tests across multiple projects

In the previous article, we showed a simplified situation of one .NET
Core console project with one corresponding XUnit test project. In real
life however, you will have dozens of projects, with many of them having
test projects as well. This requires a different set up of your VS code
files in order to run tests across multiple projects. A couple of
examples are shown below, and you'll probably end up combining a few of
them.

### Tasks.json

An easy way to build multiple projects is by extending the tasks.json
file, making use of the dependsOn property of a task:

        {
          "label": "build Project.A",
          "command": "dotnet build Project.A /property:GenerateFullPaths=true",
          "dependsOn": "clean Project.A",
          "problemMatcher": "$msCompile",
          "type": "shell",
          "group": {
            "isDefault": true,
            "kind": "build"
          },
        },
        {
          "label": "clean Project.A",
          "command": "dotnet clean Project.A",
          "dependsOn": "build Project.B.UnitTests",
          "problemMatcher": "$msCompile",
          "type": "shell"
        },
        {
          "label": "build Project.B.UnitTests",
          "command": "dotnet build Project.B.UnitTests /property:GenerateFullPaths=true",
          "dependsOn": "clean Project.B",
          "problemMatcher": "$msCompile",
          "type": "shell"
        },
        {
          "label": "clean Project.B",
          "command": "dotnet clean Project.B",
          "dependsOn": "clean Project.B.UnitTests",
          "problemMatcher": "$msCompile",
          "type": "shell"
        }

When you execute tasks 'build Project.A', it will first try to execute
'clean Project.A' because it depends on that step. 'clean Project.A' has
a dependency on 'build Project.B', which depends on 'clean Project.B'.
This means that the tasks will be executed in the following order:

-   Clean Project.B
-   Build Project.B
-   Clean Project.A
-   Build Project.A

Of course, this only cleans and builds. You can add dotnet test as well,
but in our experience running all unit tests on each build isn't very
effective. Create a separate task for that, or use the Test Explorer.
Another way to do this, is described [in a blog post by Scott
Hanselman](https://www.hanselman.com/blog/AutomaticUnitTestingInNETCorePlusCodeCoverageInVisualStudioCode.aspx)[^7].
He uses [dotnet
watch](https://docs.microsoft.com/en-us/aspnet/core/tutorials/dotnet-watch?view=aspnetcore-2.2)[^8]
to trigger tests whenever source code changes. This still runs all
tests, and while it isn't like Visual Studio's awesome Live Unit
Testing, it's a step.

The downside to this approach is that the tasks.json can become quite
big and uneasy to maintain. Read on for some ways around this.

A last interesting bit is the 'GenerateFullPaths' property. This doesn't
have anything to do with building multiple projects, but without this,
any compiler errors in VSCode aren't clickable in the error window which
degrades usability.

### PowerShell Script

Another way to build multiple projects is by combining PowerShell and
the tasks.json. Create a buildsolution.ps1 file and add the following
and anything else you require:

    dotnet clean Project.B
    dotnet build Project.B /p:GenerateFullPaths=true
    dotnet clean Project.A
    dotnet build Project.A /p:GenerateFullPaths=true

You can then call this script from the tasks.json file in a custom task:

        {
          "label": "build",
          "command": "powershell",
          "args": [
            "-ExecutionPolicy",
            "Unrestricted",
            "-NoProfile",
            "-File",
            "${cwd}/buildsolution.ps1"
          ],
          "type": "shell",
          "problemMatcher": "$msCompile",
          "group": {
            "isDefault": true,
            "kind": "build"
          }
        }

The advantage here is that this results in an easier to maintain
tasks.json file, you can do anything you want in the PowerShell script,
and you can even use that same scripts in a build pipeline, making sure
the build on a buildserver runs the same way it's run locally. It will
require you to use a PowerShell task in your build. As with the
tasks.json, the same remarks and suggestions about running tests apply
here.

### Solution file

You can create a solution file with `dotnet new sln` and refer to the
solution file with the dotnet CLI: `dotnet build ProjectsAplusB.sln`.
Ofcourse, this helps clean up the tasks.json as well as you can see
below. Using a solution file also has the added benefit that if you have
Visual Studio IDE and need one of its features, a switch can be made
easily. Also, just as with referencing a PowerShell script, this gives
you the option to create a build pipeline on Azure DevOps which behaves
more like a local build. Unlike the PowerShell solution, you can just
use the standard dotnet task for that.

Running `dotnet new sln` will just create an empty solution. Adding and
removing projects can be done with `dotnet sln add ProjectA` and
`dotnet sln remove ProjectB`. You can list all projects in the solution
with `dotnet sln list`.

The tasks.json will end up looking like this:

        {
          "label": "build",
          "command": "dotnet build ProjectsAplusB.sln /property:GenerateFullPaths=true",
          "dependsOn": "clean",
          "problemMatcher": "$msCompile",
          "type": "shell",
          "group": {
            "isDefault": true,
            "kind": "build"
          },
        },
        {
          "label": "clean",
          "command": "dotnet clean ProjectsAplusB.sln",
          "problemMatcher": "$msCompile",
          "type": "shell"
        }

### Test Explorer

In the previous article, we mentioned the Test Explorer extension, which
gives you a GUI for running all your unit tests. Making sure the Test
Explorer picks up tests from all projects is very easy. Just change the
value of `dotnet-test-explorer.testProjectPath`, making use of
wildcards: Change `"/ProjectA.Tests"` to `"/*.Tests"` and you're done.
There is a problem with this if you also use Test Explorer to generate
coverage files with Coverlet as we showed you in the previous article.
Read on to learn more!

## Collecting test coverage results across multiple projects

Configuring Test Explorer to run tests from multiple projects and also
configuring it so that Coverlet writes its output to disk results in an
issue: for every unit test project a separate coverage file is written,
and Coverage Gutters won't merge the results. Simply configuring
Coverlet to write the results to 1 file also doesn't work, the file is
overridden for every project so, after the entire run, only the coverage
of the last project is visualized by Coverage Gutters. Luckily, there is
a way to configure Coverlet to merge the results but, it's not easy:

Supply these arguments as value for
`dotnet-test-explorer.testArguments`:

`"dotnet-test-explorer.testArguments": "--filter Category!=Integration /p:CollectCoverage=true \"/p:CoverletOutputFormat=\\\"json,lcov\\\"\" /p:CoverletOutput=..\\lcov /p:MergeWith=..\\lcov.json"`
(yes including all the escaping and extra quotes)

When running the tests, this will create a lcov.json and lcov.info in
the root of the workspace. The json file is in an coverlet specific
format and is just a simple JSON file, which has some benefits like
being able to use it in the `MergeWith` parameter. The lcov file is
still needed, because this is used by Coverage Gutters. What happens
with the above configuration is that the lcov.json is merged for each
unit test project and then a new lcov.info is generated, based on the
merged file. The end result is one big lcov.info file with coverage from
all test projects. It's not in the project directory, so Coverage
Gutters won't detect it, and a workaround is needed to enable proper
detection.

There is another issue here, because running the tests will merge any
existing lcov.json file, also those from a previous run. So there's
still some work to do at the Coverlet plugin. Other issues are that this
will not update the merged file properly when tests are removed (it
seems only to add coverage lines and not remove lines which aren't
covered anymore) and last but not least, running the tests with these
arguments will crash if there isn't a file to merge with (which is
troublesome with the first project you'll run tests on). Read on for
some workarounds!

### Workaround for Coverage Gutters not picking up the coverage file

At the moment Coverage Gutters only looks for lcov files in project
directories. As mentioned before, it won't merge results if it finds
multiple lcov files in multiple project directories, so we used Coverlet
to merge. The workaround is quite trivial, just write the merged lcov
file to one of your project directories:

`"dotnet-test-explorer.testArguments": "--filter Category!=Integration /p:CollectCoverage=true \"/p:CoverletOutputFormat=\\\"json,lcov\\\"\" /p:CoverletOutput=..\\Project\\lcov /p:MergeWith=..\\Project\\lcov.json",`

There is a [GitHub
issue](https://github.com/ryanluker/vscode-coverage-gutters/issues/178)[^9]
here about being able to direct Coverage Gutters to a specific coverage
file. . As soon as this has been implemented this workaround shouldn't
be needed anymore.

### Workaround for resetting the coverage for each run and preventing a crash when running for the first time

This workaround is a bit dirty (as workarounds always tend to be). You
can alter the csproj of one of your **testprojects**. Don't pick a
project that contains the implementation, depending on your build
configuration that one is built more than once: one time by itself and
one time as a dependency of your testproject. Include this:

      <Target Name="ResetCoverageFile" AfterTargets="Build">
        <Copy SourceFiles="..\Project\lcov.empty" DestinationFiles="..\Project\lcov.json" />
      </Target>

lcov.empty is an empty json file, so this results in a clean slate each
time Coverage Gutters runs, builds all projects and executes all tests,
resulting in up-to-date coverage and fixing the problem with the first
test run. Of course all of the above: from running the tests with
coverage to clearing previous results can also be added to a powershell
or any other script and then bound to a build task. You can then check
coverage without the Test Explorer in a fairly easy way.

A small issue that remains is that coverage won't be updated when
executing a single test, but that's fine for most people.

### Debugging / viewing total coverage percentage

If you're someone who likes to measure code quality by total test
coverage, the easiest way to see the coverage percentage is by viewing
the log of the Test Explorer. Of course, we don't have to tell you that
coverage by itself doesn't mean much!

> "Being proud of 100% test coverage is like being proud of reading
> every word in the newspaper. Some are more important than others."
>
> [Kent Beck on
> Twitter](https://twitter.com/KentBeck/status/812703192437981184?s=09)[^10]

An interesting alternative, which is still very much under development,
is [Stryker](https://github.com/stryker-mutator/stryker-net)[^11]. It
alters your code right before tests run, and checks whether at least one
unit test fails. Read more on their website. For now, to check the
coverage percentage:

![Show log
button](./media/rId57.png "Show log button")


Show log button

The output will be something like this:

![Show log
output](./media/rId58.png "Show log output")


Show log output

These logs can also be very useful when debugging issues, so if things
aren't working have a look here.

## Conclusion

As shown by the large number of VS Code extension releases the tooling
landscape related to unit testing is evolving and improving at a rapid
pace. For running tests across multiple test projects, it appears that
using a `sln` file would still be the easiest way and this also allows
developers to use both VS Code and Visual Studio IDE.

We hope this article has given you a better understanding of how to
configure unit testing for .NET Core projects in VS Code. If you have
any further questions or comments, don't hesitate to contact us.

[^1]: https://docs.microsoft.com/en-us/aspnet/core/release-notes/aspnetcore-2.1?view=aspnetcore-2.2#integration-tests

[^2]: https://www.seleniumhq.org/

[^3]: https://docs.microsoft.com/en-us/visualstudio/test/use-ui-automation-to-test-your-code?view=vs-2017

[^4]: https://github.com/formulahendry/vscode-dotnet-test-explorer/releases

[^5]: https://github.com/ryanluker/vscode-coverage-gutters/releases

[^6]: https://github.com/tonerdo/coverlet/releases

[^7]: https://www.hanselman.com/blog/AutomaticUnitTestingInNETCorePlusCodeCoverageInVisualStudioCode.aspx

[^8]: https://docs.microsoft.com/en-us/aspnet/core/tutorials/dotnet-watch?view=aspnetcore-2.2

[^9]: https://github.com/ryanluker/vscode-coverage-gutters/issues/178

[^10]: https://twitter.com/KentBeck/status/812703192437981184?s=09

[^11]: https://github.com/stryker-mutator/stryker-net
