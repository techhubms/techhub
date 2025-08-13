# Was shift left the right move?

## Introduction

Shifting left, by adopting DevOps practices, improves the performance of development teams. Granting teams the responsibility for all aspects of their software products allows them to be more efficient. A study from Leiden University [\[1\]][1] shows that the adoption of DevOps practices leads to improvement in both software quality and delivery speed. This is not surprising; The goal of shifting left, and DevOps, is to shorten the feedback loop and reduce lead time. In 2014 McKinsey [\[2\]][2] already found that projects that adopt short cycle times tend to perform better, and deliver higher quality outcomes. To effectively shorten cycles, and improve the performance of development teams, autonomy is essential.

![Silos](images/silos.svg)

![Shift left](images/shift-left.svg)

## The downside of shifting left

The autonomy that comes with shifting left has a price. It has increased the load on development teams. Teams build and maintain all aspects of their software products. They are responsible for security, compliance, costs, sustainability, regular operations, and any other aspect that comes with running applications. All these responsibilities take a lot of time and attention away from building software and creating business value, which is the purpose of the teams.

This high cognitive load negatively impacts productivity. It is hard to keep all aspects of the software product in mind all the time.
Cognitive load theory suggests that your brain has a limited capacity to process and store information at any given time. If the cognitive load is too high, your brain will not be able to process and store all the information. Puppet's state of DevOps report 2021 [\[3\]][3] even found that unbounded cognitive load has a negative impact on all performance indicators.

To meet all these demands each team has to solve problems. They need setup their developer environments, manage dependencies, deal with build failures, etc. All these tasks are necessary but do not directly contribute to the business's core objectives or innovation. This is what we call developer toil.

Like cognitive load, developer toil also has a big impact on productivity, innovation, and motivation. While some level of toil is part of the job, excessive toil can be detrimental. It consumes precious time, drains motivation, and can lead to burnout [\[4\]][4].

So, whilst shifting left improves the performance of development teams by reducing cycle time, it also increases the cognitive load and developer toil for these teams. This hinders, or even partially negates, the benefits of shifting left.

## Shift down

While keeping the benefits of shifting left, the goal is to find ways to reduce the cognitive load and developer toil on development teams. Ideally teams to spend their time on innovation and creating value, while keeping the autonomy that comes from shifting left.
But instead of only shifting left, the next step would be to also shift down. Shifting down is a term introduced by Google's Richard Seroter [\[5\]][5]. It means splitting responsibilities horizontally by offering platforms for development teams which they can build upon. At Google, they offer managed experiences for coding, testing, building, releases, roll-outs, hosting, alerting and more [\[5\]][5]. These platforms are maintained by dedicated teams of engineers, so that development teams can focus again on building software and creating business value.

![Shift down](images/shift-down.svg)

## Platform engineering

Why not have special teams dedicated to building platforms that provide the infrastructure, tools, and processes that support software development across your organization. Gartner expects around 80% of organizations plan to have a team dedicated to Platform Engineering by 2026 [\[6\]][6]. Platforms abstract complexity, integrate best practices, and provide reusable components, ultimately streamlining the development process. This way we can unburden the development teams from a lot of operational aspects and repetitive tasks.

To reduce developer toil and cognitive load, platform engineering teams have to tackle a few key challenges:

1. **Automate Everything**  
Identify tasks that are repetitive and time-consuming and automate them. Continuous integration/continuous deployment (CI/CD) pipelines, infrastructure-as-code (IaC), and automated testing frameworks are great starting points.

2. **Offer a golden path**  
Create well-defined combinations of tools and building blocks that meet all the organizational requirements and standards. These golden paths should be easy to use and provide a clear path to production without requiring deep knowledge of the underlying platforms. They should cater to the majority of the teams. Teams that require more flexibility or have specific requirements can still choose their own path.

3. **Self-Service**  
Provide self-service experiences that allow developers to independently handle routine tasks, such as onboarding new team members, modifying shared infrastructure, etc. This reduces their dependency on other teams and speeds up the development process.

## Platform as a product

Platforms should be treated as products in their own right. The platforms are built by specialized teams that are end-to-end responsible for building and maintaining the platforms, just like the other development teams. DORA's state of DevOps 2023 suggests that treating development teams as the users of their product is a more successful approach than build it and "they" will come [\[7\]][7].

The platform team is responsible to ensure the platform is secure, compliant, sustainable, etc by default. They make sure the platform is easy to use and easy to understand while being cost-effective. In other words, make sure the platform is a solid foundation for other teams to build on. This way the platform team creates business value by reducing the time other teams have to spend on all the details that come with running software in the cloud.

