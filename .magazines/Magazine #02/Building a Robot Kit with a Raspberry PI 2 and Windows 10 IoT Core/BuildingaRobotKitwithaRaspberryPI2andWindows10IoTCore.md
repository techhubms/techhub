Marco Mansi

# Introduction

The Internet of Things (IoT) ecosystem is growing faster and faster, and
with the introduction of Windows 10, Microsoft has made it clear they do
not want to be a spectator. Microsoft has already been present since the
beginning with the Windows Embedded operating systems, and Windows 10
IoT Core is the next generation OS designed specifically for use in
small footprint, low-cost devices and IoT scenarios.

In this article I intend to show how easy it is to use Windows IoT and
the Universal Windows Platform.

# About Windows 10 IoT Core and the Universal Windows Platform

Windows 10 IoT Core is a version of Windows 10 that is optimized for
smaller devices with or without a display. It runs on IoT devices such
as the Raspberry Pi 2. Software for Windows 10 IoT Core can be written
using the extensible Universal Windows Platform (UWP) API.

In April 2015, a test version of Windows IoT was released, and in
November 2015, the official release was announced.

With the introduction of the Universal Windows Platform (UWP), it is now
possible to create applications for every device that runs Windows 10.
This evolution allows apps that target the UWP to call not only the
WinRT APIs that are common to all devices, but also APIs (including
Win32 and .NET APIs) that are specific to the device family the app is
running on. The UWP provides a guaranteed core API layer across devices.
This means you can create a single app package that can be installed
onto a wide range of devices. What's more, with that single app package,
the Windows Store provides a unified distribution channel to reach all
the device types your app can run on.

![](./media/image1.png)

# The Robot Kit

## Let's get started

