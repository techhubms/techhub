# Intro

Have you ever needed to call REST APIs from an embedded device or a
console app? If so, you likely needed some OAuth2 credentials to prove
who you are and what you are allowed to do.

Getting these tokens on your device in a proper way could be a pain -
hardcoding credentials in your code is ugly, and hosting an embedded web
server to let a user sign in is also something you likely hope to avoid.

Fortunately, there is the Device Authorization Grant[^1] flow, also
known as Device Flow. It is one of the standardized OAuth2
authentication flows and its usecase is to enable applications, having
limited interaction capabilities themselves, to get authenticated.

Think of embedded devices or console applications for example - they
cannot present the user easily with a login page without hosting an
embedded webserver. These devices or applications will be called
`device` in this article to emphasize the role they play.

Unfortunately, not all services offer the Device Flow. To mitigate this,
it is possible to build a proxy that enables using this flow, while the
proxy uses one of the more common other flows (like the Authorization
Code flow) under the hood.

For this example, we will focus on creating such a proxy for Spotify's
REST API. As a challenge, we will try building this proxy using free
services in Azure.

While Spotify in practice does have a device code flow for some
partners, and for its own Apple TV and Android TV apps, it does not
support it for third-party applications. Some background is described in
[Reverse engineering Spotify's own Device Authorization Grant
implementation](#X11f95283ec6525ff190e14b91d58e049c61111a).

# How does the Device Flow work?

Before we go into the details of adding a proxy, let's discuss the
original Device Flow which is shown in Figure 1. Instead of directly
presenting the user with a login page, the device requests a pair of two
codes (step 1): one for the device, and one for the user. The device
presents the user with the `user_``code` (step 2), which can be shown on
a display as numbers or a QR code, or it can be read aloud in the case
of a voice assistant. The user can then enter this `user_``code` in a
browser (step 3) on a different device that does have better interaction
capabilities -- a desktop or smartphone for example. After that, the
user is asked to authorize the request (step 4) via the normal
authorization flow of the service. The device can poll for OAuth2 tokens
in parallel (step 6) using the `device_code`, which become available
upon completion of the authorization by the user.

The device can use those tokens to make API requests (step 7) as with
any other OAuth2 mechanism.

![](./media/image1.png)

Figure : Device Flow (normal setup -- no proxy)

# Device flow with proxy

As Spotify does not officially support this flow for custom
applications, it is also possible to implement your own device
authorization grant flow by hosting an extra component (called `proxy`
below) between your device and Spotify. The modified flow can be seen in
Figure 2.

![](./media/image2.png)


Figure : Device Flow with proxy

1.  The device calls the authorize endpoint. Now, the proxy should
    generate a record containing the `device_code` and `user_code`, save
    them in the database for later and send them back in the response.
2.  The user code needs to be presented to the user, via a display or a
    speaker for example.
3.  The user enters the `user_code` in the user-facing page. The proxy
    checks the `user_code` in its database. If the `user_code` is not
    found, an error is shown.
4.  If the `user_code` is found, the proxy redirects the user to Spotify
    to login and approve the request.
5.  After login, the user is redirected to the proxy's callback page.
    The redirect contains the result of the user's action and an
    authorization code.
6.  The proxy can fetch an `access_token` and `refresh_token` from
    Spotify's token endpoint using the authorization code it received in
    the callback. The proxy should store the credentials it received in
    the earlier-stored record, next to the `user_code` and the
    `device_code`.
7.  The device can poll the proxy's token endpoint using the
    `device_code` for the availability of the tokens. When the device
    has successfully fetched the token, the record should be deleted
    from the database to prevent abuse.
8.  The device can call the target API using the obtained tokens as
    usual.

# Device flow proxy architecture

## Requirements

The proxy should

-   host an authorize API endpoint and a token API endpoint for the
    device to interact with

-   generate the `device_code`` and` `user_code` and store it in a
    database

-   host a user-facing page containing a small form where the user can
    enter the `user_code`

-   retrieve and validate the `user_code`

-   host a callback endpoint for Spotify to, after the user logged in,
    redirect the user to. This endpoint should process the data sent by
    Spotify, and should show a success or error page to the user.

For this, the proxy should have a very simple frontend with a few
backend API endpoints, as can be seen in Figure 3.

![](./media/image3.png)


Figure : Detail architecture of proxy and API endpoints

## Component choice

For our challenge, we will choose from the free services[^2] that Azure
offers.

️Note: Often the free services come with reduced specifications and / or
a reduced SLA. Do not blindly use them for production purposes. However,
they can still fit the purpose of a small application for personal use.

There are two variants of free services:

1.  services that are free during the first year of your Azure Account
2.  services that remain free for the total lifespan of your Azure
    Account

For our Device Flow proxy, we need 3 main capabilities:

-   a place to serve the frontend (form where user enters the user_code)
    and some result pages

-   some API endpoints with compute and integration functionalities

-   some database / storage to store the authentication records that are
    active

For this challenge we will use these components which fit in the second
category (free forever).

This limits our choice to Static Web Apps, App Service and Azure
Container Apps for the frontend / backend capability, and Cosmos DB for
the database capability.

### Frontend / backend

Azure Static Web Apps (SWA) can serve a static frontend and its free
tier has a slimmed down version of Azure Functions called Managed
Functions which can serve as a backend. For a simple web application it
covers the needs, and it has no limits on the time it is running. For
us, this is a good choice.

SWA has a free monthly amount of 100 GB bandwidth per subscription, 2
custom domains and .5 GB storage per app, which is more than enough for
our proxy app.

The Managed Functions are a 'supported' API in the free tier of SWA.
There are a few other products that are also available as supported APIs
in SWA, but not as part of the free tier -- they are
`Bring your own Functions`, `API Management`, `App Service` and
`Container Apps``.`

The idea of using a supported API product in SWAs:

-   the API and static website are served from the same domain via the
    SWA's built-in reverse proxy. This removes the need to add
    Cross-Origin Resource Sharing (CORS) headers on the API responses,
    so it makes the developer experience less complex.
-   Routing is done automatically.
-   The authenticated user context from the SWA is available to the API
    logic.

Managed Functions will cover our needs, but it is good to know that it
does not support all features[^3] you might have come to expect from
other Azure Functions offerings. Notable differences are:

-   No Function triggers other than HTTP triggers
-   Some security best practices are not possible to be followed
    -   Currently no `Managed Identity` support[^4]
    -   no support for Key Vault references

Alternatives considered to SWA were:

-   Azure Container Apps has a lifetime free monthly amount of 180,000
    vCPU seconds, 360,000 GiB seconds, and 2 million requests, but that
    has a dependency on a container registry. Azure Container Registry
    is only available for free for 12 months.

-   App Service has a lifetime free monthly amount of 10 web, mobile, or
    API apps with 1 GB storage 1 hour per day -- not chosen because
    preference for serverless model. 1h/day is probably enough but feels
    difficult to estimate how it works out.

-   Azure Functions Consumption plan might look free (1 million
    > invocations and 400000 GB-s), but it has a requirement of
    > providing a storage account which is not free.

### Database / storage capability

Cosmos DB is the only database or storage product that is offered as a
free forever service. It fits our needs as it can store our
authentication session records as JSON documents and is easy to work
with.

The free tier of Cosmos DB offers a free monthly amount of 1000 request
units per-second provisioned throughput with 25 GB storage which is more
than enough for our proxy application.

### Solution for time-based logic

> We want to cleanup data for security reasons (discussed in the section
> below) after a certain period or when the data is obsolete. The
> Managed Functions offering in SWA only supports HTTP-triggered
> functions, so timer-triggered functions are not supported.

Technically a separate Logic App might be an option, but using the
Cosmos DB-native Time to Live (TTL) feature is a much simpler and more
elegant option[^5].

Since we only need time-based cleanup and no other timer-triggered runs,
we will go for the native cleanup in Cosmos DB by setting a TTL on the
records.

# Security considerations

Since we are essentially building a custom part of the authentication
chain here, it is very important to pay attention to security.

Basics, like not committing secrets to git, not hardcoding secrets,
using HTTPS, and so on will not be covered in this article. However, the
RFC gives some specific guidance related to the Device Authorization
Grant logic which is good to pay extra attention to. This extra guidance
basically boils down to preventing brute-forcing and phishing of the
secrets.

-   use of a long enough user code and device code. This increases the
    amount of tries that it will require to guess codes.
-   rate limiting on proxy endpoints to prevent brute-forcing. This
    reduces the amount of tries that a bad actor can perform.
-   expiry and cleanup of authentication session record from the
    database after period X or when the flow is completed by the device
    (the OAuth2 tokens are received by the device). This ensures that
    the data is stored not longer than necessary.

# Reverse engineering Spotify's own Device Authorization Grant implementation

While reading up on the Device Authorization Grant, I found an article
showing that Spotify offers this flow for its own apps and for
partners[^6]. The Android TV app is one of them, and one benefit of
Android apps (for us 😉) is that they are relatively easy to reverse
engineer. It is possible to download its application package (.apk file)
and process it in a decompiler. There are several online decompiler
services where you can upload the apk file and download the decompiled
java source files and accompanying resources as a zip file. After
opening the unzipped files, they are easily searchable using Visual
Studio Code. Now we need to use a bit of educated guessing and puzzling.
The developers often try to obfuscate their code to make our search
difficult by mangling variable names, function names etc. An idea to
circumvent this is to search for identifiers or (parts of) URLs, that
are related to the logic we are researching and that we expect the app
to use. The reason for this is that the identifiers or URLs themselves
are usually not mangled because they need to be used in HTTP request /
response bodies or the URLs that need to be called. Candidates for
search terms could come from reading the Device Flow's RFC[^7], from
Spotify's own authorization documentation[^8] (expecting that this flow
will have some common or similar endpoints) and by iterating further on
what we find.

