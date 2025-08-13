# OpenID Connect: Mocking You

OIDC (OpenID Connect) is a protocol that allows companies to streamline user experiences by centralizing accounts in one place. This allows better security and protection of privacy. Google, Facebook, Microsoft, Auth0... provide OIDC Providers. It makes users' lives easier by having only one username and password and those should be stored securely and thus are less susceptible to data leaks. It contributes towards a more secure internet. Those identity providers add more features to try to ensure that when a user logs in, it truly is the user they claim to be.

Implementing OpenID Connect simplifies sign-in processes across different websites and applications using a single set of login information. This reduces the likelihood of forgotten or insecure passwords and makes sign-up for new services quicker.

Using an OpenID Connect provider involves trusting them with critical personal information: email addresses, passwords, and other sensitive data. These providers prioritize the security of your data as a cornerstone of their operational responsibilities. Their resources and strategies should be devoted to safeguarding it against unauthorized access or breaches. The primary focus of online stores and various other websites is fundamentally different. While they undertake measures to secure user data, their core business objectives are centered around commerce rather than data protection. Their systems are more susceptible to cybersecurity threats.

In the article `Let Us Playwright with .NET 6 MVC`, Mike explored Playwright, a framework enabling integrated UI testing in CI pipelines. However, Mike met issues with broken builds. Session and cookies with authentication information tend to expire. That requires the renewal of the session or login of the user. The out-of-the-box solution of Playwright to capture the authenticated user's context is not ideal as it requires re-authentication and recapturing that context. This makes it unsuitable for automatic CI pipelines.

This article aims to tell how Mike resolved these issues by integrating technologies mentioned in `Mocking your OpenID Connect Provider` and `Let Us Playwright with .NET 6 MVC`. The goal is to see how Mike's web application will behave when, for example, expired tokens are returned or claims that hold important data about the user, like the `id_token`, can have a claim with the name `prefered_language`. The application will use that language value to serve the webpages in that language; the `id_token` has a role-claim that has the value `admin`. When an administrator is logged on, some special settings should be visible. The fun part is it is all possible from our development machine. When developing and testing applications, it speeds up the development to only capture and use the data from third-party services. This also means bug fixing is getting easier: you can influence the data to provoke the bug reported by an end-user. The beauty of it is the middleware is not mocked. The testing and bug finding happens as close as possible to real-life situations.

The idea behind mocking your OpenID provider is to minimize the need to change configuration for testing purposes. This is especially useful when you are developing a web application using third-party libraries. When upgrading or changing those libraries, the tests will have the potential to show failure when other behavior becomes known.

The earlier article `Mock your OpenID Connect Provider` focuses on the two calls that happen towards the provider:

- Getting the well-known OpenID configuration
- Creating a self-signed certificate that exposes
  - the private key for creating a valid JWT towards
  - the public key for the validation of the JWT using the OIDC JWKS Endpoint

However, that article is focused on creating an `access_token`. That `access_token` is a result of the Client Credentials flow used in machine-to-machine communication.

The repository that holds the code for this article can be found on my GitHub page: `https://github.com/kriebb/MockOpenIdConnectIdpAspnetcore`

This article will use OIDC Endpoints to be mocked out more than only the part for validating the tokens. Let us dive into the authorization code flow. The flow is used to log in securely to web applications and to give consent to what data the web application may request from the OIDC Provider.

## OIDC Flows

OpenID Connect protocol supports various authentication flows, each designed for specific purposes. These flows end with the OpenID Connect Provider issuing an `access_token` and depending on the flow, an `id_token`. In a front-end application, the `id_token` is consumed for its claims, while the `access_token` is used for accessing protected resources that the web application needs. An `access_token` is provided in the format of a JWT (JSON Web Token).
Do note that, according to the Oauth2 specs, the `access_token` should not be decoded and be treated as a non-readable token. However, in the industry, it is common that an access token is in the format of a JWT.