Of course there can also be pitfalls with treating platforms as products. The platform team can become too focused on the platform itself and lose sight of the needs of the development teams. They can become too focused on the technical aspects and lose sight of the business value. The platform team should always keep the development teams in mind and make sure they are building a platform that is valuable for the development teams. It can also be resource intensive. Developing and managing a platform often requires substantial time, financial investment, and human resources, which can strain budgets and personnel.

## Conclusion

Shifting left is the right move! Teams get empowered to build and run their software products. Through engineering platforms, you can provide a solid foundation to unburden the teams. This way teams can spend most of their time on what matters most: innovation and creating business value.

Breaking down the silos and empowering teams by shifting left, giving teams end-to-end responsibility, has also burdened these teams with a lot of extra work. Development teams are responsible for building and running their application. They are responsible for security, costs, compliance, sustainability, regular operations, and any other aspect that comes with running applications. All these responsibilities take a lot of time away from innovation and creating business value, which is the purpose of the teams.

Shifting down, by providing platforms, allows you to keep empowering your teams with end-to-end responsibility. Dedicated platform teams build and maintain these platforms. The platforms are products on their own. They create business value by unburdening other teams. This way you can keep the benefits of shifting left, while reducing the cognitive load and developer toil on development teams by shifting down.

Not sure how to start with platform engineering? Xebia has an accelerator to give you a head start with platform engineering on Azure. For more information on a technology that can help with platform engineering, check out Cloud-Native Application Development with Radius by Loek Duys in this same magazine.

## References

\[1\] Offerman, Blinde, Stettina, and Visser. "A Study of Adoption and Effects of
DevOps Practices". arXiv, 17 Nov. 2022, pp 9. [http://arxiv.org/pdf/2211.09390](http://arxiv.org/pdf/2211.09390)

\[2\] McKinsey & Company. "Achieving success in large, complex software projects". McKinsey Digital, July 2014. [https://www.mckinsey.com/~/media/McKinsey/Business%20Functions/McKinsey%20Digital/Our%20Insights/Achieving%20success%20in%20large%20complex%20software%20projects/Achieving%20success%20in%20large%20complex%20software%20projects.ashx](https://www.mckinsey.com/~/media/McKinsey/Business%20Functions/McKinsey%20Digital/Our%20Insights/Achieving%20success%20in%20large%20complex%20software%20projects/Achieving%20success%20in%20large%20complex%20software%20projects.ashx)

\[3\] Puppet. "State of DevOps report 2021", Puppet, 2021, pp 16. [https://www.dau.edu/sites/default/files/Migrated/CopDocuments/Puppet-State-of-DevOps-Report-2021.pdf](https://www.dau.edu/sites/default/files/Migrated/CopDocuments/Puppet-State-of-DevOps-Report-2021.pdf)

\[4\] Forbes. "Developer Toil Is A Problem—Here’s Why Companies Need To Address It", Debo Ray, 20 March 2024. [https://www.forbes.com/councils/forbestechcouncil/2024/03/20/developer-toil-is-a-problem-heres-why-companies-need-to-address-it/](https://www.forbes.com/councils/forbestechcouncil/2024/03/20/developer-toil-is-a-problem-heres-why-companies-need-to-address-it/)

\[5\] "The Modernization Imperative: Shifting left is for suckers. Shift down instead", Richard Seroter, 9 June 2023. [https://cloud.google.com/blog/products/application-development/richard-seroter-on-shifting-down-vs-shifting-left](https://cloud.google.com/blog/products/application-development/richard-seroter-on-shifting-down-vs-shifting-left)

\[6\] "Platform Engineering That Empowers Users and Reduces Risk ", Gartner. [https://www.gartner.com/en/infrastructure-and-it-operations-leaders/topics/platform-engineering](https://www.gartner.com/en/infrastructure-and-it-operations-leaders/topics/platform-engineering)

\[7\] "State of DevOps Report", DORA, 2023, pp 19. [https://dora.dev/research/2023/dora-report/](https://dora.dev/research/2023/dora-report/)

[1]: http://arxiv.org/pdf/2211.09390
[2]: https://www.mckinsey.com/~/media/McKinsey/Business%20Functions/McKinsey%20Digital/Our%20Insights/Achieving%20success%20in%20large%20complex%20software%20projects/Achieving%20success%20in%20large%20complex%20software%20projects.ashx
[3]: https://www.dau.edu/sites/default/files/Migrated/CopDocuments/Puppet-State-of-DevOps-Report-2021.pdf
[4]: https://www.forbes.com/councils/forbestechcouncil/2024/03/20/developer-toil-is-a-problem-heres-why-companies-need-to-address-it/
[5]: https://cloud.google.com/blog/products/application-development/richard-seroter-on-shifting-down-vs-shifting-left
[6]: https://www.gartner.com/en/infrastructure-and-it-operations-leaders/topics/platform-engineering
[7]: https://dora.dev/research/2023/dora-report/
