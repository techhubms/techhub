# InnerSource

```
Is InnerSource written weird?
You will find a few related hits in Google if you search for "Inner Source", but back in the early days, no related hits where found. So, it was decided to call it InnerSource to get better reach!
```

## **What is InnerSource?**

InnerSource can be defined as the application of open-source software development principles within an organization's internal software development processes. It draws on the valuable lessons learned from open-source projects and adapts them to the context of how companies create software internally.

Similar to the familiarity of "Open Source", InnerSource encourages collaboration within the confines of an organization. It entails leveraging publicly available software, often used by developers in their daily work, and allows for feedback, including requests for new features, bug fixes, and changes, fostering collaboration akin to open-source projects. 

InnerSource operates on four core principles, briefly summarized here, with more details available at [InnerSource Commons](https://innersourcecommons.org). InnerSource Commons is a community-driven organization that aims to promote and facilitate the adoption of InnerSource practices to improve software development within organizations. It provides a platform for knowledge sharing, collaboration, and the development of valuable resources for the InnerSource community.

1. **Openness:** Openness in InnerSource projects ensures accessibility and simplifies contributions. It involves well-documented projects, making it easy for anyone within the organization to discover, understand, and participate. Host team contact information is readily accessible, and intentions to accept InnerSource contributions are communicated through relevant channels, promoting successful collaboration. 

2. **Transparency:** Transparency is fundamental for effective InnerSource collaboration. Host teams must provide clear insights into the project's direction, requirements, progress, and decision-making processes. Communication should be detailed and accessible to individuals beyond the core team, facilitating contributions from guest teams.

3. **Prioritized Mentorship:** InnerSource relies on mentorship from host teams to guest teams, guided by trusted committers. This mentorship elevates contributors on guest teams, enabling them to engage with and modify host team projects effectively. Host teams should prioritize mentorship, assisting guest team contributors when needed, and fostering beneficial relationships within the organization.

4. **Voluntary Code Contribution:** InnerSource thrives on voluntary participation, where guest and host teams engage willingly. Guest teams contribute code to host teams and accept these contributions voluntarily. This voluntary approach ensures alignment with each team's objectives, allowing host teams to accept contributions that align with their mission and guest teams to prioritize contributions that serve their goals. Full collaboration extends to code contributions to maximize InnerSource's benefits.

## **Why InnerSource and the Problems It Solves**

When adopting InnerSource within your organization, defining your goals and understanding what problems it can address is essential. Clarity in your objectives helps people relate to and engage with InnerSource effectively.

Are you aiming to improve Developer Velocity, as measured by the Developer Velocity Index (DVI) (1), which correlates with faster revenue growth, higher shareholder returns, increased innovation, and improved customer satisfaction? Alternatively, are you fostering a collaboration mindset, emphasizing knowledge sharing and collaboration? Perhaps your focus is on breaking down traditional boundaries through DevOps practices. Identifying your true north star for InnerSource enables you to tailor its implementation to address specific challenges and objectives within your organization.

## **Benefits of InnerSource**

The advantages of adopting InnerSource are substantial and include:

**1. Mitigating Inter-Team Dependencies:** When teams operate in isolation, working solely on their individual "projects" or "repositories" without sharing their work, it often leads to code duplication across multiple areas. This results in wasted effort as people tackle the same problems independently and can also introduce subtle variations in behavior for identical solutions. InnerSource promotes knowledge sharing and collaboration on shared solutions, significantly reducing code redundancy and enhancing efficiency.

**2. Resolving Dependencies Effectively:** In larger organizations, there's typically a constant struggle for resource allocation and prioritization. This often leads to battles outside of the team's immediate focus. InnerSource helps by providing teams with visibility into available software resources and contacts within the organization. This transparency enables teams to collaborate on improving code or adding new features, all with the approval of the original owners, without waiting for prioritization decisions. While it requires some initial coordination, it is often more time-efficient than waiting for prioritization decisions.

## **Interaction Model**

As we are building communities around projects you can clearly see communication is key. In this asynchronous world, let alone timezone differences and cross-organization collaboration, it is obvious that you need to set up your guidance in a clear and easy-to-find way. GitHub provides a comprehensive set of documented principles and practices to assist you in getting started with community collaboration. These resources cover various aspects, from establishing code of conduct guidelines and creating community profiles to utilizing pull request templates. Additionally, GitHub offers a range of communication tools to support effective collaboration within your community. You can access these valuable resources at https://docs.github.com/en/communities.

![](./images/role_overview.png)

1. **Product Team:** The original product team plays a pivotal role in the development and upkeep of the core project. They are the primary decision-makers, determining which contributions to accept or reject. Additionally, they provide valuable guidance and mentorship to external contributors, ensuring the project's alignment with its goals and maintaining its overall quality.

2. **Product Owner:** The product owner defines the project's overarching vision, goals, and priorities. Collaborating closely with the original product team ensures that contributions harmonize with the project's objectives. Often, they prioritize specific features or enhancements based on user needs and market demands.

3. **Trusted Committers:** Trusted committers are individuals or team members who understand the project and have earned the community's trust. Their primary role involves reviewing and approving contributions from external contributors. Beyond this, they are crucial in mentoring and guiding contributors, ensuring the project's ongoing quality and consistency.

4. **Contributors:** Contributors are external individuals or teams that aim to make valuable contributions to the project. They actively submit code, bug fixes, or new features for review and integration into the project. Seeking feedback and collaboration within the project's community, contributors drive the project's evolution and improvement.

5. **Consumers:** Consumers, which include end-users and stakeholders, are the beneficiaries of the project's functionality. They utilize the project or product created through the collective efforts of the original product team, external contributors, and trusted committers. By leveraging these contributions, consumers meet their needs, provide usability feedback, and enjoy ongoing enhancements.

## InnerSource Patterns
The InnerSource Patterns are a valuable resource that offers actionable insights and best practices for implementing InnerSource principles within an organization's software development processes. These patterns serve as a roadmap to facilitate effective collaboration, knowledge sharing, and project contributions, mirroring the successful dynamics of open source communities. By harnessing these patterns, organizations can streamline their development workflows, cultivate a culture of transparency, and drive innovation through collective efforts. Each pattern provides a structured approach to address specific challenges, making the adoption of InnerSource a well-guided and efficient endeavor. You can explore these patterns in detail at [InnerSource Commons Patterns](https://patterns.innersourcecommons.org/).

One particularly noteworthy pattern that stands out in revolutionizing workplaces through InnerSource is the "Gig Marketplace" Pattern.

### Gig Marketplace Pattern
The "Gig Marketplace" pattern is dedicated to dismantling organizational silos by establishing an internal marketplace for tasks or projects. This innovative approach empowers teams to collaborate with flexibility and efficiency, offering and requesting expertise or services across different departments. This pattern encourages the free flow of skills and resources, enabling teams to tackle challenges and complete projects swiftly while nurturing a culture of collaboration and knowledge exchange.

## **Areas to apply InnerSource**

### **Cloud Infrastructure**

The landscape of cloud architecture is evolving and growing increasingly intricate. Notably, many companies are witnessing the emergence of Cloud Centers of Excellence (CCoE). These entities primarily shoulder the responsibility of managing shared infrastructure within cloud environments. Beyond infrastructure management, they are vital in monitoring security and ensuring its continual upkeep. Within CCoEs, teams specializing in these tasks are commonly called platform teams.

Modern cloud infrastructures often follow the hub and spoke model, exemplified by Microsoft's Cloud Adoption Framework (CAF). In this model, the hub represents the centralized component responsible for monitoring and regulating both inbound and outbound traffic. Conversely, spokes represent isolated workloads where teams can execute their software or applications. These spokes are intricately linked to the hub. Typically, it falls upon the workload teams to create and manage the specific infrastructure they require.

To ensure that workload teams adhere to compliance standards, the platform team equips them with essential building blocks for infrastructure creation. These building blocks are available to all teams needing infrastructure resources, including the platform team. Building blocks are often constructed using tools like Bicep or Terraform, both of which support the creation of modules that can be hosted in repositories such as Azure Container Registry or Terraform Cloud.

Crucially, when the source code of these building blocks is accessible to all teams, any team member can contribute changes or updates. However, for quality control and to ensure ongoing compliance, all alterations to the building blocks require approval from the Platform team. This mechanism ensures that the building blocks continue to meet the necessary standards. In the context of InnerSource, the platform team serves as the trusted committer, overseeing these collaborative contributions.

### **Utilizing Packages**

In the contemporary landscape of software development, packages have become indispensable. Both frontend and backend applications heavily rely on these packages. Many of these packages are open-source and meticulously maintained by passionate individuals. They find their hosting platforms in package managers like NuGet and NPM, with GitHub as one of the most prominent platforms for hosting these open-source packages.

GitHub's foundation rests on principles of developer experience and open-source collaboration. This orientation means the source code for numerous packages is accessible to anyone, allowing for contributions from a wide community of developers. Changes to these packages undergo review and approval processes, typically overseen by package maintainers—dedicated groups of individuals who consistently contribute to the package's development and upkeep.

In addition to open-source packages, organizations also rely on company-specific packages. These packages often encompass specialized functionalities, such as authentication or logging methods. Rather than each team independently reinventing these functionalities, organizations follow a similar principle: creating packages that can be shared across multiple teams. These packages are available through platforms like Azure DevOps Artifacts or GitHub Packages.

When multiple teams within an organization use these shared packages, it becomes essential that they have the flexibility to make adjustments and improvements as needed. Embracing the same open-source principles that govern external packages, these organizations naturally foster a community of regular contributors. Within this community, individuals emerge as trusted committers responsible for reviewing and approving changes to these vital shared packages, ensuring they remain robust and aligned with organizational needs.

### **Applications**

Like infrastructure and open-source packages, application developers can also adopt an open-source approach. Open-source applications often serve as alternatives to well known applications, for example Photoshop and The Gimp. Some companies even choose to open-source the tools they use, making them accessible to all. By doing so, they harness the collective power of the community to enhance these applications. The same principles that apply to open-source packages are extended to open-source applications, allowing anyone to contribute new features or fix bugs. Trusted committers play a pivotal role in reviewing and approving these changes.

Now, imagine if these open-source principles, championed by passionate individuals, were applied to company software. Picture making the applications within a company available for everyone, enabling all employees to contribute to the company's software.

This approach fosters collaboration among teams and departments, effectively breaking down silos and encouraging knowledge sharing. It's a recipe for innovative solutions as a broader set of eyes scrutinizes the codebase, potentially catching bugs, security vulnerabilities, or design flaws at an early stage. InnerSource encourages developers to share their expertise and best practices, elevating the overall skill level of your team and mitigating the risk of knowledge loss when employees depart.

Distributing knowledge and responsibility makes the organization less susceptible to key-person dependencies. Others can readily step in to maintain and enhance the code if a developer leaves. Promoting InnerSource cultivates a culture of openness and transparency, resonating throughout the organization and enhancing company culture and employee morale.

## **Summary**

This article explores InnerSource, a practice that brings open-source principles to internal software development within organizations. InnerSource encourages collaboration and feedback while maintaining security boundaries. It operates on four key principles: Openness, Transparency, Prioritized Mentorship, and Voluntary Code Contribution. These principles address organizational challenges such as improving Developer Velocity and fostering collaboration.

The benefits of InnerSource include reducing inter-team dependencies and resolving resource allocation challenges in larger organizations. It promotes collaboration, knowledge sharing, and efficiency. The article also outlines a role-based interaction model involving the Original Product Team, Product Owner, Trusted Committers, Contributors, and Consumers, all working together to develop and maintain projects. 

Embracing InnerSource helps you build an inclusive organization, where people are able to showcase their expertise and offers a modern approach to work that aligns with the preferences and values of younger generations. It promotes flexibility, cross-functional collaboration, knowledge sharing, and inclusivity, all of which can enhance job satisfaction, innovation, and organizational agility.

(1): https://www.mckinsey.com/capabilities/mckinsey-digital/our-insights/tech-forward/why-your-it-organization-should-prioritize-developer-experience
