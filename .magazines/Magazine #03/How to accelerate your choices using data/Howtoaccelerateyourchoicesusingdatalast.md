# How to accelerate your choices using data

A few months ago, I arrived at the Volvo dealer to pick up my new car. I
was very excited. The car dealer tried to explain all of the car's
features but I couldn't care less. All I wanted was to hit the road, but
he insisted on installing an App on my phone before I took off.
Installing this App would allow me 'to get to know my car better'. Five
clicks later, all was set up, so finally I could hit the road with my
new baby!

A couple of days later, I opened the app, and when I opened it, I was
completely surprised by the amount of data it displays. It turns out
that my car is no longer just a brutal engine. Instead, it's a mean IoT
machine! It collects all sorts of data that give me insight into the
usage of the car. It helps me to detect problems early and control a
range of settings, and it even allows me to start the heater from my bed
when it's freezing.

![](./media/image1.png)

![](./media/image2.png)


At first it was hard to understand how collecting data could impact me
in a positive way. I became more curious when I realized that my
navigation system is updated regularly, but I've never seen an update
screen. When I asked the dealer, he explained that the navigation is
updated only when my car is 'idle'. The car determines the ideal moment
by analyzing the collected trip history. He also explained that the car
collects my street crossing approach speed. By detecting patterns in my
behavior, Volvo gets a better understanding of how I use my car. They
can now prevent me from having accidents by applying preventive
maintenance on the brakes or by offering drive assisting features (brake
assist, impact braking, lane corrections, etc.). Volvo has seen the
light, similar to the likes of Tesla, Toyota and BMW. Data will allow
them to run their business more effectively, while I get a safer car.
What do you use to improve your business?

**What can you get out of data?**

Now, just collecting data will not help you with anything. Your storage
facilities may contain petabytes of data without adding any value to
your customer or your business. To deliver the desired impact, you will
need to transform data into information, but this is easier said than
done. Before turning big data into something valuable, you'll need to
understand what you can transform it into and how to achieve it.

Typically, when we gather data from a database, a developer will be very
descriptive in his needs. He'll be explicit about the source and most
likely he'll define precisely how it will be formatted. An example of
this is the traditional poor men's reporting using a SQL statement:

SELECT Date, AVG(SpeedPerHour), AVG(TyreProfileReduction) FROM
CarUsageData\
GROUP BY Date ORDER BY Date ASC

In this example we will combine the average speed per hour and the
average tyre profile reduction, and group this by date. From a business
perspective, we want to gain insight into yesterday's performance. The
generally accepted name for this type of analytics is 'descriptive
analytics', and the request is usually expressed in query languages.

The second type of analytics I'd like to introduce you to, is very close
to its 'descriptive' brother. It is called 'diagnostic analytics'. The
focus of this type of analytics is still on the past, and the main
difference lies in the underlying business question. In a descriptive
world we ask ourselves the questions 'what happened', whereas in a
diagnostic world we wonder about the question 'why did this happen'? So
rather than searching for the occurrence of an incident, your question
is now focused on identifying the (root-) cause of the incident.

Once you understand that your performance has decreased(descriptive) and
you understand the cause of this(diagnostic), it's time to make the next
step. Using the knowledge you gained, you want to be able to predict the
future performance. This is the moment when you turn yourself to
'predictive analytics'. This kind of analytics helps you to predict
future data points using models, and you train these models by feeding
them historical and reference data.

Now that you understand what the future looks like, you'll probably want
to understand how you can influence the result. This is when
'prescriptive analytics' can show you the way. An example: your sales
director is confronted with low predictions for the next quarter. The
predictive models have shown the urgency, and now he has to act
accordingly. He can stimulate his market with a new product launch, but
what is the appropriate size of his salesforce? And what is the best
pricing strategy? Does he need to open a new shop on Bond Street in
London or should he invest in better delivery options for customers
outside of big cities? By simulating what will happen, the prescriptive
models will advise what's the best possible combination of choices, and
using this advice, the sales director can make a properly informed
decision in order to gain the best result for his organization.

If it's a split second decision, prescriptive analytics can potentially
even help to make the decision itself. The feedback gathered directly
after the decision can feed directly back into the models. This allows
the business to quickly understand whether a certain promotion text is
more effective than another. In addition, they don't have to worry
whether the marketer is online at that particular moment to disable the
less effective advertisement.

