![](./images/banner.png)

# From Concept to Reality: Crafting the GitHub Copilot Bootcamp 

* By Thijs Limmen and Randy Pagels

The journey started when GitHub requested us to build a training around GitHub Copilot, an AI-powered coding assistant. The goal was to create a hands-on immersive experience for developers to learn how to use Copilot in their daily work. We didn't just want to create an ordinary training; we ended up crafting a one of a kind training named: `GitHub Copilot Bootcamp`, which is themed around aviation and the Wright Brothers. The theme symbolizes innovation and a breakthrough in software development, akin to the Wright Brothers' revolution in aviation. It highlights how GitHub Copilot and generative AI tools are the next evolution in software engineering, pioneering a new era and offering transformative ways for developers to interact with development processes.
 
## Our Alliance with GitHub: Navigating New Horizons

Ever since we started working with GitHub, we have been able to create a strong relationship with their team, helping GitHub with a variety of trainings and projects. The last training we developed for GitHub was centered around GitHub Advanced Security for Azure DevOps. GitHub was impressed with our work and decided to entrust us again for this exciting new project to craft a Bootcamp for GitHub Copilot. We were given the freedom to create a training that teaches GitHub Partners about GitHub Copilot.

![](./images/copilot-logo.png)

## Charting the Flight Path for the GitHub Copilot Bootcamp

![](./images/copilot-webinar.png)

A few months prior to crafting the GitHub Copilot Bootcamp, Thijs delivered a Webinar about GitHub Copilot focused on the pioneering spirit of the Wright Brothers. He showcased the Core GitHub Copilot features through simple and yet powerful demos through a .NET 7.0 API and a basic REST Plane Controller.
When Randy was initially entrusted with developing the GitHub Copilot Bootcamp, he immediately contacted Thijs, armed with a basic outlined bootcamp plan and plenty of ideas. This blueprint not only embraced the aviation theme but was also in harmonious alignment with the thematic essence of the Webinar. Thijs contributed a wealth of innovative ideas to this collaborative effort, enriching the project's vision and scope.

Link to webinar: https://youtu.be/6wVpJetTIqM
![](./images/qr-code-webinar.png)

On Saturday, January 13, 2024, we launched the aviation-inspired framework, solidifying its foundation with the ethos "Soaring with Code: Navigating the Development Skies with GitHub Copilot." This thematic choice for our bootcamp felt like a daring leap, marking a significant moment in our journey. GitHub as a company is playful but is also a formal company with a lot of customers. We were not sure if they would be open to such a theme. However, we were confident the theme would be a hit. After discussing the idea with our team at Xebia, we decided to go for it!

We used many subtle references to the aviation theme throughout the Bootcamp.  A couple of examples are the use of an airplane UTF8 icon (✈️) in the markdown files, aviation-themed lab titles like "Pre-Flight Checklist", "Welcome Aboard", "Flight demonstration", "Taking Off with Code", "Navigating the Code Clouds", "Auto-Pilot Mode", "Air Crash Investigation", and "Safe Landing". Besides the themed chapters, we also generated images through DALL-E 3 that are in the style of vintage aviation to enhance the slide deck. Another feature are the code-related examples such as the Plane model that describe planes built by the Wright Brothers. We also included a Flight model that introduces flights performed by the Wright Brothers. Lastly, there is an Airfield model representing the locations where the Wright Brothers tested their aircraft designs.

The timeline was tight, offering a very short runway before Randy and Thijs conducted a crucial dry run at the Xebia XKE (Xebia Knowledge Exchange) on February 6, 2024, a mere few weeks prior to its inaugural presentation to the first customer on February 26, 2024. This carefully orchestrated sequence of events demonstrated our commitment to excellence and innovation, setting the stage for a ground-breaking debut.

## Crafting the Bootcamp: A Flight Plan for Success

![](./images/hands-on-lab-slide.png)

Our approach was to ensure each participant, regardless of their starting point, could progress at their own pace. Each module of the bootcamp begins with a presentation using carefully designed slides to clarify key concepts, setting the stage for the participants. Next, the participants would go through practical labs designed to provide hands-on experience with GitHub Copilot. Every lab introduces a new concept or feature of GitHub Copilot, building upon the previous one. 
We faced a few challenges in creating the labs. GitHub Copilot is unpredictable and can sometimes generate code that is incorrect. We used prompt engineering techniques to make the outcome more predictable. We also made sure all labs contain the solution that can be followed step-by-step by the participants. If a participant gets stuck anyway, we made sure that all labs are not dependent on each other, so they can skip a lab and continue with the next one.
 