## Authorization Code Flow [with PKCE (Proof Key for Code Exchange)]

The Authorization Code Flow is a well-established and recommended flow for web applications performing authentication with an OIDC provider. What has evolved is the addition of PKCE (Proof Key for Code Exchange) to enhance the security of this flow. PKCE adds a layer of security to prevent certain types of attacks, such as authorization code interception attacks. The usage of PKCE is considered best practice, especially for clients that cannot securely store a client secret like public clients such as single-page applications (SPAs) and mobile apps. PKCE replaces the need for a `client_secret` in environments where it cannot be securely stored, ensuring the flow remains secure even for public clients.

Mike talks to another developer about the flow, and they produce the following analogy:

Imagine that Mike decides to buy a concert ticket online. He uses his Google Pay to buy the ticket. Google hides your real payment details and supplies only an alias. That alias can only be used once. Google shows the following message after the payment succeeded:

```text
A virtual Visa account ending in XXXX was used instead of your actual card number.
```

The concert organizers send Mike a ticket in the form of a QR code. This ticket acts like an **authorization code**. It stands for a **promise of entry** but not the final access pass itself. Mike arrives at the concert venue. Mike shows his ticket at the entry and the QR code (the "authorization code") gets scanned. The scanner turned green, meaning the authorization code was declared valid. To **be sure** that Mike is the one who bought the ticket, he has been asked to **present his proof of payment**. Mike shows his Google payment card number. This process is called the **Proof Key for Code Exchange (PKCE)**. It confirms the person who bought the ticket is the one attending and ensures the ticket has not been intercepted or scalped. Once your ticket and payment are verified, Mike receives a special bracelet. In this analogy, the bracelet stands for a token that grants Mike entrance to the concert.

Mike creates the following sequence diagram to understand the flow better:

![](./images/authorizationcodeflowdiagram.svg)

The Authorization Code Flow, particularly with PKCE (Proof Key for Code Exchange), is designed to secure the exchange process further, ensuring the flow is not hijacked or replayed. This flow is for user authentication. This means that the web application will redirect the user to the OpenID provider to request an `authorization_code`. Once the user logs in, the authorization code can be used to request to retrieve the `id_token` and `access_token`. The Authorization Code Flow with PKCE ensures secure transactions by matching the `code_challenge` and `code_verifier`. Only the authentic client that initiated the authorization request can exchange the authorization code for an access token. This reduces the risk of unauthorized access in public and less secure client applications.

Mike follows the OpenID specifications to retrieve the self-created `access_token` and `id_token`, influencing the behavior of his web application. Mike needs to create those tokens. He rereads the above flow and makes adjustments; he maps the word `user` with the `Playwright UI automation`, the `web application` as our `in-memory web application` and the `OIDC Server` will be a `MockedHttpMessageHandler`, injected in the `OpenIdConfigurationManager`.

While Playwright can mock and change network traffic for testing purposes, its capabilities are centered around browser interactions. For API mocking without the browser context, there are different tools designed for API mocking. For simplicity, Mike will reuse the mocked `HttpMessageHandler`, defined in the article `Mocking your OpenID Connect Provider` to simulate the OIDC Server's responses.

![](./images/mockingdiagram.svg)

Mike does not concern himself with the `code_challenge` and `code_verifier` mentioned in the first diagram. This is important when writing an OIDC Server; however, Mike does not intend to evaluate the OIDC libraries. To influence the application, Mike is interested in the `access_token`, `id_token` and `refresh_token`. Those tokens must be signed with a certificate that later can be used to confirm the signature. The `id_token` should have `nonce` provided in the authorization request to protect against a replay attack. A replay attack involves that a token can be issued again, and the user can be impersonated.