Examples of concrete search terms that are leads for us are:

-   `client_id`
-   `accounts.spotify.com`
-   `device_code`

This way, we could find

-   the `client_id` of the app registration of Spotify's Android TV app
-   that part of the app's functionality is built around a website
    hosted at https://api-partner.spotify.com/tvapp?platform=androidtv
-   the special `scope`s that the app uses
-   the URLs and calls that the app makes to obtain tokens.

I presented the findings in a topic on the Spotify Developer forum[^9].

# Conclusion

In this article we discussed the OAuth2 Device Flow, and looked at how
we can build a proxy for services that do not offer this authentication
flow. We discussed the requirements for such a proxy, and looked at what
free components Azure offers to fulfil those requirements. We discussed
the security considerations and how to deal with them. Finally, we had a
look into how one could reverse engineer the Device Flow that Spotify
has but does not currently offer to non-partner developers.

# Reference

Other relevant articles:

-   Everybody wins with the Device Flow[^10]

-   Using the OAuth 2.0 device flow to authenticate users in desktop
    apps[^11]

-   Authentication In Smart TV App - Device Code Flow[^12]

-   Illustrated Device Flow (RFC 8628)[^13]

Other implementations:

-   Spotify player for vintage Macs[^14]
-   MacAuth (ASP.Net Core based)[^15]
-   Add the OAuth 2.0 Device Flow to any OAuth Server (PHP based)[^16]

