# OLD

## Introduction

## Shift left

Shift left is a term that is often used in the context of DevOps. It means breaking down the silos in software development by moving responsibilities to the left, to the development team. The goal of shifting left is to empower teams. Teams become responsible for building and running their software products. They are responsible for the full lifecycle of their software. This means they are not only responsible for building and maintaining features, or adding business value, but they are also responsible for the operational aspects of their software. This includes areas such as security, compliance, costs, or sustainability to name a few. Teams don't have to wait for others to do these things for them. They can do it themselves. This is a powerful concept. It enables teams to move fast and deliver value to the business.

_[add of siloed approach vs. end-to-end responsibility]_

The downside of shifting left is that teams are now responsible for a lot of things. They have to build and run their software products. They have to make sure their software is secure, compliant, sustainable, etc. This takes a lot of time and effort. Teams have to learn new things and acquire new skills. They have to keep up with the latest developments in the field. They have to make sure they are doing things right. This can be overwhelming. Teams can get bogged down by all the responsibilities. Because fo this they can become less productive and less effective over time as their application grows and responsibilities increase.

Let's think about hosting a public ASP.NET core API in an app service that uses SQL database on Azure. What are some things you need to take into account when deploying this infrastructure? You'll want to have a Web Application Firewall (WAF) in front of the app service to protect it. Make sure direct public access to the app service is blocked by using private networks and private endpoints. Network access to the database should be locked down to only the app service, again using private networks and private endpoints. Data access should also be restricted to only the app service using a managed identity and the app service should only have permissions to read and write the data it needs to function. You might also want to encrypt the database with a customer managed key for additional security. You'll need to export the audit logging to log analytics workspace or storage account so that you have a trace of all the actions taken on the database.

_[add picture of required Azure resources]

This is just the tip of the iceberg, we haven't touched upon the (git) repository for the code, the CI/CD pipeline, the monitoring and alerting, backups, scaling, availability, costs, sustainability, etc. Then there is the code, there are a lot of best practices and patterns to follow to make sure the code is secure, maintainable, testable, and scalable. As you can see this is already quite a long list of things a team has to figure out without writing a single line of code that actually implements business value.

_[add with repo/pipelines/etc]_

## Unburdening teams

So how do you help teams to spent more time on creating business value without reintroducing the silos that shifting left was supposed to break down? How do you keep empowering teams with end-to-end responsibility without burdening them with all the responsibilities and work that come with it? The answer is by splitting the responsibilities horizontally instead of vertically. Instead of moving responsibilities to other teams, you share them with other teams.

_[add picture of how end-to-end responsibilities can be shared]_

Shifting down is a term coined by Google recently to emphasize that you can move a big parts of the responsibilities down into a platform. Teams are still responsible for building and running their software products. They are still responsible for the full lifecycle and all aspects of their software. But they have to solve every problem themselves. Instead they can rely on a platform to take care of a lot of the operational aspects of their software. This platform is secure, compliant, sustainable, etc. by default. Teams build their own software on top of this platform and can rely on the qualities of the platform to cover a big part of their responsibilities. They are still responsible for security, compliance, costs, sustainability, etc. but they only have to focus on the parts that are specific to their software. This way teams can spend most of their time on what matters most, creating business value.

As an industry, we have been doing this already for quite a few year now. With the move to the public cloud a lot of the responsibilities of maintaining the actual infrastructure underpinning the applications has already been moved to big vendors like Microsoft, Amazon, or Google.

The state of DevOps 2024 report confirmed again a finding of previous years: how teams use the cloud is more important than simply using the cloud. ....

## Platform as a product

Platforms are products in their own right. The platform is build by a specialized team that is end-to-end responsible for building and maintaining the platform, just like the other development teams. The state of DevOps 2024 suggests that threating development teams as the users users of their platform is a more successful approach than build it and "they" will come.

The platform team is responsible to ensure the platform is secure, compliant, sustainable, etc by default. They make sure the platform is easy to use and easy to understand. They make sure the platform is cost-effective, for example by pooling and sharing resources whenever possible. In other words, they make sure the platform is a solid foundation for other teams to build on. This way the platform team creates business value by reducing the time other teams have to spend on all the details that come with running software in the cloud.

*A term coined by Google recently to emphasize that you can move a lot of the responsibilities down into a platform.*
[The Modernization Imperative: Shifting left is for suckers. Shift down instead](https://cloud.google.com/blog/products/application-development/richard-seroter-on-shifting-down-vs-shifting-left)

## Conclusion

Breaking down the silos and empowering teams by shifting left, giving teams end-to-end responsibility, has also burdened these teams with a lot of extra work. DevOps teams are responsible for building en running their application. They are responsible for security, costs, compliance, sustainability, regular operations, and any other aspect that comes with running applications. All these responsibilities take a lot of time away from building software and creating business value, which is the purpose of the teams.

By shifting down businesses can keep empowering the teams with end-to-end responsibility but unburden by providing a platform that is secure, compliant, sustainable, etc. by default. Building on top of this platform frees up time for the teams to build software again and create business value. Teams are still responsible for all the aspects that come with building and running their application but they do not have do all the work themselves. The platform team builds and maintains the platform and creates business value by unburdening other teams.

Shifting left is the right move! Teams get empowered to build and run their software products. Platforms provide a solid foundation to unburden the teams of a lot of operational aspects. This way teams can spend most of their time on what matters most, creating business value. At Xebia we call this "power through platforms".