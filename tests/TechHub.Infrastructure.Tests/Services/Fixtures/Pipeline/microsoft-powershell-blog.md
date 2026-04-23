We recently released PowerShell 7.6, and we want to take a moment to share context on the delayed
timing of this release, what we learned, and what we’re already changing as a result.

PowerShell releases typically align closely with the .NET release schedule. Our goal is to provide
predictable and timely releases for our users. For 7.6, we planned to release earlier in the cycle,
but ultimately shipped in March 2026.

## What goes into a PowerShell release

Building and testing a PowerShell release is a complex process with many moving parts:

- 3 to 4 release versions of PowerShell each month (e.g. 7.4.14, 7.5.5, 7.6.0)

- 29 packages in 8 package formats

- 4 architectures (x64, Arm64, x86, Arm32)

- 8 operating systems (multiple versions each)

- Published to 4 repositories (GitHub, PMC, winget, Microsoft Store) plus a PR to the .NET SDK image

- 287,855 total tests run across all platforms and packages per release

## What happened

The PowerShell 7.6 release was delayed beyond its original target and ultimately shipped in March
2026.

During the release cycle, we encountered a set of issues that affected packaging, validation, and
release coordination. These issues emerged late in the cycle and reduced our ability to validate
changes and maintain release cadence.

Combined with the standard December release pause, these factors extended the overall release
timeline.

## Timeline

- **October 2025** – Packaging-related changes were introduced as part of ongoing work for the 7.6
release.

Changes to the build created a bug in 7.6-preview.5 that caused the Alpine package to fail. The
method used in the new build system to build the Microsoft.PowerShell.Native library wasn’t
compatible with Alpine. This required additional changes for the Alpine build.

- **November 2025** – Additional compliance requirements were imposed requiring changes to packaging
tooling for non-Windows platforms.

Because of the additional work created by these requirements, we weren’t able to ship the fixes
made in October until December.

- **December 2025** – We shipped 7.6-preview.6, but due to the holidays there were complications
caused by a change freeze and limited availability of key personnel.

We weren’t able to publish to PMC during our holiday freeze window.

- We couldn’t publish NuGet packages because the current manual process limits who can perform the
task.

- **January 2026** – Packaging changes required deeper rework than initially expected and validation
issues began surfacing across platforms.

We also discovered a compatibility issue in RHEL 8. The libpsl-native library must be built to
support glibc 2.28 rather than glibc 2.33 used by RHEL 9 and higher.

- **February 2026** – Ongoing fixes, validation, and backporting of packaging changes across release
branches continued.

- **March 2026** – Packaging changes stabilized, validation completed, and PowerShell 7.6 was
released.

## What went wrong and why

Several factors contributed to the delay beyond the initial packaging change.

- **Late-cycle packaging system changes**
A compliance requirement required us to replace the tooling used to generate non-Windows packages (RPM, DEB, PKG). We evaluated whether this could be addressed with incremental changes, but determined that the existing tooling could not be adapted to meet requirements. This required a
full replacement of the packaging workflow.Because this change occurred late in the release cycle, we had limited time to validate the new system across all supported platforms and architectures.

- **Tight coupling to packaging dependencies**
Our release pipeline relied on this tooling as a critical dependency. When it became unavailable, we did not have an alternate implementation ready. This forced us to create a replacement for a core part of the release pipeline, from scratch, under time pressure, increasing both risk and complexity.

- **Reduced validation signal from previews**
Our preview cadence slowed during this period, which reduced opportunities to validate changes incrementally. As a result, issues introduced by the packaging changes were discovered later in the cycle, when changes were more expensive to correct.

- **Branching and backport complexity**
Because of new compliance requirements, changes needed to be backported and validated across multiple active branches. This increased the coordination overhead and extended the time required to reach a stable state.

- **Release ownership and coordination gaps**
Release ownership was not explicitly defined, particularly during maintainer handoffs. This made it difficult to track progress, assign responsibility for blockers, and make timely decisions during critical phases of the release.

- **Lack of early risk signals**
We did not have clear signals indicating that the release timeline was at risk. Without structured tracking of release health and ownership, issues accumulated without triggering early escalation or communication.

## How we responded

As the scope of the issue became clear, we shifted from attempting incremental fixes to stabilizing
the packaging system as a prerequisite for release.

- We evaluated patching the existing packaging workflow versus replacing it, and determined a full
replacement was required to meet compliance requirements.

- We rebuilt the packaging workflows for non-Windows platforms, including RPM, DEB, and PKG formats.

- We validated the new packaging system across all supported architectures and operating systems to
ensure correctness and consistency.

- We backported the updated packaging logic across active release branches to maintain alignment
between versions.

- We coordinated across maintainers to prioritize stabilization work over continuing release
progression with incomplete validation.

This shift ensured a stable and compliant release, but extended the overall timeline as we
prioritized correctness and cross-platform consistency over release speed.

## Detection gap

A key gap during this release cycle was the lack of early signals indicating that the packaging
changes would significantly impact the release timeline.

Reduced preview cadence and late-cycle changes limited our ability to detect issues early.
Additionally, the absence of clear release ownership and structured tracking made it more difficult
to identify and communicate risk as it developed.

## What we are doing to improve

This experience highlighted several areas where we can improve how we deliver releases. We’ve
already begun implementing changes:

- **Clear release ownership**
We have established explicit ownership for each release, with clear responsibility and transfer mechanisms between maintainers.

- **Improved release tracking**
We are using internal tracking systems to make release status and blockers more visible across the team.

- **Consistent preview cadence**
We are reinforcing a regular preview schedule to surface issues earlier in the cycle.

- **Reduced packaging complexity**
We are working to simplify and consolidate packaging systems to make future updates more predictable.

- **Improved automation**
We are exploring additional automation to reduce manual steps and improve reliability in the face of changing requirements.

- **Better communication signals**
We are identifying clearer signals in the release process to notify the community earlier when timelines are at risk. Going forward, we will share updates through the PowerShell repository [discussions](https://github.com/PowerShell/PowerShell/discussions).

## Moving forward

We understand that many of you rely on PowerShell releases to align with your own planning and
validation cycles. Improving release predictability and transparency is a priority for the team, and
these changes are already in progress.

We appreciate the feedback and patience we received from the community as we worked through these
changes, and we’re committed to continuing to improve how we deliver PowerShell.

— The PowerShell Team

 
 

Category

Topics

## Author

![Jason Helmick](https://devblogs.microsoft.com/powershell/wp-content/uploads/sites/30/2020/11/JasonHelmickDrawingPhoto-150x150.jpg)

SR. PRODUCT MANAGER

Nice to meet you! I’m a Product Manager on the PowerShell team at Microsoft. My focus is on all things PowerShell including Predictive IntelliSense, Crescendo, DSC and PlatyPS. One favorite pastime is working with the rapidly growing PowerShell community.