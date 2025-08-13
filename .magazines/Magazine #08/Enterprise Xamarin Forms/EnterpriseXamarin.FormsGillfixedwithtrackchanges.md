# Enterprise-ready Xamarin.Forms by Gill Cleeren

Building mobile applications has become much easier for .NET developers
since the dawn of Xamarin.Forms. Although the framework is capable of
building graphically rich mobile experiences, it is often the go-to
platform for line-of-business or enterprise applications. While we can
all start coding our way using File New Project, it might not be the
best approach for these types of apps. Running in an enterprise
environment means that they'll often be subject to changing requirements
(yes, it does seem that sometimes customers have changing demands...).
Dealing with this changing environment means that we'll need to harness
the apps with a decent set of unit tests so we can be confident about
the changes we'll need to make. And this in turn requires that we set up
an architecture for these mobile apps that lends itself to being tested
easily. If we come back to the result of File New Project, well, it's
safe to say that this is not the ideal starting point. In this article,
we'll talk about some of the architectural considerations we need to
make when building mobile apps that are ready for the enterprise.

## Layers in mobile apps

Since we were kids (OK, maybe now I'm exaggerating...), we've been
taught that we should layer our software. That paradigm hasn't really
changed when building mobile apps with Xamarin.Forms. The big plus of
Xamarin.Forms is the huge amount of shared code we typically will get
(easily up to 80% for real apps). That code is what we should focus on
since this is where the action will take place. The following diagram
shows a proposed approach which is definitely nothing really special if
you've been using a layered architecture in other types of projects.

![](./media/image1.png)


At the bottom of the stack we have a repository layer that will
typically handle all interactions with regard to data and webservice
access, so that the rest of the code doesn't get littered with these
low-level details. The Service layer will typically be used for the
business functionality and will interact with several repositories to
combine their responses. A very important third layer in this approach
is the view model layer. Introducing a view model and thus also the MVVM
pattern will be key in creating testable apps. From an MVVM point of
view, the services (and the repositories that they use) act as the
model. Finally, the top-most layer will be plain views, consisting of
data-bound XAML that will use the view models as their binding source.
The magic of data binding and change notifications will ensure that the
views will be loosely coupled to the view models. Again, while this
structure is far from unique, it will introduce loose coupling in this
application, thus increasing the ability to test and maintain it, which
is what we set out to achieve in the first place. Now that we have an
overall view of the structure of a typical Xamarin.Forms application,
let's zoom in on some of these layers in more detail and see some
typical approaches used in the respective layers.

## Accessing data

It's pretty hard to imagine any enterprise application that won't be
working with data. Most of the data used in mobile apps will reside on
the server and services will make sure that they are accessible from the
app. Most apps will probably use REST services for this purpose, but
other options such as WCF will work from Xamarin, albeit not always in
full force. Talking with these services will typically be done using
HttpClient while again other options exist. Today's REST services will
most commonly exchange JSON, and in Xamarin.Forms apps this JSON can be
parsed using JSON.NET. The following code snippet shows some code that
will be used to access a service.

var httpClient = new HttpClient();

var response = await httpClient.GetAsync

(new Uri(\"https://api.github.com/events\"));

if (!response.IsSuccessStatusCode)

throw new HttpRequestException(response.ReasonPhrase);

string jsonResponse = await response.Content.ReadAsStringAsync();

var json = JsonConvert.DeserializeObject\<T\>(jsonResponse);

return json;

Mobile apps for the enterprise, and in fact all mobile apps, will be
used in unpredictable circumstances. People use the app while on the
road, inside a concrete building, in and out of a wifi-covered area and
so on. Reliable network and therefore a reliable way to communicate with
a backend service is often a luxury. However, apps need to be resilient
to these possible network interruptions and preferably retry the service
communication if possible. To solve the latter problem, we can try to
code a retry-mechanism that will attempt to restore the connection after
it has failed. While that's not impossible to do, it's easier if someone
has already done this work for us. Polly
(<https://github.com/App-vNext/Polly>) is a resiliency library that's
commonly used in (mobile) apps to tackle possible failures in
communicating with web services. Low-level stuff such as retrying the
connection belongs in the repository classes. In the next snippet, you
can see how we have wrapped the call to the backend using Polly, and
have applied a retry-mechanism that will retry the call if the backend
was unavailable for some reason. The setup of the retry mechanism is
such that it uses an exponential value between different attempts.

var responseMessage = await Policy

.Handle\<WebException\>(ex =\>

{

Debug.WriteLine(\$\"{ex.GetType().Name + \" : \" + ex.Message}\");

return true;

})

.WaitAndRetryAsync

(

5,

retryAttempt =\> TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))

)

.ExecuteAsync(async () =\> await httpClient.GetAsync(uri));

In addition to retrying, another optimization that can be done in this
area is caching. Mobile apps shouldn't put load on the server to
retrieve data that they may already have. Through caching, we can quite
simply store data on the device. There are of course a number of options
to do this. One way that I particularly like is using Akavache here.
Akavache (<https://github.com/reactiveui/Akavache>) is a key-value store
that has many usages. The way I use it here is simply for throwing some
data at it that I want to cache. The data will be stored with an
expiration date and so when the data is retrieved from the cache,
Akavache will check whether the locally-stored version is still valid.
If so, it will be returned, if not, a new version can be fetched from
the underlying data source and cached in Akavache automatically. While
caching can be a lifesaver in many situations, it can also cause
problems in your application. Before applying it, think whether it makes
sense on that data to actually cache it. In the snippet below, you can
see that we're checking whether we can find data in the Akavache cache
and return it if found.

public async Task\<ObservableCollection\<Event\>\> GetAllEventsAsync()

{

List\<Event\> eventsFromCache = await
GetFromCache\<List\<Event\>\>(CacheNameConstants.AllEvents);

if (eventsFromCache != null)//loaded from cache

{

return eventsFromCache.ToObservableCollection();

}

else

{

UriBuilder builder = new UriBuilder(ApiConstants.BaseApiUrl)

{

Path = ApiConstants.CatalogEndpoint

};

var events = await
\_genericRepository.GetAsync\<List\<Event\>\>(builder.ToString());

await \_cache.InsertObject(CacheNameConstants.AllEvents, events,
DateTimeOffset.Now.AddSeconds(20));

return events.ToObservableCollection();

}

}

## MVVM to rule them all

In the quest to achieve loose coupling and a high(er) level of
testability, we will undeniably run into UI code. This code is rather
hard to test so the only option we have here is launching on an emulator
and clicking/tapping through the screens. However, this is not what we
intended in the first place, isn't it? The pattern that will help us
here is MVVM, the Model-View-View-Model pattern. I'm sure you've already
heard of it, it's a pattern that became popular at the time of WPF and
(yes!) Silverlight. It's built on the foundations of XAML, data binding
and commanding, and those are indeed available in Xamarin.Forms as well.
The following diagram shows the structure of the involved classes. The
View code is still XAML but now it contains data bound to the view
model. The view model is basically an abstraction of what is presented
in the view and doesn't contain actual UI elements. It will also
implement the behavior, such as the interaction with the model for us.
The view model will expose state (=data) and operations (=commands) to
the view. Data binding and the built-in change notification system based
on the INotifyPropertyChanged interface will ensure that the view is
updated automatically when the data changes in the view model.

![](./media/image3.png)


A view model is typically just a class which, as mentioned, exposes
state and operations for the view to bind to. Here you can see a simple
view model for a login screen, which requires a user name and password.
Essentially, this is the data for that screen. Next, interactions such
as clicking on a login button, which would typically be handled using an
event handler in the view's code-behind, will now be wrapped inside a
command in the view model instead. Commands are used to wrap
functionality which can be called from other places in the application.
In this case, they will wrap the behavior to handle a UI event.

public class LoginViewModel : ViewModelBase

{

private ICommand \_loginCommand;

public string UserName

{

get { return \_userName; }

set

{

\_userName = value;

OnPropertyChanged(nameof(UserName));

}

}

public string Password

{

get { return \_password; }

set

{

\_password = value;

OnPropertyChanged(nameof(Password));

}

}

public ICommand LoginCommand =\> \_loginCommand ?? (\_loginCommand = new
Command(OnLogin));

## } 

## 

## Simple view models

You may get the idea that a view model will simply contain all the code
that originally was located inside the code-behind and that we've
essentially just been moving some boxes around. That wouldn't be of much
help, now would it? One of the key aspects is that view models should be
as simple as possible. They are like the controller in MVC applications,
and those too should remain simple. They know about the flow of the
applications but they don't know how to perform navigation. They know
that because of a certain event in the application, a dialog should be
shown, but they don't know HOW to display that dialog.

Keeping all this knowledge outside of the view model is essential to
keeping them easy to test later on. All this "external" knowledge about
how to navigate, how to show a dialog, how to check whether we are
connected with the internet and so on should be pushed into a separate
service class, which in essence is nothing more than a simple class that
is capable of just one single piece of functionality. It's a good
example of using the Single Responsibility Principle. Think of a
navigation service, a dialog service, a connection service, and many
others. In a real-life application, you'll end up with quite a few of
these. Below, you can see (part of) a dialog service. To display
dialogs, we use another library called ACR Dialogs and that's wrapped
inside this simple service.

public class DialogService : IDialogService

{

public Task ShowDialog(string message, string title, string buttonLabel)

{

return UserDialogs.Instance.AlertAsync(message, title, buttonLabel);

}

public void ShowToast(string message)

{

UserDialogs.Instance.Toast(message);

}

}

Services are commonly registered in the application by means of a
dependency injection container such as Autofac or TinyIOC. These
containers work perfectly fine in Xamarin.Forms apps and allow us to
register service classes during the bootstrapping of the application. In
the following snippet you will see that we're using the container and
registering some of the classes we'll typically have in this type of
applications, such as a view model and a service class.

public class AppContainer

{

private static IContainer \_container;

public static void RegisterDependencies()

{

var builder = new ContainerBuilder();

//ViewModels

builder.RegisterType\<LoginViewModel\>();

builder.RegisterType\<DialogService\>().As\<IDialogService\>();

\_container = builder.Build();

}

## }

Once registered, view models will get an instance of these services
injected through dependency injection. These instances are then invoked
to perform the actual functionality such as showing the actual dialog.
Note that indeed it's the view model that will know that a dialog needs
to be shown, but it doesn't know how to do this. That's the
responsibility of the DialogService class.

##  "Hello, is this View Model? Yes, this is View Model"

Remember that at the beginning of this article we set out to create a
loosely coupled architecture that's easy to test? Well, we have another
problem to solve. Very often, view models will need to interact with
other view models. Think of a Settings View Model that needs to let
other view models know that the user has switched the currency. Our
first thought might be that we would have a direct reference from this
Settings View Model to all interested view models. While that would
work, we would end up with references from one view model to the next,
and this brings tight coupling with it, which is not what we were aiming
for! This means that the view models need another way of communicating,
and the preferred way of doing so is through a messenger using a pub-sub
model. In this model, a view model will register to send messages to the
messenger, and other view models will register to receive updates from
that messenger. Xamarin.Forms comes with support for this pattern,
built-in through the MessagingCenter class. You can see an example of
this below.

Registering to receive the message:

public async override Task InitializeAsync(object data)

{

MessagingCenter.Subscribe\<Currency\>(this,
MessagingConstants.CurrencyChanged, OnCurrencyChanged);

}

Sending the message:

private async void OnChangeCurrency()

{

MessagingCenter.Send(this, MessagingConstants.CurrencyChanged,
SelectedCurrency);

}

![](./media/image4.png)


## 

## Putting things to the test

Now that we have separated everything nicely, can we actually test the
view models and thus create a more robust code base? Well, the answer is
a definite YES! Take a look at the following snippet in which we are
creating a unit test for one of the view models in the application.

\[Fact\]

public void LoginCommandIsNotNullTest()\
{\
var authenticationService = new AuthenticationMockService();\
var loginViewModel = new LoginViewModel(authenticationService);\
Assert.NotNull(loginViewModel.LoginCommand);

}

## 

## Summary

Creating loose coupling and testable applications is definitely
applicable for mobile applications with Xamarin.Forms. The patterns
we've described here definitely put us on the right track to create
Xamarin.Forms apps that will be easier to test and maintain in the long
run. And that's exactly what enterprises are looking for right now for
their mobile endeavors.
