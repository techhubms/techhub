# Let's Playwright with .NET 6 MVC

Let's start this story with our protagonist, a consultant in the role of a backend developer with a focus on .NET 6 and Azure. Let us call him Mike. Mike likes to deliver quality. He works with automated unit tests and integration tests and ensures those run in the project's CI pipeline. He uses XUnit and NSubstitute, WireMock.Net and FluentAssertions.

Mike faces a new challenge. Due to colleagues leaving the project, he has been asked to deliver a frontend, made in Dotnet 6 MVC. Our consultant has done some research on web development. Mike liked to test his work when he did backend development. 

The transition for Mike to a full-stack developer creates a significant shift in perspective. Mike was only focused on server-side logic and data management. Now he needs to explore **and** grasp the knowledge of web development **and more so** discover the common pitfalls.

The goal of this article is to tell the story of Mike's transition from a backend developer to a full-stack developer. The world seemed simpler for him when testing was just an easy thing to do when you code SOLID in a backend environment. UI testing brings its difficulties to the table. Mike will create a Playwright project and discover how Playwright addresses these difficulties.

Visit the dotnet6mvc-Playwright (see References) repository to play with Playwright.

## The Backend Developer's and Frontend Developer's Perspective

Mike discovers and feels the extra problems that frontend development brings. He is not a frontend developer, but he is willing to learn the world of frontend development. Let us examine what he has learned so far.

In Mike's world, backend developers are creating applications and by using unit tests they ensure those applications are reliable and resilient. Traditionally, integration tests required a lot of setup and a live environment. However, Mike runs integration tests in his CI pipeline by mocking and working with other services! Mocking HTTP calls is effortless these days. Mike mocks code that integrates with non-HTTP services and he uses mocking frameworks for that. Mike uses in-memory databases so he does not depend on a real database. If a live environment still requires QA testing by potential end users, Infrastructure as Code (IaC) comes to the rescue.

Full-stack developers need to focus on handling the client-side code. Some work with VanillaJS, while others work within a framework that needs to be kept up-to-date. They need to investigate what impact that can have on the end-user. There are many different frameworks to choose from and each comes with ups and downs.

When Mike thinks about frontend development, he notices that the development is predominantly done in JavaScript, a language that offers robust support for asynchronous programming. Developers need to think about the order of execution of the code, the size of the payloads they send to the server and retrieve and limit the number of requests. The UI needs to load as fast as possible, so minifying and splitting up scripts is important.

Some frameworks will generate HTML for you. Mike likes type safety and some tools that help him out. He is only interested in ensuring the REST API is protected by a Bearer token and figuring out what kind of authentication he would use to query databases. Now, Mike needs to think about how to identify users using authentication and authorization flows using the OpenID protocol.

Because the frontend is the first thing that users and hackers are confronted with, frontend developers need to ensure their scripting techniques are up to date and the libraries they use are not vulnerable.

Client-side code needs to run in all kinds of browsers. Is it overkill for Mike to use a cloud tool that offers several different browsers to manually test the application on all kinds of devices?

## A little bit of history

Mike wants to understand what happened in the past. He wants to know how the challenges in the past were addressed and where we are today. He believes that understanding the past will help him to understand the present and the future.

In the 1980s and 1990s, as personal computers became more popular, software applications started to have more complex GUIs. This led to the development of automated UI testing tools. These tools allowed testers to record and replay user interactions, making it easier to test complex interfaces.

In the 2000s, when web applications came to be, UI testing evolved again. Web applications have more complex interfaces and are more dynamic than traditional desktop applications. This led to the development of more advanced UI testing tools, like Selenium, that can interact with web elements.

In recent years, with the rise of mobile applications, UI testing has had to adapt yet again. Mobile applications have different interfaces and interaction patterns than web or desktop applications. This has led to the development of new UI testing tools that are designed specifically for mobile applications.

With the rise of working with packages, a frontend developer is capable of reusing components. This means unit testing can be used to test individual components: individual buttons, forms, or other UI elements.

By adding tests for those components, integration tests can be used to test the interaction between components.

## Common Problems in UI Testing

Mike went to a conference with some colleagues and attended a talk about UI testing. After that session, he listened closely to what others had to say about the difficulties in UI testing. Below you find a list of what he heard.