## Very basic and Powerful labs - Airplane Docking - Add new Flight Model

Our goal was to have every lab in its most basic form, showing the most powerful result of GitHub Copilot. In the following example the participant must `Add a New Flight Model`. GitHub Copilot will suggest a new `Plane` object with the next available `Id`. Also, notice how Copilot understood that the next Plane is the Wright Model B and it automatically suggested the `Name`, `Year`, `Description`, and `RangeInKm` properties. The underlying LLM also learned from Wikipedia and other sources to understand the history of the Wright Brothers.

```csharp
    public class PlanesController : ControllerBase
    {
    /* Rest of the methods */

    private static readonly List<Plane> Planes = new List<Plane>
    {
    // Other planes
    new Plane
    {
    Id = 3,
    Name = "Wright Model A",
    Year = 1908,
    Description = "The first commercially successful airplane.",
    RangeInKm = 40
    }<---- Place your cursor here
    };
    }
```

## Step-By-Step Labs

In the following lab the participant must `Complete the Wright Brothers Fleet`. This example shows the amount of detail we added to every lab, to make sure that anyone could do the lab by following the lab step-by-step. Also note that we introduce a new feature in this lab to `Start Inline Chat` with GitHub Copilot.

---- Start Step by Step Lab ----

- Open the `Plane.cs` file located in the `Models` folder.

- Add a `ImageUrl` property to the model.

- Type `public string ImageUrl { get; set; }` in the `Plane.cs` file.

```csharp
    public class Plane
    {
    public int Id { get; set; }
    public string Name { get; set; }
    public int Year { get; set; }
    public string Description { get; set; }
    public int RangeInKm { get; set; }

    // New property
    public string ImageUrl { get; set; }
    }
```

- Open the `Controllers/PlanesController.cs` file.

- Select all content of the `Planes` List.

- Right click and select the option `Copilot` -> `Start Inline Chat`.

- Type the following command:

```
Add the new ImageUrl property and the next plane from the Wright Brothers Fleet
```

<img src="../../Images/Screenshot-Planes-List.png" width="800">

> [!Note]
> Screenshot is made at 8th of February 2024. The UI of the Copilot Chat extension can be different at the time you are doing the lab. (Please notify us if the UI is different.)

- Accept the suggestion by selecting `Accept` or pressing `Enter`.

>[!Note]
> GitHub Copilot can do more than one thing at a time. It added the new property to each plane and next Wright Brothers plane to the list of planes.

---- End Step by Step Lab ----

## Advanced Labs - Regex Aerobatics Show - Advanced Prompt Engineering

It doesn’t matter if participants of the GitHub Copilot Bootcamp are beginners or already experienced with GitHub Copilot. We made sure to add Advanced concepts and labs, to make the bootcamp engaging for all levels. The following is an example of how we designed our presentation slides. In this case we teach about the Chain-of-Thought prompt engineering concept. 
![](./images/advanced-lab-slide.png)

Chain-of-Thought (CoT) prompting enables complex reasoning capabilities through intermediate reasoning steps. With CoT you can get better results on more complex tasks that require reasoning before responding. 

Read more about Chain-of-Thought prompting here: https://www.promptingguide.ai/techniques/cot
 
## Bootcamp Boarding Procedures
The GitHub Copilot Bootcamp is created inside a Git Repository. This Git Repository consist of all the hands-on labs modules and the Wright Brothers demo application that the participants are running. Every participant needs to be able to run the bootcamp in isolation without interfering with other participants. In order to do this, we created an automation script to add the GitHub Usernames of the participants to the GitHub Copilot Bootcamp Organization. This in turn assigns a GitHub Copilot License to the GitHub User. For every GitHub Username, we also create a Clone of the GitHub Copilot Bootcamp Repository. A few days after the participants completed the bootcamp, we also de-provision the participants from the GitHub Organization saving on Copilot license costs.

## Starting the Engine

