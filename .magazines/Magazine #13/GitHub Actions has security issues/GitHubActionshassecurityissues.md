# GitHub Actions has security issues

I am fascinated with the security aspects of using GitHub Actions for my
own workloads since I have started using them. My first conference
session on this topic was at NDC London in January, 2021 [^1] and I have
been advocating on these learnings ever since. That is why I also
decided to run my usual security checks on the [entire]{.underline}
marketplace, starting with forking the actions so I can enable
Dependabot on the forked repositories.

The Marketplace shows us almost **15 thousand** actions that are
available for us to use! That means there is lots of community
engagement for creating these actions for us, but also lots of potential
for malicious actors to create actions that can be used to compromise
our systems. Do be aware that in this post I'll only be taking actions
into account that have been published to the marketplace. Since any
public repo with an **action.yml** file in the root directory can be
used inside of a workflow, there are many more actions that are
available to us that are not part of this research.

![Screenshot of the GitHub Actions Marketplace showing 14955 actions
available](./media/image1.png)


## **Analysis of the actions from the GitHub Actions Marketplace**

I created a new repo^\[2\]^ to run these checks using GitHub Actions by
scheduling a workflow that runs every hour and checks the dataset for
new actions that have not been forked to my validation organization yet.

Some caveats up front:

-   I could only load the information for 10.5 thousand actions. All the
    others have issues that makes it that I cannot find them anywhere.
    These are not included in the dataset for this analysis.

-   Some have been archived by their maintainer, but still show up in
    the Marketplace. These are of course older and have more security
    issues in them. The actions are included in this analysis. I'm
    planning to remove these when the Marketplace doesn't show them
    anymore.

-   There are some actions where I could not parse the definition file
    (if used), often because of duplicate keys in their definition file.
    I've reached out to some of the maintainers to get those fixed, but
    also want to improve my method of loading these kinds of files.
    Currently the library I use for this does not support duplicate keys
    and throws unrecoverable errors when it finds them.

I've reported this information back to GitHub and they are planning to
improve the freshness of the data in the Marketplace. Still, this is a
good two thirds of the actions that are available in the marketplace, so
this is a representable dataset to look at.\
Examples of actions that show up in the marketplace but will give an
error when you want to load the detail information for them include:

-   c-documentation-generator^\[3\]^

-   cross-commit+^\[4\]^

Additionally, all this analysis is done on the default branch for the
repository. I myself have one action, for example, that uses a
Dockerfile in the main branch, but I am working on converting it to Node
in another branch. This number should be small enough to have no
significant impact on the overall analysis.

## **Security alerts for dependencies of the Actions**

I have forked over the action repos to my own organization and enabled
Dependabot on them to get a sense of the vulnerable dependencies they
have in use. Some caveats to this analysis are:

-   Not every dependency will end up in the action itself, so a high
    alert from Dependabot will point to a 'possibly' vulnerable action.
    Since this is not something you can track automatically and see if
    this would be the case, we cannot be sure that the action itself is
    vulnerable.

-   This only works for Node based actions, which is 4.7k, so almost 50%
    of the analysed actions. Dependabot does not support Docker at the
    moment.

-   I'm only loading the vulnerable alerts back from Dependabot that
    have a severity of **High** or **Critical**.

I'm planning to add something like a Trivy container security scan to
the setup so that we get some insights from this as well.

### **Security scan results**

Of the 10488 scanned actions, 3130 of them have at least 1 high or
critical alert! This is a way higher than I even expected and very
scary! And this is only for Composite or Node based actions! If your
dependencies already are not up to date and thus have security issues in
them, how can we expect your action to be secure? That calculates to 30%
of the actions that have one or more high or critical alert on their
Dependencies.

To be complete: I have not filtered down the alerts to a specific
ecosystem. Since GitHub Actions is one of the ecosystems Dependabot
alerts on, there is a chance these alerts come from a dependency on a
vulnerable action for example, which would be unfair (since these will
not end up in the action I am checking). Since there are only 3 actions
in the GitHub Advisories Database^\[5\]^, I expect this to be of zero
significance, but still: it is worth mentioning.

