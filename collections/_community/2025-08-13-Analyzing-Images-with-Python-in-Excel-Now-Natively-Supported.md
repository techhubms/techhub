---
external_url: https://techcommunity.microsoft.com/t5/microsoft-365-insider-blog/analyze-images-with-python-in-excel/ba-p/4440388
title: 'Analyzing Images with Python in Excel: Now Natively Supported'
author: ndeyanta
viewing_mode: external
feed_name: Microsoft Tech Community
date: 2025-08-13 15:45:03 +00:00
tags:
- Computer Vision
- Data Science
- Excel Data Analysis
- Excel For Mac
- Excel For The Web
- Excel For Windows
- Image Analysis
- Image Manipulation
- Microsoft 365
- Microsoft Excel
- NumPy
- Pillow
- Python in Excel
- SciPy
- Xl() Function
section_names:
- coding
---
Ndeyanta Jallow introduces a powerful new feature in Microsoft Excel allowing users to analyze images programmatically using Python, demonstrating how developers can leverage common libraries for image analysis workflows.<!--excerpt_end-->

# Analyzing Images with Python in Excel: Now Natively Supported

**Author:** Ndeyanta Jallow

Microsoft Excel users can now analyze and manipulate images directly inside Excel using Python. This new feature, available for Windows, Mac, and Excel for the web, lets you insert images into cells and process them with Python formulas—no external tools needed.

## Key Feature Overview

- Drop images into cells and reference them directly in Python code via the `xl()` function.
- Analyze image sharpness using standard Python libraries (Pillow, NumPy, SciPy).
- Perform many types of image analyses and manipulations within familiar Excel workflows.

## Example: Detect Blurry or Sharp Images

1. **Insert an Image**
   - Go to **Insert** > **Illustrations** > **Pictures** > **Place in Cell** and select your image. Image must be placed in a single cell (e.g., **A1**).
2. **Create Python Formula**
   - In **B1**, type `=PY(` and insert the following code:

```python
from PIL import Image
import numpy as np
from scipy.signal import convolve2d

# Convert image to grayscale and array

image = xl("A1")
arr = np.array(image.convert("L"), dtype=np.float32)

# Apply Laplacian filter

laplacian = convolve2d(arr, [[0, 1, 0], [1, -4, 1], [0, 1, 0]], mode='same', boundary='symm')

# Classify based on variance

"Blurry" if np.var(laplacian) < 100 else "Sharp"
```

1. **Run and View Result**
   - Press **Ctrl + Enter** to execute. The cell displays "Sharp" or "Blurry" based on image analysis.

*Note*: Reference cell images in Python code using the `xl()` function as shown above.

## Additional Tips & Scenarios

- **Image Manipulation:** Adjust brightness, alter colors, add watermarks, or overlay logos.
- **Metadata Analysis:** Extract and analyze image metadata; generate plots from image data.
- **Performance Settings:** Adjust per-cell data size under **File > Options > Advanced > Python in Excel** (choose image size: Actual, Large, Medium, Small) to manage performance.
- **Recommended Library:** Pillow is the primary library for image operations; NumPy and SciPy support more advanced analyses.

## Availability

- **Windows:** Version 2509 (Build 19204.20002+)
- **Mac:** Version 16.101 (Build 25080524+)
- **Web:** Rolling out
- Users must have access to Python in Excel. [Learn more](https://support.microsoft.com/en-us/office/python-in-excel-availability-781383e6-86b9-4156-84fb-93e786f7cab0)

## Resources

- [Introduction to Python in Excel](https://support.microsoft.com/en-us/office/introduction-to-python-in-excel-55643c2e-ff56-4168-b1ce-9428c8308545)
- [Get Started with Python in Excel](https://support.microsoft.com/en-us/office/get-started-with-python-in-excel-a33fbcbe-065b-41d3-82cf-23d05397f53d)
- [Microsoft 365 Insider Program](https://aka.ms/MSFT365InsiderProgram)

## Feedback

Share your ideas and experiences using images in Python in Excel via the comments or through Excel's **Help > Feedback**.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/microsoft-365-insider-blog/analyze-images-with-python-in-excel/ba-p/4440388)
