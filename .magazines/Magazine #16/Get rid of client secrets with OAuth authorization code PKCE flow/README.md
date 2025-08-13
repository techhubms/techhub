# Get rid of client secrets with OAuth authorization code PKCE flow
In this article, we'll dive into the OAuth 2.0 Proof Key for Code Exchange (PKCE) flow, an extension of the Authorization Code flow that helps prevent CSRF and authorization code interception attacks. PKCE was originally designed to protect the authorization code flow in public clients (applications that cannot securely store secrets due to their execution environment, e.g., single-page web applications). Still, its ability to prevent authorization code injection makes it useful for every type of OAuth client, even confidential clients (applications that can securely store secrets, e.g., server-based web applications) that use client secrets. In OAuth 2.1, PKCE is mandated for all OAuth clients using the Authorization Code flow, not only public clients.

While PKCE can be used in confidential clients to increase security further, it can also eliminate client secrets and their accompanying challenges. It is advised, though, to determine if this is an acceptable trade-off for your use case with your CSO or security team.

## Introduction to OAuth and PKCE
### Brief overview of OAuth 2.0
OAuth 2.0 is the industry-standard protocol for authorization. It allows third-party services to exchange web resources on behalf of a user without revealing the user's credentials. It’s widely adopted due to its flexibility and security, serving as the backbone for modern authentication systems in web applications.

