 Jasper Gilhuis

Being informed and up-to-date on information is part of our daily
routine. Just think about the number of times you reach for your phone
or look at your smartwatch to see an update on headline news or a
customer email. This is true in your work, but also in your personal
life. If you focus on the former, you will see that there is always a
need to be informed, in any organization, and especially around agile
processes. This ranges from information on whether a particular feature
has been released to progress of an epic that will allow the release of
a new product.

The Agile Manifesto states that people and interactions should take
precedence over process and tools. And it is obvious that tools will
never replace human conversation and communication.

This article describes the process of enhancing reporting solutions with
Team Foundation Server with the capabilities of Power BI, showing tools
that *can* play an important role in providing adequate information.

**Reporting History**

Let's start by looking back at the reporting capabilities that came with
Team Foundation Server (TFS). Since TFS 2005, reporting capabilities
were primarily based on Microsoft SQL Server Reporting Services (SSRS)
or the use of Microsoft Excel. These types of reports needed to be
designed by developers who had knowledge of SSRS tools and application
data, as well as the ability to translate people's questions into
valuable reports. Developing these kinds of reports has proven to be
quite time-consuming. Furthermore, performance has always been an issue,
and browser support and compatibility has never been optimal. What's
more, most of the time reports were quite static and interaction was
limited to setting a large number of parameters to filter data. Changing
a single parameter for filtering or refining data would require you to
re-run the entire report. Figure 1 shows the parameter selection for an
SSRS Report.

