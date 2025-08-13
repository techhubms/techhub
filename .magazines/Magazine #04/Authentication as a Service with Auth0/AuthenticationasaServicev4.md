# Authentication as a Service with Auth0

Authentication is an important factor in the success of applications and
their architectures. Serverless architectures are no exception to this.
Authentication is an area in which serverless computing is becoming the
new norm. Without any doubt, authentication is one of the most complex
features to implement correctly. When you use a serverless platform for
authentication, you can focus on actual business functionality of the
application instead of hosting and running an authentication platform.
One of the leading serverless Authentication as a Service (AaaS)
platforms is Auth0. Others are Azure AD (B2C) and Okta.

## Auth0

Auth0 is an enterprise-grade platform for modern identity. It helps you
build authentication for your applications using a frictionless
platform. Auth0 offers an easy-to-use dashboard that allows you to start
adding authentication to your applications. From simple logins using
social accounts (Microsoft, Google and Facebook) to enterprise logins
using Azure Active Directory, for example. Auth0 provides you with total
control over the functionality without having to manage and maintain the
platform.

Auth0's Webtask.io offers a serverless platform to add customizations
and other features to the authentication flows.

In this article, I will explain how easy it is to build WebApps and APIs
with Authorization with Auth0. In this example, we will use an ASP.NET
Core WebApp, Azure API management, and ASP.NET Core WebAPI, all
connected to Auth0.

Figure 1 shows the architecture for this example.

![](./media/image1.png)


Figure

The application flow looks like this.

1.  User visits MVC Website.

2.  User logs in on Website and is redirect to Auth0.

3.  Uses logs in using a third party (Google or Microsoft) login
    provider. User gets sign-in metadata and a token for API usage.

4.  Website calls WebAPI using the token retrieved during login.

5.  Azure API Management validates signing key (RS256) using config
    endpoint.

6.  Azure API Management forwards the request to WebAPI.

7.  WebAPI returns claims information to Website in JSON message.

First of all we start by creating an application in the Auth0 dashboard
at https://manage.auth0.com. After registration, you can get started
without any costs. With up to 7000 active users in the free tier, this
should be enough to get started. After logging in you start with
creating a new client. We get a Domain, Secret and client ID that we
need in our WebSite, API Management platform, API to configure our
authentication code.

![](./media/image2.png)

Make sure that you specify the "Allowed Callback URLs" and "Allowed
Logout URLs" correctly for either development (local machine) or hosting
platform (for example Azure). Switch "JsonWebToken Signature Algorithm"
to RS256 to make the ASP.NET Core OpenID middleware work correctly.

## Website

Configure the OpenIdConnect middleware using the clientId, clientSecret,
and Authority information from Auth0. ASP.NET Core offers standard
middleware for this purpose. It is located in the
"Microsoft.AspNetCore.Authentication.OpenIdConnect" package. When you
add this package to your project, you have to configure the middleware
of your app.

This can be done by adding the following code:

app.UseIdentity();

app.UseOpenIdConnectAuthentication(new OpenIdConnectOptions

{

ClientId = Configuration\[\"ClientId\"\],

ClientSecret = Configuration\[\"ClientSecret\"\],

Authority = Configuration\[\"Authority\"\],

ResponseType = OpenIdConnectResponseType.Code,

GetClaimsFromUserInfoEndpoint = true

});

## Azure API Management

Azure API management is the first barrier for successfully calling the
exposed APIs. The Azure API Management platform is also the first place
where you can check and validate the JWT (JSON Web Token) tokens for
authenticated access to the APIs. Azure API Management is a Platform as
a Service (PaaS) solution inside Azure. API Management helps
organizations publish APIs to developers inside and outside of your
organization. It provides the core competencies to ensure a successful
API program through developer engagement, business insights, analytics,
security, and protection.

After adding the WebAPI to Azure API management it is possible to
specify policies for each individual operation or for all operations at
once. In this case, we will add an inbound policy to all operations
using the code-view. One could see these polices as another form of
serverless computing, where you can easily customize the API management
platform without worrying about the hosting or deployment of those
customizations. It just runs in the platform provided.

The following segment of xml has to be added to the inbound policies for
your API. In this case it checks for a valid JWT that has not yet
expired, and uses the OpenID configuration endpoint of Auth0 to check
the signing key of the incoming JTW token.

![](./media/image3.png)


When you try the API in the Azure API Management developer portal, you
should see the message "Unauthorized inside API Management" if no
id_token is passed to the ASP.NET Core WebAPI inside the authorization
header of the request.

## WebAPI

The ASP.NET Core WebAPI is the last part that wants to check the
incoming token to authenticate the user. In the same way as with the
ASP.NET Core WebApp, there is a standard library to authenticate the use
of JWT tokens. This can be done by adding the following lines of code to
your startup middleware.

var options = new JwtBearerOptions

{

Audience = Configuration\[\"JWTOptions:clientId\"\],

Authority = \$\"https://{Configuration\[\"JWTOptions:domain\"\]}/\"

};

app.UseJwtBearerAuthentication(options);

All that is left to do now is marking your controller operations with
the \[Authenticate\] attribute. When you test the application, you
should be able to log in using a social media account, see your claim
information, and call the API to validate the id_token.

## Conclusion

Auth0 as an Authentication as a Service offers a really powerful
platform. It is easy to integrate in your solution with a simple
registration and using the values provided. I believe this is the
enormous power of a serverless platform. Without knowing the nitty
gritty details of Auth 2.0 or OpenIdConnect you can still easily
integrate it into your solution. You just provide your code and it just
works. You can also change functionality on the fly without needing any
redeployment of your code. Meanwhile, the platform does all the work and
is totally managed for you.

The code for the sample in this article is available on
<https://xpir.it/mag4-auth>.