- Dynamic content, complex interfaces, cross-browser compatibility, resources, timing, interaction and mobile compatibility are some of the common challenges in UI testing.

- Modern web applications often have dynamic content that changes in response to user interactions. This can make it difficult to write tests that are reliable and repeatable. Dynamic content refers to web pages displaying different text, images, or layouts depending on the user, time of day, or the user's device. Inconsistent identifiers such as IDs, names, and classes might not always be unique or consistent across different versions of the web application, making it challenging for the automation script to locate elements accurately.

- To execute tests, there needs to be a system in place to test with. This system needs to be up to date and running in the correct environment. This can be a challenge in itself. This is where the Backend-For-Frontend  (BFF) pattern can help him out. The BFF pattern is a software design pattern that allows developers to create a single backend for one frontend application. This pattern is useful when you have one or multiple frontend applications that need to access the same data or functionality. It can also be used to create a single API for multiple versions of the same frontend application. This way, the backend can easily be mocked out.

- Not all interactions can be tested. If the application communicates with third-party APIs that do not have corresponding data or are not providing a testing environment, it is difficult to test the application.

- Some applications have complex interfaces with many elements. This can make it difficult to write tests that cover all possible user interactions. Some web applications involve complex user interactions such as drag-and-drop, hover menus, or keyboard shortcuts, which can be challenging to automate.

- Different browsers can render web pages in slightly different ways. This can make it difficult to write tests that work correctly on all browsers.

- Mobile devices have different screen sizes and interaction patterns than desktop devices. This can make it difficult to write tests that work correctly on both mobile and desktop devices.

- In Product Development, the continuous evolution and adaptation of products require regular updates and maintenance of UI tests, but the fragility and high maintenance requirements can decrease motivation to develop them. In Project Development, the predefined scope and limited changes can lead to minimal redundancy in UI tests, but strict contracts or tight deadlines may hinder the motivation to write them due to difficulties in modifying tests later on...

## Playwright is the new kid on the block

As Mike deepens his understanding of frontend development, he realizes the benefit of tools like Selenium and Playwright for component testing and end-to-end user interaction simulations. He discovers Selenium to be a well-established framework. Selenium has a reputation for being reliable and versatile. Selenium offers cross-browser testing and supports a large range of programming languages. It facilitates frontend UI testing across actual servers and cloud-based, real-device testing.

Despite Selenium's reputation, Mike finds himself leaning towards Playwright. Developed by Microsoft, Playwright offers certain advantages that appeal to him. Playwright's support for headless browser architecture allows for a quicker feedback cycle. This is a useful (quality of life) feature for a backend developer learning frontend development! Playwright's automatic waiting mechanism reduces the instability in tests. He read about the isolated browser contexts. These isolated contexts let you conduct tests independently without any shared state and simultaneous user logins. Debugging becomes simpler as well. Mike would not need to worry about the residual effects from previous tests.  Playwright can emulate different devices and geolocations. These features allow him to recreate all kinds of user scenarios.

## Enter Playwright

Playwright is an open-source Node library developed by Microsoft that allows developers to automate web browsers over the Chromium, Firefox, and WebKit protocols. It provides capabilities to interact with web pages, evaluate scripts, generate screenshots, and produce PDFs. It's used for end-to-end testing of web applications to ensure their correct functionality across different web browsers.

Playwright evolved from the Puppeteer project. That was limited to Chrome automation. Microsoft's effort with Playwright aim to address the multi-browser scenario, making it possible to run the same tests on different browsers without any code changes. This is a leap forward as many businesses need to ensure their web applications work seamlessly across all major browsers.

The .NET community showed interest in having Playwright's capabilities within their ecosystem. Microsoft recognized this demand and introduced Playwright for .NET, allowing .NET developers to write tests in C#.

Playwright for .NET is a client package that allows communication with the Playwright Node.js Server. Instead of writing Mike's tests in JavaScript, it brings the Playwright API to .NET developers. Because it is a client-server model, Mike has the same underlying browser automation engine and thus he can use the same capabilities.

## Features that make Playwright great

Mike wants to know what he can do with Playwright and discovers the following features on the Playwright website:

- Playwright automates the Chromium, WebKit, and Firefox browsers with a single API to cover all rendering engines.

- Playwright allows testing of how an application behaves on different devices by adjusting the viewport size of the browser.

