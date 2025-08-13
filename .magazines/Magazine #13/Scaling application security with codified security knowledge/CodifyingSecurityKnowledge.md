It is an exciting time to be working on application security. With

more information, personal information, being processed by

applications the stakes of getting security right increases each day

with each new release. We have come a long way with the availability

of amazing static analysis tooling that scan for well-known issues the

minute you push your code. With security being the responsibility of

everyone, automation is key to supporting everyone with mitigating
security issues

before they reach production.

However, there are many challenges with automating security. Today we

are going to look at an opportunity to take a step forward in one of

those challenges, scaling security knowledge.

Security knowledge is discovered when application security

practitioners audit applications, so let\'s start with a security audit.

## Application security audit

Security audits come in various shapes and forms from fully automated

to manual audits performed by security specialists. We are going to

look at an example of the latter, the one that in my opinion generates
the

most useful security knowledge, but is also the one that does not

scale well.

First, you start with mapping the attack surface, the entry points, the

way others interact with the application. Then, determine what can

be influenced by looking at what is accepted as input, how it flows

through the application, and where it is being used. An SQL query is

constructed using string concatenation without the proper contextual
output

encoding. Tracing back the flow reveals no sign of input

validation, but the endpoint requires authentication. Still, a serious

security vulnerability.

The flow just described is common in manually auditing applications for

security issues. The other part is looking for patterns learned from

previous audits or security research published by other application

security practitioners. Manual security audits are typically done once

or twice a year per application. The knowledge obtained during an

audit is mostly lost in notes and the findings are written down in a

report that hopefully ends up in the hands of those that can mitigate

the discovered issues, the developers.

## Automatic vs manual

Performing a security audit once or twice a year is obviously not

enough in a time where development teams push a new release to

production once per week, once per day, or even multiple times per

day. This is already well known and the shift left movement is

attempting to address this gap between releases and security

audits. Manual security audits, however, demonstrate that there

remains a different security gap i.e. issues not being mitigated by

developers.

This security gap that I encountered during the many manual security
audits that

I performed was interesting. While trying to understand why this

happens, I encountered the three following reasons:

1\. The developers didn\'t look at or weren\'t aware of the results
found

by the static analysis tooling integrated into their CI/CD

pipelines because the result page was not part of their development

workflow. It required a visit to a separate dashboard.

2\. Developers were provided with a tremendous task of sifting through

a lengthy list of findings, many not actually an issue and known as a
false

positive. At one point, developers simply started to ignore the

alerts.

3\. The analysis was missing significant findings, known as false

negatives, because the analysis didn\'t understand the domain of the

application nor its threat model.

So, what can we do to reduce the gap between audits and automated

security analysis? Today we are going to have a look at reason 3.

## Scaling security knowledge

The key is still automation. DevOps practices provide a unique

opportunity for security to join the effort in reporting security

issues as soon as possible. Manual security audits don\'t scale.

Even if your organization has a team of security engineers, it is still

tough when you have 100,000 repositories. Ignoring many aspects that

impact the scaling of application security, rolling out a static
analysis tool

across your organization is non-trivial. We are going to look at an

approach to scale security practitioners tasked with auditing

applications for security issues.

The way we can scale is by codifying their security knowledge.

## Codifying security knowledge

Static analysis tools have been around for ages. However, a new

generation of static analysis tooling is stepping it up a notch by

being easily programmable. Two of which have managed to establish a

community of security practitioners that are using it while making

their efforts available to the rest of the world. The first one is