### **Diving into the security results**

I've also logged the repos with more 10 (high + critical) alerts to a
separate report file and that file contains more than 600 actions!

The highest number of high alerts in one single action, is 58. Since
that repo happens to be Archived, it should not be in the actions
marketplace at all, as well as the fact that this should not be used at
all. Luckily it is only used by a small number of workflows. I'd rather
see that the runner would at least add warnings to the logs for calling
actions that are archived.

The highest number of critical alerts in one singe action is 16. This
repo is also only used by less than 10 other repos, so it is not a big
impact. Since there is no API for finding the dependents that Dependabot
finds, I cannot easily find out how many workflows are impacted by this.

I've checked some of the repos with many of alerts and found one example
that has 14 high severity alerts and 2 critical alerts. This action is
used by 34 different public repos (so in private repo usage could even
be more!). One of these dependents is a repo with 425 stars and another
has 6015 stars. That last one is producing a serverless CMS that will be
delivered as 48 different packages into the NPM ecosystem. One of those
packages sees more than a 1000 downloads a week! This is a lot of impact
for a single action that could be prevented by enabling Dependabot. Of
course, more analysis is needed for this case to see if the alerts are
actually relevant for the action. This depends on what the action does
and how it uses the dependencies.

![Image of the \'this is fine\'
meme](./media/image2.jpeg)


### **Overview**

In short, this is a top level overview of the security
results: ![Screenshot of the workflow summary, with 30% potentially
vulnerable actions of the scanned actions (which is the total of scanned
actions, not filtered to a specific
type)](./media/image3.png)
\
So for **all** action repos I could scan, 30% have at least 1
vulnerability alert with a severity of high or critical.

#### **Node based actions**

Filtering this down to only the Node action types, this becomes a lot
scarier:\
![Screenshot of the actions filtered to the Node only actions: 2752
actions potentially vulnerable, 1986 actions not
vulnerable](./media/image4.png)
\
That is 58% of the Node actions that have at least 1 vulnerability alert
with a severity of high or critical! And all demos and docs still
indicate you can just use the actions as is and only hint at the
security implications of that!

Want to learn how to improve your security stance for using actions?
Check out this guide I made: GitHub Actions Maturity Levels^\[6\]^.

## **Conclusion**

There is **a lot** of improvement that can be made to actions ecosystem.
I would like to see GitHub take a more active role in this by, for
example:

-   Enforce certain best practices before you can publish an action to
    the marketplace

-   Clean up the marketplace when an action's repo gets archived (work
    for this is underway)

-   Add a security score to the marketplace, so that users can see how
    secure an action is, run at least these type of scans on the action
    repo and report it back to the end user

-   Add a check that validates you also pushed a new release of the
    action to prevent maintainers to add Dependabot and keep their
    (vulnerable) dependencies up to date, but not actually release a new
    version of the action.

-   Add API's to not only the marketplace, but also Dependabot. This
    information should be publicly available, but currently I had to
    scrape this of the webpages.

> Of course, as maintainers of actions we are also in this together!
> It's our responsibility to make sure our actions are secure and that
> we keep them up to date. I hope this post will help you understand why
> to do that!

Footnotes:

1.  \[1\][^2]
    <https://devopsjournal.io/blog/2021/01/28/GitHub-Actions-NDC-London>

2.  <https://github.com/rajbos/actions-marketplace-checks>

3.  <https://github.com/marketplace?type=actions&query=c-documentation-generator+>

4.  <https://github.com/marketplace?type=actions&query=cross-commit+>

5.  <https://github.com/advisories?query=type%3Areviewed+ecosystem%3Aactions>

6.  <https://devopsjournal.io/blog/2021/12/11/GitHub-Actions-Maturity-Levels>

[^1]:

[^2]:
