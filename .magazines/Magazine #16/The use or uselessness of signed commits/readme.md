# The use or uselessness of signed commits
![Fancy signatures are nothing new](./header.jpeg)

> Image Source: https://commons.wikimedia.org/wiki/Category:Miko%C5%82aj_Radziwi%C5%82%C5%82_the_Black

Each commit you make stores the name and email address you've configured in your git config. But Git doesn't verify whether that's you. Even you can easily craft a commit that uses the name and email address of any famous coder out there in the world, and your Git repo will accept that.

This is why it's possible to sign a commit. Signing a commit adds cryptographic proof of your identity using a public/private key verification. [Commits can be signed using GPG, SSH or S/MIME from your workstation](https://docs.github.com/en/authentication/managing-commit-signature-verification/signing-commits) and GitHub can sign commits you make on the web "on your behalf" (it basically signs it with GitHub's own key). 
I'll use GitHub as an example hosting platform for your git repositories, but conceptually this applies to Git in general and all other Git hosting platforms.
To verify your identity on GitHub, you need to upload your keys, so they can verify you are who you are. If done correctly, you'll see a small badge <img src="https://github.com/XpiritBV/Xprt-Magazine/blob/main/Magazine%20%2316/The%20use%20or%20uselessness%20of%20signed%20commits/verified-badge.png?raw=true" /> / ${\textsf{\color{green}(verified)}}$ next to a commit that is signed this way.

![Example of the verified badge](./verified.png)

The cryptographic signature is part of the commit, so when a repository is transferred from one machine to another or one hosting provider to another, you should still be able to verify the repo is "intact" if you have a copy of all the public keys of the contributors. This is why commit-signing works agnostic to the repository host. *This is also why it's so hard to do it right.*

"Great!" You might say. And many experts will confirm this on the web, in conference talks and books, including a few of my own colleagues. But if you read through these blogs, you'll see immediately that it isn't simple to set up. 

> ## Signing Git Commits Using YubiKey on Windows
> There are several things we need to do in order to achieve end-to-end security in our release…
>
> https://scatteredcode.net/signing-git-commits-using-yubikey-on-windows/

> ## How to setup Signed Git Commits with a YubiKey NEO and GPG and Keybase on Windows
> This week in obscure blog titles, I bring you the nightmare that is setting up…
>
> https://www.hanselman.com/blog/how-to-setup-signed-git-commits-with-a-yubikey-neo-and-gpg-and-keybase-on-windows

> ## Elevate Your Git Security: Signing GitHub Commits with 1Password in Windows WSL and Containers
> This is a step by step guide on how to setup 1Password SSH agent to provide you with ssh authentication and signing keys to enable a smooth git workflow in WSL and VSCode DevContainers.
>
> https://writeabout.net/2023/08/10/elevate-your-git-security-signing-github-commits-with-1password-in-windows-wsl-and-containers/

And, yes, it's great... In a way. And it's also not great. Let me explain.

## What does the ${\textsf{\color{green}(verified)}}$ badge mean?

A commit that shows the ${\textsf{\color{green}(verified)}}$ badge will tell you the following:

 - ✅ The commit was signed
 - ✅ The signature is correct
 - ✅ The signature matches the email used in the commit
 - ✅ The key used to sign is known by github
 - ✅ They key and email are owned by the account that's shown (in this case octocat)
 - ✅ Whether the contents of the commit (still) match the signature
 - ✅ Potentially: That the key is owned/derived/trusted by an organization or corporation.
 - ✅ The commit was likely made by the person you think made the commit

Great!

What does the ${\textsf{\color{green}(verified)}}$ badge not mean?

