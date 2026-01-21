---
external_url: https://blogs.windows.com/windowsdeveloper/2025/09/23/windows-ml-is-generally-available-empowering-developers-to-scale-local-ai-across-windows-devices/
title: 'Windows ML Now Generally Available: Empowering Developers to Deploy Local AI on Windows Devices'
author: stclarke
feed_name: Microsoft News
date: 2025-09-24 16:23:25 +00:00
tags:
- AI Dev Gallery
- AI Inference
- AI Toolkit For VS Code
- AMD Ryzen AI
- App Development
- Company News
- CPU
- Edge AI
- GPU
- Intel OpenVINO
- Local AI
- Model Deployment
- Model Quantization
- NPU
- NVIDIA TensorRT
- ONNX Runtime
- Qualcomm Snapdragon
- Silicon Partners
- Windows 11
- Windows App SDK
- Windows ML
section_names:
- ai
- coding
---
stclarke summarizes how Microsoft’s Windows ML is now generally available for developers to deploy, optimize, and manage AI models locally on Windows 11 devices, with deep integration across diverse hardware and tools.<!--excerpt_end-->

# Windows ML Is Generally Available: Empowering Developers to Scale Local AI Across Windows Devices

**Author**: stclarke

## Overview

Microsoft announces that Windows ML, its built-in AI model inferencing runtime for Windows 11, is now generally available for production use. Windows ML allows developers to deploy, optimize, and manage AI models locally across a diverse array of Windows hardware, seamlessly supporting CPUs, GPUs, and NPUs from major silicon partners like AMD, Intel, NVIDIA, and Qualcomm.

### Key Capabilities

- **Local AI Inference**: Developers can run advanced AI workloads directly on Windows devices, increasing responsiveness, privacy, and cost-efficiency.
- **Cross-Hardware Support**: Windows ML integrates with CPUs, GPUs, and NPUs through execution providers (EPs), abstracting away hardware differences and enabling optimal performance.
- **ONNX Model Compatibility**: Developers can bring existing ONNX models or convert PyTorch models using the AI Toolkit for VS Code for straightforward deployment on Windows devices.
- **Streamlined Deployment**: Automatic detection and download of required execution providers reduce app size and complexity.
- **Silicon Partner Optimizations**: Deep integration with AMD (Ryzen AI), Intel (OpenVINO), NVIDIA (TensorRT for RTX), and Qualcomm (QNN EP for Snapdragon) to harness the strengths of each platform.

### Developer Tooling

- **AI Toolkit for VS Code**: Allows model conversion (e.g., PyTorch to ONNX), quantization, optimization, compilation, and evaluation—streamlining preparation and deployment.
- **AI Dev Gallery**: Offers an interactive workspace for experimenting with custom AI models and deployment scenarios.
- **Windows App SDK Integration**: Windows ML ships as part of the Windows App SDK (starting with v1.8.1) and supports all devices running Windows 11 24H2 or newer.

### Real-World Adoption

Leading software vendors are integrating Windows ML into their products:

- **Adobe (Premiere Pro, After Effects)**: Accelerated semantic search, tagging, and scene detection using local NPUs.
- **BUFFERZONE**: Real-time secure web page analysis, enhancing privacy and user protection.
- **Camo by Reincubate**: Real-time image segmentation and AI-powered video improvements.
- **Dot Vista by Dot Inc.**: Voice control and OCR for accessibility scenarios.
- **Wondershare Filmora**: AI-driven body effects and real-time previews.
- **McAfee**: Automatic deepfake detection and scam protection.
- **Topaz Photo**: AI-powered image enhancement for photography.

### Getting Started

To use Windows ML in production applications:

1. Update your project to the latest [Windows App SDK](https://learn.microsoft.com/en-us/windows/apps/windows-app-sdk/stable-channel#version-171-17250401001).
2. Use Windows ML APIs to initialize execution providers and load any ONNX model.
3. Reference guides, tutorials, and sample code:
   - Learn more: [Windows ML Overview](https://learn.microsoft.com/en-us/windows/ai/new-windows-ml/overview)
   - Interactive samples: [AI Dev Gallery](http://aka.ms/ai-dev-gallery)
   - Tutorials: [ms/TryWinML](http://aka.ms/TryWinML)

## Why Local AI Matters

With Windows ML, developers no longer need to build or manage separate binaries for different hardware configurations. Microsoft’s collaboration with leading silicon partners ensures top performance and compatibility, unlocking the broadest reach for AI-powered experiences on Windows PCs.

## Resources

- [Windows ML Documentation](https://learn.microsoft.com/windows/ai/new-windows-ml/overview)
- [AI Toolkit for VS Code](https://marketplace.visualstudio.com/items?itemName=ms-ai-toolkit.vscode-ai-toolkit)
- [Windows App SDK](https://learn.microsoft.com/en-us/windows/apps/windows-app-sdk/stable-channel)
- [AI Dev Gallery](http://aka.ms/ai-dev-gallery)

## Conclusion

Windows ML is now ready for production deployment, empowering developers to scale local AI across Windows devices with streamlined deployment, model management, and leading performance across a diverse hardware ecosystem.

This post appeared first on "Microsoft News". [Read the entire article here](https://blogs.windows.com/windowsdeveloper/2025/09/23/windows-ml-is-generally-available-empowering-developers-to-scale-local-ai-across-windows-devices/)
