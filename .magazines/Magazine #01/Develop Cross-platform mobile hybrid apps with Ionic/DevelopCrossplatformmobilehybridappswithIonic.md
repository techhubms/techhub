# Develop Cross-platform mobile hybrid apps with Ionic

Before you start with the development of a mobile app, there are some
tough choices to be made. The most important one is how the app will be
developed. You have the option to develop it as a native, HTML5 or a
hybrid app. If you choose to develop a native app, the number of mobile
platforms that you would like to support usually states the number of
apps that will be developed. Currently you probably want to support at
least iOS and Android, perhaps Windows Phone. The consequences are you
will develop in three different environments, in three different
languages and the apps need to be developed in three different
development tools. It's clear a lot of challenges needs to be addressed.
The main challenges are you will need a broad technical knowledge and it
will be hard to keep the apps functionally equal.

One of the available solutions is Xamarin. Xamarin allows you to develop
a native app in one language, C#, for all platforms. Especially with
Xamarin Forms you have only one codebase. However, Xamarin Forms is
still maturing, since it's a relatively new product. Luckily, Xamarin
becomes more stable with each update. Some of Xamarin's major advantages
are quick app response, platform specific look and feel and the use of
XAML to define the views. **If you have experience with XAML, you will
quickly feel familiar with Xamarin Forms**.

An alternative to these native apps is a HTML 5 solution. Basically you
develop a web application which will be used and themed as an app. Just
like Xamarin there is a single code base, used for all platforms. When
the development team has experience in web development, this approach is
probably the most appropriate. The drawback to this solution is there
are restrictions with respect to access device hardware. For example,
you cannot use the camera or the GPS. Since HTML rendering is browser
specific it can be a challenge to make your app look and feel exactly
the same on each platform. Another disadvantage is there is always an
Internet connection required, thus data loss could occur.

For a long time it's possible to opt for a hybrid solution. This is an
HTML5 app, hosted in a native container. Examples of such containers are
PhoneGap, Cordova and Trigger.io. When you choose for a hybrid app, you
choose for both the benefits of an HTML5 and a native app. The native
container has access to device-specific functionality and removes the
internet connection availability requirement. This solution has some
disadvantages: the end user might experience your app as slow and the
look and feel is different from what the end user of a native app is
used to.

# Hybrid apps 2.0

