---
external_url: https://techcommunity.microsoft.com/t5/azure-integration-services-blog/data-mapper-test-executor-a-new-addition-to-logic-apps-standard/ba-p/4472440
title: Data Mapper Test Executor in Logic Apps Standard Test Framework
author: WSilveira
feed_name: Microsoft Tech Community
date: 2025-12-08 16:00:00 +00:00
tags:
- Automated Test Framework
- Azure Integration Services
- Data Mapper Test Executor
- Integration Testing
- Liquid Templates
- Logic Apps Standard
- Map Definitions
- NuGet
- Schema Validation
- SDK
- Transformation Testing
- Unit Testing
- XML
- XSLT
section_names:
- azure
- coding
- devops
primary_section: coding
---
WSilveira details how developers can leverage the Data Mapper Test Executor for Logic Apps Standard, offering native capabilities to test data transformations efficiently within automated workflows.<!--excerpt_end-->

# Data Mapper Test Executor: A New Addition to Logic Apps Standard Test Framework

WSilveira presents the Data Mapper Test Executor, bringing first-class support for map testing directly to the Logic Apps Standard Automated Test Framework. This new feature streamlines unit testing for data transformations and empowers developers with robust validation and performance improvements.

## Why It Matters

Testing complex transformations, including those built with XSLT (1.0/2.0/3.0), Liquid templates, and custom map definitions, is often a bottleneck for Logic Apps developers. Manual verification and reliance on external tools can slow feedback and introduce reliability risks. The Data Mapper Test Executor solves these challenges by providing:

- **Direct integration for map unit testing**
- **Faster feedback loops and greater reliability**
- **Streamlined developer experience within Azure Logic Apps workflows**

## Key Features

- **Native Support for Data Mapper Testing**
  - Run unit tests for Logic Apps Data Mapper (.lml) files and XSLT within development environment
- **Integrated Automated Test Framework SDK**
  - Leverage SDK version 1.0.1+ for test execution and reporting
- **Schema Validation and Transformation Checks**
  - Validate map output using integrated schema checks
- **Performance Optimizations**
  - Built-in resource management and caching for efficient runs

## Getting Started