- Playwright also allows for network throttling, where developers can simulate slow network connections and assess the impact on the application's performance. Using Playwright's built-in network management features, developers can emulate slow or offline network conditions to measure how the application endures under different scenarios.

- To speed up UI testing, developers can employ parallel test execution. Playwright's auto-wait mechanism and support for intercepting network requests make it ideal for testing single-page applications (SPAs). Developers can ensure critical page elements are available and the application is making the expected API calls during navigation and user interactions.

- Playwright enables developers to automate testing form submission and validation.

- Playwright offers to reuse the authentication of the browser, making it easier to test applications.

- Playwright allows you to automate browser interactions, and you can run those headed or headlessly.

- Playwright has some features for capturing screenshots and recording videos of your browser sessions. This will help out when it comes to debugging, documentation, or even visual verification. Combine this strategy with a CI/CD pipeline, and you have more context when a test fails.

## Headed vs Headless

Alright, Mike dives into the terms `headless` and `head` in the context of browsers and unit testing, and Playwright.

When Mike runs a browser in `headed` mode, it means Mike is getting the entire graphical user interface. Mike sees the web pages loading and he can click around — the whole shebang.

In `headless` mode, the browser runs without a GUI. It's all happening in the background, so Mike can't see it, but it's there doing its thing. This is super useful for automated tasks, server environments, or testing scenarios where you don't need the GUI.

In unit testing, `headless` and `headed` usually tie back to how tests run on a browser. When Mike's tests run in a visible browser window (`headed`), he can watch as the browser navigates through the test steps. It's slower but good for debugging.

When the browser remains in the background (`headless`), Mike does not see any GUI. Tests run faster this way, which is ideal for CI/CD pipelines where Mike just wants to know if things pass or fail without the visual overhead.

## Mike's introduction to UI Testing with Playwright in .NET 6 MVC

Let's follow Mike's steps on how to get started with Playwright in .NET 6 MVC using Playwright's documentation.

Mike searched and reused somebody's web application. He found an e-commerce website written in Dotnet 6 MVC. It is a small application where the user needs to be created and be authorized to view, create and/or update a list of products. Mike ensures the application is running and he can access the webpage.

Mike reads that Playwright for .NET works best with NUnit. While Playwright supports other test runners like MSTest, Mike will use NUnit. The Playwright's test runners' key focus is to optimize test performance by reusing Playwright and Browser instances and running each test case in a new `BrowserContext` to isolate browser states.

Playwright does not support the parallelization of tests. By default, NUnit, MSTest and XUnit will run all test files in parallel. Playwright offers support for configuring NUnit and MSTest so each test within a test file is running sequentially. To set up NUnit, there is an option `ParallelScope.Self` to create as many processes as there are cores on the host system. Running tests in parallel using `ParallelScope.All` or `ParallelScope.Fixtures` are not supported.

Mike followed Playwright's tutorial with ease. He copies a test that visits <https://Playwright.dev> and validates the title of the homepage. When he ran the test, he was happy it turned green.

However, he started to question the tool. He did not see anything happen. He knew that the tool had support for creating screenshots. He added a line of code that will take a print screen from the page. The screenshot will be saved in the `bin/Debug/net6.0` folder.

```csharp
await Page.ScreenshotAsync(new PageScreenshotOptions { Path = "image.png" });
```

He ran the test again and saw the screenshot appear in the `bin/Debug/net6.0` folder

![image](./images/image.png)

Mike browsed the debug folder and he noticed Playwright-related files and folders:

- a folder called `.playwright`: This folder contains two other folders that contain NodeJS and the Playwright code

- a file named `playwright.ps1`: A PowerShell file that will execute the method `Program.Main` in the `Microsoft.Playwright.dll`.

- Microsoft Playwright DLLs: The code that ensures communication with the Playwright NodeJs Server.

Because Mike discovered the `.playwright` folder, he became curious about how the code in the test behaves. To understand this process, he needs to understand the architecture. Mike discovered that Playwright communicates all requests through a single web socket connection. That connection stays in place until the test execution is completed. This reduces points of failure and allows commands to be sent quickly over a single connection. Playwright also uses a single browser instance for all tests. That reduces the overhead of creating and destroying browser instances.

![image](./images/ExternalInternalProcessOverview.png)

Mike is now ready to write his tests. He wants to test the application's login page. To achieve that goal, he needs to start the e-commerce application. He read the Microsoft documentation on how to create integration tests and started the application using the `WebApplicationFactory`.

