# Under attack! How we fought off massive DDoS attacks

_Author: Bas van de Sande - Azure Coding Architect / Certified Trainer_

It was September 2023, a nice warm summer day. All of us gathered for the weekly office day at our client - a bank in the Netherlands. We were joking about the fact that it was business as usual - quiet, calm and a bit boring.  Suddenly we received a call from the IT service desk. Customers complained that the public websites were down and that they were no longer able to log in to their environments. What was going on? All hell broke loose.

At the time, we were running websites hosted in Azure App Service Environments exposed to the internet using an Azure Application Gateway, protected by a Web Application Firewall. The Application Gateway was configured with an instance count of two, more than enough to handle the normal amount of visitors to our websites, approximately 400-500 web requests per minute.

![Application Gateway](./images/wag.drawio.png)

We tried to visit our websites and noticed they were unreachable. We opened Azure Portal to investigate our Web Application Gateway. The overview page showed we were getting 600,000 requests per minute. The Application Gateway struggled but kept running. To give it some more breathing room we increased the maximum number of instances to ten. The Application Gateway began responding in a better way, but our websites remained unreachable. What should we do next?

## Ouch

In the meantime, it became clear we were having a major issue. Unreachable websites at a bank will raise serious concerns at rating agencies, the Dutch National Bank, and the European Central Bank.

A crisis team was formed with infrastructure, application, and security specialists. One of the security specialists discovered through Telegram we were one of the strategic targets of a well-coordinated global DDoS attack, initiated by a Russian hacker collective. The attack was a reaction to military support announcements by our political leaders. An attack targeted to destabilize daily life by taking down banking, governmental and public transportation websites.

In our Azure tenant, we had an Azure DDoS Network Protection Plan. In this plan up to 100 public IP addresses are protected regardless of the subscription, as long as the subscription is a subscription under the specific tenant. One of the benefits of the DDoS Network Protection Plan is that customers who are under attack can contact Rapid Response. Another benefit is that you are covered by cost protection (you only pay for what you configured).

Microsoft was informed that we were under a heavy DDoS attack and we asked them to take some countermeasures. Soon our Application Gateway started to scale up massively (Did I mention the benefits of having cost protection). At this point, we were able to serve a generic error page indicating that we were offline due to a technical issue.
This gave us room to do a thorough analysis of the originating IP Addresses. We found out that several IP addresses caused the main spike in our traffic. We created a firewall rule to block those IP addresses, but this didn't lead to our websites becoming online again. They simply launched their attacks from other IP addresses.
Then we looked at the user agents (each web request carries information about the user agent) and we decided to block a set of suspicious and anonymous user agents. Among the suspicious user agents, we noticed `go-http-client/1.1` and `go-http-client/2.0`. These user agents are normally the identifier for the Google Bot/Crawler. A service that indexes your website for the Google search index ranking. This service will visit your website multiple times a day, but will never hammer your site. In our case, the hackers were pretending to be the Google crawler.

Soon our websites became online again albeit slow.
We also noticed that the attack focused on the search page on our public websites. The application team took the page temporarily offline. The websites started to respond in a normal but slow way.  

The first attack was mitigated, the high number of requests being fired upon us took place until the next day. Traffic returned to normal numbers. Time for us to take serious measures! No matter what, our online presence was of the highest priority. Soon after,  the words "DDos Attack" opened all kinds of doors within the bank, paving the way to make all kinds of changes required to our infrastructure.

At the time the DDoS attacks started, one of the IT themes at the bank was "disaster recovery". In case of a region outage of Azure, operations should continue. As a result, we were in the process of setting up our Azure infrastructure to be globally redundant - in our case we were building a complete infrastructure in our paired region, North Europe.
For our public websites to be available under the same URLs, I was working in the background to implement Azure Front Door - Microsoft's global content delivery network. A fairly complex undertaking to set it up using infrastructure as code (IaC).

![Front Door](./images/Frontdoor.drawio.png)

## Front Door woes, outdated examples and multiple products

