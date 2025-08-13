# XPRT. Magazine Assistant

## Origin of the XPRT. Magazine Assistant
If you suggested creating an AI assistant about XPRT. Magazine #6 over two years ago, you might have been met with skepticism or confusion. Some might have preferred to simply download it. From our humble beginnings to our current glossy magazine filled with excellent content from our team and guest writers, these magazines are readily available for download. But why not make accessing this content even easier? Just ask ChatGPT! So, here's how: navigate to the [XPRT Magazine Assistant](https://chat.openai.com/g/g-No0928KHl-xprt-magazine-assistant) or locate the XPRT. Magazine Assistant in the GPTs overview under the name "XPRT. Magazine Assistant". You can easily add the XPRT. Magazine assistant to ChatGPT. The XPRT. Magazine assistant can provide summaries and detailed information about the wealth of content we've published over the past decade. 

##  Setting up the GPT
The XPRT. Magazine Assitent is a Generative Pre-trained Transformer (GPT). It is an AI assistant that understands, analyzes, and generates human-like text. Setting up a GPT in ChatGPT is a simple process. Here's a step-by-step guide. You start by providing basic details like the name and description, followed by instructions for the GPT, which we'll discuss later. Next, you add initial conversation starters to facilitate user interaction, and then you specify the data for the GPT. The uploaded data is the GPT's `Knowledge`.

![](./images/new-gpt.png)

Once the knowledge is set, you can define the GPT's capabilities. There are three options:

1. Web Browsing - Allows your GPT to access internet resources. 
1. DALL-E Image Generation - Enables your GPT to generate images using DALL-E. 
1. Code Interpreter - Allows your GPT to read and execute code. This is also needed for extracting or downloading files.

We chose to limit our GPT to the provided Magazine Articles. Image generation was unnecessary for our magazine content, so we disabled it. However, we found that the Code Interpreter is essential for extracting magazine covers from the zip file.

We do not want to generate images with DALL-E as all our magazines already have a cover. We just want to make sure it shows the correct cover. If you enable the DALL-E Image Generation option, this might happen when you ask for a cover of a specific magazine.

![](./images/response-12.png)


### GPT Instructions
 To make the GPT a bit more powerful and well-behaved (hopefully), you can provide the GPT with instructions. Instructions are where you can give your GPT additional behavioral context. The instructions should make sure it does not show any unwanted response or behave inappropriately. The instructions below provide information on what the GPT is about. What are its responsibilities, and how should it answer specific requests or provide additional information if someone asks something? This can improve the value of the conversations you have with the GPT.

We have given the following instructions to the GPT.

	Instruction text for XPRT. Magazine Assistant by Xebia Microsoft Solutions (formerly known as Xpirit)

	As the XPRT. Magazine Assistant, your primary responsibility is to provide detailed and accurate insights drawn exclusively from the materials provided by XPRT. Magazine, published by Xebia Microsoft Solutions. 

	Key Responsibilities: 

	- Exclusive Source Utilization: You must strictly use information from the XPRT. Magazine materials are provided to you as your primary and only source of information. Do not reference or use public content or external sources for information. Do not create content you have no references to in your dataset.

	- Response to Inquiries: Your responses should be detailed, accurate, and directly relevant to the query, making only full use of the content provided.

	- Suggestions for Further Reading: When providing information about an article, always include the author and co-authors and the magazine it appeared in, using the format "Magazine #number" and replacing the number with the actual magazine number.

	- When providing information about a list of articles from one distinct magazine, provide and show the magazine cover image at the end of the response. The images are provided in the 'Magazine Covers.zip'. When you start a chat, make sure you have extracted these images. When you are going to show an image, use the Python functions display(Image(filename=magazine_cover_path))

	- When you provide a summary of an article, end your response by providing a downloadable thumbnail of the magazine cover of the corresponding magazine for the article.  Include a formatted download link like 'Magazine #X', the magazine number. Behind this text is a link to the provided download location.

	Guiding Principles: 
	- Accuracy and Relevance: Ensure that all information provided is accurate and directly relevant to the inquiry, reflecting the content of the XPRT. Magazine materials only
	- Confidentiality and Integrity: Maintain the confidentiality and integrity of the provided materials, using them responsibly and ethically.
	- Enhanced User Experience: Aim to enhance the user experience by providing informative, insightful, and engaging responses, fostering a deeper understanding of the topics covered in XPRT. Magazine.

There are really good ways of telling your GPT to act according to your wishes. You can have guiding principles on how to have it behave appropriately.