While OAuth 2.0 provides a solid framework, it’s not without its challenges, especially when managing client secrets. Traditionally, client secrets have been used to secure communications between clients and authorization servers. However, these secrets can be compromised in environments where confidentiality is not guaranteed, such as mobile or single-page applications. In addition, because these secrets are effectively passwords and need to be saved somewhere to be used, they often mistakenly end up in source code repositories. They must be rotated to reduce the risk of long-term unnoticed compromises of the secret. As a result of this rotation, the clients that use these secrets need to be updated as well, which can lead to downtime or even client applications that stop working.
### Introduction to Authorization Code Flow
The OAuth 2.0 authorization code grant type, or _authorization code flow_, enables a client application to obtain authorized access to protected resources like web APIs. The following diagram, courtesy of [Postman](https://blog.postman.com/wp-content/uploads/2020/06/image5.png), shows a high-level flow overview.

![alt text](<images/Auth code flow.png>)

Over time, it became apparent that public clients utilizing the Authorization Code flow are susceptible to the authorization code interception attack. In this attack, the attacker intercepts the authorization code returned from the authorization endpoint within a communication path not protected by Transport Layer Security (TLS), such as inter-application communication within the client's operating system. An example of this is in operating systems that allow applications to register themselves as a handler for specific URI schemes (e.g., '```xebia.ms.app://```'). Since multiple applications can be registered as a handler for the specific redirect URI, the vulnerability of this flow is that a malicious client could also register itself as a handler for the same URI scheme that a legitimate application handles. If this happens, the operating system may send the authorization code to the malicious client. The following diagram, courtesy of [WSO2 IdentityServer](https://is.docs.wso2.com/en/latest/assets/img/setup/secure/mitigate-auth-code-interception.png), shows the attack:
![alt text](<images/Auth code flow attack.png>)
### Introduction to Proof Key for Code Exchange (PKCE)
To address this issue, the Proof Key for Code Exchange (PKCE, pronounced “pixy”) was introduced. Initially designed for mobile applications, PKCE provides an additional layer of security for OAuth 2.0 using dynamic, one-time codes. This method ensures that even if the authorization code is intercepted, it would be useless without the corresponding code verifier held by the client. The following diagram, courtesy of [Postman](https://blog.postman.com/wp-content/uploads/2020/06/image6.png), shows a high-level overview of the authorization code flow with PKCE. Two new concepts are introduced here:
1. Code verifier: A high-entropy cryptographic string created by the client.
2. Code challenge: A transformation (using a hashing method agreed between the client and the server) of the code verifier sent in the initial authorization request.

As you can see in the flow, the client sends the challenge on the initial request for an authorization code, which the server then stores. When the client tries to retrieve an access token using the authorization code, it also sends the code verifier, which allows the server to verify the authorization code is sent by the same client that sent the original request.
![alt text](<images/Auth code flow with PKCE.png>)
### Comparing Authorization Code flow with PKCE to the OAuth Implicit Flow
The OAuth Implicit Flow, once recommended as the standard and popular for its simplicity in client-side applications, poses security risks due to exposed tokens in URLs and easier token interception. The Authorization Code Flow with PKCE, which uses a backchannel call (script XHR) to retrieve the token, is now considered a more secure alternative for public clients like mobile and single-page apps. In OAuth 2.1, the Implicit Flow has been removed.
### Is there any difference in the tokens?
In the PKCE flow used for public clients, such as in single-page applications, the lifetime of access and ID tokens issued by Entra ID is the same as that issued for confidential clients, which is around 1 hour. The main difference lies in the refresh token's lifetime. For a single-page application (a public client), the refresh token is valid for 24 hours. For confidential clients, the exact lifetime of a refresh token isn't specified but is generally much longer.
## Using PKCE with Microsoft Entra ID
In the following paragraphs, we'll request (access, refresh, and ID) tokens from Entra ID as a confidential and public client. For public clients, Entra ID does not allow sending a client secret when redeeming the authorization code. For confidential clients, sending the client secret is required.

I've created a small ASP.NET core demo application that can be found here: https://github.com/MvRoo/PKCEarticle. This application exposes three endpoints:

1. ```/pkceconfidential```: this endpoint will request a token using PKCE with a client secret
2. ```/pkcepublic```: this endpoint will request a token using PKCE without a client secret
3. ```/pkcepublics256```: this endpoint will request a token using PKCE without a client secret, using a SHA256 hashed code challenge
### Configure an application registration
In Microsoft Entra ID, create a new app registration with two platforms:

1. Web: this should be configured with the ```/pkceconfidential``` endpoint as the redirect URI
2. Mobile and desktop application: this should be configured with two redirect URIs: ```/pkcepublic``` and ```/pkcepublics256```.

An example manifest can be found in the GitHub repository linked above. An excerpt with the relevant bits is shown here:

```json
"replyUrlsWithType": [
    {
        "url": "http://localhost:5218/pkcepublics256",
        "type": "InstalledClient"
    },
    {
        "url": "http://localhost:5218/pkcepublic",
        "type": "InstalledClient"
    },
    {
        "url": "http://localhost:5218/pkceconfidential",
        "type": "Web"
    }
]
```
Three things are noteworthy here:
The URLs use scheme ```http://```, which is only allowed for localhost.
2. For the ```/pkcepublic``` or `/pkcepublics256` endpoints, we could also use type spa, but Entra then expects the token request to use CORS
3. The port number could be different for your local instance of my demo app. Port numbers are optional if you host the app on port 80 or 443.
### Acquiring tokens with the authorization code PKCE flow
Acquiring tokens in the authorization code flow is always a two-step process:

1. Send a GET request to the ```/authorize``` endpoint to request an authorization code. In my example, the following query string parameters are sent (a few additional optional parameters can be sent as well):
   ```
   client_id: the application ID of your app reg
   response_type: code, for authorization code flow
   redirect_uri: endpoint in your app where authentication responses can be received
   scope: the scope(s) the user should consent to
   code_challenge_method: plain or S256. This should be S256, but the spec allows the use of plain if the client can't support SHA256. In case of plain, code_challenge = code_verifier
   code_challenge: a string between 43 and 128 characters. Could be anything, but should have enough entropy to make it impractical to guess the value.
   ```
2. Send a POST request to the ```/token``` endpoint with the authorization code to request tokens. In my example, the following form-encoded data is sent in the body of the request:
   ```
   client_id: the application ID of your app reg
   scope: the scopes that should be returned in the token(s)
   redirect_uri: The same redirect_uri value that was used to acquire the authorization code.
   code: the authorization code received back from the call to the /authorize endpoint
   grant_type: authorization_code, for the authorization code flow.
   code_verifier: the code verifier, derived by means of the code_challenge_method from the code_challenge that was sent to and stored by Entra ID on the call to the /authorize endpoint.
   client_secret: as mentioned before, required for confidential clients, disallowed for public clients
   ```
### Requesting tokens with code_challenge_method plain
As already mentioned earlier in the article, if 'plain' is chosen as the code_challenge_method, the code_verifier and the code_challenge should be the same. As you can see in the demo app, as the code challenge (and thus the verifier as well) I've chosen '`~ThisIsThe1stArticleI_veWrittenForXmsMagazine.IHopeYouFindItInformative-`', to show you which characters are allowed. The full URL for retrieval of the authorization code is then as follows:
```http
https://login.microsoftonline.com/00000000-0000-0000-0000-000000000000/oauth2/v2.0/authorize?client_id=00000000-0000-0000-0000-000000000000&response_type=code&redirect_uri=http://localhost:5218/pkceconfidential&response_mode=query&scope=openid&code_challenge=~ThisIsThe1stArticleI_veWrittenForXmsMagazine.IHopeYouFindItInformative-&code_challenge_method=plain
```
### Requesting tokens with code_challenge_method S256
The PKCE RFC states that if the client is capable of using S256, it is Mandatory To Implement (MTI). In the case of our .NET demo web application, it is possible. The code_verifier and code_challenge can be generated with the standard .NET core libraries ```RandomNumberGenerator``` and ```SHA256```:
```csharp
public static string CreateCodeVerifier()
{
    const int size = 32; // Size recommended by RFC 7636 for code verifier
    using var rng = RandomNumberGenerator.Create();
    var bytes = new byte[size];
    rng.GetBytes(bytes);
    // Using URL-safe base64 encoding without padding
    return Convert.ToBase64String(bytes)
        .TrimEnd('=') // Remove any base64 padding
        .Replace('+', '-') // 62nd char of encoding
        .Replace('/', '_'); // 63rd char of encoding
}

public static string CreateCodeChallenge(string codeVerifier)
{
    using var sha256 = SHA256.Create();
    var challengeBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(codeVerifier));
    // Using URL-safe base64 encoding without padding
    return Convert.ToBase64String(challengeBytes)
        .TrimEnd('=') // Remove any base64 padding
        .Replace('+', '-') // 62nd char of encoding
        .Replace('/', '_'); // 63rd char of encoding
}
```
As mentioned earlier in the article, the server will store the code_challenge that it received on the call to ```/authorize```. Because the code_verifier should be, as the PKCE RFC states, a '*high-entropy cryptographic random string*' and the code_challenge has been derived from the code_verifier, the client also needs to store the code_verifier somewhere to be able to send it to the server on the call to ```/token```. In the demo app, I've opted for sending the code_verifier to the server as well, using the ```state``` querystring parameter, which can be used specifically for the purpose to 'save state' without actually needing to save the state client-side. If this should be used for the code_verifier as well is another interesting discussion, but for demo purposes it's ok. 

The full URL for retrieval of the authorization code is then as follows:
```http
https://login.microsoftonline.com/00000000-0000-0000-0000-000000000000/oauth2/v2.0/authorize?client_id=00000000-0000-0000-0000-000000000000&response_type=code&redirect_uri=http://localhost:5218/pkcepublics256&response_mode=query&scope=openid&state=8p1BQjDGG_t6mymu0UJJfIWVX7ycZvxaN97jbNVt898&code_challenge=bnxEgm7cqE38fMI3AoW4RrKQ_b--Q9uwjPI65M-f_FU&code_challenge_method=S256
```
The code_verifier I then get back from the server in the ```state``` querystring of the HTTP 302 response of the server, which can then be sent to the server on the request to the /token endpoint as the code_verifier.
## Summary
As the Authorization Code with PKCE flow is now recommended as the standard for both confidential and public clients, the various official Microsoft libraries for authentication (e.g., MSAL.NET, MSAL.js, Microsoft.Identity.Web) support it, as well as popular open source projects, like OAuth2-proxy. I recommend using these libraries instead of creating your own code to generate the code_verifier and code_challenge, even though you've seen that it is not hard to make a basic implementation.

In this article, I've shown that PKCE is used with client secrets to enhance the security of confidential clients. For public clients, it is used without client secrets. But nothing is stopping us from doing this with our confidential client as well and getting rid of all the accompanying challenges that client secrets bring. As mentioned, you should determine with your CISO or security team if this is appropriate for your use case.

#### Sources
- RFC 7636 - Proof Key for Code Exchange by OAuth Public Clients: https://www.rfc-editor.org/rfc/rfc7636
- OAuth 2.1 draft: https://datatracker.ietf.org/doc/html/draft-ietf-oauth-v2-1-10
- Mitigate Authorization Code Interception Attacks: https://is.docs.wso2.com/en/latest/deploy/mitigate-attacks/mitigate-authorization-code-interception-attacks/
- OAuth 2.0: Implicit Flow is Dead, Try PKCE Instead: https://blog.postman.com/pkce-oauth-how-to/