Many people have an IoT device without knowing it. For example, a small
Raspberry Pi which they probably use for watching TV. But there is much
more that you can do with these kinds of devices.\
\
![](./media/image2.jpeg)
When you need inspiration for an applicable
IoT device, you can look at the [Microsoft Windows
IoT](https://microsoft.hackster.io) page on
[www.hackster.io](http://www.hackster.io). A project that really teases
everyone's imagination is building a robot, so let's do this!

This is how the result of the project should be:

![](./media/image3.jpeg)


## Components needed

  -----------------------------------------------------------------------
  ![](./media/image4.png)
  Wooden robot frame in 7 pieces
  -----------------------------------------------------------------------

  -----------------------------------------------------------------------

+-----------------------------------------------------------------------+
| ![](./media/image5.jpeg)                 |
|                                          |
|                                                                       |
| A ball caster (Pololu Ball Caster with 1/2″ Metal Ball)               |
+=======================================================================+
+-----------------------------------------------------------------------+

  -----------------------------------------------------------------------
  ![](./media/image6.png)
  2x continuous rotation servos (SpringRC SM-S4303R
  Continuous Rotation Servo)
  -----------------------------------------------------------------------

  -----------------------------------------------------------------------

+--------------------+------------------------+-----------------------+
| +---------------+  |   ------------------   |   -------------       |
| | ![](./media/i |  | ---------------------- | --------------------- |
| | mage7.jpeg){w |  |   ![](./media/imag     | --------------------- |
| | idth="1.35in" |  | e8.jpeg) |   ![](./media         |
| | hei           |  |   height="1.69206      | /image9.jpeg){width=" |
| | ght="1.35in"} |  | 36482939633in"}6x 15cm | 1.8701388888888888in" |
| |               |  |   (6\") Male-to-       |   he                  |
| | A digital     |  | Female wires (2 red, 2 | ight="1.6916666666666 |
| | switch        |  |   white and 2 black)   | 667in"}2x 15 cm (6\") |
| | (D2F-01L      |  |   ------------------   |                       |
| | switch)       |  | ---------------------- | Female-to-Female wire |
| +===============+  |                        | s (1 red and 1 black) |
| +---------------+  |   ------------------   |   -------------       |
|                    | ---------------------- | --------------------- |
|                    |                        | --------------------- |
|                    |                        |                       |
|                    |                        |   -------------       |
|                    |                        | --------------------- |
|                    |                        | --------------------- |
+====================+========================+=======================+
+--------------------+------------------------+-----------------------+

  -----------------------------------------------------------------------
  ![](./media/image10.jpg)
   A set of M2.5 screws, nuts, bolts and
  standoffs
  -----------------------------------------------------------------------

  -----------------------------------------------------------------------

  -----------------------------------------------------------------------
  ![](./media/image11.jpeg)
   Raspberry Pi 2, a 2 Amp power supply, SD
  card, network Ethernet cable
  -----------------------------------------------------------------------

  -----------------------------------------------------------------------

## The assembly

For the wooden frame there is a GitHub repo with cutting plans that can
be uploaded to an online laser cutting, 3D printing & metal machining
services, e.g.
[Ponoko](file:///G:\000%20HK%20werk\XPirit\Magazine%20Jan%202016\www.ponoko.com)
for the U.S.A or [Formulor](http://www.formulor.de) in Europe
(Germany).\
I uploaded the sumbotjr-3mm_ponoko.eps cutting plan from the GitHub repo
and used a Plywood Birch 3 mm on a 384x384 mm sheet.\
The order process was really easy and it was nice to see the existence
of such online services.

The final laser-cut package looks like
this:![](./media/image12.jpeg)


The assembly is really simple -- just connect all together like Lego
pieces.![](./media/image13.jpeg)


## The Pin connections

The following pin layout is available from the Hackster Robot Kit page:
![](./media/image14.png)


I had to figure out which number belongs to a pin, but luckily after
some research I found the following images:

![](./media/image15.png)


![](./media/image16.jpeg)

## Installing Windows IoT

### Set up your development PC

To set up your development PC, do the following:

-   Make sure you are running the public release of Windows 10 (version
    10.0.10240) or better.

-   Install Visual Studio 2015 with Update 1 (any version is good,
    Community, Professional, Enterprise) and be sure to have the
    Universal Windows App Development Tools installed.

-   Install Windows IoT Core Project Templates. The templates can be
    found by searching for Windows IoT Core Project Templates in the
    Visual Studio Gallery or you can find the link in the references
    section.

-   Enable developer mode on your Windows 10:

    -   On your device that you want to enable, go to Settings.

    -   Choose Update & security, then choose For developers:

> ![](./media/image17.png)
> 

### Set up a Windows 10 IoT Core Device (in our case the Raspberry PI 2)

When you have a Raspberry PI 2, you can set up and configure it easily
using the Windows 10 IoT Core Dashboard. The dashboard can be used to
set up the RTM (public) version of Windows 10 IoT Core and requires a PC
running Windows 10. I added a link to the dashboard application in the
References section.

Put your MicroSD card into your PC and carry out the following steps:

-   Start the IoT Dashboard application.

-   Click on Set up a new Device.

-   Select Device Type: Raspberry Pi 2 and Windows IoT Core for
    Raspberry PI 2.

-   Click on I accept the software license terms.

-   Click on Download and Install.

The dashboard will start to download the Windows IoT image:

![](./media/image18.png)


After the download has been completed, it will ask to install it on the
microSD card:

![](./media/image19.PNG)


![](./media/image20.PNG)

After the image has been written on the microSD, safely remove it from
your PC and insert it into the Raspberry PI.

### Connect your Windows 10 IoT Core device to your development PC:

In order to develop apps for your IoT device, the IoT Core device and
development PC should be on the same local network. There are a few
options for setting this up.

#### Option 1: Plug your device into your local network

The easiest and best way to connect to your device is to plug it into a
local network that your development PC is already connected to. Plug the
Ethernet cable from the device into a hub or switch on your network. To
keep things simple, it's best if you have a DHCP server (such as a
router) present on your network so the device gets an IP address when it
boots.

#### Option 2: Connecting your Windows 10 IoT Core device directly to your PC & setting up Internet Connection Sharing (ICS)

If you don't have a local network to plug your device into, you can
create a direct connection to your PC. In order to connect and share the
internet connection in your PC with your IoT Core device, you must have
the following:

-   A spare Ethernet port on your development machine. This can be
    either an extra PCI Ethernet card or a USB-to-Ethernet dongle.

-   An Ethernet cable to link your development machine to your IoT Core
    device.

Follow the instructions below to enable Internet Connection Sharing
(ICS) on your PC:

-   Open up the control panel by right-clicking on the Windows button
    and selecting Control Panel, or by opening up a command prompt
    window and typing control.exe.

-   In the search box of the control panel, type adapter.

-   Under Network and Sharing Center, click View network connections.

-   Right-click the connection that you want to share, and click
    Properties.

-   Click the Sharing tab, and select the Allow other network users to
    connect through this computer's Internet connection box.

After you have enabled ICS on your PC, you can now connect your Windows
10 IoT Core device directly to your PC. You can do this by plugging in
one end of the spare Ethernet cable into the extra Ethernet port on your
PC, and the other end of the cable into the Ethernet port on your IoT
Core device.

**Note:**

The Sharing tab won't be available if you have only one network
connection.

### Boot Windows 10 IoT Core

If you have an HDMI cable and a monitor with HDMI input, connect it to
your Raspberry Pi. This is not required, but it makes it a lot easier to
see what is happening.\
Now it's time to connect the power adapter to the Raspberry Pi, after
which it will start loading the Windows IoT image on the microSD card.
This can take some time, so be patient.

If you have connected the Raspberry Pi with a HDMI cable to a monitor,
the following information will appear on the screen:

![](./media/image21.png)


As you can see, the IP address of the Raspberry Pi is shown. Note it
because we will need it.

If you have not used the HDMI connection, you will need to open the
Windows IoT Dashboard application. To do so, go to My Devices and after
waiting a number of seconds, it will detect the Raspberry Pi, and show
you the IP address:

![](./media/image22.png)

### Connect to the Windows Device Portal through your browser

Enter the IP address into the address bar. Add :8080 onto the end.

![](./media/image23.png)


In the credentials dialog, use the default username and password:

Username: Administrator

Password: p@ssw0rd

The Windows Device Portal should launch and display the web management
home screen!

You can also launch the Windows Device Portal tool from the Windows IoT
Dashboard application by clicking on your device, and clicking on Open
in Device Portal.

![](./media/image24.png)

## Installing the Robot Kit app

The Robot Kit application is on Github, so we need to clone the repo.
Open Visual Studio and clone the following git address:

https://github.com/ms-iot/build2015-robot-kit.git

Open the solution, search for the following string:

![](./media/image25.emf)and change the string into your PC's IP address
(not the Raspberry Pi address!).

### Deploy Your App

-   With the application open in Visual Studio, set the architecture in
    the toolbar dropdown to ARM.

-   Next, in the Visual Studio toolbar, click on the Local
    Machine dropdown and select Remote Machine.

![https://ms-iot.github.io/content/images/AppDeployment/cs-remote-machine-debugging.png](./media/image26.png)


At this point, Visual Studio will present the Remote Connections dialog.
Use the IP address of your Raspberry PI.\
After entering the device IP, select Universal (Unencrypted Protocol)
Authentication Mode, then click Select.

![https://ms-iot.github.io/content/images/AppDeployment/cs-remote-connections.PNG](./media/image27.png)


You can verify or modify these values by navigating to the project
properties (select Properties in the Solution Explorer) and choosing
the Debug tab on the left:

![https://ms-iot.github.io/content/images/AppDeployment/cs-debug-project-properties.PNG](./media/image28.png)


When everything has been set up, you should be able to press F5 from
Visual Studio. If there are any missing packages that you did not
install during setup, Visual Studio may prompt you to acquire those now.

The Robot Kit app will deploy and start on the Windows IoT device:

![](./media/image29.jpeg)


If you attach a USB keyboard to the Raspberry Pi and you press one of
the keys shown in the app (W, A, D, X, Z, C, E, Q, S), the robot will
start to move!

If you surf on the Windows Device Portal and click on Apps, you will see
that the Robot App has been deployed and is running:

![](./media/image30.png)

### Run the application locally

From Visual Studio, stop the running application pressing the stop
button.\
In the configuration manager dropdown, choose x86 or x64, and Local
Machine:

![](./media/image31.PNG)


Press F5 and the application will also run from your PC! This is because
it is a Universal Windows Application, which can run on every device
running Windows 10 without changing a single line of code.

Leave the application running, go back to your browser and go to the
Windows Device Portal, click on Apps and from the dropdown menu, select
the Robot App (it should be something with a strange GUID name in it),
and press start:

![](./media/image32.png)

From the Robot App running on your PC, again press one of the buttons
(W, A, D, X, Z, C, E, Q, S) and you will see that the Robot will move
again. However, now it is controlled from your PC!

### Visual Studio

From Visual Studio you are now able to debug what's happening on the IoT
device:

![](./media/image33.png)

As you can see, the code is written in C#, which means everything is
managed, and there is no need to write in a lower level language such as
C or C++ to talk with the hardware.

![](./media/image34.PNG)

# Conclusion

This article has demonstrated how Windows 10 IoT Core can be used to
work with low cost devices, and how the UWP ecosystem makes it a lot
easier to write software that can run in different hardware environments
without changing a single line of code. We didn't dig into the code, but
it certainly is worth checking it out so you can learn how it works.\
There is a starter pack that you can buy, and which contains all the
items required to learn the basics of programming with Windows IoT
devices. The relevant link is included in the references section.

The Internet of Things is becoming more and more popular, with business
companies creating new devices every day, for example wearables (smart
bands, watches etc....) and domotica components (smart thermostats,
security systems etc....). With Windows 10 IoT Core and the Universal
Windows Platform there is now a unified ecosystem that allows you to
easily create software that can interconnect with different devices,
making it easier to focus on business functionality.

Marco Mansi\
<mmansi@xpirit.com>\
\@mansi_marco

# References:

The following links are the sources for my article, or the sites where I
refer to.

## Starter Pack for Windows 10 IoT Core on Raspberry Pi 2

<http://ms-iot.github.io/content/en-US/AdafruitMakerKit.htm>

## Get Started with Windows IoT

<https://ms-iot.github.io/content/en-US/win10/ConnectToDevice.htm>

## Connect your Windows 10 IoT Core device to your development PC

<https://ms-iot.github.io/content/en-US/win10/ConnectToDevice.htm>

## Windows 10 IoT Core Dashboard

<http://go.microsoft.com/fwlink/?LinkID=708576>

## Set up a Raspberry Pi 2

<https://ms-iot.github.io/content/en-US/win10/RPI.htm>

## Microsoft Hackster page 

<https://microsoft.hackster.io>

## Robot Kit Hackster page

<http://www.hackster.io/windowsiot/robot-kit>

## Robot Kit Source Code on Github

<https://github.com/ms-iot/build2015-robot-kit/>

## Servo Pin-out

<http://imgur.com/a/tDZY5>

## Cutting plans

<https://github.com/makenai/sumobot-jr/tree/master/cutting_plans>

## Ponoko locations

<http://www.ponoko.com/about/contact>

## Formulor

<http://www.formulor.de/>

## Windows IoT Core Visual Studio Project Templates

<https://visualstudiogallery.msdn.microsoft.com/55b357e1-a533-43ad-82a5-a88ac4b01dec>

## Windows Device Portal

<https://ms-iot.github.io/content/en-US/win10/tools/DevicePortal.htm>
