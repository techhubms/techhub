# Upgrade Your App to the Future: Migrating from WPF/WinForms to Blazor

*This article is based on a WPF to Blazor migration project and
Microsoft documentation*

Desktop applications built with WPF or WinForms have been widely used
for many years, allowing developers to create feature-rich desktop
applications with complex user interfaces. However, with the increasing
demand for cross-platform applications and modern user interfaces, it
has become an appealing option to migrate legacy WPF/WinForms
applications to modern web technologies, even as Microsoft continues to
maintain WPF and WinForms.

Blazor is a relatively new web framework developed by Microsoft that
enables developers to build web applications using .NET. With Blazor,
you can leverage your existing .NET libraries, frameworks, and skills to
create web applications that run in the browser (Blazor WebAssembly) or
on a server (Blazor Server) without the need for plugins or JavaScript.\
More detailed information about Blazor can be found in the article
"Introduction to Blazor" by Mark Foppen in magazine #13.

In this article, you will learn about the potential process of upgrading
your legacy WPF/WinForms application to a modern Blazor web application.
We will also cover the steps you need to take, the decisions you\'ll
need to make, and the potential pitfalls to avoid.

**Why Blazor?**

When deciding to upgrade your existing application, you\'ll need to
determine the best approach. You probably will investigate what solution
fits best for you. Do you want to start greenfield and start collecting
the right specifications from scratch? Time and budget might be limited,
then it's hard to make a decision with these options.\
In that case, you might want to explore Blazor. Since you already have a
lot of business logic in your existing .NET application, you can save
time by reusing this logic. Also, having .NET knowledge within your team
or organization provides a significant advantage. The learning curve is
more gradual than learning an entirely new framework.

Compared to WinForms or WPF, Blazor offers several advantages:

-   Blazor is supported by all major browsers [^1](Safari, Chrome, Edge,
    FireFox), including mobile

-   Deployment/Delivery is easier to manage

-   Plenty of widely supported UI frameworks

-   Multiple different hosting methods

While both WPF and WinForms still have roadmaps[^2] [^3], Microsoft's
product teams are mainly focused on performance and bug fixes, and the
capacity of these teams is limited. Blazor, on the other hand, is
gaining popularity. We can expect many improvements with almost every
ASP.NET Core upgrade, benefiting both application performance and
developer productivity.

**\
**

**The preparation**

Hopefully, your application\'s architecture separates view logic from
business logic, possibly using MVC or MVVM or a similar architecture. If
not, migrating this (business) logic from the old view to the new modern
web view will require extra work.

Also, best practices widely used today may not have been common when
your WPF/WinForms application was built. For example, dependency
injection, NuGet packages, pipelines for deploying the application, and
more will require additional time. Take this into account when planning
and implementing the solution.

Moreover, ensure that your project\'s .NET versions, NuGet packages, and
other dependencies are up-to-date, using at least the .NET Standard or
the latest supported .NET version. Outdated libraries may cause
problems, especially when using Blazor WebAssembly.

Lastly, consider your generic components. You might have created
reusable WPF/WinForms components or used a library with components. For
Blazor, you\'ll want to do something similar.

Numerous open-source or free-to-use component libraries are available
within the community. Check if these libraries offer the features you
need and explore them early to avoid switching between libraries and
wasting time. MudBlazor[^4] and Radzen[^5] are well-known libraries with
a wide variety of user interface components. Both MudBlazor and Radzen
are free to use and contain a large list of components that are easy to
use in your application. Keep in mind not all (component) libraries are
just free to use. Make sure to understand their license model.

**Moving from a desktop to the browser**

Transitioning from WPF or WinForms to Blazor means moving from a desktop
(probably Windows) environment to a browser. This shift may limit some
functionality, such as interactions with the host. For example, if your
application opens Microsoft Word and controls its usage, you\'ll lose
that control when moving to a browser-based application. You can still
enable users to open documents in Word, but you won\'t be able to
control their actions within Word.

If your application relies heavily on the operating systen, consider
looking into Blazor Hybrid[^6] or .NET MAUI[^7] as alternatives.

Making your application accessible in a browser also requires a
different deployment approach. Depending on the chosen hosting method,
your application should be made available on the internet or an
(internal) network. This means you\'ll need to prepare or adjust your
network infrastructure, set up SSL certificates to secure connections,
and configure DNS to expose your application on a familiar and safe web
address.

**\
**

**Blazor Server vs Blazor WebAssembly**

When setting up your new solution, you\'ll need to choose between Blazor
Server and Blazor WebAssembly. While it may not seem crucial, this
decision significantly impacts your infrastructure and application
reliability. It\'s a good idea to consider this choice early in your
project. You can still switch between the two, but it will require time
and effort. If you\'re unsure which option is best, you can initially
support both and test their reliability.

Understanding the difference between the two hosting models is
essential. Blazor WebAssembly runs entirely in the user\'s browser,
while Blazor Server renders HTML server-side, and client interactions
are processed through SignalR.

