# Creating 3D experiences for Azure Digital Twins

A digital twin is a connected, digital representation of a physical
environment. To create these living systems, Microsoft created Azure
Digital Twins. This product has been around for a while, but it has
always lacked built-in visualisation options for end users. This article
explains how to create a 3D visualisation on top of your digital twin
without needing developer skills and why you would want to.

Digital twins and 3D seem to go hand in hand. However, a digital twin
could be visualised in many ways. A dashboard or a web page can be just
as effective, and you can always give access to your data through an
API. Some digital twins don't even need any visualisation. Perhaps their
data is used in other systems for further processing.

However, 3D visualisations of digital twins are everywhere to the point
that the two have become synonymous for some people. In a way, this
makes sense. When you build a digital twin, you dissect the physical
environment into smaller logical parts and capture them into a logical
model. When creating a visual overview of this twin, 3D can help bring
these parts together in a recognisable fashion. Maybe that is why
Microsoft decided to strengthen the Azure Digital Twins offering with
the 3D Scenes Studio. This new preview allows users to create 3D
experiences for their digital twin data. It offers the opportunity for
non-developers to provide insights into the data that is gathered in the
twin.

Let\'s use a connected office to demonstrate the features of 3D Scenes
Studio. Our office has three phone booths for which occupancy and air
quality are measured. To display the status of these phone booths, it is
essential to know which booth is occupied and if the air quality allows
a productive meeting or phone call. For this purpose, we will show the
entire booth as occupied or unoccupied and show an alert when the air
quality is poor.

## Making a Scene

Everything starts with the Scene, which is a 3D model which contains all
the objects. If you already have 3D models that you want to use, you
will need to combine them into a *Graphics Language Transmission Format*
(or .glTF) file. Various products can do this. For the example below,
Blender was used.

*\[X\]*

*Picture 1. A 3D model of a phone booth, loaded in Azure Digital Twins*

The first thing to do when loading the Scene is to assign Elements to
the 3D model. Each Element can be linked to a twin in the Azure Digital
Twins graph, giving it access to the twin's data. A twin can be assigned
to a mesh, which is a collection of polygons, defining a subset of your
3D model. In the case of our phone booths, the 3D model is split into
two meshes: one for the booth itself and another for the glass pane next
to the door. This allows for assigning different twins to a booth
instead of the booth being one massive 3d model. Assigning an element to
a mesh is a manual process. Click on the item that needs to be assigned,
find the twin to associate and give it a name. This is fine for a couple
of objects but is very tedious to do for a few hundred booths, as it
currently cannot be automated.

After defining all the Elements, it is time to create Behaviours. A
Behaviour can change the colour of an Element or attach Widgets and
Alerts to it. In this example, the booth will turn red when occupied.
The air quality can be monitored in a widget, but if the CO2 sensor
reports over 900 parts per million, an alert is also shown.

***Picture 2. A booth with two widgets and one alert.***

After the behaviours have been created, they can be assigned to the
various elements. The whole process, from creating the Scene, to
assigning behaviours, can be done in a matter of minutes. This is the
strong suit of 3D Scenes Studio. When viewing the scene, you can freely
navigate the 3D model where data is updated constantly.

***Picture 3. Two booths are occupied. One is free but with bad air
quality***

## Why (not) use 3D Scenes Builder

When creating a scene and assigning Elements and Behaviours, a
non-technical user can provide a rich 3D experience for other users. The
result is a 3D environment that is updated in real-time. But when
digging a little deeper into how the data is updated, the whole
application is built on polling Azure Digital Twins. Every ten seconds,
the data of the twins being displayed is retrieved. This will lead to an
increase in API calls, which is also how Azure Digital Twins is being
billed; you pay for what you use. When designing a (near) real-time
application on Azure Digital Twins, a better practice would be to use
the property change events and feed them to your app through SignalR. If
you're already using the change events, the price increase would be
minimal. But in the current preview state, polling for all the twins
every ten seconds for every browser window looking at the scene can lead
to increased API calls.

Another challenge with 3D Scenes Builder is that one 3D model equals one
Scene. If you create a Scene for your factory and decide to change the
configuration, you need to start over again with a new 3D model. The
model cannot be edited in 3D Scenes Builder; therefore, you will always
need someone with 3D experience to maintain the 3D models.

It is hard to point to any particular feature as being the best part of
3D Scenes Builder. For us, the best part is that finally there is a way
to share the twin data with users that need it. For a long time, the
default visualisation was limited to Azure Digital Twins Explorer.
Having the 3D Scenes Builder adds to the maturity of Azure Digital
Twins. We expect that we will see a lot of splendid examples of projects
and prototypes making use of 3D. Because in the end, while a digital
twin is not synonymous with 3D, it undoubtedly contributes to an
impressive experience.

![Picture 1. A 3D model of a phone booth, loaded in Azure Digital
Twins](./media/image1.png)


*Picture 1. A 3D model of a phone booth, loaded in Azure Digital Twins*

![Picture 3. A booth with two widgets and one
alert.](./media/image2.png)


*Picture 2. A booth with two widgets and one alert.*

![Picture 4. Two booths are occupied, one is free but with bad air
quality ](./media/image3.png)


*Picture 3. Two booths are occupied. One is free but with bad air
quality*