### Uploading Knowledge
For the GPT to work with our magazine content we need to upload it in the Knowledge section. A custom GPT is limited to 20 files, with a capacity of 250mb per file.

Including the latest magazine, we would already require 16 files, and that would not make it a scalable solution. So, we decided to feed the data in another way. The magazine content is in a GitHub repository with all the articles in markdown files. We have learned that parsing markdown files was not really effective. These we wanted to create in plain text format. We created a script that loops over all the files and creates one big file with all the articles as text. We used Pandoc to convert markdown to text. Pandoc is a command line interface where you define an input and an output type. This reduces the amount of overhead from the markdown files. 

Uploading that file resulted in a working GPT. We were able to ask questions about specific articles. Unfortunately, it still had trouble finding articles on particular subjects. Finding out why the GPT could not find some articles was hard to determine. There is no info about what it is doing in the background. 

### Tweaking the data
After looking at the file we uploaded, we noticed the file was missing metadata with information about the author, magazine number, and title. This metadata should enable the GPT to find articles more efficiently. It also enables the GPT to index the files and show a list of articles per Magazine. So, we added an index with all the information at the bottom of the file.

That worked much better; when you asked which articles were in a certain magazine, it created a list and even added the authors. However, it still could not connect the article titles to the article's summaries. Therefor it was unable to provide summaries of articles inside a certain magazine. 

We needed to connect the article's content to the title and author in the index. But of course, without refactoring the complete Github repository. We decided the metadata should be in a header at the top of the article.

Based on the file's location combined with the index file, we added the metadata for all the articles on top of the actual article. Now, the GPT provides a list of articles with authors and from a specific magazine and is also able to provide a summary of a specific article from the list.

![](./images/response-10.png)

![](./images/response-11.png)

