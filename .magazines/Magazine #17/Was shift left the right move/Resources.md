# Resources

## Statements

- ~~Shifting left, by adopting DevOps practices, improves the performance of development teams.~~
  - ~~[The adoption of DevOps practices has been shown to improve software quality and delivery speed, as evidenced by a study from Leiden University](http://arxiv.org/pdf/2211.09390)~~
- However, the autonomy that comes with shifting left has a price.

- Higher cognitive load can hinder productivity and innovation.  
  - ~~[Cognitive load theory suggests that excessive cognitive load can hinder productivity and innovation.](https://link.springer.com/chapter/10.1007/978-3-319-49094-6_44)~~
  - ~~[Unbounded cognitive load shows negative impact in all performance indicators](https://www.dau.edu/sites/default/files/Migrated/CopDocuments/Puppet-State-of-DevOps-Report-2021.pdf)~~
  - [We conclude that by using load-reducing strategies and a motivating style characterized by structure and autonomy support, teachers can reduce students’ cognitive load and improve their self-regulated motivation, engagement, and achievement](https://link.springer.com/article/10.1007/s10648-023-09841-2)
- Platforms are a way to share responsibilities horizontally.
  - [The state of DevOps report 2021 found a high degree of correlation between DevOps evolution and the
use of internal platforms.](https://www.dau.edu/sites/default/files/Migrated/CopDocuments/Puppet-State-of-DevOps-Report-2021.pdf)
  - [Puppet's state of DevOps report 2024 outlines the key benefits and responsiblities of platform teams](https://www.dau.edu/sites/default/files/webform/documents/26881/2024-state%20of%20devoops%20report.pdf)
- Platforms should be treated as products in their own right.
  - [The state of DevOps report 2023 suggests that treating development teams as the users of their platform is a more successful approach than build it and "they" will come.](https://www.dau.edu/sites/default/files/webform/documents/26881/2024-state%20of%20devoops%20report.pdf)
- Platforms provide value by unburdening teams.
  - [Case studies show that companies implementing platform engineering have seen improvements in operational efficiency and developer productivity](https://savvycomsoftware.com/blog/platform-engineering/)
  - [The 2024 State of DevOps Report highlights the increasing integration of platform engineering within DevOps practices, emphasizing its role in enhancing efficiency, speed, and security](https://www.dau.edu/sites/default/files/webform/documents/26881/2024-state%20of%20devoops%20report.pdf)

- Platform engineering isn't new
  - [with Gartner expecting around 80% of organizations planning to have a team dedicated to Platform Engineering by 2026](https://www.gartner.com/en/infrastructure-and-it-operations-leaders/topics/platform-engineering)
- The public cloud has already moved a lot of responsibilities to big vendors.
- Using the public cloud correctly is not straightforward.
- A single team cannot be expected to have know and do everything.

### State of DevOps report 2021, Puppet

[Link](https://www.dau.edu/sites/default/files/Migrated/CopDocuments/Puppet-State-of-DevOps-Report-2021.pdf)

#### page 16

> If cognitive load is left
> “unbounded” (i.e. keeps growing as teams’ responsibilities grow without limits)
> then all these performance metrics will be negatively affected, preventing teams
> from evolving to higher levels

#### page 18/19

> See graphics

### State of DevOps report 2024, Puppet

#### page 11

Shows what should be in scope of the platform team.

#### page 12

> While DevOps empowered some highly skilled, multi-talented developers
> to innovate, it unintentionally burdened a majority with repetitive tasks that
> don’t add value. This “toil” diverted their focus from areas where they could
> truly make a difference.
> Platform Engineering isn’t an attempt by ops to regain control or centralize
> power. Instead, it seeks a harmonious balance, enabling developers and
> operations to work together on a platform that benefits both parties.

### State of DevOps report 2023, v. 2023-12, DORA 

#### page 19

> Platform engineering teams
>
> Platform engineering teams might
> adopt a “build it and they will come” approach
> to building out a platform. A more successful
> approach might be in treating developers as
> users of their platform. This shift in focus requires
> platform engineering teams to understand how
> developers work today to successfully identify
> and eliminate areas of friction. Teams can use the
> software delivery and operational performance
> measures as signals to monitor whether platform
> efforts are helping teams achieve better results.

#### page 39

Same result was already found in the [2018 DORA report](https://cloud.google.com/blog/products/devops-sre/new-research-what-sets-top-performing-devops-teams-apart)
> Results
>
> Once again, we confirmed previous findings:
> how a team uses the cloud is a stronger predictor
> of performance than simply that they use the cloud.
> While the use of cloud can be a powerful enabler,
> it doesn’t automatically produce benefits. In fact,
> we see strong indicators that public cloud leads to
> decreased software and operational performance
> unless teams make use of flexible infrastructure.
> This finding further promotes the idea that simply
> “lifting and shifting” (the act of shifting workloads
> from a data center to the cloud) is not beneficial
> and can be detrimental.
> The use of cloud computing is associated with a
> substantial decrease in burnout, and substantial
> increases in job satisfaction and productivity

#### page 43

> Cloud computing platforms, when used in a
> way that maximizes the flexible infrastructure
> characteristics, predict a positive impact on software
> delivery and operational performance. This difference
> in impact speaks to what most practitioners and
> leadership already know—simply shifting your
> workloads from a data center to the cloud does not
> bring success. The key is to take advantage of the
> flexible infrastructure that cloud enables.
>
> To maximize your potential for benefit, you must
> rethink how you build, test, deploy, and monitor your
> applications. A big part of this rethinking revolves
> around taking advantage of the five characteristics
> of cloud computing: on-demand self-service, broad
> network access, resource pooling, rapid elasticity,
> and measured service

### The Modernization Imperative: Shifting left is for suckers. Shift down instead

[Link](https://cloud.google.com/blog/products/application-development/richard-seroter-on-shifting-down-vs-shifting-left)

> To be sure, “shift left” — the practice of incorporating security and QA reviews earlier in the development process — is a perfectly sound idea. But over the years, more types of work not traditionally part of a developer’s job description have been sliding left in the name of empowering “full stack engineers.” And that needs to stop.
> [Mini, but justified, rant: There are like nine actual “full stack engineers” on planet Earth. Virtually nobody writes a frontend in React, sets up Kubernetes, configures a RabbitMQ instance, provisions space on the SAN, and lights up the top-of-rack switch. Today’s developers are asked to know web frameworks, architecture patterns, testing strategies, build systems, multiple types of databases, caches, automation tools, container orchestrators, L4-L7 networking concepts, SaaS APIs, monitoring systems, numerous public clouds, and oh, maybe a little machine learning. I just flipped through Indeed.com, and it’s remarkable to see what we’re asking of junior and senior developers. It’s too much.]
> Don’t force people to know so much to do their jobs. Offer platform abstractions. I recently did a talk for a customer about “how Google does DevOps' and it highlighted how many platforms we offer our engineers. We offer managed experiences for coding, testing, building, releases, rollouts, hosting, alerting and more. Dedicated Google teams support these critical platforms so our product engineers can focus on what they need to do without knowing about or operating a “full stack” of infrastructure. Every organization should be doing the same.
> Rather than demanding more from their current engineering resources — learning new languages, platforms, and clouds — technology leaders need to stand up platform engineering teams that treat their platforms like products. Optimization begins with reducing the cognitive load on developers, and removing unnecessary obligations that distract them from innovating

### The great DevOps burnout

[Link](https://devops.com/the-great-devops-burnout/)

### How to Get Developer Productivity Engineering Right, 2023, Bill Doerrfeld
[Link](https://devops.com/how-to-get-developer-productivity-engineering-right/)

> Amado Gramajo, VP, infrastructure and DevOps engineering, Nasdaq, defined DPE as > enhancing developer productivity and happiness through accelerated technologies. > And to him, the SDLC really has focused on improving the Ops side in recent > years. Now, there is a big opportunity to focus on improving the build and > testing aspects of this toolchain.
>
> To distribute common features, Nasdaq adopts a systematic approach to developer > velocity, as he called it. They haven’t created a centralized team per se but > have developed a shared service available to all developers to help them improve > velocity. This global service cuts across otherwise siloed teams, bringing them > shared requirements, like resiliency and compliance features. As areas like > Kubernetes, SRE and containerization become more normalized, said Gramajo, we > need to build the operational controls so things can be automated.
