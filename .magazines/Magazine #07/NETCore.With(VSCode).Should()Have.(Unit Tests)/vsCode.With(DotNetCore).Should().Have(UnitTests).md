# .NETCore.With(\"vsCode\").Should().Have(\"Unit Tests\")

# Benefits of unit testing

# Although unit testing has been a known practice for decades among software developers, it is still not being applied on a daily basis everywhere. The value of unit testing has been explained in the literature, showing that fixing bugs early in the software development lifecycle is orders of magnitudes cheaper than fixing bugs in production[^1].

# Bugs can never be prevented by just using unit tests alone, but by writing unit tests, or even better, applying Test Driven Development (TDD), you ensure that your application is designed in such a way that it is easily testable and maintainable. So when bugs are found (and they will be found) you can fix and refactor code with confidence and within time.

# Another benefit of having unit tests (if they are well-written) is that they provide a clear explanation of how the application works. This helps you and future developers to understand the code base and to make changes to it much more easily than without unit tests.

# So, what if you\'re a hip developer who prefers to hang around in VS Code instead of Visual Studio IDE and is just starting with .NET Core? How well are the unit testing frameworks and related tools supported? Let\'s find out...

# .NET Core and VS Code

In this article we (Reinier van Maanen & Marc Duiker) will focus on how
unit testing is done these days using the .NET Core framework and the VS
Code editor. We assume you have a basic understanding of unit testing
and mocking in .NET full framework and Visual Studio IDE.

The .NET Core framework is very popular since it runs cross-platform and
it performs better than the full .NET framework. VS Code has been
adopted quickly by all kinds of developers thanks to the great number of
extensions. VS Code is also a lightweight editor when compared to the
Visual Studio IDE, so it\'s an ideal editor to write .NET Core
applications. But it\'s not just C#! -- support for Python, JavaScript,
Java, Go, Ruby, PHP and other languages is also available. And did we
mention it's free for everyone to use?

# Installation

![](./media/image1.png)
First things first, you're going to need
VS Code[^2] and the .NET Core SDK[^3]. Installation is straightforward
and differs per platform, so we won\'t go into that, just follow the
instructions! When you first start VS Code, it doesn\'t contain a lot of
features, so you\'re going to need some extensions. Find the button with
all the squares to manage your extensions and make sure you install the
ones listed below and any others you fancy. Tip: have a look around the
marketplace[^4]!

-   C#

-   .NET Core Test Explorer

-   Coverage Gutters.

Reload VS Code to activate the plugins and you're good to go.

# Frameworks

When you're going to write unit tests you to need to make some choices,
such as which test framework and which mocking framework to use. They
all have their pros and cons and it really doesn't matter much, as most
popular frameworks are now supported in .NET Core.

In our example code, we'll be using xUnit[^5] and NSubstitute[^6], but
this is merely our personal preference in this case. In our opinion, you
should use the frameworks your team is the most familiar with. If you
have little experience with test frameworks, stick with xUnit as it is
well documented, very popular, extensible and supported by the dotnet
CLI, as we'll see in a moment. For mocking, NSubstitute is a good start
because it's easy to learn. If you feel like it, throw
FluentAssertions[^7] in the mix for more readable assertions, or
AutoFixture[^8] to run different scenarios against one test. We won't go
into any of this in detail in this article.

# Set up your projects

We use the dotnet CLI for creating projects with *dotnet new*. There are
many templates available:

![](./media/image2.png)


Figure 1 File templates when using dotnet new

Here's how to set up a new project with a corresponding unit test
project:

-   Launch VS Code if you haven't done so already.

-   Control-K + Control-O[^9]: Open (or create) a new empty directory
    for your projects. VS Code doesn't work with solution files, so just
    open a directory and work from there.

-   Control-Shift-Tilde: Open a new terminal so we can use the dotnet
    CLI.

-   Execute the commands listed below:

    -   dotnet new console \--name TicTacToe

    -   dotnet new xunit \--name TicTacToe.Tests

    -   cd TicTacToe.Tests

    -   dotnet add reference ../TicTacToe/TicTacToe.csproj

    -   dotnet add package NSubstitute