1. **Install Latest Framework SDK**
   - Reference at least v1.0.1 to take advantage of the new executor class.
   - NuGet package: [Microsoft.Azure.Workflows.WebJobs.Tests.Extension](https://www.nuget.org/packages/Microsoft.Azure.Workflows.WebJobs.Tests.Extension)
   - Example reference:

     ```xml
     <PackageReference Include="Microsoft.Azure.Workflows.WebJobs.Tests.Extension" Version="1.0.1" />
     ```

2. **Add Data Mapper Test Executor to Your Test Project**
   - Incorporate the new class in your unit test suite.
3. **Write Your First Test**
   - See the following example illustrating several testing styles:

     ```csharp
     using System.Collections.Generic;
     using System.Threading.Tasks;
     using Microsoft.Azure.Workflows.UnitTesting.Definitions;
     using Microsoft.VisualStudio.TestTools.UnitTesting;
     using la1.Tests.Mocks.Stateful1;
     using Microsoft.Azure.Workflows.UnitTesting;
     using System.IO;
     using System.Text;
     using Microsoft.Azure.Workflows.Data.Entities;
     using Newtonsoft.Json.Linq;

     namespace la1.Tests
     {
         [TestClass]
         public class DataMapTest {
             public const string MapName = "source_order_to_canonical_order";
             public string PathToMapDefinition { get; private set; }
             public string PathToCompiledXslt { get; private set; }
             public string PathToXsltTestInputs { get; private set; }

             public TestExecutor TestExecutor;

             [TestInitialize]
             public void Setup() {
                 this.TestExecutor = new TestExecutor("Stateful1/testSettings.config");
                 this.PathToMapDefinition = Path.Combine(this.TestExecutor.rootDirectory, this.TestExecutor.logicAppName, "Artifacts", "MapDefinitions", MapName + ".lml");
                 this.PathToCompiledXslt = Path.Combine(this.TestExecutor.rootDirectory, this.TestExecutor.logicAppName, "Artifacts", "Maps", MapName + ".xslt");
                 this.PathToXsltTestInputs = Path.Combine(this.TestExecutor.rootDirectory, "Tests", "la1", "Stateful1", "DataMapTest", "test-input.json");
             }

             [TestMethod]
             public async Task DataMapTest_GenerateXslt() {
                 var dataMapTestExecutor = this.TestExecutor.CreateMapExecutor();
                 var xsltBytes = await dataMapTestExecutor.GenerateXslt(mapName: MapName).ConfigureAwait(false);
                 Assert.IsNotNull(xsltBytes);
                 Assert.AreEqual(File.ReadAllText(this.PathToCompiledXslt), Encoding.UTF8.GetString(xsltBytes));
             }

             [TestMethod]
             public async Task DataMapTest_GenerateXsltWithMapContent() {
                 var mapContent = File.ReadAllText(this.PathToMapDefinition);
                 var generateXsltInput = new GenerateXsltInput { MapContent = mapContent };
                 var dataMapTestExecutor = this.TestExecutor.CreateMapExecutor();
                 var xsltBytes = await dataMapTestExecutor.GenerateXslt(generateXsltInput: generateXsltInput).ConfigureAwait(false);
                 Assert.IsNotNull(xsltBytes);
                 Assert.AreEqual(File.ReadAllText(this.PathToCompiledXslt), Encoding.UTF8.GetString(xsltBytes));
             }

             [TestMethod]
             public async Task DataMapTest_RunMap() {
                 var dataMapTestExecutor = this.TestExecutor.CreateMapExecutor();
                 var mapOutput = await dataMapTestExecutor.RunMapAsync(
                     mapName: MapName,
                     inputContent: File.ReadAllBytes(this.PathToXsltTestInputs)
                 ).ConfigureAwait(false);
                 Assert.IsNotNull(mapOutput);
                 Assert.IsTrue(mapOutput.Type == JTokenType.Object);
                 Assert.AreEqual("FC-20250603-001", mapOutput["orderId"]);
                 Assert.AreEqual("VIP-789456", mapOutput["customerId"]);
                 Assert.AreEqual("NEW", mapOutput["status"]);
             }

             [TestMethod]
             public async Task DataMapTest_RunMapWithXsltContentBytes() {
                 var dataMapTestExecutor = this.TestExecutor.CreateMapExecutor();
                 var mapContent = File.ReadAllText(this.PathToMapDefinition);
                 var generateXsltInput = new GenerateXsltInput { MapContent = mapContent };
                 var xsltContent = await dataMapTestExecutor.GenerateXslt(generateXsltInput: generateXsltInput).ConfigureAwait(false);
                 var mapOutput = await dataMapTestExecutor.RunMapAsync(
                     xsltContent: xsltContent,
                     inputContent: File.ReadAllBytes(this.PathToXsltTestInputs)
                 ).ConfigureAwait(false);
                 Assert.IsNotNull(mapOutput);
                 Assert.IsTrue(mapOutput.Type == JTokenType.Object);
                 Assert.AreEqual("FC-20250603-001", mapOutput["orderId"]);
                 Assert.AreEqual("VIP-789456", mapOutput["customerId"]);
                 Assert.AreEqual("NEW", mapOutput["status"]);
             }
         }
     }
     ```

   - Extend your `TestExecutor` with a method to facilitate creation of DataMapTestExecutor:

     ```csharp
     public DataMapTestExecutor CreateMapExecutor()
     {
         var appDirectoryPath = Path.Combine(this.rootDirectory, this.logicAppName);
         return new DataMapTestExecutor(appDirectoryPath);
     }
     ```

## Limitations

- **Loop Structures**: No dynamic execution in repeating structures yet
- **Non-Mocked Connectors**: All execution path actions should be mocked
- **Unsupported Actions**: Integration Account maps, custom code actions, EDI encode/decode currently unsupported
- **Preview SDK Caveats**: Limited testing on private preview components compared to GA releases

## References

- [Logic Apps Standard Automated Test Framework SDK](https://learn.microsoft.com/en-us/azure/logic-apps/testing-framework/automated-test-sdk)

> **Note:** Available in SDK v1.0.1+. Update dependencies for full intellisense.

_Last updated: Dec 07, 2025_

Version 1.0

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/data-mapper-test-executor-a-new-addition-to-logic-apps-standard/ba-p/4472440)