![http://dynamicobjx.com/wp-content/uploads/2014/07/html5-css.png](./media/image1.png)
As in the entire software world, the world
of hybrid apps is moving fast. Last year many frameworks popped up to
provide a solution to the classic disadvantages of a hybrid app with
HTML5. There are Famo.us, Framework7, Monaca, AppGyver, OnsenUI and
Ionic. The latter looks promising, with Ionic, apps can be developed
rapidly and cross platform. **When you have basic experience with
webdevelopment, in particular AngularJS, Ionic is the next logical
step**.

# Ionic

![http://upload.wikimedia.org/wikipedia/commons/thumb/d/d1/Ionic_Logo.svg/2000px-Ionic_Logo.svg.png](./media/image2.png)
Ionic is an open source framework for
building HTML5 mobile apps to give the end user the experience of a
native app. This is done by making use of HTML, JavaScript and CSS. Like
a number of other frameworks AngularJS is used for the User Interface.
Ionic exists for almost a year, after a long list of betas and release
candidates, Ionic is about to release version 1. Ionic is already mature
enough for developers to put their applications into production. Below a
list with some need-to-knows about Ionic:

-   Ionic works on Ionic 2, which supports Angular 2

-   Optimized for speed

-   On both iOS and Android, apps look like a native app. An end user
    will not be able to quickly tell whether the app is native or hybrid

-   Windows Phone is not supported (yet). The support of this is on the
    roadmap

-   All native app containers are supported, but Cordova is favored

-   A major advantage is the existence of a large community. There is a
    high response rate and speed on the forum

-   Ionic is open source. The license even allows commercial apps to be
    developed without the app being open-source itself.

-   Drifty just invested 2.6 million dollars

-   The book 'Developing an Ionic Edge' has just been released

-   The book 'Ionic in action' will be released soon

-   Ionic provides more and more services and tools to simplify the
    app-developers life

-   Ionic is 100% free to use

-   There are numerous apps in the stores developed with Ionic. For
    example, the Android and iOS apps Sworkit and Chief Steps are a good
    example to see there is almost no difference from a native
    experience

-   Ionic focuses on the latest versions of iOS and Android. At this
    time, IOS 6 and Android 4.1 and later are supported

Ionic is working hard to become a mature framework to develop
cross-platform mobile application with.

# Starting with Ionic

![http://www.gaggl.com/wp-content/uploads/2014/04/Apache_Cordova1.png](./media/image3.png)
To Install all necessary software to be
able to start developing, seems to be a project in itself, but it's well
documented by Ionic
([[http://ionicframework.com/getting-started/]{.underline}](http://ionicframework.com/getting-started/)).
Ionic requires NodeJS and Apache Cordova. Besides this also the Android
SDK and Java SE Development Kit need to be installed. There are several
ways to install all necessary software. Of course it's possible to
follow Ionic's documentation. Make sure that the correct System Paths
are set in Windows. Another way is particularly useful if you want to
use Visual Studio as a development tool. You can limit the actions by
only installing or configuring Visual Studio Tools for Apache Cordova.
For Visual Studio 2013 with update 4 the installer can be found here:
[[https://www.microsoft.com/en-us/download/details.aspx?id=42675]{.underline}](https://www.microsoft.com/en-us/download/details.aspx?id=42675).
For Visual Studio 2015 it's a matter of checking the correct checkbox in
the setup.

![Single install](./media/image4.png)


After installation, a new project template is available in Visual
Studio. The new template is called \'Blank App (Apache Cordova) \"and
can be found under Templates -\> JavaScript -\> Apache Cordova Apps.

![http://www.daoudisamir.com/wp-content/uploads/2015/01/visual-studio-2013-logo.png](./media/image5.png)
![https://userecho.com/s/logos/2013/2013.png](./media/image6.png)
The most common way to develop apps with
Ionic is often done with Sublime Text, a free third-party tool. Visual
Studio is also a viable option.

![http://ionicframework.com/img/blog/ionicbox-header.png](./media/image7.png)
Ionic offers an alternative to start developing. For
this, the project Ionic Box is available. It's a virtual machine which
can be downloaded with all necessary tooling. More on this can be found
at: <https://github.com/driftyco/ionic-box>

# Create an app with Ionic

It is advisable to stay as close as possible to Ionic's intended manner
to start a project. There are Visual Studio templates and Nuget packages
available. However, it is important to know what these libraries
actually do for you.

Ionic offers multiple templates to start developing an app:

  ------------------------------------------------------------------------
  blank        An empty app
  ------------ -----------------------------------------------------------
  tabs         A starter app where tabs are used for navigation
  (default)    

  sidemenu     A starter app where a sidemenu is used for navigation

  Maps         A sidemenu starter app with Google Maps integration

  Salesforce   Uses Salesforce OAuth autentication and the Salesforce REST
               API to call Salesforce functionality.

  Analytics    A starter app which uses the Ionic IO Analytics Service

  Push         A starter app which uses the Ionic IO Push Service

  io           A starter app which uses all Ionic IO services
  ------------------------------------------------------------------------

  ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
  ![http://ionicframework.com/img/getting-started/blank-app.png](./media/image8.png)   ![http://ionicframework.com/img/getting-started/tabs-app.png](./media/image9.png)   ![http://ionicframework.com/img/getting-started/menu-app.png](./media/image10.png)
                                                                                                                                                                      
  --------------------------------------------------------------------------------------------------------------- --------------------------------------------------------------------------------------------------------------- ----------------------------------------------------------------------------------------------------------------
  blank                                                                                                           tabs                                                                                                            sidemenu

  ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

Start the NodeJS command prompt, navigate to the directory where the app
should be created and type the following command to use the sidemenu
template for a new app with the name 'myappname':

[ionic start myappname sidemenu]{.mark}

A directory structure is generated with all necessary files inside. The
www directory contains all files providing the app's user interface.

# Run the project

There are several ways to run a project. Because an Ionic project is a
web project, it can be run in the browser. In addition, it can run in an
iOS or Android emulator. It's also possible to run the app in the
device's browser or of course as a native app on a device. Since a
device's browser interprets the app differently from the native app,
it's advised not to use this option.

[Run in the browser]{.underline}

To do this, execute the following command in NodeJS:

[ionic serve]{.mark}

A local development server is started, the default browser launches and
displays the project. A very productive feature is called LiveReload.
This ensures that as soon as changes have been made to the source, the
browser automatically refreshes. All changes are immediately visible
without rebuild time, redeploy or local development server restart.
Especially when you have a development environment with two screens,
this is a relief.

[Run in the emulator]{.underline}

This sample only shows how to start the emulator for Android. When you
want to start the simulator for iOS, you will need a Mac.

Execute the following commands:

[ionic platform add android\
ionic build android\
ionic emulate android]{.mark}

The emulator is started and the app will be displayed.

[Run as an app on a device]{.underline}

Before release it's necessary to test the app on an actual device. If
the app should be installed on iOS, an Apple Developer license is
required which costs \$99,- per year. After this XCode should be
installed and configured. This is necessary because this way Cordova can
package and sign the app for iOS. XCode can only run on Mac hardware.
For Android, the following command can be executed in NodeJS after the
Android device is attached to your PC. (USB debugging must be enabled on
the device)

[ionic run android]{.mark}

The project structure created by Ionic is very well arranged. The
project includes package manager Bower. Ionic is already loaded with
Bower and can be used immediately to load other packages.

# Use Visual Studio to develop Ionic projects

Besides Sublime Text, Visual Visual Studio can also be used to develop
the Ionic project. This requires some manual steps. It is convenient to
create a 'Blank \'Blank App (Apache Cordova)\' with the name www. Both
the Ionic directory and the directory Visual Studio created for the
project correspond. Close the solution and copy the sln file and all
files and directories, except the images folder and index.html to
Ionic's www folder. Open the solution from its new path and add all
directories of Ionic to the project. Now both Sublime Text and Visual
Studio can be used for development.

# Ionic Components

Ionic offers a comprehensive set of components that can be used directly
in the views. These are all well documented on
<http://ionicframework.com/docs/components/>. Examples include List,
Grid, Toggle, Cards, Checkbox, Button and many more components.
Additionally Ionic provides a layout for standard HTML tags such as h1
and p. No layout-specific code needs to be written for iOS or Android.

Based on these components, **Ionic is similar to Bootstrap for web
applications. Ionic however, goes even further.** Ionic is not just a
CSS framework it is also a JavaScript UI library. For example,
JavaScript provides services to display a pop-up or a spinner. All
services can be found here: <http://ionicframework.com/docs/api/>. This
altogether provides a broad range of functionality to the developer, for
conveniently developing the User Interface. Besides User Interface
services, Ionic also offers other services.

# Ionic ecosystem

Although Ionic only exists little over a year, the project is already in
the top 50 most popular frameworks on GitHub. Ionic releases more and
more tooling to make developers' life easier. The goal of Ionic is to
**give hybrid app developers a head start over developers of native
apps**.

## Ionic IO

Ionic offers all services under the name Ionic IO. To use these services
you need to personally register and the app must also be registered.
After this, the following services are available:

[Ionic User]{.underline}

This service provides user tracking. In addition, this service is
required when using other services like Analytics and Push.
<http://docs.ionic.io/v1.0/docs/push-quick-start>

[Ionic Push]{.underline}

This service can send push notifications to the app.
<http://docs.ionic.io/v1.0/docs/push-quick-start>

[Ionic Deploy]{.underline}

Usually when updating your app, you have to go through the resubmission
process. With Ionic Deploy new versions of the app can be deployed
without going through the resubmission process. This type of update does
not support binaries. Only HTML, CSS and JavaScript can be updated this
way. <http://docs.ionic.io/v1.0/docs/deploy-install>

[Ionic Analytics]{.underline}

This Ionic IO service provides a way to add Analytics, metrics and error
tracking to your app. Of course you are free to choose a different
analytics provider. <http://docs.ionic.io/v1.0/docs/analytics-install>

[Ionic View]{.underline}

All people involved in the app development can install Ionic View on
their device. With Ionic View they get access to the app under
development and are able to review it.\
Ionic View is available for both iOS and Android. It's possible to
upload your app via the command prompt and make it available. Ionic view
is currently in beta. More on this at: <http://view.ionic.io/>

## Icons and splashscreen

Every platform has different requirements regarding the images being
used. This has to do with screen resolutions. Ionic takes away this
burden by generating correct images, per platform, from a single
source-image. Ionic does the same for Splashscreen images. More on this
can be read at:
<http://ionicframework.com/docs/cli/icon-splashscreen.html>

## Creator

With Ionic Creator it's possible to create mockups and prototypes of
mobile apps. After this a template can be generated which can be used to
create an app. This ensures a quick start with the development of an
app. The tool is not available yet. For more information:
<http://ionicframework.com/creator/>

## Ionic lab

![http://ionicframework.com/img/blog/ionic-lab-header.png](./media/image12.png)


Ionic Lab displays the apps UI for both iOS and Android at the same time
in a browser window. LiveReload is still working when using Ionic Lab.
Both windows are being displayed side-by-side. To use this feature
execute the following command:

[Ionic serve \--lab]{.mark}

## Icons

Over 700 icons, created by Ionic, can be downloaded and used for free.\
[[http://ionicons.com/]{.underline}](http://ionicons.com/)

![](./media/image14.png)

## ngCordova

![C:\\Users\\pasca_000\\Pictures\\ngCordova.png](./media/image15.png)


Ionic has created AngularJS wrappers to call native functionality on
phones through Cordova. This functionality is grouped in a library
called ngCordova. These wrappers work on both Cordova and PhoneGap. With
this library functionality is made available such as taking a picture, a
barcode scanner, upload a file, the flashlight, GPS and much more.
ngCordova works on all AngularJS frameworks that operate on Cordova.
<http://ngcordova.com/>

# Conclusion

The world of mobile web development is very dynamic at this time. It is
not yet clear which method of developing a cross-platform app will
become the de facto standard. AngularJS is very popular among web
developers and Ionic provides the ability to use AngularJS for
cross-platform mobile apps. This is offered with powerful services that
make it very easy for the developer to quickly develop an app with a
native user experience.\
Ionic is fairly new player and there are many competing frameworks in
this area. Windows Phone is not yet supported. Ionic only supports
recent versions of the mobile platforms. Especially because phones have
to be fast enough to do the rather heavy rendering. In spite of this, in
several cases such as long lists, apps might not feel like a native app.
Ionic should give you the look and feel of a native app. My experience
is that Ionic particularly delivers the looks but does not always give
the feeling of a native app.\
Ionic is working hard to resolve these performance issues. Maybe the
performance problem can be found in the use of AngularJS. As soon as
Angular 2 is released, Ionic 2 will be released short after.

Despite the drawbacks, **Ionic has been embraced by many developers and
has a large community**. Without doubt this has to do with the fact that
Ionic is open-source and free to use, even for commercial applications.
The tooling that Ionic offers is very result oriented and reduces the
choices a developer needs to make, although he surely can. The
LiveReload feature is really great and saves lots of time. All this
could make Ionic a player in the mobile apps market that's here to stay.
In addition, it's clear that hybrid apps are a formidable competitor to
native apps and other cross-platform solutions like Xamarin.