![http://blogs.gartner.com/it-glossary/files/2012/11/predictive_analytics.gif](./media/image3.gif)


*Gartner's way to visualize the different types of Big Data Analytics*

**Technical Limitations**

For a very long period, technical challenges stood in the way of making
proper analytics solutions happen. For the past 20 years, IT departments
have been facing a financial wall, which disallowed them to handle large
chunks of data in their infrastructure. Luckily, cloud-based large data
stores, instant data processing power, and advanced access control are
now very affordable.

Limitations at application level are also disappearing quickly. The
three main cloud providers deliver very competitive solutions in the
domain of machine learning. These solutions are updated constantly and
offer a low entry level. Advanced web-based dash boarding techniques,
such as PowerBI, speed up the time to market significantly, and they
take away the visualization headache. These tools also reduce the time
to market, which in turn allow business users to access their data a lot
quicker. Algorithms and reference data are nowadays often publicly
available. For commercial usage, several standard packages can be
acquired from public marketplaces requiring limited investment.

**Making the difference**

Now that the technical barriers can be broken down by newly available
capabilities, it's time to bring this advantage to your organization.
Large analyst firms such as Gartner, predict[^1] that between now and
2018 more than half of the large organizations will try to disrupt the
industry by using advanced algorithms and analytics. They even expect
that this growth will accelerate beyond this point. Knowing that these
models become smarter while feeding them data, the urgency to start
immediately becomes bigger every day. You can either allow your
competitor a head start or you can jump on the boat and take a leading
position.

It's important to understand that not every business question can be
answered by using advanced analytics. Especially decision making
involving your own employees is still considered a no-go; when there's
Personable Identifiable Information in the process, it's recommended to
take a very defensive approach with advanced analytics technologies.
Typically, I believe that you can make the biggest difference in the
following areas:

-   Creating transparency in your organization about business results

-   Enabling experimentation in customer needs & understanding variation
    in those needs

-   Segmenting customer bases to allow better and more targeted offers

-   Replacing human decision making in order to reduce operational costs

-   Innovating business models, products and services.

**Making a head start**

Your organization is not the first one to attack the problem. To avoid
making the same mistakes and stand up on the shoulders of giants, we've
listed the most important recommendations that will help you to keep
gaining speed.

-   [When choosing a cloud vendor]{.underline}

AWS, Google and Microsoft all have competing big data offerings. When
choosing from one of them, realize that you'll marry this vendor. Even
though it's technically possible, you'll never move 5 petabytes of data.

-   [Involve your legal department\
    ]{.underline}Your legal department can help you to understand what
    you can do with data and where you reach limits such as privacy and
    legalities. By involving them at an early stage, you can look at
    possibilities, rather than restrictions.

-   [Start storing raw data today\
    ]{.underline}Well-structured data in application databases is often
    optimized for a specific purpose. Since you don't know how you'll
    use this data in a model, don't process it. Instead, save the raw
    data.

-   [Obtain reference data]{.underline}

It's going to be impossible to train your intelligent business models in
a few months. You will need more data to train these models. Therefore,
try to obtain relevant reference data on data marketplaces. This is both
more effective and often cheaper than collecting it yourself.

-   [Train or hire talent?]{.underline}

Data Science is a large part of computer science. It also requires a
completely different mindset. To get started, hire the talent while you
start building up the internal competence.

-   [Generate new input]{.underline}

Consider everything as a new source of information. Consider tools such
as Microsoft Cognitive Services to gain new types of data, even from
human conversations.

-   [Not pretty]{.underline}

Many times, the output of the numbers aren't that pretty. Rather than
focusing on the visualization, allow self-service business intelligence
using Excel or a dashboard. This allows you to focus on the quality of
the data.

**Conclusion**

Getting started with big data analytics is never easy. Even though it
became a lot more affordable, the investment in handling large pieces of
data is still significant. In order to make sure that the investment is
worth the money, you need to find an achievable, well-designed business
objective, and you'll need to prove to your organization that this
investment will yield returns[^2]. In this game of figures, you must not
overlook the possibility of adding customer value. Volvo has always
committed itself publicly to passenger safety. With their investments in
Big Data, they are proving very clearly that their value proposition is
not just an advertisement, but that it's part of their product
experience. And I hope it will keep on proving that, every day when I
hit the road on my way home...

[^1]: <http://xpir.it/mag3-analytics1>

[^2]: <http://xpir.it/mag3-analytics2>