[^1]: https://www.rfc-editor.org/rfc/rfc8628

[^2]: https://azure.microsoft.com/en-us/pricing/free-services/

[^3]: https://learn.microsoft.com/en-us/azure/static-web-apps/apis-functions

[^4]: https://github.com/Azure/static-web-apps/issues/88

[^5]: https://learn.microsoft.com/en-us/azure/cosmos-db/nosql/time-to-live

[^6]: https://pragmaticwebsecurity.com/articles/oauthoidc/device-flow.html

[^7]: https://www.rfc-editor.org/rfc/rfc8628

[^8]: https://developer.spotify.com/documentation/general/guides/authorization/

[^9]: https://community.spotify.com/t5/Spotify-for-Developers/Device-Authorization-Grant-authentication-flow-for-custom/m-p/5485468

[^10]: https://pragmaticwebsecurity.com/articles/oauthoidc/device-flow.html

[^11]: https://thomaslevesque.com/2020/03/28/using-the-oauth-2-0-device-flow-to-authenticate-users-in-desktop-apps/

[^12]: https://www.c-sharpcorner.com/article/authentication-in-smart-tv-app-device-code-flow/

[^13]: https://darutk.medium.com/illustrated-device-flow-rfc-8628-d23d6d311acc

[^14]: https://68kmla.org/bb/index.php?threads/building-a-spotify-player-for-my-mac-se-30.32182/

[^15]: https://github.com/antscode/MacAuth

[^16]: https://developer.okta.com/blog/2019/02/19/add-oauth-device-flow-to-any-server
