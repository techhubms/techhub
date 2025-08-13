**API Management**

It is for vendors of products very difficult to make mobile applications
for every possible mobile platform. Of course by preparing and making a
good responsive mobile website available will enhance your reach. Not
getting the most out of the mobile devices is the downside of a mobile
website.

Because of this vendors make their back-office via API's available to
the world. As a vendor you want earn some money on the usage of your
API. This means you need a Developer portal where you can divide
developers in groups, generate help pages, provide demo/example code,
monitoring of the usage, place to get issues, FAQ, but also block or
blacklist certain developers/users/applications. In short there is more
needed then simple provide some webservices.

A while ago I had such a dream too. I wanted to create an evaluation app
for the SDN. Attendees of a SDN event should be able to fill in an
evaluation form via their mobile devices. The evaluation was stored on
Azure in a datastore. Of course I did not want to store my
tokens/connection strings/passwords etc with my mobile app. And I am not
able to create an app for every platform, so I definitely did not want
to share my secrets with some unknown/wild developers.

![Screenshot
(1)\_thumb](./media/image1.png)


So I thought of preparing some WebApi services and expose them. But like
I described above, I needed a portal to explain my services and their
responses. That is a lot of work for a small API like this. This is my
portal now;
[[http://sdnevalapp.azurewebsites.net/]{.underline}](http://sdnevalapp.azurewebsites.net/).

![1-7-2015 4-34-32
PM_thumb\[2\]](./media/image2.png)


Ok, this solution was an older one. Made in the period where the project
template for WepApi wasn't integrated in Visual Studio. If you us the
proper WebApi template you will get some sort of documentation by
default. Which depends most of your documentation in code.

![](./media/image3.png)

To make this help page more interactive I could use Swashbuckle (NuGet
package). Your users get more details and even can try the methods.

![](./media/image4.png)

If you are using the newly introduced API Apps you get the swagger stuff
out-of-the-box. It is also nicely integrated in the preview Azure
portal. At this moment the try-it-out part is not implemented, perhaps
it will come soon on the fast changing preview portal.

![](./media/image5.png)

But still all these solutions are far from optimal. The documentation is
part of the source, which means redeploying an API after a documentation
typo change. There is no monitoring and auditing on the API for
individual users/developers/apps. If I give the API uri's to a developer
which uses the API's in a very chatty way and makes it almost impossible
for other developers to use my API. It is impossible to revoke the
rights of a specific user on using the API.

Luckily there is a solution on the Azure platform. The [[API management
service]{.underline}](http://azure.microsoft.com/en-us/services/api-management/)
([[documentation
site]{.underline}](http://azure.microsoft.com/en-us/documentation/services/api-management/)).

![1-7-2015 4-40-33
PM_thumb\[1\]](./media/image6.png)


![1-5-2015 9-53-21
AM_thumb\[4\]](./media/image7.png)


After creating the service, there is a different portal
([[https://marcelmeijer.portal.azure-api.net/admin]{.underline}](https://marcelmeijer.portal.azure-api.net/admin))
to do the settings, looking at the metrics, the applications, the usage
etc.

![1-5-2015 9-53-33
AM_thumb\[2\]](./media/image8.png)


At the settings is the place to make the webservices available. The
different operations, HTTP actions, the response codes, which URL,
description and informational texts. If you have a Swagger doc or an
ApiApp url, based on these an API can be imported without the manual
labor.

![1-5-2015 9-56-59
AM_thumb\[4\]](./media/image9.png)


![1-5-2015 9-57-15
AM_thumb\[2\]](./media/image10.png)


The URL to the source services can also be hosted on-premises. Of course
it is smart and wise to secure the endpoints on this URL with
Certificates, Username/Password combination or with OAuth.

This was the management portal for the admin of the API
([[https://marcelmeijer.portal.azure-api.net/admin]{.underline}](https://marcelmeijer.portal.azure-api.net/admin)),
for the developers there is a separate portal
([[https://marcelmeijer.portal.azure-api.net/]{.underline}](https://marcelmeijer.portal.azure-api.net/)).
Which can be styled and changed within limits.

This Developer portal is rather complete. All the mentioned
functionality can be found on it.

![1-5-2015 9-53-44
AM_thumb\[1\]](./media/image11.png)


There is a handy overview of the available API's.

![1-5-2015 9-53-59
AM_thumb\[2\]](./media/image12.png)


From the available API you can see the endpoints. You see the
descriptions and the URI for calling the endpoint. To use the endpoint
in an application the addition of a subscription key. The whole idea of
this portal is to regulate usage and with these subscription key is
bound per API to an application/developer. Because the base endpoints
are secured by Certificaten, Username/Passwords or with OAuth, bypassing
the API management is useless.

On this Developer portal there is even a possibility to try out the API
method for the different HTTP actions. The trace and the result is
shown.

![1-5-2015 9-54-38
AM_thumb\[1\]](./media/image13.png)


![1-5-2015 9-54-56 AM_thumb\[2\]](./media/image14.png)


![1-5-2015 9-55-30
AM_thumb\[1\]](./media/image15.png)


At the bottom of the page you can find example code for a lot of
programming languages. Everything to help your 'customers' of your API.

![1-5-2015 9-55-45
AM_thumb\[1\]](./media/image16.png)


As I told in the beginning of this article, making an API available is
one thing, but document/manage it is another thing. By using the Azure
service you can focus on the fun and most important part of the API, the
functionality.

An API isn't a hype anymore, but more used for business creation. Here
the adage is: "build an API around your Business Model and not a
Business Model around your API".

Using the specialized products for API management for third parties
gives you more focus on your API and less on the management part. Why
develop it yourself, when you can use the expertise of others. "You can
reach further while standing on the shoulders of giants"
