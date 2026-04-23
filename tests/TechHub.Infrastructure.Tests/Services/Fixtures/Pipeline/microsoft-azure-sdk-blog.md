After July 9, 2026, the [Azure SDK for JavaScript](https://github.com/Azure/azure-sdk-for-js) will no longer support Node.js 20.x, which reaches [end-of-life](https://nodejs.org/en/about/previous-releases) on April 30, 2026. We encourage you to [upgrade to an Active Node.js Long Term Support (LTS) version](https://nodejs.org/en/download).

## Why are we doing it?

Node.js has a well-defined release schedule, with each even-numbered version enjoying an Active LTS period followed by a Maintenance LTS period before eventually reaching end-of-life. Once a Node.js version moves out of Maintenance, it no longer receives bug fixes or security updates. To ensure we’re providing up-to-date and secure libraries, we routinely drop support for Node.js versions that reached end-of-life.

As a reminder of the [Azure SDK for JavaScript support policy](https://github.com/Azure/azure-sdk-for-js/blob/main/SUPPORT.md#microsoft-support-policy), dropping support for such Node.js versions may be done without increasing the major version of the Azure SDK libraries.

## What happens on July 9, 2026?

On July 9, 2026, the Azure SDK for JavaScript will specify Node.js 22.x as the minimum supported version in its [engines field](https://docs.npmjs.com/cli/v11/configuring-npm/package-json#engines). If you’re using Node.js 20.x, installing the later versions of the SDK causes an engine deprecation warning to appear. If you set [engine-strict=true](https://docs.npmjs.com/cli/v11/using-npm/config#engine-strict), an npm installation error occurs.

The Azure SDK for JavaScript library versions released without Node.js 20.x support may continue to work on Node.js 20.x. This doesn’t imply a continuation of support. You can continue to use older versions of the libraries with Node.js 20.x, but we strongly recommend upgrading to a supported Node.js version to receive the latest features and security updates.

## Azure SDK support policies

For more information on maintenance and support policies, see the [Azure SDK support policy](https://azure.github.io/azure-sdk/policies_support.html#azure-sdk-dependencies) and the [Azure SDK for JavaScript support policy](https://github.com/Azure/azure-sdk-for-js/blob/main/SUPPORT.md#microsoft-support-policy).

## What should you do?

To ensure you’re using a supported version of Node.js, we recommend upgrading to the latest Active LTS version. You can find the latest LTS version of Node.js on the [Node.js website](https://nodejs.org).

 
 

Category

Topics

## Author

![Minh-Anh Phan](https://devblogs.microsoft.com/azure-sdk/wp-content/uploads/sites/58/2026/04/Screenshot-2026-04-06-132129-96x96.webp)

Software Engineer II at Microsoft