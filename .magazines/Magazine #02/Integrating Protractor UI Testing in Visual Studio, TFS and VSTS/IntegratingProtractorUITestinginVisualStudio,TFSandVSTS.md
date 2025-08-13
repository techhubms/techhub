# Integrating Protractor UI testing in Visual Studio, TFS and VSTS

Marcel de Vries

Automating software development helps to minimize the development cycle
time, and in trying to achieve this, it makes a lot of sense to use the
most suitable and effective tools. Out of the box solutions offer
several options to test your software. For example MSTest for unit
testing, CodedUI for automated UI testing, etc. With the growth of test
automation, a huge open source eco system of test tools has become
available that have specific advantages when using specific frameworks
in applications. This article will show how you can integrate any test
tool by adding a little piece of glue code called a test adapter.

## Integrating a popular AngularJS UI test tool Protractor

Many applications are built that use AngularJS as the UI framework of
choice. When these applications need to be tested, a large number of
options is available. You can choose to test the UI using Microsoft
CodedUI that is part of Visual Studio, Team Foundation Server (TFS) and
Visual Studio Team Services (VSTS). But when you build a user interface
with AngularJS, you may prefer to write your tests in the same language
as your UI. One of the frameworks that supports this is Protractor.
However, Protractor is not with a standard component of Visual Studio
and TFS. So let's have a look at how you can change this and make
Protractor a fully supported test framework by writing a test adapter.
But before you write the adapter, let's take a short look at Protractor.

## Introducing Protractor

