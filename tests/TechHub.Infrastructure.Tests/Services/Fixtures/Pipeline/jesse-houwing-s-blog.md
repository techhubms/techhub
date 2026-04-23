GitHub Copilot will start training on your data... You've probably seen this on your LinkedIn timeline in the past few days. Probably with alarming notes that this can't be controlled and that there is no policy for it.

But this simply isn't true.

 

 

 
- 
 [
 *
 ](https://jessehouwing.net/author/jesse-houwing/)
 
 

 

02 Apr 2026
 • 2 min read

 

 
 ![GitHub Copilot will start training on your interactions](https://storage.ghost.io/c/13/29/1329ef25-4e8e-4ebc-be08-4f1135b51be8/content/images/size/w2000/2026/04/image-1-.png)
 The new Allow GitHub to use my data for AI model training policy
 

 

 
 

GitHub Copilot will start training on your data... You've probably seen this on your LinkedIn timeline in the past few days. Probably with alarming notes that this can't be controlled and that there is no policy for it*.

**But this simply isn't true.**

## So, let's get some things straight.

GitHub Copilot will enable training on data it receives through **GitHub Copilot Free, Pro and Pro+ subscriptions** unless you opt out.

And GitHub won't suddenly start training its models on the contents of your private repos or your locally stored projects in the background. But it will use the data shared with GitHub Copilot through interactions to train its systems.

[

Updates to GitHub Copilot interaction data usage policy

From April 24 onward, interaction data from Copilot Free, Pro, and Pro+ users will be used to train and improve our AI models unless they opt out.

*The GitHub BlogMario Rodriguez

![](https://storage.ghost.io/c/13/29/1329ef25-4e8e-4ebc-be08-4f1135b51be8/content/images/thumbnail/generic-github-logo-right.png)

](https://github.blog/news-insights/company-news/updates-to-github-copilot-interaction-data-usage-policy/?ref=jessehouwing.net)

**It will not enable this for Copilot for Business or Copilot Enterprise licenses.**

And there are controls in place to prevent your *users from using the non-commercial tiers of GitHub Copilot. Here are a few:

- If your company assigns a Copilot for Business or Copilot Enterprise license to a GitHub user, this will override the individual user's settings.
- You can [centrally block access to free, pro and pro+ licenses though network controls such as DNS filtering or Proxy filtering or PiHole](https://docs.github.com/en/enterprise-cloud@latest/copilot/how-tos/administer-copilot/manage-for-enterprise/manage-access/manage-network-access?ref=jessehouwing.net).
- You [can block access to free, pro and pro+ licenses through adding 1 line to a machine's hosts file](https://docs.github.com/en/enterprise-cloud@latest/copilot/how-tos/administer-copilot/manage-for-enterprise/manage-access/manage-network-access?ref=jessehouwing.net).
- You can [use Windows Group Policy to control which Copilot tiers are available in Visual Studio](https://learn.microsoft.com/en-us/visualstudio/ide/visual-studio-github-copilot-admin?view=visualstudio&ref=jessehouwing.net#disable-copilot-skus).
- Companies using GitHub Enterprise with Managed Users (EMU) can [inject a special HTTP header to prevent people from using a non-enterprise GitHub account](https://docs.github.com/en/enterprise-cloud@latest/admin/configuring-settings/hardening-security-for-your-enterprise/restricting-access-to-githubcom-using-a-corporate-proxy?ref=jessehouwing.net).
- If you're on GitHub Enterprise with Managed Users with Data Residency you can block access to the GitHub.com logon page on a proxy.

Luckily GitHub Copilot has many of these controls, many more than many of its competitors. And while assigning a license to these users will cost you money, this trade-off isn't new. You're hopefully not running your company on the free tier of Gmail either or allowing your engineers to send sensitive documents through their personal email accounts.

And if you're worried about GitHub Copilot stealing your data, I hope you are protected against personal cloud drive backups (onedrive, dropbox), supply chain attacks against npm, pipy and other package management tools used by your engineers, malicious Visual Studio Code or browser extensions or your users simply bringing any of the many other AI tools available out there in the market, many without any policy controls.

It's time to take control of your development environments. Allowing contractors and new hires to simply start working on your source code on unmanaged devices just isn't an option anymore.

Want to learn how to put some controls on your environments? Ping me!