A commit that shows the ${\textsf{\color{green}(verified)}}$ badge doesn't tell you the following:

 - ⛔ Whether the signer inspected and approves the content of the commit
 - ⛔ Whether the contents of the commit were altered between staging and committing
 - ⛔ Whether the signer made the commit in person (or had automation sign on their behalf)
 - ⛔ Whether the account is compromised or not
 - ⛔ Whether the account owner has securely stored their private key
 - ⛔ Whether the account is actually the account you were expecting. This may be difficult for you to distinguish. Is it Octocat or 0ctocat?
 - ⛔ Whether the account is stil owned by the same person last time you encountered their profile (it might have been repo-jacked)

In essence: Whether the commit was actually made by the person you think made the commit

So, the ${\textsf{\color{green}(verified)}}$ badge isn't a seal of approval, nor a universal signal of trust.

## What does the ${\textsf{\color{orange}(unverified)}}$ badge mean?

That leaves us with the ${\textsf{\color{orange}(unverified)}}$ badge. Which might be the most useful signal that commit signing may give. it signals that:

 - ✅ The commit was signed with a key that doesn't belong to the account that made the commit.
 - ✅ The commit was signed with a key that doesn't match a known email for the account that made the commit
 - ✅ The commit wasn't signed and the account has enabled vigilant mode
 - ✅ *You cannot trust the commit was made by the person you think made the commit.*

## How can this all go wrong

### Generating and storing your keys
To perform commit signing correctly, users must take several steps.

 1. They need to acquire a keypair. 
    - Most people will generate a ssh key or a GPG key on their box.
    - Some companies will generate a key for you and hand it over to you.
 2. You need to protect the private keys in some way
    - Encrypt them using a passphrase
    - Protect them with a 2FA token (like a YubiKey)
    - Set the file system permissions
    - Store them in a software key-vault (for example: windows credential manager or 1password)
    - Or put them on a smartcard
    - Back up the key somewhere safe
    - Delete the unprotected key thoroughly
 3. You need to put the private key on every device, VM, container, user profile you want to use to sign commits. (Or generate a new key for each device, but then, how will people know they should trust your keys?).
 4. You need to upload your keys to GitHub and other platforms where you want your identity to be known.
 5. You need to make sure your keys are "locked" most of the time to prevent malware on your machine to intercept the key during development.

If you search for instructions to start signing your commits, you'll find [the generic instructions to generate the key and to setup git to use the key](https://docs.github.com/en/authentication/managing-commit-signature-verification/generating-a-new-gpg-key), but these generally won't tell you how to then store the private key securely and how to ensure it is locked most of the time.

I've seen a lot of people that have stored their key material in their user's home directory, without a passphrase. I've made that mistake myself at least once during my career.

