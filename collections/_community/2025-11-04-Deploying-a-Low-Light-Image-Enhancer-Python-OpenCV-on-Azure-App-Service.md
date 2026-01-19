---
layout: post
title: Deploying a Low-Light Image Enhancer (Python + OpenCV) on Azure App Service
author: TulikaC
canonical_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/low-light-image-enhancer-python-opencv-on-azure-app-service/ba-p/4466837
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-11-04 07:33:04 +00:00
permalink: /coding/community/Deploying-a-Low-Light-Image-Enhancer-Python-OpenCV-on-Azure-App-Service
tags:
- App Architecture
- Azure App Service
- Azure Developer CLI
- Brightness Adjustment
- CLAHE
- Deployment
- Flask
- Gamma Correction
- Image Processing
- Linux
- NumPy
- OpenCV
- Python
- Saturation Boost
- Web Application
section_names:
- azure
- coding
---
TulikaC presents a Python and OpenCV-based web app for enhancing low-light images, with practical deployment instructions for Azure App Service. This guide covers the key steps in setting up the enhancement pipeline and deploying using Azure Developer CLI.<!--excerpt_end-->

# Deploying a Low-Light Image Enhancer (Python + OpenCV) on Azure App Service

**Author:** TulikaC

This guide shows how to build and deploy a lightweight, explainable low-light image enhancement web app using Python, Flask, and OpenCV. The solution leverages classic image processing techniques, wrapped in a convenient web interface, and is deployable on Azure App Service for Linux using the Azure Developer CLI.

---

## Overview

- **App Purpose**: Instantly enhance low-light photos in the browser using a fast, explainable pipeline (no heavyweight ML models required).
- **Pipeline Stages**:
  - CLAHE (Contrast Limited Adaptive Histogram Equalization)
  - Gamma Correction
  - Brightness Adjustment
  - Subtle Saturation Boost

## What You’ll Build

- A Flask web app accepting image uploads (with tunable parameters).
- On upload, the backend applies the enhancement pipeline and returns both original and improved images as base64 PNG data URIs—enabling side-by-side browser previews without persistent storage.

## App Architecture

1. **Web Browser** posts to `/enhance` with image and settings (clip_limit, gamma, brightness)
2. **Flask Backend** converts upload to NumPy RGB and calls `LowLightEnhancer.enhance_image()`
3. **Response** returns JSON: base64-encoded original and enhanced images

## Prerequisites

- Azure subscription
- **Azure Developer CLI (azd)** installed ([Setup instructions](https://aka.ms/azd))
- (Optional) Python 3.9+ for local development

## Quick Deployment Steps

```bash
git clone https://github.com/Azure-Samples/appservice-ai-samples.git
cd appservice-ai-samples/lowlight-enhancer
azd init
azd up
```

*Once complete, navigate to the provided URL, upload an image, and compare the enhanced results.*

## Key Implementation Details

### 1. Flask App Surface

- File upload size limited to 16MB:

  ```python
  app = Flask(__name__)
  app.config['MAX_CONTENT_LENGTH'] = 16 * 1024 * 1024
  ```

- Routes:
  - `/` (GET): Renders upload UI
  - `/enhance` (POST): Accepts uploads, returns JSON with images
- Parameter handling with defaults:

  ```python
  clip_limit = float(request.form.get('clip_limit', 2.0))
  gamma = float(request.form.get('gamma', 1.2))
  brightness = float(request.form.get('brightness', 1.1))
  ```

### 2. In-Memory Processing and Data URI Response

- No temp files needed: Input is converted to RGB, enhanced, then both versions are base64-encoded for instant inline display. Key functions:

  ```python
  def process_uploaded_image(file_storage, clip_limit=2.0, gamma=1.2, brightness=1.1):
      img_pil = Image.open(file_storage)
      if img_pil.mode != 'RGB':
          img_pil = img_pil.convert('RGB')
      img_array = np.array(img_pil)
      enhanced = LowLightEnhancer().enhance_image(
          img_array,
          clip_limit=clip_limit,
          gamma=gamma,
          brightness_boost=brightness
      )
      # Convert both to base64 PNGs for response
      ...
  ```

### 3. Enhancement Pipeline (`enhancer.py`)

- **CLAHE** applied to L-channel in LAB color space (prevents color artifacts)
- **Gamma correction** with a lookup table for perceptual brightness
- **Brightness boost** is a multiplier post-CLAHE/gamma
- **Saturation**: A 10% bump restores color vibrancy

  ```python
  class LowLightEnhancer:
      def enhance_image(self, image, ...):
          ...
          # Apply CLAHE on LAB L channel
          ...
          # Gamma correction
          ...
          # Brightness lift
          ...
          # Merge and add subtle saturation boost
          ...
          return enhanced
  ```

### 4. Tuning Tips

- *Too flat/muddy?* Raise `clip_limit` (e.g., 3.0–4.0)
- *Too dark?* Increase `gamma` (e.g., 1.4–1.6)
- *Too harsh?* Lower `clip_limit` or decrease `brightness_boost`
- *Color too muted?* Raise saturation factor slightly (e.g., 1.15)

### 5. Next Steps and Enhancements

- Add UI controls for more parameters (e.g., tile grid size)
- Store images in Azure Blob Storage for sharing
- Enable batch processing via background jobs

## Resources

- **[GitHub Repository: appservice-ai-samples](https://github.com/Azure-Samples/appservice-ai-samples/)**
- **[Azure App Service AI Integration Docs](https://learn.microsoft.com/azure/app-service/overview-ai-integration)**

---

*For further learning, explore more Azure App Service samples and best practices provided in the official documentation and GitHub repository.*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/low-light-image-enhancer-python-opencv-on-azure-app-service/ba-p/4466837)