The `WebApplicationFactory` serves as an in-memory host for Mike's web application. What sets the `WebApplicationFactory` apart is its usage of a `DeferredHostBuilder`. The web application is started right before the `HttpClient` is created. The sequences of method calls occurring in `Program.cs` are recorded by the `WebApplicationFactory`, without executing them. This grants Mike the flexibility to override registered services, which is useful to ensure the application and tests do not access third parties (e.g. a database).

Mike creates a `HttpClient` using the `WebApplicationFactory.CreateClient` to access the e-commerce webpage and he retrieves the homepage! When he started to write his first Playwright tests, he noticed the following error message.

```csharp
Message:  Microsoft.Playwright.PlaywrightException : 
net::ERR_CONNECTION_REFUSED at http://localhost/ 
=========================== logs =========================== 
navigating to "http://localhost/", waiting until "load" 
============================================================
```

Searching the internet, Mike discovered that the `WebApplicationFactory` is not a great fit *at this moment*. That class is tightly coupled with the HTTP Server called `TestServer`. The `TestServer` can host our application and is approachable from the `HttpClient` that is created by the method `WebApplicationFactory.CreateDefaultClient`. External processes, such as Playwright, cannot access the e-commerce web application.

One solution is using `Kestrel`. `Kestrel` can expose the application's endpoints and pages. Playwright can then interact with the application. When Mike investigated further, he read some threads on the issues list of dotnet on GitHub. Microsoft will do some refactoring, however, this is not a priority for DotNet 7 and seems it is in triage for DotNet 8 *at this moment*.

Mike creates a class that extends the `WebApplicationFactory` called `PlaywrightCompatibleWebApplicationFactory`. When you look at the code below, Mike noticed the creation of two Hosts.

```csharp
protected override IHost CreateHost(IHostBuilder builder) { 
    try {  
        _hostThatRunsTestServer = builder.Build(); 
        builder.ConfigureWebHost(webHostBuilder => webHostBuilder.UseKestrel()); 
        _hostThatRunsKestrelImpl = builder.Build(); 
        _hostThatRunsKestrelImpl.Start(); 
        var server = _hostThatRunsKestrelImpl.Services.GetRequiredService(); 
        var addresses = server.Features.Get(); 
        ClientOptions.BaseAddress = addresses!.Addresses.Select(x => new Uri(x)).Last(); 
        _hostThatRunsTestServer.Start(); 
        return _hostThatRunsTestServer;
    } 
    catch (Exception e) { 
        _hostThatRunsKestrelImpl?.Dispose(); 
        _hostThatRunsTestServer?.Dispose(); 
        throw; 
    } 
}
```

The application wants to create a `Host` that encapsulates the HTTP server `TestServer`. This means Mike needs to create an extra HTTP server `Kestrel`. Mike needs that because `WebApplicationFactory` exposes a property called `Server`. That property exposes the type `TestServer` and not the type `IServer`. The `TestServer`'s Host is created first.
If Mike configured `Kestrel` on the builder first, then he retrieves an instance of `KestrelImpl`, he cannot return an instance of the type `TestServer`.

![image](./images/testprojectnodejs.png)

Now the application is accessible to the outside world and thus for Playwright, he wanted to write some tests. He started with a simple test that will visit the login page and validate the title of the page. To achieve this, he uses the code generator that Playwright offers. By executing `.\Playwright.ps1 codegen`, a window called `Playwright Inspector` appeared.

In that window, Mike noticed some code it generates for you:

![Code generation on startup](./images/image-1.png)

When Mike clicked on the dropdown box Target, he noticed a list of languages: he can use C#, Java, Python or JavaScript. Mike is not familiar with those other languages, so he will stick with C# and choose NUnit.

![Supported languages](./images/image-3.png)

Mike hovered over the elements on the browser and noticed that Playwright marked the element. Playwrights added a label below the marked element. That label contains the locator to fetch that element. Mike can copy that locator and use it in his tests.

![Hovering with the mice over an element and getting the locator](./images/image-2.png)

Mike clicked and pressed some keys while recording his actions to achieve his first test: *Not able to log in with the wrong credentials.*