You just created a new console application called *TicTacToe* and a new
test project called *TicTacToe.Tests*. Then you added a reference
between them and added the *NSubstitute* NuGet package to the test
project.

As we assume basic knowledge of how to write unit tests, we won't go
into that right now. A reference project containing the TicTacToe
project including its unit tests can be found on GitHub[^10]. The next
section will describe that codebase.

# .NET Core Projects Overview

We've created a repository[^11] for you to clone that contains two .NET
Core projects: *TicTacToe* and *TicTacToe.Tests*. The test project
contains ten unit tests in two classes.

## Test Project

The tests in the *EndGameStrategyTests* class verify the functionality
of the *EndGameStrategy* class, i.e. when is a game of TicTacToe
complete and who has won. The tests are quite straightforward, and
nothing is .NET Core specific here. The tests are based on *xUnit* and
use *FluentAssertions* in order to use the fluent result.Should().Be()
syntax. The unit tests use various instances of the TicTacToe board
which are created in the *BoardFactory* class.

The tests in the *AvailablePositionsTests* class verify the available
number of positions on the TicTacToe board when moves are made. The
system under test is the *GameEngine* class. It requires an instance of
*IEndGameStrategy* which is injected into the constructor. Since we want
to test the *GameEngine* class in isolation, we create a fake
implementation of I*EndGameStrategy* by using *NSubstitute*, see the
*CreateGameEngine* method.

## .vscode Folder

Note that the repository contains a folder called *.vscode*, as you
might expect this contains some specific files that define how VS Code
handles the projects. There are three files present in the .vscode
folder:

-   *launch.json*: Specifies what happens when you launch a debug
    session for the TicTacToe project.

-   *settings.json*: Contains the VS Code workspace settings. This is
    the place for settings you want to share with everyone in your
    development team since they are in version control. In this case it
    contains a setting specific for the *.NET Core Test Explorer*
    extension (more on that later in this article).

-   *tasks.json*: Contains a list of tasks which can be executed using
    the VS Code Command Palette or shortcuts. For .NET Core it always
    contains a build task (which can be selected with *CTRL+SHIFT+B*)
    and in this case it also contains a test task, so we can run the
    unit tests (more on that in the next section). The tasks that can be
    specified here are not limited to building or testing projects. Any
    tool that has a command line interface can be configured here.

-   *extensions.json:* Contains a list of recommended extensions. You
    receive a nice suggestion to install these when opening the
    directory in VS Code.

# Running Unit Tests

Unit tests in VS Code can be executed from the terminal, the Command
palette, or from the .NET Core Test Explorer extension.

Let\'s have a look at each of these.

## VS Code Terminal

The most basic way to execute unit tests is to use the dotnet CLI
directly through the terminal as follows:

-   Open the VS Code Terminal (*CTRL+\`*).

-   Navigate to the TicTacToe.Tests folder.

-   Type *dotnet test.*

The following response appears:

Build started, please wait\...Build completed.

Test run for \<LOCAL_REPO_PATH\>
\\TicTacToe.Tests\\bin\\Debug\\netcoreapp2.1\\TicTacToe.Tests.dll(.NETCoreApp,Version=v2..csproj\'.1)

Microsoft (R) Test Execution Command Line Tool Version 15.8.0

Copyright (c) Microsoft Corporation. All rights reserved.

Starting test execution, please wait\...

Total tests: 9. Passed: 9. Failed: 0. Skipped: 0.

Test Run Successful.

Test execution time: 1.7864 Seconds

Note that you only get the summary information for successful tests and
nothing about individual tests that have been executed. To get more
information, you can run the following:

dotnet test -v n

where -v is the verbosity switch and n is the normal verbosity level.

After execution of the verbose command you will see a list of the unit
tests and the indication whether they have passed or failed.

## VS Code Command Palette

Instead of typing dotnet test all the time and making sure you\'re in
the right folder, you can also utilize the *Run Test Task* in the
Command Palette. This task will run the dotnet test command for you, so
you don't need to type it repeatedly.

Before you can run the task, you will need to add it to the *tasks.json*
file located in the *.vscode* folder.

### Editing tasks.json

The following JSON snippet shows a task with the label *test*. This
instructs VS Code to run the dotnet CLI command and passes *test* and
the csproj file location as arguments.

{

\"version\": \"2.0.0\",

\"tasks\": \[

{

\"label\": \"build\",

\"group\": {

\"isDefault\": true,

\"kind\": \"build\"

},

\"command\": \"dotnet\",

\"type\": \"shell\",

\"args\": \[

\"build\",

\"\${workspaceFolder}/TicTacToe.Tests/TicTacToe.Tests.csproj\"

\],

\"problemMatcher\": \"\$msCompile\"

},

{

\"label\": \"test\",

\"command\": \"dotnet\",

\"type\": \"shell\",

\"group\": {

\"isDefault\": true,

\"kind\": \"test\"

},

\"args\": \[

\"test\",

\"\${workspaceFolder}/TicTacToe.Tests/TicTacToe.Tests.csproj\"

\],

\"presentation\": {

\"reveal\": \"always\",

},

\"problemMatcher\": \"\$msCompile\"

}

\]

}

Make sure you save the *tasks.json* file after editing.

### Running the test task

Once you have defined the test task you can run it from the Command
Palette as follows:

-   Open the Command Palette (*CTRL+SHIFT+P*).

-   Type *test* and select *Tasks: Run Test Task* in the list of
    matches.

-   Hit Enter to run the task.

The output is the same as when you run *dotnet test* manually.

### Creating a Shortcut for the Run Test Task 

If even typing the task in the Command Palette is too much work for you,
you can add a custom keyboard shortcut to select the *Run Test Task*:

-   Open the *Keyboard Shortcuts* preferences (*CTRL+K*, *CTRL+S*).

-   Type *run test task* in the *Search keybindings* field. *Run Test
    Task* appears, and it should not have any key bindings set yet.

-   Edit the key binding (*CTRL+K*, *CTRL+K* or double-click) and enter
    a key binding which will activate the *Run Test Task*. You will be
    notified when you specify a key binding that is already in use.
    We've chosen *CTRL+ALT+T*.

-   Save the key bindings file.

-   Now type the new key binding (*CTRL+ALT+T)* and you\'ll see the *Run
    Test Task* appear in the Command Palette. Note that it doesn't
    execute the task, so you need to hit Enter to run it.

## 

![](./media/image3.png)


Figure 2 Functionality, provided by the C# extension, to run and debug
unit tests

All tests can be run by selecting the *Run All Tests* and *Debug All
Tests* links above the class declaration. Individual tests can be run by
selecting the *Run Test* and *Debug Test* links above the unit test
method signatures (see Figure 2).

## 

## Test Explorer

The final method to run unit tests that we'll cover in this article uses
a VS Code extension named .NET Core Test Explorer.

![](./media/image4.png)


Figure 3 .NET Core Test Explorer Extension

As you can see in Figure 3 we have version 0.5.2 installed. There have
been three releases in this month alone (August 2018) so there is a lot
of active development going on. That is one of the reasons this
extension is growing in popularity. As with any 0.x release there are
some minor shortcomings to this extension. One of these things is that
the UI only updates if you start the tests from the extension. If you
run dotnet test manually, or by pressing "Run Test" above your test
method signature, it will still display a green check in the UI even if
the test failed. Also, the detection of (new) unit tests isn't as fancy
as in Visual Studio, you have to press the refresh button manually.

### Unit test discovery

Once this extension is installed and you have opened the TicTacToe
repository you need to build the test project for the tests to be
discovered by the Test Explorer.

-   Click the flask icon to open the Test Explorer (see Figure 4).

-   Click the Refresh button in the top right of this window.

-   If you don't see any tests yet, ensure that the
    *dotnet-test-explorer.testProjectPath* setting is set to the correct
    value to locate the unit test project.

    -   Go to *Settings* (*CTRL+,*) to verify this value. For the
        TicTacToe tests this workspace setting is defined as follows:\
        \
        dotnet-test-explorer.testProjectPath\": \"\*\*/\*.Tests.csproj\"

    -   The unit tests should now be visible in the Test Explorer.

![](./media/image5.png)


Figure 4 Test Explorer showing unit tests

### Running the Tests

Click the play icon in the top right corner (see Figure 4 above) to run
all unit tests, or use shortcut *ALT+R*, *ALT+A*.

If you need to run a selection of unit tests, based on trait values or
project names, you can use the *dotnet-test-explorer.testArguments*
setting. For instance, if you only want to run tests with a specific
Trait, e.g. \[Trait("Category", "Strategy")\], specify the following
setting in either the user or workspace settings:

\"dotnet-test-explorer.testArguments\": \"\--filter Category=Strategy\"

Of course, this argument can also be passed to the CLI. For more
information on how to use the \--filter switch see the Microsoft
documentation[^12].

The output of the Test Explorer (visible under the *.NET Test log* in
the output window) is similar to the plain dotnet test output, but it
also contains logging to a test results file (trx), which the Test
Explorer uses:

Executing dotnet test \--logger
\"trx;LogFileName=\<USER_PATH\>\\AppData\\Local\\Temp\\test-explorer\\0.trx\"
in \<LOCAL_REPO_PATH\>/TicTacToe.Tests

Build started, please wait\...

Build completed.

Test run for
\<LOCAL_REPO_PATH\>\\TicTacToe.Tests\\bin\\Debug\\netcoreapp2.1\\TicTacToe.Tests.dll(.NETCoreApp,Version=v2.1)

Microsoft (R) Test Execution Command Line Tool Version 15.8.0

Copyright (c) Microsoft Corporation. All rights reserved.

Starting test execution, please wait\...

WARNING: Overwriting results file:
\<USER_PATH\>\\AppData\\Local\\Temp\\test-explorer\\0.trx

Results File: \<USER_PATH\>\\AppData\\Local\\Temp\\test-explorer\\0.trx

Total tests: 9. Passed: 9. Failed: 0. Skipped: 0.

Test Run Successful.

Test execution time: 1.8404 Seconds

# Test Coverage

The ability to see how much of your code is being covered by tests is
useful. You can use this to check if you missed any testing scenarios.
It's also a common metric to review against: "new code should have at
least X % code coverage".

## Code coverage percentage as a metric

Just checking for at least X% code coverage is not recommended -- it
says very little about the quality of your code and your tests. Also
make sure your unit tests are being reviewed and that they are useful:
check whether assertions are in place, make sure only one scenario at a
time is being tested, and if dependencies are mocked or stubbed; also
check whether tests follow the Arrange-Act-Assert pattern and that the
test names are self-explanatory, etc.

Viewing unit test coverage in .NET Core has been a bit tricky in the
past, but with a few libraries and add-ons this has become a lot easier.
We're going to use Coverlet[^13] and Coverage Gutters[^14] for this.

Installing Coverlet can be done in two ways: as a global tool or as a
NuGet package in your test projects:

-   dotnet tool install \--global coverlet.console

-   dotnet add TicTacToe.Tests package coverlet.msbuild

As we don't want to force every developer to install the global tool, we
recommend the NuGet package. After installing Coverlet and Coverage
Gutters, it's easy to get code coverage going. Just specify some extra
arguments when executing "dotnet test":

-   dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=lcov
    /p:CoverletOutput=./lcov.info\"

This generates a "lcov.info" file in the root of your unit test
directory that corresponds with the default settings for Coverage
Gutters, so when you activate that extension the coverage should be
visible immediately.

Of course, it's possible and recommended to add these parameters to your
tasks.json file too. We recommend adding it to the
\"*dotnet-test-explorer.testArguments*\" parameter as well so you'll
always have the code coverage within reach: just press the "watch"
button in your VS Code taskbar[^15]. This will visualize the code
coverage and even update it when you rerun your tests and new coverage
data is available. It should look like this by default:

![](./media/image6.png)


Figure 5 Default visualization of code coverage by Code Gutters

However, as you can see, the indication for code coverage is in the same
place as where you would normally set your breakpoints. If you want to
visualize the code coverage and use breakpoints at the same time,
disable the gutter coverage and enable the line coverage via the
settings: "*coverage-gutters.showGutterCoverage*" and
"*coverage-gutters.showLineCoverage*" respectively. This will highlight
the entire code line, something we prefer anyway.

![](./media/image7.png)

Figure 6 Alternative visualization of code coverage by Coverage Gutters,
enabling the use of breakpoints

You can read more on this topic on Scott Hanselman's blogpost[^16].

# Finally

As you've seen in this article, .NET Core development and unit testing
in VS Code works well. Since the major unit test frameworks are .NET
Core compatible, this is no excuse for not writing unit tests for your
.NET Core code. Also, the tasks in VS Code are very flexible and easy to
use so there's little need to use the dotnet CLI directly. The test
explorer and coverage tooling are not as feature-complete yet as their
Visual Studio IDE counterparts, but it is definitely workable.

In a follow-up article, we'd like to dig a bit deeper into unit testing
with VS Code, especially how to handle larger solutions with multiple
test projects.

Please let us know if you have specific questions regarding unit testing
.NET Core projects in VS Code so we can address your issue in the next
article.

# Sidenote

Other extensions we can recommend include:

-   *Azure CLI Tools* for developing and running commands with the Azure
    CLI.

-   The *Docker* extension makes it easy to build, manage and deploy
    containerized applications from Visual Studio Code.

-   *GitLens* supercharges the Git capabilities built into Visual Studio
    Code. It helps you to visualize code authorship

-   *REST Client* allows you to send HTTP requests and view the response
    in Visual Studio Code directly.

-   *Visual Studio Live Share* allows you to collaboratively edit and
    debug with others in real time.

[^1]: Code Complete, 2^nd^ Edition, Steve McConnell

    [Error Cost Escalation Through the Project Life
    Cycle](https://ntrs.nasa.gov/archive/nasa/casi.ntrs.nasa.gov/20100036670.pdf),
    NASA

[^2]: [https://code.visualstudio.com](https://code#.visualstudio.com)

[^3]: <https://microsoft.com/net/download>

[^4]: <https://marketplace.visualstudio.com>

[^5]: <https://xunit.github.io/>

[^6]: <https://nsubstitute.github.io/>

[^7]: <https://fluentassertions.com/>

[^8]: <https://github.com/AutoFixture/AutoFixture>

[^9]: If your shortcuts aren't behaving as expected, they might have
    been overwritten by global Windows 10 shortcuts. This seems to
    happen often with "control-tilde". As this can be done by any app,
    we can't provide any advice on how to fix this, other than removing
    the application that triggers when you press the shortcut.

[^10]: [https://github.com/XpiritBV/unit
    testing-dotnetcore-vscode](https://github.com/XpiritBV/unittesting-dotnetcore-vscode)

[^11]: [https://github.com/XpiritBV/unit
    testing-dotnetcore-vscode](https://github.com/XpiritBV/unittesting-dotnetcore-vscode)

[^12]: <https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-test?tabs=netcore21#filter-option-details>

[^13]: <https://github.com/tonerdo/coverlet>

[^14]: <https://marketplace.visualstudio.com/items?itemName=ryanluker.vscode-coverage-gutters>

[^15]: The default shortcuts for this are 'Control-Shift-8' to enable
    watching and 'Control-Shift-9' to disable watching. So we recommend
    remapping this or just clicking the button.

[^16]: [https://www.hanselman.com/blog/AutomaticUnit
    testingInNETCorePlusCodeCoverageInVisualStudioCode.aspx](https://www.hanselman.com/blog/AutomaticUnitTestingInNETCorePlusCodeCoverageInVisualStudioCode.aspx)
