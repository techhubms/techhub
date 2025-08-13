I've built numerous apps in my life and the one thing they had in common
was that they all had to retrieve their data from somewhere --
typically, a REST-based API. Cloud technology made it easier to
implement these APIs, but our mobile software architecture hasn't
changed that much. Is cloud-native technology going to change this? In
this article, I'll cover several options to implement a cloud-native
mobile backend using Azure and I'll explain what impact this will have
on your Xamarin mobile apps.

# Cloud-native Mobile Architecture

The cloud is already a common platform to host your mobile app backend.
However, the architecture of these backends is often just the same as if
they were hosted anywhere else. This doesn't have to be a bad thing
since this gives you the flexibility of being able to host them
anywhere. In this article, we'll look at using cloud-native features of
Azure that can help you reduce your development effort and therefore
enable you to adapt to your customer demands faster, and build
innovative solutions.

If you've ever built a mobile backend, you've probably built something
like the following example: a set of APIs or web services that expose
all your business logic to the client through REST or procedural calls.
If you are a .NET developer, you've probably implemented this using
ASP.NET Web API. These APIs are your central access point to all data
and logic that is stored somewhere in the backend. As an app developer
you don't care what happens behind the API, because you just communicate
with this API and its contract.

![](./media/image1.tiff)


When building a cloud-native mobile backend this doesn't have to be
different, but there are several options that will give you these kinds
of APIs for free, so you can spend time on things that really matter
instead of building all this plumbing. As an app developer, you do have
to know a bit more about what happens in the backend because you'll be
talking to certain components directly through different channels. Let's
look at a simple example: we want to store our products in a DocumentDB
on Azure. We can directly query the DocumentDB from our client, but
inserts will probably still go through some form of custom-built APIs
because you want to do validations. Or there is some other business
logic involved in creating a document that you don't want to have
executed by your mobile app. In this simple example, we can use
cloud-native components that make it super easy to build this, but the
client has to know that there are two different APIs to be used which
are in different locations instead of doing a GET or PUT on the same
resource like they did in the past.

![](./media/image2.tiff)


In the rest of this article I'll zoom in on three different cloud-native
solutions that could make your life as a mobile backend developer a lot
easier: DocumentDB, Azure Mobile Apps and Azure Functions.

# DocumentDB

Storing data in your backend is key to most applications. DocumentDB is
an Azure cloud-native NoSQL database with automatic scaling features and
possibilities to replicate your data globally without making any changes
to your app. It's possible to connect your app directly to DocumentDB in
the cloud without creating any service layer on top of it.

## Benefits of a DocumentDB as mobile backend

DocumentDB is a schemaless JSON database, which means that you don't
have to design your data model or set up indexes up front. DocumentDB
will be able to execute rich queries on top or your JSON objects that
may have different properties without throwing any exceptions. This
allows you to iterate and release frequently without having to do a full
database schema upgrade, which can be a real pain if you want to do it
often.

With DocumentDB being a cloud-native solution it's set up with cloud
scale in mind. The scale supports up to millions of requests per second
and has native features to replicate the data over multiple regions
within Azure.

Some features that can be especially useful for mobile apps are the
geo-spatial queries to query things in your neighborhood, or the use of
binary attachments.

## When not to use DocumentDB?

While DocumentDB is flexible and offers great performance, it is not a
relational database. If your data model works best as a relational
model, don't try to fit it into a NoSQL database like DocumentDB. Its
design just isn't made for it and you will probably kill the performance
by trying to fit your relational model into the DocumentDB.

## Adding a DocumentDB to your app

Adding a DocumentDB to your app is simple. There is a native SDK you can
add to your portable class library through a nuget package
"Microsoft.Azure.DocumentDB.Core". After adding this nuget package we
can add a new DocumentClient that passes the URL to our DocumentDB, as
well as a resource token to define permissions.

Now that we've set up a connection, it is possible to add items or to
query your collections in the DocumentDB. To do a simple query, you can
create a DocumentQuery querying on a specific Type, and create a query
using Linq in the same way as you would expect any other Linq Query.

var query = Client.CreateDocumentQuery\<PointOfInterest\> 

(collectionLink, 

new FeedOptions{ MaxItemCount = -1, 

PartitionKey = new PartitionKey (this.UserId) })

.Where (poi =\> poi.Location.Distance(currentLocation) \< 500)\
            .AsDocumentQuery ();\
\
var poiList = new List\<PointOfInterest\> ();\
Items = await query.ExecuteNextAsync\<PointOfInterest\> ();

Querying on geospatial properties is part of the SDK as you can see in
the sample above. On properties that are Points you can query their
distance without having to use any special code in your Linq queries.

Inserting items is just as simple. You just need the DocumentClient and
a link to your collection in the DocumentDB. With those properties you
can insert any C# object, as long as the object is serializable as JSON.
By adding the UserId to the object, the permissions are automatically
set up correctly so that only our user or other users who have been
given permission can find this item.

pointOfInterest.UserId = this.UserId;\
var result = await Client.CreateDocumentAsync (collectionLink, pointOfInterest);\
pointOfInterest.Id = result.Resource.Id;

DocumentDB does not have any authentication mechanism built in and will
just require the user-specific resource token you pass to the client
constructor. Retrieving this token can be done by adding a separate
Azure App Service app that will do the authentication for us.

To set this up you have to follow the following steps.

1.  Create a DocumentDB collection where you define a partition key on
    the UserID property.

2.  Log into your app using an OAuth mechanism.

3.  Use the OAuth token to authenticate to the resource token provider
    API that is part of Azure App service.

4.  Resource token API will request a DocumentDB resource token with
    read/write permissions on the collection.

5.  Resource token API returns the resource token to the app.

6.  App passes the resource token to its queries.

![](./media/image3.tiff)


# Azure Mobile Apps

The most well-known solution to build your cloud-native mobile backend
in Azure is to use Azure Mobile Apps. Azure Mobile Apps are part of
Azure App Service, Microsoft's solution for building a PaaS (Platform as
a Service) backend solution. Azure Mobile Apps offer specific mobile
features on top of Azure App Service.

## Benefits of an Azure Mobile Apps backend

Azure Mobile Apps offer specific mobile features such as Client SDKs for
iOS, Android, Windows, Xamarin and even Apache Cordova.

There is out-of-the-box support for OAuth 2.0 with most social networks
to make authenticating easy and there is integration with Azure
Notification Hubs to send push notifications to all platforms using a
single backend.

For proof of concepts or relatively simple apps, Azure Mobile Apps
includes Easy Tables. This is a feature that uses an SQL Azure database
behind the scenes. What makes Easy Tables so easy is that the columns
and tables will automatically be generated when data is added to the
database.

The most powerful feature of Azure Mobile Apps is the possibility to
create offline data sync between your backend and your mobile app. To
enable the offline data sync you'll have to implement a local storage
using SQLite or SQLCipher, and set up the out-of-the-box sync with the
data provider that is part of the Azure Mobile App. This feature allows
you to create the offline data sync with only a couple of lines of code,
and without having to think of conflict resolution of sync errors due to
network issues.

Offline synchronization of your data offers a lot of new business
scenarios in your apps because initially your UI will save all changes
locally in your SQLite database and sync this data to the backend
asynchronously in the background. This can make performance of the app
look great because you don't have to wait because of the communication
delay to your backend, which can be slow depending on the location and
reception your phone has when using the app. Imagine building such a
synchronization mechanism yourself. It would be quite hard to build,
especially if it includes conflict resolution and sync errors fixing.

## When not to use Azure Mobile Apps

Azure Mobile Apps can be used for all kinds of purposes, from small apps
to enterprise apps. Adding custom ASP.NET Web APIs within Azure Mobile
Apps can handle all kinds of scenarios. Easy Tables works great for
small apps or POCs, but is limited when building larger apps with
complex data models. It is wise to use custom APIs within the Azure
Mobile App when building more complex mobile backends so you have full
control over your business logic on the server side.

## Using Azure Mobile Apps

Offline synchronization for a table within Azure Mobile Apps to your
native mobile app can be created in under 50 lines of code. To add an
Azure Mobile App to your app, first create a new Azure mobile app in the
Azure Portal. After that you can start coding your app by adding a nuget
package called "Microsoft.Azure.Mobile.Client.SQLiteStore". This will
give you access to the SDK to sync data from a local SQLite database to
an SQL Azure database.

After adding the nuget package, the SDK has to be initialized by adding
the Init method in the FinishedLaunching method of the AppDelegate class
in iOS. On Android you don't have to initialize the SQLitePCL; you only
need to add the first line within the OnCreate Method of your
MainActivity.

Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();\
SQLitePCL.CurrentPlatform.Init();

After initializing the SDK you have to initialize the sync of a table,
in this case a table containing Friend objects which is just a plain old
C# object. Create a new class called AzureMobileService, and this is
where you add a method Initialize. In this method, you'll first have to
create a new MobileServiceClient and pass the url of the Azure Mobile
app you created. On the client, you can define a new table of type
Friend.

public async Task Initialize()\
{\
    if (Client?.SyncContext?.IsInitialized ?? false)\
        return;\
\
    var appUrl = \"https://friendsappdemo.azurewebsites.net\";\
    var path = \"friends.db\";\
    Client = new MobileServiceClient(appUrl);    \
    path = Path.Combine(MobileServiceClient.DefaultDatabasePath, path);\
\
    var store = new MobileServiceSQLiteStore(path);\
    store.DefineTable\<Friend\>();\
\
    await Client.SyncContext.InitializeAsync(store);\
    friendTable= Client.GetSyncTable\<Friend\>();       \
}

After initializing, you need to implement three methods, a sync method
that will synchronize the changes between your local database and the
SQL Azure database and two other methods to add Friends and to retrieve
Friends.

public async Task SyncFriends()\
{\
    await friendsTable.PullAsync(\"allFriends\", friendsTable.CreateQuery());\
    await Client.SyncContext.PushAsync();\
}\
\
public async Task\<IEnumerable\<Friend\>\> GetFriends()\
{\
    await Initialize();\
    await SyncFriends();\
\
    return await friendsTable.OrderBy(c =\> c.DateUtc).ToEnumerableAsync(); ;\
}\
\
public async Task\<Friend\> AddFriend(Friend friend)\
{\
    await Initialize();\
    await friendsTable.InsertAsync(friend);\
\
    await SyncFriends();\
    return friend;\
}

This is all the code you need to set up offline sync. On my GitHub page
you can find a fully working sample that runs on all mobile platforms.

# Azure Functions

We've looked at two different ways of implementing a cloud-native,
backend solution in Azure. With the upcoming trend of serverless
architectures, Azure Functions could also play a role in acting as a
mobile backend. Azure Functions deliver nano services that really take
advantage of the cloud with only paying for what you use and can be
scaled up and down quickly.

The functions are small pieces of code that can be triggered by HTTP
calls, a timer schedule, or by new items in blobs or queues. There are
several business scenarios you could image where this would work for
mobile apps. Azure Functions also have a set of output bindings to store
output on several different data storage types in Azure. Some of these
are the DocumentDB and Azure Table storage as part of Azure Mobile Apps.
Another output type consists of Mobile push notifications using Azure
Notification Hub.

This magazine contains an article by my colleague Pascal Naber
explaining all the details of creating an Azure Function, so we won't go
into the specifics of implementing a Function.

Implementing an Azure Function should focus on a specific task that
isn't covered by the functionality of DocumentDB APIs or Azure Mobile
App Table Storage in your Mobile backend. This way you can compose your
perfect mobile backend out of cloud-native components in the same way as
I described this in the first chapter.

# Conclusion

There are multiple ways of building your mobile backend in Azure.
Hopefully this article has made you think about the options you can use
instead of just lifting and shifting a mobile backend from your
on-premises solution.

In the future, we will see more and more mobile backends use the best
tools for the job that could, for example, be a single backend composed
of several types of storage, out-of-the-box APIs, or custom built APIs.

This is why it is important to have knowledge of the various techniques
you can use. The best part of these cloud-native solutions is that they
are easy to create and scale, but also to tear down. This makes them
perfect for proof of concepts or small apps where you have the
possibility to scale up or down, depending on the success of your
application.

If you want to get more technical details on implementing these Azure
features, check out my GitHub Page on
github.com/geertvdc/cloudnativeappdemo to find a full working sample
with DocumentDB and an Azure Mobile App.