Once a participant is added to the GitHub Organization and a GitHub Repository containing the hands-on labs are provisioned, it’s also very important that anyone can easily get started with the labs without having to configure their local environment. In this case that means running the NET 7.0 Web API, having the GitHub Copilot extensions plus some extra extensions installed inside Visual Studio Code and an active GitHub Copilot license configured, as described in previous chapter.
To achieve this, we used GitHub DevContainers. A DevContainer is the description of an Operating System / Virtual Machine that can run for free on a hosted GitHub CodeSpaces environment, but also runs inside a Docker Container on the local machine of the participant. 
At one point we had a group of participants that all had a Java development background. They had never developed on Visual Studio Code IDE and had never written any C# or run a .NET application. By having a DevContainer set-up, they were able to get their hands on GitHub Copilot without any issues and they learned some things about another technology.


Devcontainer.json

```json
    {
        "name": "C# (.NET)",
        "build": {
            "dockerfile": "Dockerfile",
            "args": { 
                "VARIANT": "7.0"
            }
        },
        "settings": {},
        "customizations": {
            "vscode": {
                "extensions": [
                    "ms-dotnettools.csharp",
                    "ms-vscode.powershell",
                    "humao.rest-client",
                    "GitHub.copilot",
                    "GitHub.copilot-chat",
                    "ms-dotnettools.csdevkit",
                    "github.vscode-github-actions"
                ]
            }
        },
        "forwardPorts": [1903],
        "remoteUser": "vscode",
        "features": {
            "docker-in-docker": "latest",
            "kubectl-helm-minikube": "latest",
            "azure-cli": "latest"
        }
    }
```

Dockerfile

```Dockerfile
    ARG VARIANT="7.0-bullseye-slim"
    FROM mcr.microsoft.com/vscode/devcontainers/dotnet:0-${VARIANT}
```

## Buttery Landing: The GitHub Copilot Bootcamp's Impact

Initially, GitHub tasked us to craft a 3-hour bootcamp, to teach everything there is about GitHub Copilot. We were very fanatic in creating hands-on labs so we accidently ended up making at least 6 hours of hands-on lab material. We felt like to show GitHub Copilot as a product, it would need as much training material as possible. We would now call it a happy accident, as Bob Ross would explain it in his famous paintings. A good thing is we now can offer a 6-hour version of the bootcamp and a trimmed down 3-hour version denoted by “optional” hands-on labs.

Before this bootcamp we had never met each other and from day one of creating this bootcamp we already felt like buddies, working towards the same goal. We experienced many moments where one of us had a crazy idea and then took it to the next level. We can’t wait for a follow-up on this Bootcamp, to take it up a notch. The GitHub Copilot Bootcamp has resonated with over 2,200 attendees already, sparking enthusiastic feedback, and notably driving GitHub Copilot adoption. Although many attended the live sessions, the Bootcamp's reach extends far beyond its immediate audience, with even more people viewing the recordings. More importantly, customers are observing a noticeable increase in GitHub Copilot usage, highlighting the Bootcamp's role to springboard the integration of AI-assisted coding into daily development work. 

More recently, GitHub officially announced the following message about the GitHub Copilot Bootcamp to all the GitHub Partners:

> \"Be a part of the leading edge by joining the GitHub Copilot Bootcamp exclusively designed for GitHub by Xebia\" -- GitHub

This message touches the effort we put in to make it a one-of-a-kind Bootcamp and the unique relationship we have as Xebia with the GitHub Team.

!!! @Olaf can you feature this chapter in the magazine? !!!

## Copilot Clips: YouTube Channel to learn more about GitHub Copilot 

![](./images/copilot-clips-banner.jpg)
![](./images/copilot-clips-logo.jpg)

Thijs and another colleague Maik Müller have started a YouTube channel called `Copilot Clips`. The channel showcases the power of GitHub Copilot in short clips, highlighting its capabilities and features. The channel is a great resource for developers to learn more about GitHub Copilot and how it can be used in their daily work. Subscribe to stay up-to-date with the latest GitHub Copilot clips and tricks!


Link to Copilot Clips channel: https://www.youtube.com/@CopilotClips
![](./images/qrcode-copilot-clips.png)



!!! @Olaf can you feature this chapter in the magazine? !!!