Recently [vendors like 1Password have taken notice of how hard it is to actually do this right, and they're now offering features to handle most of the key generation and secure storage](https://blog.1password.com/git-commit-signing/) for you.
Which is great, they're making commit signing easy. Like how Let's Encrypt made it easy for everyone to secure their website.

Of course, key generation and distribution could be handled by your IT department. And they might hand you 2 Yubikeys on your first day at work. Unfortunately, for the vast majority of maintainers and contributors there is no IT department to take care of all the nitty gritty details.
Or if you're contributing to the Linux kernel and submit your patch over email using a signed patch file, it's the only way to have your work integrated into the repository:

> ## Submitting patches: the essential guide to getting your code into the kernel — The Linux Kernel documentation
>
> https://www.kernel.org/doc/html/latest/process/submitting-patches.html


### Verifying the identity of a commit author

It still doesn't solve the issue of distributing your keys to your peers. Your commit may show ${\textsf{\color{green}(verified)}}$ on github, but you won't be able to verify the signature of a commit on your local machine, build agent or int he repo of a different vendor, like GitLab. Unlike the browsers, which are preloaded with trusted root certificates, the git client and hosts don't have a built-in system to share public keys.

So, to provide true protection, not just on GitHub, you'll need:

 - A way to distribute your public key.
 - Be able to revoke a key
 - Be able to rotate a key
 - A way to receive the keys of the people you work with
I
n a corporate environment, your IT or security department may be able to set this up for you, but for most contributors in the world, this is something they have to do themselves. And it is something many have no clue how to do well.
What makes this even harder is that identity verification is hard. Many people use an email address from gmail.com or outlook.com, so it would be trivial to create an email address that's similar, but different. For example, I own: `jesse.houwing@gmail.com`, but nothing blocks you from creating `jhouwing@gmail.com` or `jesse.h0uwing@gmail.com` or `jesse.houw1ng@gmail.com`, generating a key for that and registering it with github. GitHub will show ${\textsf{\color{green}(verified)}}$ in the commit and it's up to you to inspect the identity of the underlying user.

The same is true for GitHub profiles. It's not hard to create a new account, mirror a bunch or repos and then create a pull request that will look authentic at first glance. Another issue is the fact that when a GitHub account is renamed or deleted, it will become available to new users after 90 days. So `octocat` from last year's excellent pull request may be someone completely different than `octocat` today. Since the new owner of the account can add new key material, any commit they make will show as ${\textsf{\color{green}(verified)}}$. Not every repo host currently prevents you from creating highly similar usernames or warns their users a highly similar username was created. [GitHub even suggests to slightly change your desired username if the account you want is already taken, and won't prevent you from creating an account that looks a lot like an existing account](https://docs.github.com/en/site-policy/other-site-policies/github-username-policy#what-if-the-username-i-want-is-already-taken).

And then there is one another issue: *privacy*. GitHub and other platforms allow users to make their true email address private. Instead github will use an address in the following format `ID+USERNAME@users.noreply.github.com` for your commits. Users opting into this privacy feature will be even harder to verify, as you won't be able to see their true email address.

### Validating the contents of a commit
Even if the commit was signed by the correct person, there is no guarantee the contents of the commit were written by them.
When you want to add new code to your repository you'd go through a few steps:

```
# change a set of files
echo "// small change" >> thefile.cs

# stage the changes
git add thefile.cs

# commit & sign
git commit -m "committing my stuff" -s

# push the commit to the remote
git push origin main
```

The thing is, malware could inject itself between any of these steps. So, a diligent contributor would run git status and do a diff to verify the contents of the staging area and the results of the commit prior to pushing. A slightly more paranoid maintainer will have to type their SSH passphrase to initiate the push. But let's be honest: **most of us don't**.

A pre-commit hook or a git filter or an executable running in the background could change the contents of the file between git add and git commit and most contributors would be none the wiser. Or it might intercept your passphrase for your private key and exfiltrate it from your machine.
But probably even simpler, a malicious npm package may have slipped onto your machine and has nestled itself into the package-lock.json, for which most diffs are hidden by default anyway:

![Contents of generated and large files is hidden in diff view by default.](./package-lock.png)

Hopefully any of these changes to the code are detected during the pull request or by some tool that runs a pull request check. It wouldn't be the first time such changes slip by undetected.
Unfortunately, very few IDEs and Git clients will show you the contents you are about to sign or the contents you have just signed. And even if they did, if you're committing a large change to a file, you are unlikely to notice any code that was injected or appended indented 600 tabs to the right.

And if a contributor hasn't properly protected their key material, it would be even more trivial for an attacker to take the key and send it somewhere to use it at any later point in time, without you ever noticing.

Even if a user has properly set up a passphrase or 2-factor authentication, they may just leave their YubiKey in their usb port and tap it whenever prompted. Unless a user knows exactly when to expect a prompt, they may just enter their passphrase into every prompt that asks for it. 

*A lot of the "secure" aspect of commit-signing depends on the contributor being fully aware of how many "obscure" utilities work, and when it's safe to enter their password in a prompt. I suspect that it would be much safer if GitHub would prompt the user through GitHub Mobile when they receive a push, than expecting a user to know which prompts to trust.*

## The ${\textsf{\color{green}(verified)}}$ badge is like the 🛡️ we removed from the browser bar.
Which brings me to the reason I wrote this article. Currently, most contributors and maintainers that have properly set up their signing infrastructure are power users who (hopefully) know how to do this well. But pushing the general population to set up commit-signing will only degrade the trust I have in the${\textsf{\color{green}(verified)}}$ badge. I currently do not believe that the whole population of GitHub users will be able to set up their environment correctly or will know how to protect their private keys.
In its own way, the${\textsf{\color{green}(verified)}}$ badge is a lot like the 🛡️ shield we used to show in the URL bar of the browser. Banks would even tell you that the shield was a beacon of trust. Websites would proudly show a badge to boast about their certificates:

![The '90s seal of trust](./verisign-verified.png)



Until `https` became the standard and everybody started to use it. Bad actors have now equipped their phishing sites with the same quality SSL certificates and the value of the shield has completely evaporated. It's only when a site truly tries to impersonate someone, that the browser now shows that you're entering dangerous territory. All major browsers now only highlight when a connection is `unsafe`.

> ## Evolving Chrome’s security indicators
> Previously, we posted a proposal to mark all HTTP pages as definitively “not secure” and remove secure indicators for HTTPS pages. HTTPS us…
> 
> https://blog.chromium.org/2018/05/evolving-chromes-security-indicators.html

To me there is little value in the ${\textsf{\color{green}(verified)}}$ badge, but I do see a lot of value in the ${\textsf{\color{orange}(unverified)}}$ or ${\textsf{\color{red}(invalid)}}$ status. Those are a clear signal that something is wrong. Unfortunately, with the current mess in setting things up and the complete lack of a ubiquitous infrastructure to share public keys and verify identity effectively the presence of the ${\textsf{\color{green}(verified)}}$ badge just won't be enough.

I predict that the ${\textsf{\color{green}(verified)}}$ badge will go the way of the `https` shield. Once most people are verified, vendors like GitHub will not show it like a mark of trust, but they'll only warn you when something's off.
[There are already policies you can enable on your repo to force all commits to be signed](https://docs.github.com/en/repositories/configuring-branches-and-merges-in-your-repository/managing-protected-branches/about-protected-branches#require-signed-commits) and for [all commits made from github.com to be signed](https://docs.github.com/en/repositories/managing-your-repositorys-settings-and-features/managing-repository-settings/managing-the-commit-signoff-policy-for-your-repository). These will verify that the commits weren't signed incorrectly or not signed at all. *But it cannot raelly verify the commits were made by people you should trust.*

## Why do we need this anyway

To make things worse, git hosting platforms like GitHub, Azure Repos, GitLab already have better ways to establish who you are. Before I push to GitHub, I'm signed in with my GitHub account, performed SSO with my corporate identity and provided multiple 2nd factors along the way. GitHub knows my email addresses, because it has verified them already. And other hosting platforms can already do the same and they have an account identity store for all the users on their platforms. The current way of dealing with signing commits is of course there because Git doesn't need a hosting platform. It's a distributed version control system and it might be used by people who don't ever connect to GitHub or Azure Repos or GitLab. But a large portion of git's users do.

GitHub won't need me to generate any keys myself to verify that **I** pushed a commit to GitHub. But we need the whole signing infrastructure to ensure **you** can verify I authored a commit when you're not looking at it on GitHub. And to me that's probably more important information than the commit author's name and email address they had configured in their git client.

I suspect that, like the Git Credential Manager which finally securely stores your logon credentials on your system, we will have a Git Signing Manager in the future that will be able to sign your commits with your authentication token. GitHub basically already does that when you commit changes from GitHub Codespaces or when you edit files directly on the web. [I wouldn't be surprised when DNS will play a central role in public key distribution](https://gushi.org/make-dns-cert/HOWTO.html).

> ## Publishing PGP Keys in DNS
> https://gushi.org/make-dns-cert/HOWTO.html

We'll surely end up in a state where everyone signs their commits, but we won't see this anywhere unless there is something wrong. And that's how it should be, if you ask me.