Mike rereads the `Let Us Playwright with .NET 6 MVC` along with the article about `Mocking your OpenID Connect Provider`. He downloaded the sources to play around, setting breakpoints. He added a simple web application called the Weather API. The web application shows the name of the logged-in user. It will be the middleware that will auto-redirect the user to the login page of the OpenID Provider. The user logs in and sees a welcome message mentioning his name. The setup to override the configuration using the `WebApplicationFactory` is the same as provided in the article `Mocking your OpenID Connect Provider` and `Let Us Playwright with .NET 6 MVC`.

Mike writes a test to verify the behavior of the web application. The test's purpose is to navigate to the homepage and check the welcome text. The test's execution will pass if the welcome text is displayed and will fail if the welcome text is not displayed.

```csharp

    [Test]
    public async Task WhenWeNavigateToTheHomePage_AWelcomeTextWithTheUserNameIsDisplayed ()
    {
        _fixture.SetAccessTokenParameter((AuthorizationCodeRequestQuery, TokenRequestQuery, IdTokenParameters ) => new AccessTokenParameters());
        _fixture.SetIdTokenParameter((AuthorizationCodeRequestQuery, TokenRequestQuery, IdTokenParameters ) => 
            new IdTokenParameters(
            sub: "Mike",
            nonce: AuthorizationCodeRequestQuery["nonce"]!,
            scope: AuthorizationCodeRequestQuery["scope"]!)
        );

        Page.SetDefaultTimeout(30000);
        Page.SetDefaultNavigationTimeout(30000);

        await Page.GotoAsync($"{SetUpConfig.UiWebApplicationFactory.ServerAddress}weatherappui");

        await Expect(Page.GetByText( "Welcome Mike")).ToBeInViewportAsync();
    }
```

By focusing on the requests and responses of the OIDC Provider, Mike extends the `MockingOpenIdProviderMessageHandler`. This handler will be injected in the `OpenIdConfigurationManager`. In the code snippet below the triple dots (...) show the removal of code that is needed for the solution. However, for this article, it will make it less clear to mention that again. The full code is available in the article `Mock your OpenID Connect provider` or can be found on GitHub. Below you will find new code that is added to the `MockingOpenIdProviderMessageHandler` class. Mike injects a function that will create the needed tokens: `id_token`, `access_token` and `refresh_token`. The function (`Func<...>`) will be called when the Token Endpoint of the OIDC Provider is called. The creation of the tokens is based on the query strings provided in the requests towards the Authorization Endpoint and the Token Endpoint. The tokens should be signed using the private key of the generated self-signed certificate. A scope stands for one or more claims. The claims in the tokens are the ones that are requested using the scope mentioned in the request towards the Authorization Endpoint. The response message will contain the tokens.

```csharp
public sealed class MockingOpenIdProviderMessageHandler : HttpMessageHandler
{
    ...
    private readonly OpenIdConnectDiscoveryDocumentConfiguration _openIdConnectDiscoveryDocumentConfiguration;

    private readonly ConcurrentDictionary<string?, NameValueCollection> _requests = new ();
    private readonly Func<(NameValueCollection AuthorizationCodeRequestQuery, NameValueCollection TokenRequestQuery), (string AccessToken, string IDToken, string RefreshToken)> _tokenFactoryFunc;

    public MockingOpenIdProviderMessageHandler(
        ...
        Func<(NameValueCollection AuthorizationCodeRequestQuery, NameValueCollection TokenRequestQuery), (string AccessToken, string IDToken, string RefreshToken)> tokenFactoryFunc)
    {
        ...
        _tokenFactoryFunc = tokenFactoryFunc;
    }
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        ... See earlier article ...

        if (request.RequestUri.AbsoluteUri.Contains(_openIdConnectDiscoveryDocumentConfiguration.AuthorizationEndpoint))
            return await GetAuthorizationResponseMessage(request);

        if (request.RequestUri.AbsoluteUri.Contains(_openIdConnectDiscoveryDocumentConfiguration.TokenEndpoint))
            return await GetTokenResponseMessage(request);
        ...
    }
```

