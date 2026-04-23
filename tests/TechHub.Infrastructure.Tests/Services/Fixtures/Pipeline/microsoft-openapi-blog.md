## Announcing the Biggest Update Ever: OpenAPI.NET v2 & v3

We are excited to announce two major releases of the OpenAPI.NET library—the most significant update since its launch in 2018. These updates ensure developers can build and document modern, interoperable APIs with the latest OpenAPI specifications.

- **OpenAPI.NET v2** Adds support for OpenAPI Specification v3.1.0, delivering performance improvements, streamlined API enhancements, and reduced dependencies. Try it [here](https://www.nuget.org/packages/Microsoft.OpenApi/2.3.9)!

- **OpenAPI.NET v3** Introduces full support for OpenAPI Specification v3.2.0, including new serialization methods, richer model properties, and expanded functionality across the API surface.Try it [here](https://www.nuget.org/packages/Microsoft.OpenApi/)!

## The impact

OpenAPI.NET is the backbone for many .NET tools and frameworks, including Swashbuckle, Semantic Kernel, NSwag, and .NET itself. These updates ensure:

- Future-proof API documentation for ASP.NET Core projects, especially as .NET 10 moves toward native OpenAPI support.

- Better performance and reliability for large-scale API ecosystems.

## Key highlights from OpenAPI.NET v2

- **OpenAPI spec v3.1 Support**: Full serialization and model support.

- **API Surface improvements**: Better API surface for the properties of type “json” which now use the native JSON node API.

- **Performance Gains:** Faster JSON parsing using System.Text.Json while maintaining YAML support.

Our [results](https://github.com/microsoft/OpenAPI.NET/blob/main/docs/upgrade-guide-2.md#results) outline an overall **50% reduction in processing time** for the document parsing as well as **35% reduction in memory allocation** when parsing JSON.

- **Lazy Reference Resolution:** Improves load times for documents with heavy `$ref` usage.

- **Reduced Dependencies:** Core library now reads/writes JSON without requiring additional packages.

## Key highlights from OpenAPI.NET v3

Here is a summary of the key new features:

- **OpenAPI spec v3.2 Support:** Full serialization and model support.

- **Enhanced Media Types:** New properties for encoding and schema support.

- **Hierarchical Tags:** Support for tag organization with kind, summary, and parent relationships.

- **Security Enhancements:** Deprecated flag and device authorization flow support.

- **Enhanced Examples:** New data value and serialized value properties.

- **Extended Parameter Support:** New locations and styles for parameter.

## Upgrade guides

For a full list of new features and a detailed developers guide to update your version please refer to the detailed upgrade guides below. We recommend upgrading to v2 first, and then v3. 

- [Upgrade guide for v2](https://github.com/microsoft/OpenAPI.NET/blob/main/docs/upgrade-guide-2.md).

- [Upgrade guide for v3](https://github.com/microsoft/OpenAPI.NET/blob/main/docs/upgrade-guide-3.md).

## Support going forward 

We are an open source project and welcome contributions from all developers. If you feel passionate about the OpenAPI initiative and OpenAPI.NET, we welcome, and appreciate your contribution. Please leverage the [contributing guide](https://github.com/microsoft/OpenAPI.NET/blob/main/CONTRIBUTING.md) before making a new contribution.

## Shoutout to our key contributors

It takes a village, and this milestone could not have been achieved without the help of key contributors – naming a few, we probably missed some: Vincent Biret (baywet) , Matthieu Costabello (kilifu), Maggie Kimani (MaggieKimani1) , Adrian Obando (adrian05-ms), Darrel Miller (darrelmiller), Irvine Sunday (irvinesunday), Martin Costello (martincostello), Safia Abdalla (captainsafia), Mike Kistler (mikekistler) and Rachit Malik (RachitMalik12).

## Feedback

For any feedback, please create a GitHub issue [here](https://github.com/microsoft/OpenAPI.NET/issues).

 
 

Category

## Author

![Darrel Miller](https://devblogs.microsoft.com/openapi/wp-content/uploads/sites/82/2024/08/e66423172aef1595eb5b25f32385e903-96x96.jpeg)

Principal API Architect

PM for Graph Developer Tooling

![Rachit Kumar Malik](https://devblogs.microsoft.com/openapi/wp-content/uploads/sites/82/2025/11/profile-pic-96x96.webp)

Product Manager II 

A Product Manager II on the M365 Copilot Agent platform. I also work on the OpenAPI.NET library.