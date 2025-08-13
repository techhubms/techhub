# Securely deploying to Azure using Octopus Deploy with Entra Workload Identity
In this article, we'll explore how Octopus Deploy (I'll use Octopus from now on) can be used to deploy applications to Azure securely. Before diving into the technical details, we should define what 'secure' means in this context. With 'secure', we mean that we want to:
- Apply the least privilege principle. We don't want to provide an account/credential with more permissions than necessary.
- Protect Octopus properly so that accounts cannot be compromised easily.
- Use secrets with a short validity period. The shorter, the better. The longer a secret is valid, the more time someone with ill will has to discover the secret, and the longer a compromised secret can be abused. Or, even better:
- Don't use any secrets. By not using secrets, we remove a potential attack vector because there are no more secrets that could inadvertently be leaked or stolen.

## Octopus concepts and their relation to Azure
Because we'll use some Octopus-specific terminology in the rest of this article, we should define what these mean:

**Environments** in Octopus represent the deployment pipeline's different stages, such as development, testing, and production. These environments can be mapped to, for example, Azure Resource Groups or Azure Subscriptions, allowing for organised and isolated deployments across different stages.

**Projects** in Octopus encapsulate an application's deployment process, defining the steps and configurations required for a release.

**Variables** play an important role in Octopus by allowing the definition and management of configuration settings across deployment processes. They can also store sensitive information like API keys, ensuring these values are securely managed and only accessible when needed. Variables can be scoped to different levels, such as environments, deployment targets, or tenants, providing flexibility in managing configurations. They are used in the various steps of the deployment process to provide values necessary at application deploy time or runtime or to decide which steps to run in the process.

### Octopus authentication
Another important area in Octopus, certainly when discussing security, is authentication. Authentication is the process of verifying the identity of a user, device, or system before granting access to resources or performing actions. Authentication typically involves providing credentials or proof of identity, which the system verifies against its records. In Octopus, authentication can be broadly grouped into Users and Accounts. In my head, I also categorise them as 'Internal Accounts' and 'External Accounts', reflecting the account's purpose.

