---
layout: "post"
title: "Announcing the First Beta Release of the Azure SDK for Rust"
description: "Microsoft announces the first beta release of the Azure SDK for Rust, featuring libraries for Identity, Key Vault (secrets & keys), Event Hubs, and Cosmos DB. The blog post details getting started, the unique strengths of Rust for Azure development, supported services, and future plans."
author: "Ronnie Geraghty"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/azure-sdk/rust-in-time-announcing-the-azure-sdk-for-rust-beta/"
viewing_mode: "external"
feed_name: "Microsoft DevBlog"
feed_url: "https://devblogs.microsoft.com/azure-sdk/feed/"
date: 2025-02-19 18:19:05 +00:00
permalink: "/2025-02-19-Announcing-the-First-Beta-Release-of-the-Azure-SDK-for-Rust.html"
categories: ["Azure", "Coding"]
tags: ["Azure", "Azure SDK", "Beta Release", "Cloud Development", "Coding", "Cosmos DB", "Developer Tools", "Event Hubs", "Identity", "Key Vault", "Memory Safety", "Microsoft", "News", "Open Source", "Rust", "Secure Coding"]
tags_normalized: ["azure", "azure sdk", "beta release", "cloud development", "coding", "cosmos db", "developer tools", "event hubs", "identity", "key vault", "memory safety", "microsoft", "news", "open source", "rust", "secure coding"]
---

In this post, Ronnie Geraghty announces the first beta release of the Azure SDK for Rust, providing official support for Rust developers working with Azure services like Identity, Key Vault, Event Hubs, and Cosmos DB.<!--excerpt_end-->

## Rust in time! Announcing the Azure SDK for Rust Beta

**Author:** Ronnie Geraghty

### Overview

Microsoft has released the first official beta for the Azure SDK for Rust, signaling robust support for Rust developers building applications on Azure. This beta offers libraries for Identity, Key Vault secrets & keys, Event Hubs, and Cosmos DB, and aims to deliver an idiomatic and seamless Azure experience in Rust.

---

### Why Rust for Azure?

Rust is celebrated for its:

- High performance
- Reliability
- Memory safety (helps prevent null pointer dereference, buffer overflows, etc.)

Its type system and ownership model lead to more secure and stable code. Its modern syntax and strong tooling foster a vibrant and productive community. Given Rust’s rising popularity both externally and inside Microsoft, the SDK aims to let Rust developers harness Azure’s full potential for robust, efficient, and secure solutions.

---

### Beta Libraries Included

The beta release features the following Azure SDKs for Rust:

| Service         | Crate Name                                    | Source Code Link                                                                                                  |
|-----------------|-----------------------------------------------|-------------------------------------------------------------------------------------------------------------------|
| Identity        | [`azure_identity`](https://crates.io/crates/azure_identity) | [GitHub](https://github.com/Azure/azure-sdk-for-rust/tree/main/sdk/identity/azure_identity)              |
| Key Vault Secrets| [`azure_security_keyvault_secrets`](https://crates.io/crates/azure_security_keyvault_secrets) | [GitHub](https://github.com/Azure/azure-sdk-for-rust/tree/main/sdk/keyvault/azure_security_keyvault_secrets)|
| Key Vault Keys  | [`azure_security_keyvault_keys`](https://crates.io/crates/azure_security_keyvault_keys) | [GitHub](https://github.com/Azure/azure-sdk-for-rust/tree/main/sdk/keyvault/azure_security_keyvault_keys)|
| Event Hubs      | [`azure_messaging_eventhubs`](https://crates.io/crates/azure_messaging_eventhubs) | [GitHub](https://github.com/Azure/azure-sdk-for-rust/tree/main/sdk/eventhubs/azure_messaging_eventhubs)  |
| Cosmos DB       | [`azure_data_cosmos`](https://crates.io/crates/azure_data_cosmos) | [GitHub](https://github.com/Azure/azure-sdk-for-rust/tree/main/sdk/cosmos/azure_data_cosmos)             |

---

### Getting Started

To use the Azure SDK for Rust:

1. **Add Dependencies**

   ```bash
   cargo add azure_identity azure_security_keyvault_secrets tokio --features tokio/full
   ```

2. **Import Libraries in Your Code**

   ```rust
   use azure_identity::DefaultAzureCredential;
   use azure_security_keyvault_secrets::{
       models::SecretSetParameters, ResourceExt as _, SecretClient,
   };
   ```

3. **Create a Secret Client**

   ```rust
   #[tokio::main]
   async fn main() -> Result<(), Box<dyn std::error::Error>> {
       // Create a credential
       let credential = DefaultAzureCredential::new()?;
       // Initialize the SecretClient with the Key Vault URL and credential
       let client = SecretClient::new(
           "https://your-key-vault-name.vault.azure.net/",
           credential.clone(),
           None,
       )?;
       // Additional code goes here...
       Ok(())
   }
   ```

4. **Create a New Secret**

   ```rust
   // Set parameters for the new secret
   let secret_set_parameters = SecretSetParameters {
       value: Some("secret-value".into()),
       ..Default::default()
   };
   // Create the new secret
   let secret = client
       .set_secret("secret-name", secret_set_parameters.try_into()?, None)
       .await?
       .into_body()
       .await?;
   ```

5. **Retrieve a Secret**

   ```rust
   // Get the version
   let version = secret.resource_id()?.version.unwrap_or_default();
   // Retrieve the secret
   let secret = client
       .get_secret("secret-name", version.as_ref(), None)
       .await?
       .into_body()
       .await?;
   // Print its value
   println!("{:?}", secret.value);
   ```

For more, including detailed documentation and additional code samples, visit the [GitHub repository](https://github.com/Azure/azure-sdk-for-rust).

---

### Roadmap & Future Plans

The development team aims to expand and stabilize the SDK with:

- More Azure service libraries
- Enhanced usability, without sacrificing flexibility
- Improvements like response buffering for uniform policy application (e.g., retry policies)
- Simplified array deserialization as empty `Vec<T>` where suitable

### Feedback & Community

Developers are encouraged to share feedback, report issues, and suggest improvements via the [GitHub issues page](https://github.com/Azure/azure-sdk-for-rust/issues). Community input is highly valued to shape the SDK’s future trajectory.

---

**Summary**

Microsoft’s official Azure SDK for Rust empowers Rust developers to build fast, secure, and reliable applications on Azure. With beta libraries for core services and a commitment to further growth, this marks a significant milestone in Azure’s language ecosystem.

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/azure-sdk/rust-in-time-announcing-the-azure-sdk-for-rust-beta/)