The first step in the Authorization Code with PKCE flow is that the Authorize Endpoint will receive a `state` and respond to the caller with a redirect request, mentioning an `authorization_code` and that same `state`. Throughout the entire flow, the same `state` will be used. With that authorization code and state, the application creates a new request towards the Token Endpoint to exchange the authorization code for the much-needed tokens.
In the code below, to redirect back to the application, Mike extracts the `redirect_uri` with the `state` and adds a fixed `authorization_code`. The `state` will be used later to retrieve the query parameters supplied in the authorization request.

```csharp
    private async Task<HttpResponseMessage> GetAuthorizationResponseMessage(HttpRequestMessage request)
    {
        var queryString = HttpUtility.ParseQueryString(request.RequestUri?.Query);
        var redirectUri = queryString["redirect_uri"];
        var code = Consts.AuthorizationCode; //better is to generate a unique number. 
        string locationHeader = Uri.UnescapeDataString(redirectUri);
        locationHeader += $"?code={code}&state={state}"; 
        if(!_requests.Contains(code)) //we are not intrested in the entire flow called each time. just the tokens.
        _requests.Add(code, queryString);

        var message = new HttpResponseMessage(HttpStatusCode.Redirect);
        message.Headers.Location =new Uri(locationHeader); 
        return message;
    }
```

The Token Endpoint needs to return an `access_token`, `id_token` and a `refresh_token`. The `TokenFactoryFunc` method creates the tokens based on the query string provided in the request towards the Authorization Endpoint and Token Endpoint.

```csharp
    private async Task<HttpResponseMessage> GetTokenResponseMessage(HttpRequestMessage request)
    {
        var queryString = HttpUtility.ParseQueryString(request.RequestUri?.Query);
        var code = queryString["code"];
        var authorizationCodeQueryString = _requests[code]; //exception when not available. authorize should be called first.
        var state = authorizationCodeQueryString["state"]!; //XSS protection 

        var generatedTokens = _tokenFuncFactory(new(authorizationCodeQueryString!,queryString!));

        var message = new HttpResponseMessage(HttpStatusCode.OK);
        message.Headers.CacheControl = new CacheControlHeaderValue { NoStore = true};

        var tokenMessage = new
        {
            access_token = generatedTokens.AccessToken,
            token_type = "Bearer",
            expires_in = 3600, //tests can play with these values as well to see what happens in the app
            refresh_token = generatedTokens.RefreshToken,
            id_token = generatedTokens.IDToken,
            state = state
        };
        message.Content = JsonContent.Create(tokenMessage, mediaType: MediaTypeHeaderValue.Parse("application/json"));

        return message;
    }
```

The method below will reuse the class `JwtBearerAccessTokenFactory` defined in the article `Mock your OpenID Connect provider`. The `Create` method accepts not only the `AccessTokenParameters` class but the `IdTokenParameters` class as well. The needed data to create the tokens should be supplied in the test below or in the test fixture. The `CreateRefreshToken` method will create a refresh token. The `TokenFactoryFunc` method will return the tokens.

```csharp
    public (string AccessToken, string IDToken, string RefreshToken) TokenFactoryFunc(
        (NameValueCollection AuthorizationCodeRequestQuery, NameValueCollection TokenRequestQuery) arg)
    {
        var accessToken = JwtBearerAccessTokenFactory.Create(AccessTokenParameter(arg.AuthorizationCodeRequestQuery, arg.TokenRequestQuery));
        var idToken = JwtBearerAccessTokenFactory.Create(IdTokenParameter(arg.AuthorizationCodeRequestQuery, arg.TokenRequestQuery));
        var refreshToken = JwtBearerAccessTokenFactory.CreateRefreshToken();
        return (accessToken, idToken, refreshToken);
    }
```

To create an `id_token`, a `sub`, `nonce` and `scope` are supplied in the constructor of the `IdTokenParameters` class. The `sub` is short for subject. It stands for what or who the token is for. For example, in the case of the Authorization Code flow with PKCE, the sub is the identifier of the user. The sub of a token created using the Credential flow will stand for the Client ID. The `nonce` (number used only once) is a random string used to protect against replay attacks. By adding the `nonce` to the `id_token`, the `id_token` can be linked to the original request towards the Authorization Endpoint. The `scope` is the scope requested in the authorization request, standing for the needed data in the actual ID Token.

```csharp
public record IdTokenParameters: TokenParameters
{
    public IdTokenParameters(string sub, string nonce, string scope)
    {
        Audience = Consts.ValidAudience;
        Issuer = Consts.ValidIssuer;
        SigningCertificate = Consts.ValidSigningCertificate.ToX509Certificate2();
        Claims = new List<Claim>
        {
            new(Consts.SubClaimType, sub),
            new(Consts.ScopeClaimType, scope),
            new(Consts.CountryClaimType,
                Consts.CountryClaimValidValue),
            new("auth_time", DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
            new("nonce", nonce)
        };
...
```

All the building blocks are now in place! Mike can run his test and see if the web application behaves as expected. That is not the case. The test fails. The browser redirects to the URL `https://i.do.not.exist/authorize?client_id=69313df8..&redirect_uri=https%3A%2F%2Flocalhost%3A56407%2Fsignin-oidc&response_type=code&prompt=select_account&scope=openid...&code_challenge=hWoEjZ4unyaDNrT...&code_challenge_method=S256&nonce=6384990...NDlmNDJhNGIt...&state=CfDJ8...&x-client-SKU=ID_NET8_0&x-client-ver=7.1.2.0`. After investigating and looking to the first sequence diagram, he noticed arrow number `3`. The web application will ask the browser to navigate to the `OIDC Server`.

The authorization code is retrieved using the browser. The `MockingOpenIdProviderMessageHandler` class is utilized by the `ConfigurationManager` to intercept configuration requests. It is the browser that receives a redirect uri from the web application. Mike creates a new method `GetAuthorizationLocationHeaderFromFullUri` based on `GetAuthorizationResponseMessage`. The method `GetAuthorizationLocationHeaderFromFullUri` returns a URL that the browser will navigate to. That URL that contains an authorization code and state, skipping the execution of the authorize request. In this case, the authorization code is `12345678` and the state is reused from the authorization request: `https://localhost:56567/signin-oidc?code=123456789&state=CfDJ8Hy8...`. By configuring the event `OnRedirectToIdentityProvider` with the `PostConfigure` method in the `PlaywrightCompatibleWebApplicationFactory` class, Mike can ensure that the browser does not redirect to `https://i.do.not.exist`."

```csharp
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
                {
                    ...
                    services.PostConfigure<OpenIdConnectOptions>(OpenIdConnectDefaults.AuthenticationScheme,
                    options =>
                    {
                        MockingOpenIdProviderMessageHandler backChannelMessageHandler = ConfigForMockedOpenIdConnectServer.CreateHttpHandler(Constants.ValidIssuer, TokenFactoryFunc, UserInfoResponseFunc);
                        options.ConfigurationManager = ConfigForMockedOpenIdConnectServer.Create(backChannelMessageHandler);
                        
                        options.Events = new OpenIdConnectEvents()
                        {
                            ...
                            OnRedirectToIdentityProvider = context =>
                            {
                                //code that happens in the OpenIdConnectHanlder of asp.net core, but not exeucte due to context.HandleResponse()
                                context.Properties.Items.Add(OpenIdConnectDefaults.RedirectUriForCodePropertiesKey, context.ProtocolMessage?.RedirectUri);
                                context.ProtocolMessage!.State = context.Options.StateDataFormat.Protect(context.Properties);
                                
                                var authorizationRequestUri = context.ProtocolMessage?.BuildRedirectUrl()!;
                                //Use the backChannelMessageHandler to generate the URL and ensure that it can be linked to the tokenrequest by using the same instance
                                var mockedAuthorizationCode = backChannelMessageHandler.GetAuthorizationLocationHeaderFromFullUri(authorizationRequestUri);
                                
                                logger?.LogInformation("Override Browser Redirect! Redirected to authorization endpoint:" + mockedAuthorizationCode);

                                context.HandleResponse();
                                context.Response.Redirect(mockedAuthorizationCode);
                                
                                return Task.CompletedTask;
                            },
                            ...
                        }
                    }
                }
                ...
    }
```