Microsoft provides an overview[^8] to help you choose the right hosting
model for your needs.

![A picture containing table Description automatically
generated](./media/image1.png)


*†Blazor WebAssembly and Blazor Hybrid apps can use server-based APIs to
access server/network resources and access private and secure app code.\
‡Blazor WebAssembly only* *reaches near-native performance
with ahead-of-time (AOT) compilation.^8^*

For Blazor WebAssembly to work in your environment, you\'ll need a
service (preferably an API) to interact with your internal systems (e.g.
databases). In contrast, Blazor Server can act as the single entry point
for your environment, interacting with all internal systems.

However, Blazor Server has some drawbacks, such as maintaining the state
of interacting users on the server. This can complicate matters, as
scaling or rebooting your application could cause users to lose their
session. Additionally, you\'ll need to ensure no shared state between
users to avoid undesirable or harmful situations.

Compared to your WPF/WinForms application, Blazor WebAssembly may be a
more suitable choice, as both WPF/WinForms and Blazor WebAssembly run
client-side. However, keep in mind that not all .NET libraries are
supported in Blazor WebAssembly. It also has a longer initial load time,
as all relevant DLLs need to be downloaded first. Moreover, since the
application runs client-side you will need a public API to communicate
to databases or internal services. Or you need to expose these databases
and internal services so your application can access it from the client,
but you probably don't want this due to security measures.

As a result, your application will behave like a Single Page Application
(SPA) with excellent performance. You can host your application as a
static app, eliminating concerns about frontend scaling.

With .NET 8 Blazor will get an enhancement called "Blazor United". This
will be a combination of Blazor Server and Blazor WebAssembly. This
means when the DLL's are downloaded the application will use these and
have a quick response time. When the DLL's are not available yet, it
will use the Blazor Server capability to make sure you don't have to
wait on downloading these DLL's. By using this the application users
will get a better/smoother experience. If "Blazor United" is available
for you then you should look into this as this offers best of both
worlds.

**User experience**

One important aspect to consider is the change in user experience (UX)
for your end-users. With your current application, users are accustomed
to a desktop experience, often featuring a Microsoft Windows look and
feel. Transitioning to a browser-based application will introduce a
different experience. Elements such as URLs, opening tabs, and cookies
might be new to some users. As a result, it\'s essential to provide
clear instructions on how to use the application within a browser
environment.\
Authentication will also be different. With WPF/WinForms you might have
username password authentication or maybe Windows Authentication. With
Blazor you could use Azure Active Directory B2C to provide a safe
solution.

Additionally, this transition presents an opportunity to make
significant UX improvements to your application. If applicable, consider
utilizing monitoring tools to analyze usage patterns and make
data-driven enhancements. By focusing on user experience and addressing
potential challenges, you can ensure a smoother transition and a more
satisfying experience for your application\'s users.

**Conclusion**

In conclusion, migrating your legacy WPF/WinForms application to a
modern Blazor web application can be a strategic and beneficial
decision. Blazor enables you to leverage your existing .NET knowledge
and resources while providing a more future-proof, browser-based
solution that supports a wide range of devices and platforms.

The migration process requires careful planning, architectural
considerations, and an understanding of the differences between Blazor
Server and Blazor WebAssembly hosting models. By taking the time to
analyze your current application\'s architecture, dependencies, and
components, you can ensure a smoother transition to Blazor.

Moreover, embracing best practices and modern development techniques
will help you create a more maintainable and scalable application in the
long run. Keep in mind the trade-offs between Blazor Server and Blazor
WebAssembly when choosing a hosting model that best suits your
application\'s requirements and infrastructure.

As you embark on this journey, it\'s essential to consider the impact of
the migration on user experience and address potential challenges.
Transitioning from a desktop application to a browser-based solution
presents an opportunity to make significant UX improvements. Utilize
monitoring tools to analyze usage patterns and make data-driven
enhancements to optimize the experience for your application\'s users.

By focusing on these aspects and providing clear instructions to help
users adapt to the new application, you can ensure a smoother transition
and a more satisfying experience for your users. Remember, while there
may be challenges along the way, the end result will be a more
versatile, powerful and maintainable application that embraces modern
web technologies.

Will you upgrade your app to the future?

[^1]: https://learn.microsoft.com/en-us/aspnet/core/blazor/supported-platforms?view=aspnetcore-7.0

[^2]: https://github.com/dotnet/wpf/blob/main/roadmap.md

[^3]: https://github.com/dotnet/winforms/blob/main/docs/roadmap.md

[^4]: https://blazor.radzen.com

[^5]: https://mudblazor.com

[^6]: https://learn.microsoft.com/en-us/aspnet/core/blazor/hybrid/?view=aspnetcore-7.0

[^7]: https://learn.microsoft.com/en-us/dotnet/maui

[^8]: https://learn.microsoft.com/en-us/aspnet/core/blazor/hosting-models?view=aspnetcore-7.0