- **Users / Internal Accounts**: Users perform actions in Octopus. They are defined at the Octopus system level as user or service accounts. User accounts can use the Octopus Web Portal and the Octopus API and authenticate with a username and password, Active Directory credentials, or an Octopus API key. Service accounts are API-only accounts that should be used for automated services that integrate with Octopus and can only authenticate with an Octopus API key.
- **(External) Accounts**: Accounts are used for Octopus to authenticate itself during deployments. Octopus supports a variety of account and credential types. Examples of account types are 'cloud' accounts (Azure, AWS and Google Cloud), Username/Password and SSH key. An Azure Account (it's actually called an 'Azure Subscription' in Octopus) can be configured with three types of credentials: 1. Service Principal with a client secret 2. OpenID Connect 3. Management Certificate. For this article, we will only talk about OpenID Connect.

#### OpenID Connect
In Octopus, using OpenID Connect with Azure means we'll use Entra ID Federated Identity Credentials. Federated identity credentials are a relatively new type of credential that enables workload identity federation for software workloads. Workload identity federation allows you to access Microsoft Entra-protected resources without managing secrets.

A trust relationship is created between an external identity provider (IdP, in this case, Octopus) and an app in Microsoft Entra ID by configuring a federated identity credential. The federated identity credential indicates which token from the external IdP your application can trust. After creating a trust relationship, Octopus can exchange trusted ID tokens for access tokens from Microsoft Entra ID. Octopus then uses that access token to access the Microsoft Entra-protected resources to which the Entra app has access. This process eliminates the maintenance burden of manually managing credentials and eliminates the risk of leaking secrets or having certificates expire. The underlying tokens exchanged between both systems are short-lived, which means that even if a token is compromised, the window of opportunity for an attacker is limited.

## Configuring an Azure account with OpenID Connect credentials
To configure an Azure account with OpenID Connect, the following properties will have to be provided:
- **Client/application ID**: This is the unique identifier generated for the application when it was created in Entra ID. Entra ID uses it to identify the application during the authentication process.
- **Tenant ID**: The Tenant ID is the unique identifier of the Azure tenant the application is part of.
- **Subscription ID**: An Azure account must be tied to an Azure subscription in Octopus, identified by its unique identifier, even though this is not strictly necessary from the Azure perspective.
- **Audience claim**: This is put into the ``aud`` claim of the ID token sent to Entra ID. Microsoft and Octopus recommend using the value '``api://AzureADTokenExchange``'.
- **Subject claim**: This is put into the ``sub`` claim of the ID token sent to Entra ID. The value can be configured to be a combination of the name of the account, environment or project (and a few other options). The value can differ based on where/when the account is used in deployments/runbooks or health checks.

Examples of subjects:

From a deployment:
- Space (name=default) & account (name=azureaccount) => ``space:default:account:azureaccount``
- Environment (name=uat) & project (name=website) & type => ``project:website:environment:uat:type:deployment``

From account test (this is used when testing out the account, through the website or API):
- Space (name=default) & account (name=azureaccount) => ``space:default:account:azureaccount:type:test``

### Programmatically creating the account
An account in Octopus can be created via the Octopus Web Portal or programmatically using the Octopus API. In our examples, we'll use the Octopus API to create an Azure account with OpenID Connect credentials, calling the API from PowerShell.

The following JSON payload will need to be sent to the API. Most of the properties have been described above. In addition to those, a name and the subject keys to be used for deployments/runbooks, health checks, and the account test need to be provided.
```powershell
$octopusApiPayload = @{
    Name                   = 'NameOfTheAccountInOctopus'
    AccountType            = 'AzureOidc'
    SubscriptionNumber     = '1f111d11-11d1-1d11-bcc1-1da111111111'
    ClientId               = '2f222d22-22d2-2d22-bcc2-2da222222222'
    TenantId               = '3f333d33-33d3-3d33-bcc3-3da333333333'
    Audience               = 'api://AzureADTokenExchange'
    DeploymentSubjectKeys  = @('space', 'account')
    HealthCheckSubjectKeys = @('space', 'account')
    AccountTestSubjectKeys = @('space', 'account')
} | ConvertTo-Json
```
To call the Octopus API, you'll need to provide the API key and the Content-Type as HTTP request headers. The API key can be for a User Account or a Service Account. This account needs to have the permissions to create, update, and delete accounts. The 'Environment Manager' User Role in Octopus covers these permissions.
```powershell
$headers = @{
    'X-Octopus-ApiKey' = 'API-1234567890abcdefghijklmn'
    'Content-Type'     = 'application/json; charset=utf-8'
}
```
To create a new account, an HTTP POST request needs to be sent to the Octopus API ``/accounts`` endpoint:
```powershell
Invoke-RestMethod -Uri 'https://youroctopusfqdn/api/accounts' -Method Post -Headers $headers -Body $octopusApiPayload
```
To update an existing account the same payload can be sent, but using an HTTP PUT request instead of a POST. You'll also need to know the accountId of the account that you want to update. I use the ``/accounts/all`` endpoint for this purpose. This returns all accounts in the Octopus instance, which I then filter on the name.
```powershell
# retrieve existing accounts
$existingAccounts = Invoke-RestMethod -Uri 'https://youroctopusfqdn/api/accounts/all' -Method Get -Headers $headers
# get the correct account by filtering on account name, which is unique
$existingAccount = $existingAccounts | Where-Object { $_.Name -eq 'NameOfTheAccountInOctopus' }

# update the existing account
Invoke-RestMethod -Uri "https://youroctopusfqdn/api/accounts/$($existingAccount.Id)" -Method Put -Headers $headers -Body $octopusApiPayload
```
That was the first of the two parts to setting up the trust relationship between Octopus and Entra ID. Next, we'll configure Entra ID side.
## Creating federated credentials in Entra ID
In Entra ID, federated credentials can be created for two types of principals: Service Principals and User-Assigned Managed Identities (I'll call it a UMID from now on). A UMID is a special type of Service Principal because its lifetime is tied to a resource in the Azure Control Plane, whereas a Service Principal only lives in Entra ID. In the context I've been using Octopus with Entra federated credentials, I've chosen to use Service Principals to create the Azure Infrastructure needed for my application and the UMIDs I've chosen to use. The UMIDs I then primarily use for data plane operations in Azure, for example, a database deployment using SQL package.

Next, I'll show how to create federated credentials for both types of principals because there are slight differences in how you create them.
### Using a Service Principal with federated credentials
To create federated credentials for a Service Principal, a few properties will need to be provided:
- **Issuer**: As mentioned in the Octopus docs, your Octopus instance will need to have two URLs anonymously exposed to the Internet: ``https://server-host/.well-known/openid-configuration`` and ``https://server-host/.well-known/jwks``, where '``server-host``' is the Internet-resolvable FQDN of your Octopus instance. Entra uses these endpoints to look up the relevant OpenID Connect information of Octopus. The value for '``issuer``' should be the Internet-resolvable FQDN of your Octopus instance.
- **Subject**: This should be set to the same 'subject' value you've configured your Octopus account with.
- **Audiences**: As mentioned in the previous section, the value for this should be ``api://AzureADTokenExchange``
- **Name**: Name of your choosing, identifying the credential.
- **Description**: Description of the credential.

We'll use the Azure CLI. The relevant command requires sending a JSON payload to create the Federated Credential. We'll first define the payload, again using PowerShell:
```pwsh
$parameters = @{
    name        = 'NameOfTheFederatedCredential'
    issuer      = 'NameOfTheFederatedCredential'
    subject     = 'project:website:environment:uat:type:deployment'
    description = ''
    audiences   = @('api://AzureADTokenExchange')
} | ConvertTo-Json
```
Because I was having issues with correctly quoting the JSON string on the command line, I passed in the JSON payload from a temporary file.
```pwsh
$tempFile = [System.IO.Path]::GetTempFileName()
$parameters | Out-File -FilePath $tempFile -Encoding utf8
az ad app federated-credential create --id '4f444d44-44d4-4d44-bcc4-4da444444444' --parameters @$tempFile -o none
```
As you can see, you will also need to pass in the Object ID of the Service Principal for which you are creating the Federated Credential.
### Using a UMID with federated credentials
As you would expect, to create a Federated Credential for a UMID, you must also provide the ``issuer``, ``subject`` and ``audiences``. Additionally, you'll need to provide the UMID you are creating the credential for, which is identified using ``identity-name``, ``subscription`` and ``resource-group``.
```pwsh
az identity federated-credential create --identity-name 'NameOfYourUMID' --resource-group 'ResourceGroupName' -n 'NameOfTheFederatedCredential' --issuer 'NameOfTheFederatedCredential' --subject 'project:database:environment:uat:type:deployment' --audiences 'api://AzureADTokenExchange' --subscription '5f555d55-55d5-5d55-bcc5-5da555555555'
```
## Testing the Octopus Azure account
Now that we've configured Octopus and Azure, we can test if Octopus can use the federated credential properly. There are two ways of doing that: Manually through the Octopus Web Portal and through the Octopus API.
### Testing in the Octopus portal
In the Octopus portal, on the account creation/update page, there's a button which has Octopus verify if the account you've configured works. The subject claim you've configured for 'account test' is used in this case. Octopus will exchange an ID token for an Azure access token and, with that access token, will try to retrieve the subscription info for the subscription ID you've configured. This means that the Azure principal you've configured will at least need read permissions on the subscription in question for this test to succeed.

![alt text](images/TestAccountPortal.png)

If the test succeeds, Octopus will report success:

![alt text](images/AccountTestSuccess.png)
### Testing through the Octopus API
We can use the Octopus API to create a task. This task will then have Octopus run the account verification, after which we'll use the task's state to verify if the account test was successful. First, we'll define the JSON payload for such a task:
```pwsh
$jsonPayload = @{
    "Name"        = "TestAccount"
    "Description" = "Test Azure account"
    "Arguments"   = @{
        "AccountId" = 'Accounts-123'
    }
} | ConvertTo-Json
```
As you can see, we need to pass in the account's Octopus ID. With this JSON payload, we can create the task:
```pwsh
$task = Invoke-RestMethod -Uri "https://youroctopusfqdn/api/tasks" -Method Post -Headers $headers -Body $jsonPayload
```
Using the State property of the response object we can verify if the test was successful.
```pwsh
if ($task.State -eq "Success") {
    Write-Host "The Octopus Account can successfully connect to Azure"
}
```
Be aware that often the task state returned will be 'Queued'. You'll have to wait some time (mostly a few seconds) and retrieve the status again to see if the test was successful. This can be done as follows:
```pwsh
$taskStatus = Invoke-RestMethod -Uri "https://youroctopusfqdn/api/tasks/$($task.Id)" -Method Get -Headers $headers
```
## Summary
In this article we've used the (relatively new) Entra ID workload identity federation to allow Octopus to authenticate against Entra ID without using a secret and retrieve an access token which can be used to execute permitted actions in Azure.