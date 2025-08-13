Real world mocking! Http Service testing in C# using Wiremock.Net

Authors: Kristof Riebbels and Bas van de Sande

Writing tests is hard by itself, and often it is forgotten that there is
a need for different kinds of tests. There are unit tests that cover
specific functionality and are scoped clearly. There are system tests
that cover many functionalities but replace the external systems with
fake ones. There are also Integration tests - which are like system
tests - but this time external systems are involved in the test
process.\
\
Maintaining Integration tests is hard as there are a lot of dependencies
not necessarily under your control. Furthermore, it is not easy to
incorporate the integration tests into the CI-pipeline.

Each type of test has its own difficulties. In this article the tests
that are going to be discussed are system and integration tests.
WireMock.Net is going to play an important role in converting the hard
to maintain integration tests into controllable system tests and vice
versa.

## What is WireMock.Net?

WireMock.Net is a GitHub community project **[\[1\]]{.mark}** and
contains the C# implementation of mock4net which mimics functionality
from the JAVA based WireMock.org.

The idea behind WireMock.Net is to mimic the behaviour of a real-life
HTTP API. HTTP requests, that are made from the code that is tested, are
captured and sent to a WireMock.Net HTTP server (which is part of the
testing framework) and as a result an HTTP response is returned that can
be verified against an expected behaviour.

## Which features are offered by WireMock.Net?

One of the most interesting features of WireMock.Net is it can be used
in Test projects. It can record and playback captured messages. In
integration tests WireMock.Net can be set up to act as a proxy in order
to capture and/or forward the HTTP requests. WireMock.Net can also be
configured to give the matching response when it sees similar requests.
This means integration tests can be turned into system tests and vice
versa. Of course, assertions can be written for those incoming requests.

In case the requests are too dynamic, request matching is the technique
used to generalize incoming requests. Requests can be matched by URL,
path, request method, request header, cookies and/or request body.

WireMock.Net can also be used when manual testing is needed but when the
dependencies are not ready or are unavailable on the testing
environment. It can be run as a standalone tool.

## Hello world! 

With the understanding of what WireMock.Net is all about, let's see how
it can be set up using XUnit.

The example below describes a simple test method that sets up a
wiremock.net server. It defines a request and a response. When a
Get-request with path "/foo" is sent to the WireMock-server, a response
will be created with status code OK and with content "bar".

public class GivenAWireMockServer

