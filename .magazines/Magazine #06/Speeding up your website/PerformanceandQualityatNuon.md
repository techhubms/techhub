# Speeding up your website

# Introduction

Development teams often are pushed by the business to deliver new
functionality as soon as possible in order to stay ahead of the
competition. However, although development teams are keen to deliver,
they want to maintain certain quality standards.

Unfortunately, there are times when the quality consciousness of the
development team is overruled by the business, and decisions are made at
the expense of quality, i.e.: more functionality, less quality and more
technical debt. As a consequence, bad performance, bugs and service
outage may occur. This can lead to a point where end-users are affected,
and at that point business cannot ignore these issues.

I have selected three issues that impact performance:

1.  Serial request execution

2.  Chatty client

3.  Large image size

These issues were encountered on a live customer portal and this article
describes how to tackle them. The customer portal allows registered
users to log in and provides a dashboard to view relevant statistics.
The data to generate these statistics is retrieved from back-end
systems.

# Serial request execution

The best fixes are those that have a low impact on the landscape while
yielding great results.

To a certain extent, the technology stack I encountered consists of an
Angular front-end, an Azure middleware .NET API, and an on-premise
back-end database system. This means a great deal of logging is done in
Azure Application Insights, and this provides a good starting point for
analyzing application performance.

![](./media/image1.png)


In the end-to-end trace above, the query string part (not depicted here)
of the five Details calls is almost identical, which raises the
question: "Can't we combine this into one call?" Although this is
possible, after analyzing the code and talking to domain experts, a
solution with less impact on the current application landscape was
chosen. This solution answers another question that may arise by
observing the trace: do similar calls need to wait for each other?

When examining the code, it turns out that a part of it doesn't need to.
The first await statement has no dependencies between offers in the
foreach loop.

The ForEachAsync is done serially. This is shown next, where an action
only proceeds to the next action after the previous action is completed.

In the suggested improvement the actions for the first await statement
are executed in parallel. To yield the same output we need to keep the
second await statement serial, because of the if statement. Otherwise
the if statement will always be true. An improvement would be to remove
this dependency on ContentData.

The [ForEachParallelAsync]{.mark} method is added to run tasks in
parallel. This is done with the [Task.Run(() =\> action(item))]{.mark}
command. This queues the tasks on a thread pool. The
[Task.WhenAll]{.mark} method creates a new task that completes when all
the provided tasks have been completed.

The resulting performance gain is dependent on the amount of offers.
Usually there are multiple offers. The response time has been measured
with SoapUI and on average, this is cut in half:

![](./media/image2.png)


# Chatty client

When navigating on the customer portal and inspecting the network trace,
a lot of traffic can be identified on each navigation click.

![](./media/image3.png)


This is fine when the navigation path has not been clicked before, but
not when this is repeated in the same session. The response time is
fairly short (\<350 ms as shown above) as the data is fetched from an
Azure Table Storage cache. On the other hand, the data that is
transferred increases and makes the client chatty. This means that it
could lead to unnecessary waiting time on slow connections and an
increase in data transfer costs on devices with a metered connection.
Furthermore, to realize caching in Azure Table Storage, a fair amount of
custom code has been written that could make it difficult to maintain
over time.

Multiple approaches can be identified to improve the caching mechanism.
The most prominent approaches are browser caching with AngularJS
\$cacheFactory, and HTTP caching with ETags. I will zoom in on the
former, since this method is currently being implemented for the
customer portal. It is a lightweight solution and fulfills the
requirements for caching on the customer portal.

With the AngularJS \$cacheFactory approach, caching is done in AngularJS
on the JavaScript heap memory, and lasts for the duration of the session
(until page refresh). It is possible to specify caching per request.
However, by default it is not possible to specify a cache expiration
window. There are libraries available to realize this, such as the
Angular-cache[^1]. The implementation is done in the front-end code and
is fully supported by AngularJS. The client remains in control and is
able to flush the cache by refreshing the page.

An example of the newly added code (highlighted in red) is shown below:

Only GET (and JSONP) methods can be cached and, in the example shown
above, this is controlled per request. It can also be applied
application-wide by using a configuration file.

The result of this would be:

![](./media/image4.png)


First-time calls were left out in the screenshot. No subsequent calls to
services are made, which makes the client less chatty, i.e. fewer
requests and fewer transferred bytes between the client and the server.

# Large image size

A great tool to generate performance audits for a website is Google
Lighthouse. This tool comes as part of Google Chrome and can be run from
the Audits tab in the developer toolbar.

One of the main bottlenecks on our website is the image size:

![](./media/image5.png)


The image in the screenshot is retrieved from the server as a 2400 by
1122 pixels image, after which it is scaled down in AngularJS to 248 by
244 pixels. As advised by the Lighthouse report, if we would serve a
smaller image, the potential savings could be 83%.

Images are served through EPiServer, which is a content management
system. We could simply ask a content manager to replace the original
images with smaller versions of them, but if they are used elsewhere,
this could impact other consuming applications as well.

A better approach would be to let the AngularJS application request a
smaller image from the original one. EPiServer includes an image resizer
which needs a query string as input.\
The original GET request <http://EPiServer/myimage.jpg> can be replaced
by
<http://EPiServer/myimage.jpg?height=300&mode=crop&quality=75&anchor=bottomright>.

The result is as follows:

![](./media/image6.png)


The savings are approximately 81% and we still have an image with
acceptable quality.

This can be deployed to production and will probably work. However, from
a quality perspective we need to test this locally to proceed to the
deployment environments. Assuming we don't have EPiServer installed,
this could be an issue. The image resizing functionality in EPiServer
needs to be mimicked somehow in order to show that this change works.

The first part of the solution is to enhance BrowserSync[^2].
BrowserSync keeps the AngularJS application in sync with the source code
while serving it. It contains a middleware option in which server-side
functionality can be defined.

The second part is a library that does the image resizing for us. I used
the sharp from lovell[^3], which can be found on GitHub. The following
code, which is part of the BrowserSync init function, is added:

middleware: \[

//This function is mimics episerver\'s image resize function

function (req, res, next) {

let anchorMapping = {

\'bottomright\': sharp.gravity.southeast

};

let requestUrl = url.parse(req.url, true);

let queryObject = requestUrl.query;

if (queryObject.width \|\| queryObject.height) {

//Removes leading slash, otherwise: image not found

let resizedImage = sharp(requestUrl.pathname.replace(/\^\\/+/g, \'\'))

.resize(parseInt(queryObject.width) \|\| null,

parseInt(queryObject.height) \|\| null)

.crop(queryObject.anchor ? anchorMapping\[queryObject.anchor\] :

sharp.gravity.topleft)

.jpeg({

quality: queryObject.quality ?

parseInt(queryObject.quality) \|\| 80 : 80

});

res.writeHead(200, {

\'Content-Type\': \'image/jpeg\'

});

resizedImage.pipe(res);

}

else {

next();

}

}

\]

The anchorMapping is a key value object that maps the value of the query
string parameter "anchor" to Sharp's definition of anchoring cropped
images. For this example, only one key is defined. The request URL is
parsed to extract the query string parameters and values. The Sharp
resizing function is then called with these values.\
The regular expression removes a forward slash from the path name. In
this case, the images are located on /images/. When leaving the leading
slash in place, the gulp serve command (which calls this code) will
trigger an error because it cannot find the image. The content type in
the response header is set to an image and the resized image stream is
piped to the result stream so the body of the response will contain the
resized image. Finally, the next() function is required to let all other
requests pass through.\
\
The result is as follows:

![](./media/image7.png)
\
![](./media/image8.png)


The height can be changed while the application is running and when
reloading the page this yields:\
![](./media/image9.png)


![](./media/image10.png)


# 

# Conclusion

Poor product quality can lead to performance degradation up to a point
where the customer is affected. Different tooling has been used to test
website performance and pinpoint bottlenecks. Several best practice
implementations were shown that have a low impact on the existing
landscape and that can be implemented rather easily. The resulting
increase in performance is significant.\
Although these solutions improve customer experience for the near
future, there is more to an application that performs well. How can we
make sure that teams become more self-controlled, stop building up
technical debt, and really help the customer? This is the root cause and
it involves company culture and the willingness to practice true agile
principles. I´ll gladly hear your thoughts on this.

[^1]: <https://github.com/jmdobry/angular-cache>

[^2]: [https://browsersync.io](https://browsersync.io/)

[^3]: https://github.com/lovell/sharp
