In the world of e-commerce, customers are becoming increasingly mobile.
In The Netherlands, 50% of consumers are shopping on their mobile phone.
Among those under 35 years of age, mobile purchases are at 65%.
[Numbers](http://www.marketingfacts.nl/berichten/helft-nederlanders-doet-aankopen-op-mobiel-onderzoek)
for searching and browsing for a potential purchase is over 70% overall.

Converting these visitors into customers is a delicate task. Consumers
are still hesitant to make mobile purchases. The challenge lies in
optimizing the mobile user experience. Some shops have tried to improve
user experience using a native mobile app. These apps are installed
through the Apple or Google app store. However, installation rates for
mobile apps are decreasing. [[The app boom is
over]{.underline}](https://www.recode.net/2016/6/8/11883518/app-boom-over-snapchat-uber).
Most smartphone users download zero apps per month. Unless you're a
major player and consumers use your app on a weekly basis, chances are
that your expensively built app is never installed.

##### Alibaba.com [[increased mobile conversions by 76%]{.underline}](https://developers.google.com/web/showcase/2016/alibaba) with a Progressive Web App.

So how can you get those smartphone users to make purchases? Luckily the
web has evolved a lot in recent years. Features such as push
notifications, real-time updates, seamless screen transitions and
geolocation are all available on the web platform. Advances in front-end
development make it possible to achieve a highly interactive,
high-performance user experience that matches a native app. The web also
offers some advantages over native apps such as discoverability through
search and social media, and not having to install anything. Key to a
successful web app is user experience. Reduce friction in your sales
funnel and more customers will [[make it to the
end]{.underline}](http://blog.gaborcselle.com/2012/10/every-step-costs-you-20-of-users.html).

![](./media/image1.png)

###### From Search to Checkout can be done without loading delays.

# The mobile web experience

Two major breakthroughs in high-performance web experiences came from
Google in 2015 as it introduced Accelerated Mobile Pages (AMP) and
Progressive Web Apps (PWA). Two very different techniques trying to
solve the same problem: how to make the web fast and reliable.
Accelerated Mobile Pages is a way to build a lightweight version of your
webpage that Google (and others) will make instantly available right
from its search results, at least on mobile. AMP pages are very limited
in functionality, but blazingly fast. They are also more likely to be at
the top of the search results.

##### One e-commerce company recently deployed an immersive AMP experience and saw a [[20--30% conversion uplift]{.underline}](http://searchengineland.com/amp-ecommerce-case-study-event-ticket-center-277444/amp).

Progressive Web App is a broader concept aimed at describing a web app
that offers certain features that make it fast, reliable and engaging. A
PWA is a state-of-the-art web application that uses all of the power of
the web platform, and does so responsibly. It's generally a single-page
application, meaning page transitions are fast and smooth and we can
offer a high-end user experience. A Progressive Web App is accessible
from the web like any other website, but can also be "installed" by
adding an icon for it on the home screen. Opening the app from the home
screen will render it in full-screen mode, essentially mimicking a
native mobile app. We can also implement push notifications so that
customers can be actively engaged.

![](./media/image2.png)


###### Google shows the AMP tag for search results that support it.

Accelerated Mobile Pages and a Progressive Web App together make for a
very potent combination. Because an AMP page is instantly available from
Google and AMP allows us to pre-cache our PWA in the background, we can
achieve a sales funnel that goes from Google search to checkout without
ever showing a loading indicator or staring at a blank screen. Combine
it with a user-friendly PWA and you've got a sales funnel as smooth as
silk.

\<amp-img src=\"image.jpg\" width=\"90\" height=\"50\"
layout=\"responsive\"\>\</amp-img\>

###### The AMP project provides a predefined set of web components to build your pages in order to optimize performance across devices.

# Driven by modern web technologies

The technology required to achieve this seamless shopping flow is about
to become mainstream. Google introduced Accelerated Mobile Pages in 2015
but has only recently started encouraging it for e-commerce. Progressive
Web Apps are made possible by new browser features such as Service
Workers and the Push API.

It's a common misunderstanding that a Progressive Web App must be
powered by a modern client-side UI rendering library. In fact, you can
still render webpages on the server. All you really need in order to get
a baseline PWA is an app manifest file and a service worker, which is
just a tiny bit of JavaScript that can be bolted onto any webpage.
However, this will not get anywhere close to offering that app-like
feeling that we would like to achieve.

[{\
\"short_name\": \"Acme Shop\",\
\"name\": \"Acme Corporation Online Shop\",\
\"icons\": \[\
{\
\"src\": \"launcher-icon-2x.png\",\
\"sizes\": \"96x96\",\
\"type\": \"image/png\"\
},\
{\
\"src\": \"launcher-icon-3x.png\",\
\"sizes\": \"144x144\",\
\"type\": \"image/png\"\
},\
{\
\"src\": \"launcher-icon-4x.png\",\
\"sizes\": \"192x192\",\
\"type\": \"image/png\"\
}\
\],\
\"theme_color\": \"#6c1d5f\",\
\"background_color\": \"#f8f7fc\",\
\"start_url\": \"/index.html\",\
\"display\": \"standalone\",\
\"orientation\": \"landscape\"\
}]{.mark}

###### A Web App Manifest makes your app look like a native one.

In practice, most companies that have deployed a PWA are indeed using a
modern UI library such as React. These tools help us build
high-performance user experiences, including fluent transitions and
animations. As a bonus, we can usually run the same code to render HTML
on the server. Server-Side Rendering makes sure that search engines and
social media sites are able to crawl and access your content.
Accelerated Mobile Pages must always be rendered on the server. This
simplifies caching and avoids running heavy JavaScript code on a slow
device. In effect this means that to combine a PWA with AMP, we have to
render our pages both on the client-side and the server-side. To do this
efficiently, it makes a lot of sense to run the same code on both sides
using Node.js.

\<!doctype html\>\
\<html ⚡\>\
\<head\>\
\<meta charset=\"utf-8\"\>\
\<script async src=\"https://cdn.ampproject.org/v0.js\"\>\</script\>\
\<title\>Acme Corporation Online Shop\</title\>\
\<!\-- \... \--\>\
\</head\>\
\<body\>\
\<!\-- \... \--\>\
\</body\>\
\</html\>

###### The AMP HTML must be rendered server-side.

# Building a stellar e-commerce experience

Following the previous paragraphs it should come as no surprise that the
two primary ingredients for our e-commerce front-end are a PWA and AMP.
Here's what it takes to build these and hook them up.

## A high performance front-end framework with SSR capability

To get started, we need a tool to help us build the user interface and
deal with client-side logic such as keeping state and handling
transitions. In order to serve AMP documents without a lot of additional
work, it will need to support Server-Side Rendering. Because a large
part of our audience will be on mobile, performance is an important
aspect. Luckily there are tools such as
[[Lighthouse]{.underline}](https://developers.google.com/web/tools/lighthouse/)
to measure performance and [[plenty of benchmark
apps]{.underline}](https://hnpwa.com/). Although React is the most
popular choice in this space, Preact and Vue are very solid alternatives
worth considering. Setting up universal server-side rendering is still
very complex. Luckily there are several frameworks which provide
universal server-side rendering out of the box:
[[Next.js]{.underline}](http://zeit.co/next) for React projects and
[[Nuxt.js]{.underline}](https://nuxtjs.org/) for Vue. Both offer
significant benefits over a plain React or Vue setup.

#### **Back-end for Front-end ([[BFF pattern]{.underline}](http://samnewman.io/patterns/architectural/bff/))**

![](./media/image3.png)


###### You may want to split up your back-end for front-end to create verticals and avoid a monolithic front-end.

AMP documents must be rendered on the server. We could of course do this
in any back-end technology, but to be able to reuse interface components
between AMP and our PWA, we should be using the same technology. As such
we'll need [[Node.js]{.underline}](https://nodejs.org/) to render our
Preact components on the server. Running JavaScript on the server has
the added benefit that your front-end developers can take ownership and
responsibility for their entire product. In addition, many innovations
in front-end optimizations are adopted by the Node.js community first,
and generally very easy to apply. On top of Node.js, we'll need a server
framework such as [[Express]{.underline}](https://expressjs.com/). We
can still use our existing back-end services as backing for the Node.js
service, so we won't be moving any important business logic.

#### **A Service Worker to install from AMP**

A Service Worker is what allows our app to work offline and enable push
notifications. It's a script that can be installed in the browser and
run in the background to cache resources and act on events. To make the
transition from Google to our app appear instantly, we can use
[[amp-install-serviceworker]{.underline}](https://www.ampproject.org/docs/reference/components/amp-install-serviceworker)
to install our Service Worker in the background while the customer is
browsing our AMP page. This way we can pre-cache necessary resources for
our app in the background so it will load instantly when the customer
clicks through to the product detail page. Because the PDP is part of
our single-page app, any subsequent page transitions can be instant too.

#### **Attention to detail**

Perhaps the most import aspect in building a high-quality web experience
is attention to detail. There are many aspects to web development which
are easily overlooked or discarded because they are "too much work".
These include performance, user experience design and overall look &
feel. Unfortunately, they are the first casualties of deadlines, budget
restrictions and developer laziness. It's very sad to see a great
concept being poorly executed, but in reality that's what happens. The
key is to not settle for a sub-par user experience. It's better to build
half a product with only a few well built features than to build a
half-assed one with many poorly executed features. Setting a
[[performance
budget]{.underline}](https://www.keycdn.com/blog/web-performance-budget/)
is a good way to keep an eye on performance. There's a [[whole list of
things]{.underline}](https://www.smashingmagazine.com/2016/12/front-end-performance-checklist-2017-pdf-pages/)
that we can do to improve it. Of course, performance alone isn't going
to cut it. Consumers expect a polished product, especially on mobile.
That means hiring UX designers and [[performing usability
tests]{.underline}](http://www.sensible.com/rsme.html). Because we're
targeting many devices, taking a [[mobile-first
approach]{.underline}](https://www.lukew.com/resources/mobile_first.asp)
to interface design and development is highly recommended.

### **Final thoughts**

The web is evolving at a rapid pace. Google continues to push the web as
a platform. Targeting the web as the primary platform makes a lot of
sense for e-commerce, as smartphone users are unlikely to install a
native app. New browser capabilities allow us to provide a high-end
mobile user experience, while reaping the benefits of the web platform.
Luckily, tools have evolved to make adoption of current best practices
much easier than it used to be. Nowadays we can offer our customers an
e-commerce experience that will not alienate or drive them away, but one
that they will love.