{

\[Fact\]

\[Trait(\"Category\",\"SystemTests\")\]

public async Task WhenSendingAGetRequestTo_foo_ReceiveAResponse_bar()

{

//Arrange

var wireMockServer = WireMock.Server.WireMockServer.Start();

wireMockServer.Given(Request.Create()

.UsingGet()

.WithPath(\"/hello\")

).RespondWith(Response.Create()

.WithStatusCode(HttpStatusCode.OK)

.WithBody(\"world\"));

var httpClient = wireMockServer.CreateClient();

//Act

var barResponse = await httpClient.GetAsync(\"hello\");

var body = await barResponse.Content.ReadAsStringAsync();

//Assert

Assert.Equal(HttpStatusCode.OK, barResponse.StatusCode);

Assert.Equal(\"world\", body);

}

}

Of course, the scenario above is to show the simplicity of setting up a
test. Let\`s see how it can be used as an integration test.

## Test scenario

The following code represents a WeatherForecastController with a Get
method. Browsing to <http://localhost:3011/weather>forecast results in
one of the following messages: "Too hot", "Cozy", "Cold" or "Too cold".\
\
The method consumes a weather forecasting service
[https://api.open-meteo.com/v1/forecast](https://api.open-meteo.com/v1/forecast?latitude=50.8371&longitude=4.3676&daily=temperature_2m_max&timezone=Europe%2FBerlin&start_date=2022-08-01&end_date=2022-08-01).
In order to consume the service, the WeatherForecastController is
dependent on the IOpenMeteoClient which depends on the
IHttpClientFactory.\
\
The httpClientFactory creates an HttpClient that will get the forecast
from the weather forecasting service. Based on the result, the method in
the WeatherForecastController returns one of the messages defined above.
The OpenMeteoClient is actually a proxy that helps to hide the fact an
HttpClientFactory is used.

## Define the first integration test

Before an integration test can be defined, it needs to know what
messages and responses go back and forth over the line. Fiddler can be
used to intercept the traffic from the WeatherForecastController to the
open-meteo API.

The actual request and response look like the following:

**request**

GET
<https://api.open-meteo.com/v1/forecast?latitude=51.09&longitude=4.06&daily=temperature_2m_max,temperature_2m_min&current_weather=true&timezone=Europe%2FBerlin&start_date=2022-07-31&end_date=2022-07-31>
HTTP/1.1

Host: api.open-meteo.com

traceparent: 00-1e25de5aeaa91dba70b47f8679ea7dc9-d74d7ed281f40bdb-00

**response**

HTTP/1.1 200 OK

Date: Sun, 31 Jul 2022 15:13:41 GMT

Content-Type: application/json; charset=utf-8

Transfer-Encoding: chunked

Connection: keep-alive

{"latitude":53.1,"longitude":5.06,"generationtime_ms":0.38301944732666016,"utc_offset_seconds":7200,"elevation":3.0,"current_weather":{"temperature":22.5,"windspeed":22.8,"winddirection":249.0,"weathercode":3.0,"time":"2022-07-31T17:00"},"daily_units":{"time":"iso8601","temperature_2m_max":"°C","temperature_2m_min":"°C"},"daily":{"time":\["2022-07-31"\],"temperature_2m_max":\[23.6\],"temperature_2m_min":\[17.0\]}}

When using WireMock.Net to do the integration test, a mocked
IhttpClientFactory is set up to return the HttpClient of the embedded
WireMock.Net Server. The WireMock.Net Server runs in the same process
but it is reachable from the "outside" as well.

A basic integration test of the Get method on the
WeatherForecastController from the test scenario, can look like this:

\[Fact\]

\[Trait("Category","IntegrationTests")\]

public async Task
WhenRequestingCurrentWeatherInformation_DateShouldBeUtcToday()

{

//Arrange

var application = new WebApplicationFactory\<Program\>()

.WithWebHostBuilder(builder =\>

{

builder.ConfigureServices(

services =\> services.AddConfiguredServices());

});

//Act

var httpClient = application.CreateClient();

//Assert

var response = await httpClient.GetAsync("/WeatherForecast");

Assert.Equal(HttpStatusCode.OK, response.StatusCode);

var weather = await
response.Content.ReadFromJsonAsync\<WeatherForecast\>();

\...

}

This integration test tests if a filled WeatherForecast response was
returned from the API and if the date of the weather forecast matches
with the local system date.

## Integration test: using the Request Matcher

The code below shows setting up the request matcher and defining the
corresponding response. The matching is set up to be fairly generic but
can be set up as strictly as desired.

public async Task
WhenRequestingCurrentWeatherInformation_DateShouldBeUtcToday()

{

//Arrange

var openMeteoWireMockServer = WireMock.Server.WireMockServer.Start();

openMeteoWireMockServer.Given(Request.Create()

.UsingGet()

.WithPath(path =\> path.Contains(\"forecast\"))

).RespondWith(Response.Create()

.WithStatusCode(HttpStatusCode.OK)

.WithBody(\...\"current_weather\\\":{\\\"temperature\\\":22.5,\\\"windspeed\\\":22.8,\\\"winddirection\\\":249.0,\\\"weathercode\\\":3.0,\\\"time\\\":\\\"2022-07-31T17:00\"\...));

var openMeteoHttpClient = openMeteoWireMockServer.CreateClient();

var fakeHttpClientFactory = new Fake\<IHttpClientFactory\>( );

fakeHttpClientFactory.CallsTo(httpClientFactory =\>
httpClientFactory.CreateClient("OpenMeteo"))

.Returns(openMeteoHttpClient);

var application = new WebApplicationFactory\<Program\>()

.WithWebHostBuilder(builder =\>

{

builder.ConfigureServices(

services =\>

{

services.AddConfiguredServices();

services.AddScoped(provider =\> fakeHttpClientFactory.FakedObject);

});

});

//Act

var httpClient = application.CreateClient();

//Assert

var response = await httpClient.GetAsync(\"/WeatherForecast\");

Assert.Equal(HttpStatusCode.OK, response.StatusCode);

var weather = await
response.Content.ReadFromJsonAsync\<WeatherForecast\>();

\...

}

Open Fiddler, execute the test and notice there is a call from the
WeatherForcastController to the open-meteo API. Instead of accessing the
original host at the Url
"*https://api.open-meteo.com/v1/forecast?lattitude=51\..." ,* the host
being called is the WireMock.Net server, running at localhost listening
to port 49894. The Url contains the part "forecast" and that will result
in a Http status code 200, with the Http body that was defined in the
test.

![Graphical user interface, text, application, chat or text message
Description automatically
generated](./media/image1.png)


## Integration test: working with recorded messages

A new test is set up using recorded messages. In this scenario
WireMock.Net will play these messages back if the matching request comes
in. Fiddler is used as a proxy server to monitor what is happening.

In order to prepare the test to work with recorded messages, the
response messages need to be captured initially. The code below shows
how to record the messages. The recorded messages are stored in the
local debug folder.

//Arrange

var openMeteoWireMockServer = WireMock.Server.WireMockServer.Start(

new WireMockServerSettings()

{

ProxyAndRecordSettings = new ProxyAndRecordSettings()

{

Url = \"https://api.open-meteo.com\",

SaveMapping = true,

SaveMappingToFile = true,

WebProxySettings = new WebProxySettings()

{

Address = \"127.0.0.1:8888\"

},

ExcludedHeaders = new string\[\]{\"Host\", \"traceparent\" }

},

StartAdminInterface = true,

FileSystemHandler = new LocalFileSystemHandler(\".\")

});

var openMeteoHttpClient = openMeteoWireMockServer.CreateClient();

Open Fiddler and execute the test. Notice there is an actual call
forwarded to the open-meteo API.

![Graphical user interface, text, application, chat or text message,
email Description automatically
generated](./media/image2.png)


The mappings are recorded in the project's debug folder as shown below.

![Graphical user interface, application, Word Description automatically
generated](./media/image3.png)


Once the initial recording is done, the playback mechanism can be used
as shown in the example below. For more advanced use cases, the mapping
model can be opened, split or organized. In the example below
WireMock.Net will handle the mapping model the way it was recorded.

//Arrange

var openMeteoWireMockServer = WireMock.Server.WireMockServer.Start(

new WireMockServerSettings()

{

ReadStaticMappings = true

});

//Act

## var openMeteoHttpClient = openMeteoWireMockServer.CreateClient(); 

\...

//Assert

\...

## Using WireMock.Net in a CI pipeline

WireMock.Net can be used for automated testing in GitHub actions
workflows as well. In the pipeline example below a simple GitHub actions
workflow is setup that will restore, build and test the code. The
following YAML is an example.

  ------------------------------------------------------------------------
  name:   
  .NET    
  ------- ----------------------------------------------------------------
          on:

          push:

          branches: \[ \"main\" \]

          pull_request:

          branches: \[ \"main\" \]

          jobs:

          build:

          runs-on: ubuntu-latest

          steps:

          \- uses: actions/checkout@v3

          \- name: Setup .NET

          uses: actions/setup-dotnet@v2

          with:

          dotnet-version: 6.0.x

          \- name: Restore dependencies

          run: dotnet restore ./src/WiremockSamples.sln

          \- name: Build

          run: dotnet build ./src/WiremockSamples.sln \--no-restore

          \- name: Test

          run: dotnet test ./src/WiremockSamples.sln \--no-build
          \--verbosity normal \--filter \"Category=SystemTests\"
  ------------------------------------------------------------------------

The tests have the attribute Trait with key "Category" and value
"IntegrationTests" or "SystemTests". When running all tests, the
pipeline will fail because the IntegrationTests are actually trying to
contact the actual open-meteo API. This API cannot be reached from the
the CI server. In order to overcome this, a filter is applied to ensure
that only "SystemTests" are executed.

Let's see what happens if the pipeline has been executed. The pipeline
results show that the system test ran successfully.

## ![Text Description automatically generated](./media/image4.png) 

## Summary

This article just scratches the surface when it comes to testing with
WinMock.Net. There is much more to discover with WireMock.Net, but the
above will help to setup integration and system tests in any .NET
project using Http based services.\
\
The WireMock.Net Wiki **[\[2\]]{.mark}** pages at GitHub are
self-explanatory. The repository contains many samples covering the most
common scenarios. The ease of use to test Http based services with real
requests and responses makes it worthwhile to revaluate existing unit
tests. Instead of mocking entire functions just the internal Http
requests and responses can be mocked, giving an additional level of
control in the software testing process. The sources in this article are
shared on GitHub as well **[\[3\]]{.mark}**.

References:

\[1\] <https://github.com/WireMock-Net/WireMock.Net>

\[2\] <https://github.com/WireMock-Net/WireMock.Net/wiki>

\[3\] <https://github.com/kriebb/Wiremock.net>