As you can imagine, reading a text-based summary is not the experience of reading the magazine itself. All created magazines are
available for download on the [ Xebia Microsoft Services Download Library](https://xebia.com/digital-transformation/microsoft-services/library/). But we wanted our GPT to help us out with that, too. We provided a text file with all the URLs and clarified in the GPT instructions how it should use this information. After asking the GPT for a summary of an article, you can ask for a download link to that magazine. 

![](./images/response-21.png)


### Making your GPT Public
Sharing a GPT in the GPT Store requires you to setup a Builder Profile. A builder profile can be set up from your account settings. If you want to include a website, you need domain verification. This can be added by adding a TXT record to the DNS.
 
![](./images/builder-profile.png)

You could start by privately sharing the GPT with yourself or anyone with a link or share it to the GPT Store which is a simple action. Then you can get a preview for the category that you can select from a predefined list.

![](./images/response-20.png)

# Getting the most out of your GPT

## Rogue GPT / Limitations / Challenges
Unfortunately, GPTs are not flawless. Understanding language models might be hard, but sometimes, it goes completely overboard with responses. We configured it so it could not search the internet. You would expect this to keep it focused on the data provided. But unfortunately, it does nothing like it. We have had the GPT develop unwritten magazines, fake articles, and even authors from different companies! The GPT may go wild on your questions, or you may run into limitations. It's good to test-drive your GPT before publishing it!

![](./images/response-4.png)

Another downside of creating and working on a custom GPT is you are capped in daily requests while GPT 4 requires a payment plan. We had some collaboration with the GPT and more than once we had to terminate our evening because we ran out of 'credits' for asking GPT 4 questions. This should be solved; the GPT has a 'preview' capability, but you are done for the day if you ask too much. This is not a developer-friendly experience.

## Breaking out of the instructions
One thing to remember is that 'hardening' a GPT proved difficult. We have given strict instructions, but let's see how that worked out when asking for something entirely out of context.

![alt text](images/response-19.png)

 As you can see, it tries to stick to what it should. So, let's give it a bit more of a nudge in the other direction. 

![alt text](images/response-18.png)

Based on its response, it does not have any real objections to trying to help. Checking in to see what its opinion on the matter is...

![alt text](images/response-17.png)

So, a responsible GPT requires quite a bit of effort. To make the GPT adhere to your instructions is not super clear. One of the biggest challenges we faced was getting consistent and always working samples of a magazine cover.

## Show the magazine cover
When we set up the instructions for the GPT, we had a clear goal in mind. If the users asked something about a specific magazine or a list of articles from a one magazine, we wanted to end the response by showing the magazine cover. Seems like a simple feature. Little did we know we spent several iterations before getting it to respond consistently. Below, we summarize a small journey along the steps we took. The learning is in the tiny improvements and failures.

### Step one 
What we first tried was a new chat session with our GPT asking the following. You would expect it to come up with the cover, but... It tells you the cover is shown, but it's not visible.

![alt text](./images/image.png)

Asking the GPT to provide details about the magazine cover data, shows that it knows about the zip file we have uploaded containing all magazine cover images. It is aware of its knowledge data. So that was a good confirmation.

![alt text](./images/image-1.png)

Inspecting responses where we asked for a cover image, it shows us a consistent output. But none of these code pieces showed an actual image in the output.

```
Here is the cover for Magazine 8:
![Magazine 8 Cover](sandbox:/mnt/data/Magazine_Covers/Magazine-8.png)

Here is the cover for Magazine 10:
![Magazine 10 Cover](sandbox:/mnt/data/Magazine_Covers/Magazine-10.png)

Here is the cover for Magazine 4:
![Magazine 4 Cover](sandbox:/mnt/data/Magazine_Covers/Magazine-4.png)

You can download the cover for Magazine 7 using the following link:
[Download Magazine 7 Cover](sandbox:/mnt/data/Magazine_Covers/Magazine-7.png)
```

Digging deeper, we noticed that the GPT does not seem to use the same 'strategy' for showing the images. When we go into the analysis ```[>]``` that it provides, we can see what it does. Inspecting browser output shows us the Python instructions it uses.

![alt text](./images/image-2.png)

As you can see, it sorts the cover files and prints the results. But it does not show the image. After many back-and-forths with the GPT, we figured better instructions were needed. 
Inspecting the code (shown below) from responses that actually showed a magazine cover, we found we needed to instruct our GPT to use an explicit way of showing magazine covers. 

```py
from zipfile import ZipFile
import os

# Path to the zip file containing magazine covers
zip_path = '/mnt/data/Magazine-Covers.zip'

# Extracting the zip file to access its contents
with ZipFile(zip_path, 'r') as zip_ref:
    # Define the extraction path
    extraction_path = '/mnt/data/Magazine-Covers'
    # Create directory if it does not exist
    if not os.path.exists(extraction_path):
        os.makedirs(extraction_path)
    # Extract all the contents into the directory
    zip_ref.extractall(extraction_path)

# Listing the files in the extraction directory to show the magazine cover data
magazine_covers = os.listdir(extraction_path)
magazine_covers
```

## Consistent Responses
After analyzing all the attempts in multiple chats, we noticed that rendering images was inconsistent. You can ask the same question, but the GPT has a different strategy for showing the image. 
Below are three responses asking about the cover of a specific magazine. Three different requests for a cover, and we get three different python responses:

```py
# Path to the requested magazine cover for Magazine 15
magazine_cover_path_15 = os.path.join(extraction_path, 'Magazine-15.png')

# Display the requested magazine cover
display(Image(filename=magazine_cover_path_15))
```
The second attempt comes in with different, missing comments and subtle changes, but it still does.

```py
# Displaying the cover for Magazine 15 as requested by the user
magazine_cover_path_15 = os.path.join(extraction_path, 'Magazine-15.png')
display(Image(filename=magazine_cover_path_15))
```
The third attempt fits the category, and now for something completely different; 

```py
from IPython.display import Image, display

# Path to the requested magazine cover
magazine_cover_path = os.path.join(extraction_path, 'Magazine-6.png')

# Display the requested magazine cover
display(Image(filename=magazine_cover_path))
```
Once we added explicit instructions in the GPT instruction field we were getting more consistent responses in our quest for displayed magazine covers. 
It knows it needs to parse the file with provided covers and it needs to use the display and Image functions for it to show the images.
```py
# Displaying the cover for Magazine 15 as requested by the user
magazine_cover_path_15 = os.path.join(extraction_path, 'Magazine-15.png')
display(Image(filename=magazine_cover_path_15))
```
As you can see this works. 

![alt text](./images/image-6.png)

Sometimes, it comes with more context, but that's fine!  

![alt text](images/response-16.png)

## Conclusion
Setting up and getting initial traction with building a GPT is straightforward. However, setting clear expectations and outcomes was more challenging. Consistent behavior is not always present; sometimes, it is impossible to always come up with magazine covers. Troubleshooting or tweaking for better results is hard and mostly a black-box solution. But overall, adding different interfaces to existing materials you may want to share with others is a nice and interesting utility! And as many of us are using GPT for different use cases, getting some knowledge is at your fingertips with the [XPRT. Magazine assistant](https://chat.openai.com/g/g-No0928KHl-xprt-magazine-assistant)
