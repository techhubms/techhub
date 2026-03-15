---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/best-practice-using-self-signed-certificates-with-java-on-azure/ba-p/4496900
title: 'Best Practice: Using Self-Signed Certificates with Java on Azure Functions (Linux)'
author: wanjing
primary_section: dotnet
feed_name: Microsoft Tech Community
date: 2026-02-25 08:08:28 +00:00
tags:
- Application Configuration
- Azure
- Azure App Service
- Azure Functions
- Azure Key Vault
- Community
- Deployment Considerations
- DevOps
- Java
- JVM Truststore
- Keytool
- Kudu
- Linux
- Persistent Storage
- Security
- Security Best Practices
- Self Signed Certificate
- SSL Handshake
- TLS/SSL
- .NET
section_names:
- azure
- dotnet
- devops
- security
---
wanjing presents a hands-on walkthrough for Java developers deploying on Azure Functions (Linux), showing how to configure JVM truststores for self-signed certificates and discussing practical deployment and security considerations.<!--excerpt_end-->

# Best Practice: Using Self-Signed Certificates with Java on Azure Functions (Linux)

When running Java applications on Azure Functions (Linux dedicated plan) needing to connect to services secured by self-signed certificates, developers often face SSL handshake errors like:

```
PKIX path building failed: sun.security.provider.certpath.SunCertPathBuilderException: unable to find valid certification path to requested target
```

## Why Is This Needed?

By default, the JVM trusts only certificates from well-known Certificate Authorities. Applications connecting to endpoints using self-signed certificates will fail unless those certificates are added to the JVM's truststore.

However, on Azure Functions (Linux) the root filesystem is ephemeral -- changes made outside /home are lost on restart or scale. Thus, making certificate changes persistent requires storing keystores under /home.

## Step-by-Step: Custom Truststore in Persistent Storage

### 1. Prepare Custom Keystore

- SSH into your Function App using the Kudu site (`https://<your-app-name>.scm.azurewebsites.net/webssh/host`).
- Locate your existing JVM cacerts file. Confirm its path with `echo $JAVA_HOME`.
- Copy the default truststore to a persistent location:

  ```sh
  cp /usr/lib/jvm/msft-17-x64/lib/security/cacerts /home/site/wwwroot/my-truststore.jks
  ```

### 2. Import the Self-Signed Certificate

- Upload your certificate file (e.g., `self-signed.badssl.com.cer`) to your Function App (via Kudu or FTP).
- Import it using keytool:

  ```sh
  ./keytool -import -alias my-self-signed-cert \
    -file /home/self-signed.badssl.com.cer \
    -keystore /home/site/wwwroot/my-truststore.jks \
    -storepass changeit -noprompt
  ```

### 3. Verify the Import

- Confirm your certificate is in the truststore:

  ```sh
  ./keytool -list -v \
    -keystore /home/site/wwwroot/my-truststore.jks \
    -storepass changeit -alias my-self-signed-cert
  ```

- You should see certificate details displayed.

### 4. Configure the JVM to Use Your Truststore

- In Azure Portal, navigate to **Configuration > Application Settings**.
- Add or update the setting:
  - **Name:** `JAVA_OPTS`
  - **Value:** `-Djavax.net.ssl.trustStore=/home/site/wwwroot/my-truststore.jks -Djavax.net.ssl.trustStorePassword=changeit`
- Save your settings. This restarts your Function App and applies the truststore.

## Important Considerations

### File Location & Deployment

Keep in mind that `/wwwroot` may be overwritten during deployments, depending on method (e.g., ZipDeploy or Run From Package). To avoid losing your keystore, consider placing it elsewhere under `/home`, such as `/home/my-certs/`, and update the path in your JAVA_OPTS.

### Maintenance

- On certificate expiry, re-import into the same keystore; full recreation isn't necessary.
- Managing certificate validity is your responsibility.

### Azure Key Vault Note

Azure Key Vault typically manages private keys (.pfx, .pem). For root CA/public certificates used by JVM trust validation, using a custom JKS is most direct.

## Alternative Approaches

### 1. Load Azure-Managed Certificate via Code

- Upload your `.cer` certificate to the **TLS/SSL settings (Public Keys Certificates)** blade in Azure.
- Add the Application Setting `WEBSITE_LOAD_CERTIFICATES` to `*` or the thumbprint.
- The platform deposits your cert at `/var/ssl/certs/<thumbprint>.der`.
- On App Service (Linux), this may be automatically imported to JVM; on Azure Functions (Linux), it is not. You must load it via Java code into the SSLContext.

> [Use TLS/SSL Certificates in App Code - Azure App Service | Microsoft Learn](https://learn.microsoft.com/en-us/azure/app-service/configure-ssl-certificate-in-code?tabs=linux)

### 2. Build JKS Locally and Load via Code

- Build the truststore locally and package it inside your app (e.g., in `src/main/resources`).
- At runtime, load it in your Java code when initializing SSL.

> [Configure Security for Tomcat, JBoss, or Java SE Apps - Azure App Service | Microsoft Learn](https://learn.microsoft.com/en-us/azure/app-service/configure-language-java-security?pivots=java-javase#initialize-the-java-key-store-in-linux)

## Summary

By using a custom persistent keystore and the right JVM settings, your Azure Functions-based Java app can securely connect to endpoints protected by self-signed certificates—surviving restarts and deployments. Alternative programmatic strategies are also possible for more advanced setups.

---

*Author: wanjing*

Updated Feb 25, 2026 | Version 1.0

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/best-practice-using-self-signed-certificates-with-java-on-azure/ba-p/4496900)