Protractor is a popular test framework for building UI tests by writing
a Spec in a JavaScript file. Before you can run these tests, you need to
set up the Protractor toolchain. This toolchain uses Node.js, Jasmine,
Selenium and the Protractor tools. To install all the required
components, you need to install Node.js (downloadable from
<http://nodejs.org>), then you run the following command from the
command line using the Node Package Manager (npm) tools:

npm install -g protractor

This command installs the Protractor tools. The --g flag ensures that
the tools are installed for all users on the machine. As a next step,
you need to ensure you have the latest Selenium web driver tools
installed on your machine. To do so, you can run the following command
from the command line:

webdriver-manager update

After running these commands you have all the tools you need. When you
run Protractor from the command line but a crash occurs, you probably
have not installed the Java Virtual Machine on your computer. If this is
the case, you also need to install the latest JVM from
<http://www.java.com>

Now you are ready to write your first tests. Write Protractor tests in
JavaScript and do this by using the Jasmine framework. A Jasmine test is
defined by specifying a function that can be used by a **describe**
function. Below you see a basic test using the Jasmine framework
(describe function) and Protractor elements (browser object):

[describe(\'Protractor Demo App\', function () {]{.mark}

[it(\'should have a title\', function () {]{.mark}

[browser.get(\'http://juliemr.github.io/protractor-demo/\');]{.mark}

[expect(browser.getTitle()).toEqual(\'Super Calculator\');]{.mark}

[});]{.mark}

[});]{.mark}

This test is called a spec and it can be run using the command line. You
can specify the configuration on the command line. You can also define a
configuration file that defines which browser you want to use to run
this UI test.

To run this test from the command line, you will need to save the file
-- to e.g. firstspec.js -- and then run the following command:

protractor \--specs firstspec.js \--framework jasmine \--browser chrome

This will run the standalone Selenium server and will use Jasmine as the
test framework and the Chrome browser.

The **expect** keyword defines an assertion and will show up in the test
results. The result of this test will be: *1 spec, 1 assertion and 0
failures*. If you specify an additional option -- resultJsonOutputfile
-- you can specify a json file in which the results will be logged.

To learn more about the Protractor framework, follow the tutorials at:
<http://angular.github.io/protractor/#/tutorial>

## Integrating Protractor into VS, TFS and VSTS

Now that you know about some fundamentals of Protractor, the question
remains: how can you integrate this as a first class test framework in
Visual Studio, Team Foundation Server and Visual Studio Team Services in
the cloud? If you would be able to make this work, you would have best
of both worlds.

Fortunately, this is possible by creating a Test Adapter. A Test Adapter
fully integrates in the IDE, the test window and the build system of
TFS, so you can report the test back as part of your build and release
process. Figure 1 shows a conceptual picture of a test adapter.

![](./media/image1.png)


Figure 1: Test adapter

A test adapter provides a common way to discover and run tests. The test
discovery part accepts files and checks whether they contain tests and
if so, the name of those tests and the test executer to be used to
execute those tests. The test Executer also accepts files and knows how
to execute a test it can find in the file. Microsoft introduced these
test adapters to provide the flexibility to use any test tool and enable
any vendor or open source initiative to fully integrate in their
development tools. This is not only limited to Visual Studio, but also
allows tests during the build phase on the build server or in the
various stages that can be defined in the release pipelines using
release management.

## Creating a Test Adapter

You can build a Test Adapter by implementing a set of predefined
interfaces in a Test Adapter assembly. Start with a simple Class Library
Project type, and then add references to the assemblies that define the
interfaces and fundamental types to get the integration to work. You
need to add references to the following assemblies:

-   Microsoft.VisualStudio.TestWindow.Interfaces

-   Microsoft.VisualStudio.TestPlatform.Core

-   Microsoft.VisualStudio.TestPlatform.Common

You can find these assembles at the following location:

%system drive%\\Program Files (x86)\\Microsoft Visual Studio
14.0\\Common7\\IDE\\CommonExtensions\\Microsoft\\TestWindow

Now implement the discovery of tests by creating a class that implements
the interface called **ITestDiscoverer.** This interface only has one
method to be implemented. It is called: **DiscoverTests, and** gets the
following parameters passed when called:

a list of sources, the discovery context, a message logging interface
and the **ITestCaseDiscoverySink**.

The list of sources is a list of files that have the required extension.
To ensure that only the correct file types are passed, specify an
attribute on top of the implementation class. This attribute is called
**FileExtensionAtttribute**, which is passed inthe file extension. In
this case you only want JavaScript files as input, so specify ".js" as
extension.

The implementation of the test discovery is shown in code sample 01:

[\[FileExtension(\".js\")\]]{.mark}

[\[DefaultExecutorUri(]{.mark}\"executor://ProtractorTestExecutor\"[)\]]{.mark}

[public class ProtractorTestDiscoverer : ITestDiscoverer]{.mark}

[{]{.mark}

[public void DiscoverTests(IEnumerable\<string\> sources,]{.mark}

[IDiscoveryContext discoveryContext,]{.mark}

[IMessageLogger logger,]{.mark}

[ITestCaseDiscoverySink discoverySink)]{.mark}

[{]{.mark}

[GetTests(sources, discoverySink);]{.mark}

[}]{.mark}

*Code sample 01*

One additional attribute you see specified on the implementing class is
the **DefaultExecutorUri** attribute. This attribute specifies the
unique Uri for the implementation of a class that knows how to execute
the tests discovered here. This executor Uri is defined in the class
that implements the Executor.

To discover the actual tests, you need to implement the method
**GetTests**, which reads the contents of the source file and then
checks whether the keyword "Describe" can be found. Describe denotes the
start of a test as you have seen in the previous part of this article.
If a test is found, you need to create an instance of a type called
TestCase and pass the name of the test that will be shown in the UI,
together with the actual file location and line number where the test
was found. If the adapter is used inside the Visual Studio IDE, the
discoverySink is available and the test case is sent to this
implementation. The discoverySink will then show the test case in the
IDE test window. The basic implementation of this method is shown in
code sample 02:

[internal static IEnumerable\<TestCase\> GetTests(IEnumerable\<string\>
sources, ITestCaseDiscoverySink discoverySink)]{.mark}

[{]{.mark}

[var tests = new List\<TestCase\>();]{.mark}

[foreach (string source in sources)]{.mark}

[{]{.mark}

[var testNames = GetTestNameFromFile(source);]{.mark}

[foreach (var testName in testNames)]{.mark}

[{]{.mark}

[var testCase = new TestCase(testName.Key,]{.mark}

[ ProtractorTestExecutor.ExecutorUri, source);]{.mark}

[tests.Add(testCase);]{.mark}

[testCase.CodeFilePath = source;]{.mark}

[testCase.LineNumber = testName.Value;]{.mark}

[if (discoverySink != null)]{.mark}

[{]{.mark}

[ discoverySink.SendTestCase(testCase);]{.mark}

[ }]{.mark}

[}]{.mark}

[}]{.mark}

[return tests;]{.mark}

[}]{.mark}

private const string DescribeToken = \"describe(\'\";

[private static Dictionary\<string, int\> GetTestNameFromFile(string
source)]{.mark}

[{]{.mark}

[var testNames = new Dictionary\<string, int\>();]{.mark}

[if (File.Exists(source))]{.mark}

[{]{.mark}

[int lineNumber = 1;]{.mark}

[using (var stream = File.OpenRead(source))]{.mark}

[{]{.mark}

[using (var textReader = new StreamReader(stream))]{.mark}

[{]{.mark}

[while (!textReader.EndOfStream)]{.mark}

[{]{.mark}

[var resultLine = textReader.ReadLine();]{.mark}

[if (resultLine != null && resultLine.Contains(DescribeToken))]{.mark}

[{]{.mark}

[var name = GetNameFromDescribeLine(resultLine);]{.mark}

[testNames.Add(name, lineNumber);]{.mark}

[}]{.mark}

[lineNumber++;]{.mark}

[}]{.mark}

[}]{.mark}

[stream.Close();]{.mark}

[}]{.mark}

[}]{.mark}

[return testNames;]{.mark}

[}]{.mark}

[private static string GetNameFromDescribeLine(string
resultLine)]{.mark}

[{]{.mark}

[//find describe(\']{.mark}

[int startIndex = resultLine.IndexOf(DescribeToken) +
DescribeToken.Length;]{.mark}

[int endOfdescription = resultLine.IndexOf(\"\',\");]{.mark}

[var testname = resultLine.Substring(startIndex, endOfdescription -
startIndex);]{.mark}

[return testname;]{.mark}

[}]{.mark}

Code sample 02

Now you have the foundation for discovering your tests from JavaScript
files. The next step consists of implementing a test. When you select a
test in the IDE, a second interface implementation is needed that will
execute it. To do so, you need to create an implementation of the
interface **ITestExecutor**. This interface consists of **RunTests** and
**Cancel**. **RunTests** has two different overloads, i.e. one where a
reference is created to the raw source files, the other overload accepts
TestCase objects as arguments. Both overloads need to do the same thing:
just run the test using the information already learned about Protractor
tests.

The implementation of the two different method signatures of RunTests is
rather simple. The implementation is shown in code sample 03:

\[ExtensionUri(ProtractorTestExecutor.ExecutorUriString)\]

public class ProtractorTestExecutor : ITestExecutor

{

  
public const string ExecutorUriString = \"executor://ProtractorTestExecutor\";

   public static readonly Uri ExecutorUri = new Uri(ProtractorTestExecutor.ExecutorUriString);

   private bool Cancelled;

[public void RunTests(IEnumerable\<string\> sources, IRunContext
runContext,]{.mark}

[ IFrameworkHandle frameworkHandle)]{.mark}

[{]{.mark}

[ IEnumerable\<TestCase\> tests =
ProtractorTestDiscoverer.GetTests(sources, null);]{.mark}

[ RunTests(tests, runContext, frameworkHandle);]{.mark}

[}]{.mark}

[public void RunTests(IEnumerable\<TestCase\> tests, IRunContext
runContext,]{.mark}

[IFrameworkHandle frameworkHandle)]{.mark}

[{]{.mark}

[ m_cancelled = false;]{.mark}

[ foreach (TestCase test in tests)]{.mark}

[ {]{.mark}

[ if (Cancelled)]{.mark}

[ {]{.mark}

[ break;]{.mark}

[ }]{.mark}

[ frameworkHandle.RecordStart(test);]{.mark}

[]{.mark}

[ var testOutcome = RunExternalTest(test, runContext,
frameworkHandle,test);]{.mark}

[]{.mark}

[ frameworkHandle.RecordResult(testOutcome);]{.mark}

[ }]{.mark}

[}]{.mark}

public void Cancel()

{

    Cancelled = true;

}

}

Code sample 03

When you receive a call to the **RunTests** overload that accepts a list
of sources, you simply call into the previous class that was created to
discover the tests in the source files and that will return all the
**TestCases** found. After that, call into the second method
**RunTests** that accepts the list of **TestCases**.

In the implementation of **RunTests** you need to check whether there
was a call to Cancel while the tests are executed. To do so, use the
flag **Cancelled**. You need to signal the test infrastructure that you
started with the run of a single test, and then run the actual tests.
When this is finished, signal that this was done by calling
**RecordResult** on the **frameworkHandle**. This will show the results
in the IDE or will ensure that you get the test output recorded to the
\*.trx file when you run this from the build infrastructure in TFS.

The final step consists of executing the test. To do so, call the
Protractor command line as shown at the beginning of this article. Then
specify you want the json result file to record the results. Then parse
those results and report this back as the test outcome. The
implementation of the test execution is shown in code sample 04:

[private TestResult RunExternalTest(TestCase test, IRunContext
runContext,]{.mark}

[IFrameworkHandle frameworkHandle, TestCase testCase)]{.mark}

[{]{.mark}

[var resultFile = RunProtractor (test, runContext,
frameworkHandle);]{.mark}

[var testResult = GetResultsFromJsonResultFile(resultFile,
testCase);]{.mark}

[return testResult;]{.mark}

[}]{.mark}

[public static TestResult GetResultsFromJsonResultFile(string
resultFile,]{.mark}

[TestCase testCase)]{.mark}

[{]{.mark}

[var jsonResult = \"\";]{.mark}

[if (File.Exists(resultFile))]{.mark}

[{]{.mark}

[using (var stream = File.OpenRead(resultFile))]{.mark}

[{]{.mark}

[using (var textReader = new StreamReader(stream))]{.mark}

[{]{.mark}

[jsonResult = textReader.ReadToEnd();]{.mark}

[}]{.mark}

[}]{.mark}

[}]{.mark}

[var results =
JsonConvert.DeserializeObject\<List\<ProtractorResult\>\>(jsonResult);]{.mark}

[var resultOutCome = new TestResult(testCase);]{.mark}

[resultOutCome.Outcome = TestOutcome.Passed;]{.mark}

[foreach (var result in results)]{.mark}

[{]{.mark}

[foreach (var assert in result.assertions)]{.mark}

[{]{.mark}

[if (!assert.passed)]{.mark}

[{]{.mark}

[resultOutCome.Outcome = TestOutcome.Failed;]{.mark}

[resultOutCome.ErrorStackTrace = assert.stackTrace;]{.mark}

[resultOutCome.ErrorMessage = assert.errorMsg;]{.mark}

[break;]{.mark}

[}]{.mark}

[}]{.mark}

[}]{.mark}

[return resultOutCome;]{.mark}

[}]{.mark}

[private string RunProtractor(TestCase test, IRunContext
runContext,]{.mark}

[IFrameworkHandle frameworkHandle)]{.mark}

[{]{.mark}

[var resultFile = Path.GetFileNameWithoutExtension(test.Source);]{.mark}

[resultFile += \".result.json\";]{.mark}

[resultFile = Path.Combine(Path.GetTempPath(), resultFile);]{.mark}

[]{.mark}

[ProcessStartInfo info = new ProcessStartInfo()]{.mark}

[{]{.mark}

[ Arguments = string.Format(\"\--resultJsonOutputFile \\\"{0}\\\"
\--specs \\\"{1}\\\" +]{.mark}

[\" \--framework jasmine\", resultFile, test.Source),]{.mark}

[FileName = \"protractor.cmd\"]{.mark}

[};]{.mark}

[Process p = new Process();]{.mark}

[p.StartInfo = info;]{.mark}

[p.Start();]{.mark}

[p.WaitForExit();]{.mark}

[return resultFile;]{.mark}

[}]{.mark}

Code sample 04

## Additional implementation required for non dll based tests

Since the Protractor spec files are JavaScript files, these files are
not tracked by default inside the Visual Studio IDE. To do so, you need
to implement some additional infrastructure using Shell Interop and you
need to watch the files for changes. This implementation falls beyond
the scope of this article, but what you have seen thus far is the full
implementation of the real adapter part. This additional work is only
needed for Visual Studio Integration and the details can be found in the
implementation that you can find at the GitHub repo
(http://bit.ly/ProtractorAdapter) , where the full implementation of the
Protractor adapter is published. The adapter is available as a NuGet
package and as a VSIX integration package for full support in the Visual
Studio IDE.

## Deploying the Test Adapter in Visual Studio

The test adapter itself is something you can deploy as a NuGet Package
and then upload to Nuget.org. This is also what I have done with the
Protractor Adapter. I published it as a package on Nuget with the name
ProtractorTestAdapter. If you want to use the test adapter in any of
your projects, you have to get a reference to the implementation from
NuGet. To test the adapter that was just created, you can create a
simple web application and from there install the NuGet package with the
following package command:

Install-Package ProtractorTestAdapter

This will add two references to the project. One that is the
Protractor.TestAdapter assembly and the second one is the Json.net
assembly -- this is required because the adapter depends on it for
result parsing.

When you build the adapter, you will also find a VSIX package. To get
full support in your IDE for the Protractor spec files, you will also
need to install this VSIX package.

Now you can create a JavaScript file and the adapter will automatically
discover the specs from the files.

The following screenshot shows a simple web application project with one
spec file that specifies the test of the Angular home page:

![](./media/image2.png)

If you click the Run All option in the test window, you will see the
Protractor test run, while the results are reported back to the test
explorer window:

![](./media/image3.png)

So now you have Protractor spec files as first class citizens in your
Visual Studio IDE.

Because you created a test adapter and provided it as a NuGet Package,
it now also automatically integrates into the build infrastructure.

## Using the Test Adapter in TFS and VSTS builds

If you commit the ProtractorValidation project to your version control
repository, you can define a Build on the TFS server. To show how this
is done, I am using Visual Studio Team Services, but this also works for
TFS on premises.

To define a minimum build that can run your tests, you need to get the
sources, then build the solution and finally run the tests. The build
definition is as follows:

![](./media/image4.png)

You can see that for the Visual Studio Test step, you had to specify
that you are interested in the test files that are JavaScript files.
This is why you replaced the default search for dll's by a search for
JavaScript files.

In the advanced options you need to specify that you want to get the
test adapter you referenced using NuGet. Therefore specify a path to
Custom Test Adapters to point to the packages folder. This will do a
recursive search in all packages downloaded from NuGet. This includes
the custom adapter that will be used. In order to successfully execute
the tests, you need a build agent that can run interactively. This can
be done by configuring a custom build host. This build host can be a
simple Azure machine on which you download the build agent and run the
configure command. Here you can specify that you do not want the build
agent to run as a service and this enables the agent to run
interactively. On the build server you also need to install the
Protractor tools as described in the beginning of this article. Once you
have the interactive build agent registered on your TFS server, you can
run the tests as part of your build. When you run this build on your
custom build server, you will see the following results:

![](./media/image5.png)

## Conclusion

This article demonstrated how you can turn a very popular framework for
UI testing of Angular websites into an integral part of the daily tools
you use. By using the standard extensibility options of Visual Studio,
Team Foundation Server and Visual Studio Team Services, you can build a
so-called custom test adapter that integrates in the Visual Studio IDE
and in the standard build and test infrastructure. Custom Test Adapters
allow you to turn any framework you would like to use in your daily
build and test cycle into a fully integrated experience. This will make
it easy to use these tools and ensures that you don't have to step
outside of your daily flow in the IDE. As you can see, Visual Studio,
the TFS and VSTS ALM tools have come a long way. Instead of dictating
what you need, it is now an open and flexible work environment that can
integrate any test tool you would like to use.