```csharp
[Test] 
public async Task MyTest() { 
    await Page.GotoAsync("https://localhost:44304/Identity/Account/Login?ReturnUrl=%2F"); 
    await Page.GetByLabel("Email").ClickAsync(); 
    await Page.GetByLabel("Email").FillAsync("test@test.be"); 
    await Page.GetByLabel("Email").PressAsync("Tab"); 
    await Page.GetByLabel("Password").FillAsync("ABc.123!"); 
    await Page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync(); 
    await Page.GetByText("Invalid login attempt.").ClickAsync(); 
}
```

With some adjustments, Mike manually created two tests from the recorded user interactions using the `Playwright Inspector`.

- One to verify if the redirection is working
  
  ```csharp
  [Test] 
  public async Task WhenProvidingBaseUrl_ShouldRedirectToLoginPage() { 
      await Page.GotoAsync(_webApplicationFactory.ServerAddress); //Should be redirected. 
      await Expect(Page).ToHaveURLAsync(_webApplicationFactory.ServerAddress+"Identity/Account/Login?ReturnUrl=%2F");
  } 
  ```

- One to verify if the login is working when the wrong credentials are provided
  
  ```csharp
  [Test]
  public async Task WhenProvidingWrongCredentials_ShouldRespondWithInvalidLoginAttempt() { 
      await Page.GotoAsync(\_webApplicationFactory.ServerAddress);
      await Page.GetByLabel("Email").ClickAsync(); 
      await Page.GetByLabel("Email").FillAsync("test@test.be"); 
      await Page.GetByLabel("Email").PressAsync("Tab"); 
      await Page.GetByLabel("Password").FillAsync("ABc.123!");
      await Page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
      await Expect(Page.GetByText("Invalid login attempt.")).ToBeVisibleAsync(); 
  } 
  ```

When Mike ran the test, he saw that everything just worked. He wanted to view the actions in the test coming to life before his eyes. Mike followed the documentation and decided to use the `.runsettings` file. He configures Visual Studio by clicking on Test `>` Configure Run Settings `>` Select Solution Wide Settings `>` Select the `.runsettings` file.

By applying the default settings, the browser appeared and he saw the test executing the actions. This is because `Playwright.LaunchOptions.Headless` is set to `false`. The `DEBUG` environment variable is set to `pw:api` to get more information about the API calls that are made.

```xml
<?xml version="1.0" encoding="utf-8"?>
<RunSettings>
  <!-- NUnit adapter -->  
  <NUnit>
    <NumberOfTestWorkers>24</NumberOfTestWorkers>
  </NUnit>
  <!-- General run configuration -->
  <RunConfiguration>
    <EnvironmentVariables>
      <!-- For debugging selectors, it's recommended to set the following environment variable -->
      <DEBUG>pw:api</DEBUG>
    </EnvironmentVariables>
  </RunConfiguration>
  <!-- Playwright -->  
  <Playwright>
    <BrowserName>chromium</BrowserName>
    <ExpectTimeout>5000</ExpectTimeout>
    <LaunchOptions>
      <Headless>false</Headless>
      <Channel>msedge</Channel>
    </LaunchOptions>
  </Playwright>
</RunSettings>
```

Mike added the `PWDEBUG` environment variable with the value `console`. That allowed him to debug the selectors in the console of the browser using the variable `playwright`

![Alt text](./images/image-5.png)

Mike has added the environment variable `PWDEBUG` with value `1`, ran the test and the `Playwright Inspector` opened up. He stepped through using the popular `F10` key. Mike saw the `Playwright Inspector` in action. In this test case, he noticed a problem. The Username `admin@test.be` is already used in a registration.

![Alt text](./images/image-8.png)

For this test, no further help was needed, but he was curious about another tool called Trace Viewer. That tool should help in diagnosing and fixing problems. When recording a trace, it captures a snapshot of the page after every action and records network requests, JavaScript logs, etc. Mike browses the `BrowserContext` using IntelliSense:

![Alt text](./images/image-10.png)

Mike did want to know more about Playwright and how it could help him with automating the authenticating of a user so he could test his creating/editing and listing products. He found two methods that can help him with that. To authenticate, Mike can fill and submit login forms as he did before:

```csharp
await Page.GotoAsync(_webApplicationFactory.ServerAddress);
await Page.GetByLabel("Email").ClickAsync();
await Page.GetByLabel("Email").FillAsync("test@test.be");
await Page.GetByLabel("Email").PressAsync("Tab");
await Page.GetByLabel("Password").FillAsync("ABc.123!");
await Page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
```