As it turns out, there are multiple products within Azure called Front Door. And this makes it very confusing when you dive into this product. There is  
Front Door classic `Microsoft.Networking/Front Doors` and Front Door Standard/Premium `Microsoft.CDN/profiles`. It is the latter that you should use as the first one is being phased out.
Most of the examples on MS Learn use the classic version, which makes me wonder why Microsoft is not updating its examples to show how the latest and greatest should be used.

A week after the first DDoS attack we were able to roll out FrontDoor CDN in front of our websites; over dinner, we had the DNS records toggled to use Azure Front Door. After a little hick-up and some last-minute hacking between the main course and desert the public websites were being served through the Azure Front Door CDN network.

The beauty of a CDN is that you don't have an IP address that can be targeted by a hacker, instead, all you have to do is provide CNAMES in your DNS that point to the specific Front Door instance that you are using.

![CNAME](./images/cname.png)

The problem however with most DNS providers is that you need to have an A record pointing to your top-level domain (e.g. bank.nl); an A record points to a specific IP address. Microsoft recommends that you choose a DNS provider that offers CNAME flattening. A feature that is not widespread among DNS providers. CNAME flattening allows you to use a domain name such as "bank.nl" as a CNAME record, while a CNAME record should look like "www.bank.nl". The way we solved it was by using the IP address of the nearest Front Door CDN instance in our region - which is in the Amsterdam Azure data center - as our A record.  

To ensure that the traffic is sent to our Application Gateway, we implemented a Web application firewall policy to allow only traffic coming from our Front Door (by sending the Front Door ID in the header). As a second measure, we implemented a Network Security Group on the subnet of the Application Gateway to allow incoming traffic from Front Door only.

## Here we go again

Anyway, with the new Front Door in place, it was waiting for the next DDoS attack to happen... Which didn't take long as our government came with a new announcement for military aid. The day after, a new DDoS attack was launched by the same hacker collective.

This time we noticed that our websites were overloaded. The users received a nice error page, indicating that we were having technical difficulties.
With the lessons learned from the first attack, we decided to block the search page and implement a rule to block certain bots. Within a very short time, our websites became online again and remained online since then. We were almost there, but not quite yet.

Once the attack passed by, we added the newly added rules to our IaC and decided to implement another feature from Azure Front Door Premium as well, Throttling. Throttling allowed us to put a cap on the number of web requests coming from a unique client in a timespan. We implemented a maximum number of requests per minute based on heavy usage multiplied by two.
At the same time, we were working on moving away from App Service Environments and implementing Application Service Plans to host the websites.
A last measure we implemented was an early warning system, that would alert us if traffic to our websites was increasing above a certain threshold. What we learned from the previous attacks was the hackers were ramping up their attack at a gradual pace, to circumvent DDoS checks at internet service providers (ISPs). For the ISP, the metrics would look like a very well-visited website.

## No one noticed

With all measures in place, we were attacked during the Christmas holiday season. No one noticed, except for the engineer on duty. He received an alert we were being attacked in the most massive attack the hackers had launched yet; 1.2 million requests per minute were fired at our websites. Our customers were still able to visit our websites and to use their environments.

## Lessons learned

If you have mission-critical websites, putting the websites behind a Front Door CDN might be a good idea. Front Door can take the complexity out of your internal environment and put rule processing at Microsoft's end, the same applies for firewall policies and HTTP to HTTPS conversion. Traffic being passed to the actual website is scanned.

Front Door can be set up to use throttling, a feature which I highly recommend. Throttling prevents your website gets hammered. Use the Front Door caching feature for static content, your webserver will thank you.

The Application Gateway only has to deal with the basic routing to the webservers. By having a custom firewall policy and a Network Security Group we can guarantee the incoming traffic is coming from our Front Door instance only (all other traffic is rejected).

Use a log analytics workspace to analyze your traffic, find out which user agents visit your website, and set up a policy to block unwanted guests.

Implement an early warning system. Most attacks will build up gradually to stay under the radar. In case the alert is triggered, decide if you need to take countermeasures.

Keep working and keep refining, security is a never-ending story. The question you have to ask yourself is "**Not 'if' but 'when' am I being targeted?**", and then you should be prepared for it!
