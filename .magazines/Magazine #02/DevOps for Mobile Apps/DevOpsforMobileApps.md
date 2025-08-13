**DevOps for Mobile Apps**

By Sam Guckenheimer, Microsoft

Three technology trends have dominated this decade. First, applications
have become connected across mobile, cloud services, and sensors, to the
point where each part depends on connectivity to deliver a complete
experience. Second, DevOps practices and tools and tools have
revolutionized the pace and quality of service delivery. Third, mobile
devices have become the primary means of consumer and employee access to
these connected systems, but their practices and tools have lagged
behind impediments imposed by distribution and diversity.

According to ComScore, mobile became to majority source of web traffic
in mid 2014.[^1] And Gartner believes that 60% of all business processes
will be optimized for mobile by 2020.[^2] This indicates a permanent
shift in technology usage and a permanent change in the way software
applications are being built.

*DevOps* evolved for web applications and defined a New Normal for
software teams. Lean management and continuous delivery practices
created the conditions for delivering value faster, sustainably. For
example, high-performing IT organizations report 30x more frequent
deployments with 200x shorter lead times, 60x fewer failures and 168x
faster recoveries.[^3]

Unfortunately, these deployment pipelines are tuned for centrally
administered servers. When servers directly push data directly to the
web, it is relatively easy to monitor both the quality of service and
customer behavior. Errors can be corrected, performance can be improved,
and experiences can be enhanced with new server deployments alone.

Unfortunately, deployment to mobile devices is typically gated behind
app stores, managed by the device vendors. This creates the need for a
dual release pipeline: one path for the service side and one for the
mobile devices.

What is needed to make DevOps for mobile applications effective? The
same kind of practices that apply for services apply for devices, tuned
to the differences in technology:

1.  *A release pipeline.* Continuous integration should produce mobile
    packages for the appropriate operating systems (e.g. iOS and
    Android) and versions and launch automated testing of standard
    configurations and scenarios as part of a continuous deployment
    pipeline. Integration with device clouds can allow automation of
    testing across multiple form factors as part of the standard release
    process.

2.  *Distribution of "beta" releases.* Especially for consumer apps,
    where star ratings are so important, beta releases need to get
    thorough exploratory testing by cohorts of real users. This requires
    the ability to have users directly load the beta versions or to
    target cohorts with packages that have not had to go through the
    official stores. This needs to be an iterative process that allows
    as many redistributions as needed.

3.  *Test coverage* should be tracked through all these phases, not just
    against code, but also against all the device, OS, carrier, network
    and language configurations. For Android, with thousands of devices,
    this matrix expands to millions of configurations.

4.  *Remediation.* When a user experiences a crash \[or a performance
    bottleneck\], you need to learn of this instantly. Your
    application's crashes need to come back to you as stack traces,
    matched to the right version of source code, with symbols and
    environment details, so that you can fix them unambiguously.

5.  *Whole lifecycle*. Any live error that the application generates
    should be tracked as a bug so that you can tell whether you have
    remediated it and create a regression test with the code to prevent
    its recurrence.

6.  *Feedback*. Experimentation is the essential juice of a modern
    lifecycle, as you always make your apps better for users. You want
    to be sure that your cohorts of testers can give you rich feedback
    on their experiences so that you can make the apps better.

In a DevOps world, these practices are taken for granted, but they have
been very hard to implement for mobile. This has motivated Microsoft to
introduce HockeyApp to extend DevOps to mobile apps too. Together with
Visual Studio Team Services and the developer's IDE, it builds the
foundation for the Mobile DevOps cycle.

![/Users/thomi/Desktop/Screenshots/Flowchart.png](./media/image1.png)


Gene Kim has suggested that DevOps goes through a progression of the
"Three Ways".[^4] The first is the automation of the release pipeline to
allow the continuous flow from Development to Operations, or in this
case, Mobile Deployment, as that is the initial bottleneck. The second
is the feedback from Deployment to Development, to allow Development to
become increasingly aware and responsive of operation al issues. The
third is the continual amplification of the feedback loops, so that
improvements can flow faster and faster. HockeyApp is one tool that
seeks to close these gaps in practice from the server-side to mobile
development, so that your team can apply the best learnings of DevOps to
both halves of the application and both parts of the release pipeline.

[^1]: *http://www.comscore.com/Insights/Blog/Major-Mobile-Milestones-in-May-Apps-Now-Drive-Half-of-All-Time-Spent-on-Digital*

[^2]: Gartner AADI Keynote, December 2015

[^3]: 2015 State of DevOps Report

[^4]: Gene Kim, Kevin Behr and George Spafford, *The Phoenix Project*
    (Portland: IT Revotion Press, 2013)