Another method is to restore cookies and local storage. Because a test should only do what it states, he liked this functionality. If there is already a test that covers the login functionality, then there is no need to test the same functionality again in another test.

After a successful login, Mike saved the state from the cookies and local storage and reused it instead of logging in each time. The method `BrowserContext.StorageStateAsync` is helpful for that.

Playwright also mentions that Mike can manipulate the `sessionStorage` of your browser. The method `Page.EvaluateAsync` helps you with that.

```csharp
string sessionStorageData = await Page.EvaluateAsync<string>("() => JSON.stringify(window.sessionStorage)");
```

In Playwright's documentation, Mike found code that executes JavaScript when the page is being initialized. It will set the `sessionStorage` when the page is loading.

Mike still had one more splinter in his brain. He found confidence in writing and debugging tests but what about running it in a CI Pipeline? Playwright has a lot of samples on how to use a CI Pipeline on Azure, GitHub or other CI tools. Mike uses the GitHub Actions sample. However, an error occurred: 

`The argument 'bin/Debug/net6.0/playwright.ps1' is not recognized as the name of a script file.`

A quick search on the internet and Mike found a solution. He needed to add the following line to the test project file:

```xml
<PlaywrightPlatform>all</PlaywrightPlatform>
```

He added the path to the test project `dotnet6mvcEcommerce.Playwright.tests/bin/Debug/net6.0/playwright.ps1` as well as updated Powershell.

```yaml
    - run: dotnet tool update --global PowerShell
```

Mike ran the GitHub action again and it worked!

```yaml
name: Ecommerce Playwright Tests
on:
  push:
    branches: [ main, master ]
  pull_request:
    branches: [ main, master ]
jobs:
  test:
    timeout-minutes: 60
    runs-on: ubuntu-latest
    steps:
    - uses: actions/checkout@v3
    - name: Setup dotnet
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: 6.0.x
    - run: dotnet tool update --global PowerShell
    - run: dotnet build
    - name: Ensure browsers are installed
      run: pwsh dotnet6mvcEcommerce.Playwright.tests/bin/Debug/net6.0/playwright.ps1 install --with-deps
    - name: Run your tests
      run: dotnet test
```

## Conclusion

Mike is happy that he has a way to quickly generate code from his interactions. He will use the recording, tracing and debugging features so he can start writing his tests. 

Playwright is a great tool and it is easy to use. Every example and tutorial Mike found on the website just works outside the box!

I hope that you, like Mike, are inspired to give Playwright a try.

When you have feedback, do not hesitate to contact me. We all learn from each other.

## References

- [https://news.ycombinator.com/item?id=27460329](https://news.ycombinator.com/item?id=27460329)
- [https://github.com/microsoft/Playwright](https://github.com/microsoft/Playwright)
- [https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-7.0](https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-7.0)
- [https://blog.martincostello.com/integration-testing-antiforgery-with-application-parts/](https://blog.martincostello.com/integration-testing-antiforgery-with-application-parts/)
- [https://danieldonbavand.com/2022/06/13/using-Playwright-with-the-webapplicationfactory-to-test-a-blazor-application/](https://danieldonbavand.com/2022/06/13/using-Playwright-with-the-webapplicationfactory-to-test-a-blazor-application/)
- [https://github.com/dotnet/aspnetcore/issues/33846](https://github.com/dotnet/aspnetcore/issues/33846)
- https://www.meziantou.net/automated-ui-tests-an-asp-net-core-application-with-Playwright-and-xunit.htm
- https://medium.com/younited-tech-blog/end-to-end-test-a-blazor-app-with-Playwright-part-1-224e8894c0f3
- https://learn.microsoft.com/en-us/visualstudio/test/configure-unit-tests-by-using-a-dot-runsettings-file?view=vs-2022
- [https://github.com/kriebb/dotnet6mvc-Playwright](https://github.com/kriebb/dotnet6mvc-Playwright)
- https://aws.amazon.com/blogs/mobile/backends-for-frontends-pattern
- https://research.aimultiple.com/playwright-vs-selenium/
- https://www.browserstack.com/guide/playwright-vs-selenium
- https://www.linkedin.com/pulse/selenium-master-automation-qualitymatrix/