The subsequent test unfortunately fails when trying to redeem the authorization code. Mike investigates this by revisiting the first sequence diagram, specifically arrow number `6`, which illustrates that the web application should exchange the authorization code for tokens via a `backchannel`. A backchannel is a specialized `HttpClient` designed for secure, internal communications between the web application and the OIDC server.

```csharp
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
                {
                    ...
                    services.PostConfigure<OpenIdConnectOptions>(OpenIdConnectDefaults.AuthenticationScheme,
                    options =>
                    {
                        MockingOpenIdProviderMessageHandler backChannelMessageHandler = ConfigForMockedOpenIdConnectServer.CreateHttpHandler(Constants.ValidIssuer, TokenFactoryFunc, UserInfoResponseFunc); //generic handler
                        options.ConfigurationManager = ConfigForMockedOpenIdConnectServer.Create(backChannelMessageHandler); //jwks_uri and .wellknown
                        options.Backchannel = ConfigForMockedOpenIdConnectServer.CreateHttpClient(backChannelMessageHandler); //fetch the tokens, userinfo,...
```

Mike can configure this backchannel to also intercept capturing user information from the userinfo-endpoint. This approach allows him to monitor and intercept other communications over the backchannel as well.

Mike reexecutes the test, and this time it succeeds, The username is correctly displayed, confirming the proper workflow of the application authentication processes.

## Wrapping It Up

In his journey, Mike gained a profound appreciation for mocking strategies, highlighting how such approaches dramatically accelerate development cycles in his projects. His newfound comfort with working on mock external dependencies has been a meaningful change.

This article's focus on the authorization code flow with PKCE has illuminated a systematic defense mechanism against authorization code interception, emphasizing the importance of security in authentication processes.

Through sharing his experience, Mike aims to enlighten other developers with a deeper understanding of OIDC flows, empowering them to navigate the complex realm of authentication with greater ease and confidence.

## Sources

- Article Mocking your OpenID Connect Provider: <https://xebia.com/blog/mock-your-openid-connect-provider/>

- Article Let Us Playwright with .NET 6 MVC: <https://xebia.com/blog/lets-playwright-with-net-6-mvc/>

- "OpenID Connect Core 1.0" from OpenID.net: <https://openid.net/specs/openid-connect-core-1_0.html>
  - example of authorize request with state and nonce parameters and in ID token: <https://openid.net/specs/openid-connect-core-1_0.html#code-id_token-tokenExample>
- OAuth2 Specifications: <https://datatracker.ietf.org/doc/html/rfc6749>
  - state and XSS protection: <https://datatracker.ietf.org/doc/html/rfc6749#section-4.1.1>
- GitHub Repository: <https://github.com/kriebb/MockOpenIdConnectIdpAspnetcore>

- Google Pay: <https://www.inkdepot.com.au/blog/the-pros-and-cons-of-making-payments-with-google-pay>
  - Google Pay Transactions Receipts: <https://developers.google.com/wallet/tickets/open-loop/mobile-features/transaction-receipts>

- Microsoft recommends PKCE: <https://devblogs.microsoft.com/identity/migrate-to-auth-code-flow/>
- OIDC documentation on MS Learn: <https://learn.microsoft.com/en-us/entra/identity-platform/v2-protocols-oidc>
- Grant flow documentation on MS Learn: <https://learn.microsoft.com/en-us/entra/identity-platform/v2-oauth2-auth-code-flow>

- Proof Key for Code Exchange by OAuth Public Clients: <https://www.rfc-editor.org/rfc/rfc7636>