![Machine generated alternative text: Send by Mail • Create Task Create
Meeting Request VI ew Report Iteration Start (Date) Iteration End (Date)
Trend Line 100% Iteration teration 2 Al Filter: (Select All) All (No
Filter) Cl Task of I user Story Al (No Al Filter: Display Both Find Next
Related Reports • Status on All Iteratons Stories Overview Stories
Progress Burndown a Helps you track th hours of work the assigned to
each 200 the work for an iteraton. Shows how many s remain, the rate of
progress, and the work Hours Completed ours Remaining --- Ideal Trend
Actual Trend ](./media/image1.png)

Figure 1 SSRS Report Example

In addition to numerous updates of TFS, there are also updates of SQL
Server Reporting Services. However, the paradigm has not changed much.
Figure 2 shows RTM releases of the two products plotted on a timeline.

![](./media/image2.png)


Figure 2 RTM releases of TFS and SSRS

With the introduction of Microsoft Excel's 2010 Power Pivot came
functionality that allows you to create reports and shape data with
filters, which results in fast switching and filtering data to compare
it to other selections. The use of slicers was actually a very
convenient and interactive way of interacting with your data. Figure 3
shows an example of a report that combines multiple slicers to filter
data.

 

![Machine generated alternative text: Team Internet Team Lead Sites
Project Name (no projeco Team A Team Mobile Team 3 Team SIM Team Backend
Migration Team Spider Team DA Team Trading Team Europort CommitteS
](./media/image3.png)


Figure 3 Power Pivot Report Example 

Power Pivot makes it easy to create new reports. Moreover, overall
development time is much shorter, but it turns out that this only
sustainable works with a limited number of reports. Using multiple tabs
with multiple charts quickly leads to large excel files, sometimes too
large for excel. In that case, refreshing data can become very slow and
it may sometimes lead to unrecoverable errors. Enabling scheduled
refreshes is only possible by integrating them with SharePoint. However,
an Enterprise SharePoint edition for publishing files or reports does
not prove to be the most affordable solution.

**The challenging question**

Today your information needs to be updated more often. Insights need to
be up-to-date when you look at them and they need to be based on today's
data. Correctness and accessibility is not something you are willing to
cut corners on. In addition, you want to be able to do it yourself, and
get a feel of what is going on. Call it 'Do it yourself reporting' if
you like. This was nearly impossible with the technologies that existed
until now.

 

Power BI is here to change all this. Once data sources have been made
available to you, you can actually create reports yourself!

**Polar Express**

Imagine the following scenario. You are a stakeholder for a company
called \"Polar Express\". Your IT department consists of two development
Scrum teams: Team Penguins and Team Polar Foxes. Your company objective
is to be the most successful parcel shipment company in the arctic area.
The teams are adding value to your company\'s product portfolio
(Website, Mobile Apps, etcetera). You are responsible for the IT
department. Your teams are spending money fast, but when the value
delivered is well received, the company\'s future is guaranteed to be
successful.

 

As a stakeholder you want to be up-to-date on the progress and
performance of your teams. To satisfy the informational needs and your
management peers, you want to be able to monitor and respond to
activities that impact the teams, without disturbing them. During a chat
with one of the product owners it occurs to you that recent progress is
not what it used to be. Analysis of the situation with the product owner
has revealed that integration problems between the teams have led to
decreased velocity.

 

In order to gain insight into the situation, you spoke to the product
owner and told him that you would like to have detailed information on
lead times as well as cycle times. However, the look on the product
owner's face says it all. He does not have a report like that. A
question like that has never come up before, and after a brief moment
the product owner says: \"I think I have a solution".

 

**The solution**

Microsoft Power BI consists of a variety of tools, a desktop
application, a mobile solution as well as a web-based environment.

Each of these tools allows you to connect to a multitude of data
sources[^1].

This article focuses on the analysis data of an on-premise TFS server.

When you design reports, it is very handy to have a powerful desktop
editor, along with the power provided by BI. This allows you to play
around before integrating with your corporate environment. When you work
with Power BI Desktop, the context for datasets, reports and dashboards
can be saved into a Power BI file (.pbix). This is great for working
with the environment before sharing it in the organization.

Having the ability to connect to data in Power BI allows you to create a
report for it. This report can be displayed on almost any device or
browser. An even better solution consists of the native support for
mobile applications, which allows remote users to see and interact with
reports on mobile devices. This enables users to be informed anywhere
and at any time.

**Shaping the data**

The data for Polar Express are gathered from the on-premise TFS 2015
warehouse and analysis database. To support custom queries and custom
tables, we need to use a custom database[^2]. In the context of this
article we use a query that retrieves data from the analysis database,
which is then combined with local data to produce the desired view for
our report. There are many ways to connect or shape the data sources to
the desired format. The primary input for the report is a view that
queries the analysis server, and that will return the required data. The
output of the view can be seen in figure 4.

![](./media/image4.png)


Figure 4 View Results

The required source for the supporting table and views are provided in a
script[^3] that can be downloaded from the Xpirit GitHub repository.

Once the data are available, the next step is to create a data source in
Power BI that retrieves the data.

To do so, start by connecting to the SQL Server Database and then use
the Navigator to select the view. Figure 5 shows the data navigator.

![](./media/image5.png)


Figure 5 Data Navigator

Use the Data Tab of Power BI for Desktop to verify that our data has the
correct data types. To do so, click each column header and inspect the
Data Type properties.

The modeling tabs in figure 6 show several options available for data
type modifications.

![](./media/image6.png)


Figure 6 Data Tools, validate Data Types

**Creating the report**

Once the data are available in Power BI, you can start to create a
report. To do so, drag a clustered column chart to the empty report.
Naming the tab allows you to make a distinction between multiple
reports. Set the graph to fill the page, and then configure the graph.

The right hand side shows the data available in the Fields Explorer. The
left hand side shows the chart Visualizations pane. This is where you
configure the chart to use the fields from the view. Figure 7 shows the
section for configuring the report.

![](./media/image7.png)


Figure 7 Configure Visualizations

Set the Axis, Legend and Value properties, and the report is immediately
updated to reflect the changes. It will look similar to the report shown
in figure 8.

![](./media/image8.png)


Figure 8 Report result

You now have a solution that you can always use in Power BI Desktop. You
can save this as a .PBIX file and share this across the organization.
However, that does not sound like a real improvement to previous
solutions.

**Publishing the report**

You can extend this solution by using the Power BI Web capabilities, but
you need to get a Power BI account[^4] to be able to use these
capabilities.

To extend to the Power BI Web environment, you need to publish the
report, which can be done by using the Publish button from the ribbon
(see figure 9).

![](./media/image9.png)


Figure 9 Publish Button

After a successful publish to your Power BI environment, the message
shown in figure 10 will be displayed.

![](./media/image10.png)


Figure 10 Publish results

Now let's go and see the Power BI Web environment for the report.
Navigate to <https://powerbi.microsoft.com> and log in using your
credentials.

Under Datasets and Reports you will see the On-Premise TFS item. This
looks exactly the same as in Power BI for Desktop. Exposing data will be
secure by using the Power BI Gateway, which will be introduced later.

While reports and data are available, you want a better experience. To
be able to view this report easily alongside other data, put the report
on a dashboard. To do so, select the top right pin icon and pin to a new
dashboard (see figure 11).

![](./media/image11.png)

Figure 11 Pin a report to a new dashboard

Your report is now pinned to a dashboard as can be seen in figure 12.

![](./media/image12.png)


Figure 12 Dashboard overview

**Mobile experience**

While this is nice, you want to be able to view this on your mobile
phone. In the old reporting days, this would typically mean the end of
the challenge. However, Microsoft has really done a nice job by creating
a native application for every available mobile platform.

Although the stakeholder uses a great deal of Microsoft technology, he
does run an iPhone, so he downloads the Power BI Application from the
App Store.

After you have installed the app and signed in with the Power BI
account, navigate to the On-Premise TFS Dashboard and see the report
created. Figure 13 shows the dashboard with the report.

![](./media/image13.jpeg)


Figure 13 Dashboard in Power BI iPhone App

Clicking on the graph lets you navigate to the details as shown in
figure 14.

![](./media/image14.jpeg)


Figure 14 Report details in Power BI iPhone App

**Up-to-date information**

To meet the stakeholder's requirements, you need to have an automatic
data refresh schedule order to make things easier for the teams. To have
the ability of Data Scheduling, you need to have the on-premise data
source available for the Power BI environment. A safe way to do so is to
use the Power BI Gateway tool.

Installation of the gateway is straightforward. Just run the
downloadable executable[^5] and configure to connect to your Power BI
environment.

When the gateway is configured successfully, you will see the
configuration as shown in figure 15.

![](./media/image15.png)


Figure 15 Power BI Gateway configuration

After correctly configuring the gateway you can now use the Power BI Web
environment to configure a data refresh schedule. Use the context menu
and choose the "Schedule Refresh" option. Figure 16 shows the context
menu.

![](./media/image16.png)


Figure 16 Power BI schedule refresh

This pulls up the settings page. It is possible to schedule a data
refresh for a particular dataset. Always check whether the Gateway
status is OK, and check whether the Data Source credentials have been
provided correctly. If they are correct, configure a desired data
schedule refresh. Figure 17 shows the available settings for creating
the desired schedule.

![](./media/image17.png)


Figure 17 Power BI refresh schedule configuration

This enables an automatic refresh of the dataset. You now have completed
an end-to-end scenario enabling on-premise TFS warehouse data in an
enterprise mobile application! This should definitely satisfy the
stakeholder!

**Conclusion**

Visual Studio Team Services offers powerful extensibility capabilities
that allow you to connect to the operational store to create reports. In
doing so, you can retrieve data in a different way while the report
creation experience is similar. Microsoft recently released a
development API for Power BI. Integrating Power BI into your own
applications is now possible, and it enables you to develop great
reporting solutions. Moreover, Microsoft is working on new functionality
in the SQL Server Reporting Services in SQL Server 2016. Enhanced
integration between these platforms is to be expected. Content Packs are
the solution for distributing dashboards and reports towards other
people in your organization. In short, the capabilities of Power BI
allow you to really enhance your insights!

**Resources and information**

[^1]: <http://blogs.technet.com/b/dataplatforminsider/archive/2015/10/29/microsoft-business-intelligence-our-reporting-roadmap.aspx>

[^2]: <http://bit.ly/1JYiV36> blogpost by colleague Rene van Osnabrugge
    on creating a custom database to support custom views for reporting
    purposes.

[^3]: [https://github.com/XpiritBV/XpiritMagazine/tree/master/Edition-2/PowerBI/!]{.mark}

[^4]: <https://powerbi.microsoft.com/en-us/documentation/powerbi-admin-administering-power-bi-in-your-organization/>

[^5]: <https://powerbi.microsoft.com/en-us/documentation/powerbi-personal-gateway/>

    <https://msdn.microsoft.com/en-us/library/dn594433.aspx>

    <https://msdn.microsoft.com/en-us/library/ms181634(v=vs.80).aspx>

    <https://powerbi.microsoft.com/en-us/documentation/powerbi-personal-gateway/>

    <https://msdn.microsoft.com/en-US/library/ms170438.aspx>

    <http://blogs.technet.com/b/dataplatforminsider/archive/2015/10/29/microsoft-business-intelligence-our-reporting-roadmap.aspx>