[Semgrep](https://github.com/returntocorp/semgrep)[^1], a lightweight
static analysis solution for finding variants

of security bugs in software. The other, and the one discussed in this

article, is CodeQLf[^2].

What makes CodeQL unique is it turns source code into data that

can be queried using a query language named QL (which stands for query

language). QL is a language that looks like SQL, but its

semantics is based on a declarative logic language called

Datalog[^3]. This makes QL a logic language and all the operations are

logic operations. So, what does this all mean and how does this benefit

us?

QL allows you to codify code patterns in a way that is very succinct

and easy to read. The reason it can be succinct is due to its

declarative nature. If you have experience with writing SQL queries

then you are already familiar with this., It means you state

what you want to find instead of how. The following examples

will demonstrate this. QL is a generic purpose language, but with

CodeQL there are libraries available that implement many program

analysis algorithms that enable you to analyze programs.

Let's start with an example, a "hello world" in QL.

select \"Hello World\"

Short and to the point, but not very insightful. Let's have a look at

something more useful. Assuming a Java program for which we have used

CodeQL to build a database of facts we can, for example, ask the

following question.

import java

from IfStmt ifStmt

where ifStmt.getThen().getNumStmt() = 0

select ifStmt, \"\...\"

Take a minute to determine if you can fill in the \"\...\", a message to

the user, by reading the query.

If you would translate the query to English, it would state something

like - given all the if statements in the source code represented by

the class IfStmt[^4], let the variable ifStmt represent those that have

zero statements in their "then" branch. In other words, an if

statement with an empty "then" block. The message \"Redundant \'if\'

statement.\" would be an appropriate alert message instead of the

\"\...\".

Nice, but weren\'t we interested in security knowledge? Remember

the audit flow in the beginning of the article where we started from

an entry point and followed it to a security sensitive operation? In

program analysis there is an analysis called data flow analysis that

can follow how data is used in a program. By giving it a

start location in the program we can determine if that data reaches

another location in the program. The former we call a *source* and the

latter we call a *sink*.

Let's have a look at what a simplified query would look like.

import java

import semmle.code.java.dataflow.FlowSources

class SqlInjectionConfig extends TaintTracking::Configuration {

SqlInjectionConfig() {

this = \"SqlInjectionConfig\"

}

override predicate isSource(DataFlow::Node node) {

node instanceof RemoteFlowSource

}

override predicate isSink(DataFlow::Node node) {

exists(MethodCall call \| call.getTarget().getName() = \"executeQuery\"
\| call.getAnArgument() = node.asExpr())

}

}

from SqlInjectionConfig config, DataFlow::Node source, DataFlow::Node
sink

where config.hasFlow(source, sink)

select sink, \"Possible SQL injection because this query relies on
\$@.\", source.getNode(), \"user-supplied data\"

There is much more going on, but if you focus on the from

\... where \... select part, you can see that it looks for a data flow

between a source and a sink. The definition of the source and the sink

is provided by a configuration. The source is defined by a predicate

isSource that only holds if a data flow node is part of the set of

values represented by the class RemoteFlowSource[^5]. This is a class

provided by the standard library of CodeQL for the JAVA language that

represents all the elements in a program considered a source of

user-supplied data. In other words, data an attacker can influence and

which should be scrutinized. An example is a parameter in a HTTP

request such as the parameter email in the following Spring REST
endpoint

method.

\@GetMapping(\"/user\")

\@ResponseBody

public User getUserByEmail(@RequestParam String email) {

\...

}

While many details remain unexplained, I hope this shows that a

complex analysis, tracking data through a program, can be expressed in

a succinct manner that is also readable and understandable. This is a
desirable

property of a system used to capture knowledge.

What is neat is that you do not have to know the details of the class
RemoteFlowSource and

that any extensions (i.e., new HTTP frameworks) will automatically be

available to your query. The [SQL injection
query](https://github.com/github/codeql/blob/main/java/ql/src/Security/CWE/CWE-089/SqlTainted.ql)[^6]
injection query provided with CodeQL

does the same for the sinks. Any new SQL library with methods

that are susceptible to injection can be added and no change to the

query is required to find issues in applications that use

those new libraries in an insecure manner.

With this a security practitioner can write a query to find an issue

that they already found manually. How does this help scale application
security?

## Variant analysis

A strategy applied by security practitioners before they start auditing

software is to look for known prior security issues. This not only

provides them with useful information on the target, but it is common

that a fix is incomplete, or the issue follows a pattern that occurs

elsewhere in the application. Possibly in other applications as well. It
is

therefore interesting to look at variants of a security issue.

Looking for variants is a difficult process if done manually, but when

the pattern is codified it becomes much easier to look for the same

pattern in the same application or across all your applications. It also

helps with preventing regressions if the same mistake is made again or
if

a mitigation is inadvertently reverted.

Being able to look for variants is super useful, but what really helps

reduce the security gap is that the knowledge being codified is

domain specific. The queries written for your application allow

static analysis tooling to find issues that might only occur in your

application. For example, an incorrect use of a proprietary

framework. These kinds of issues are never found out of the box by any

static analysis tooling!

How cool would it be if, at the end of security audit, you would not

only get a report, but also a set of queries that you can run in your

CI/CD pipeline to find the same issue and variants in all

your applications. It can even help others if those queries are made

available. Two notable examples are:

\- A ZipSlip query to find a widespread security issue documented by
Snyk[^7] that allowed security teams to quickly assess if their projects
are vulnerable.

\- Multiple queries to hunt for Solarigate activity described in the
blog post Microsoft open sources CodeQL queries used to hunt for
Solarigate[^8].

## Conclusion

Codifying security knowledge by security practitioners is the next

step in reducing the security gap we currently have with static analysis

tooling. It will help with scaling application security by making it

easier to apply the knowledge repeatedly across many codebases. This is
a

capability you will need to help developers and

security engineers to respond to unknown application security threats.

You can start learning how to codify security knowledge today through
fun CodeQL Capture The Flag[^9] exercises made available by the GitHub
Security Lab.

[^1]: https://github.com/returntocorp/semgrep

[^2]: https://codeql.github.com/

[^3]: https://en.wikipedia.org/wiki/Datalog

[^4]: https://codeql.github.com/codeql-standard-libraries/java/semmle/code/java/Statement.qll/type.Statement\$IfStmt.html

[^5]: https://codeql.github.com/codeql-standard-libraries/java/semmle/code/java/dataflow/FlowSources.qll/type.FlowSources\$RemoteFlowSource.html

[^6]: https://github.com/github/codeql/blob/main/java/ql/src/Security/CWE/CWE-089/SqlTainted.ql

[^7]: https://security.snyk.io/research/zip-slip-vulnerability

[^8]: https://www.microsoft.com/security/blog/2021/02/25/microsoft-open-sources-codeql-queries-used-to-hunt-for-solorigate-activity/

[^9]: https://securitylab.github.com/ctf/
