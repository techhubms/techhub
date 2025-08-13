# HTTP APIs and event sourcing

## Problem

Imagine you are working on a project to build a web application. In
doing so, you want to use the latest and greatest in technology and
implement an HTTP API. You have done this before, and based on your
experience, you use the GET verb to retrieve data, either a list or a
single item. In addition, you use POST to store new data and PUT for
updates. Finally, you're using DELETE to remove a resource. As a result,
clients can connect to your API and interact with the data. While this
approach sounds simple, it can become complicated when your application
uses the 'CQRS' and 'Event Sourcing' design patterns. These patterns
ensure great power, but they also increase complexity.

> separate textbox start\>\>\>\>\>\>\>\>\>\>\>\>\>\> *About CQRS and
> Event Sourcing* The acronym CQRS stands for Command Query
> Responsibility Segregation. This design pattern effectively separates
> read operations from write operations. This allows you to use two
> different models, a read model and a write model. Applications often
> read data more often than they write. Continuing the segregation all
> the way up to the data store allows you to optimize the design for
> both situations. For example, you can store denormalized data in a SQL
> database read model so it can be queried efficiently, and use a
> document database for the write model to get the best write
> performance. People often combine CQRS with Event Sourcing (ES). The
> easiest way to explain what Event Sourcing means is by comparing it
> with a bank account. Instead of storing only the latest value of the
> state of an object, you persist all changes. It is likely that you
> made your first deposit when you opened your bank account. Since then,
> the bank has stored all deposits and withdrawals. To know how much
> money is in your account, you calculate all changes. It's easy to see
> that this design pattern will help you create an audit trail inside
> your application. You can trace back all changes to a domain model. It
> also enables you to perform what-if scenarios by adding 'fake' events
> to the calculation and see how they influence the outcome. You can
> imagine that calculating bank statements for thousands of customers
> can soon become a burden for any server. In this situation, Event
> Sourcing and CQRS become a great combination. You store changes
> (events) only by adding records to a data store (the write model). But
> after storing the change, you can also update a read model with the
> current account balance. This way, your application can also respond
> to more complicated queries in a very efficient way. separate textbox
> end\>\>\>\>\>\>\>\>\>\>\>\>\>\>
>
> separate textbox start\>\>\>\>\>\>\>\>\>\>\>\>\>\> *HTTP APIs* HTTP
> APIs are often based on REST; REST stands for REpresentational State
> Transfer. An API of this type is called a RESTful API, which means an
> Application Program Interface (API) that uses standard HTTP requests
> to GET, PUT, POST and DELETE data. This enables you to expose data as
> resources, identified by URLs, and perform operations on these
> resources. separate textbox end\>\>\>\>\>\>\>\>\>\>\>\>\>\>

Now imagine a domain in which we interact with a customer resource. You
start in a simple way by including a GET request at the endpoint
`/api/customers` that will return a list of customers from your
read-model. To create a new customer, you perform a POST web request to
the same endpoint. Updating of data is handled by using PUT; i.e. you
send the new state to the endpoint `/api/customers/{customerId}`. The
last part of the URL identifies a unique customer.

In your API Controller, you handle the intent of changes to a Customer
by converting them to commands, e.g. a `CreateCustomerCommand` or an
`UpdateCustomerCommand`. A command queue and command handlers will do
the rest. Eventually, the new customer representation will appear in the
read-model.

Although this looks simple, you can quickly run into a more complex
situation. What if your customer entity contains related items? A
customer might have one or more addresses or legal entities. Are those
addresses different resources, or will you consider them part of the
customer graph? Besides that; a customer can go through different stages
in his life cycle. Approval might be needed before a change to a
customer is applied. Are these changes mere updates on the customer with
a 'State' property? If so, how would you capture user comments for that
specific state change or a reassignment of approval to another user?

Deleting a customer is another interesting scenario. Are you removing a
customer, or do you require soft (reversible) deletes? Again, somebody
might need to approve the operation before the system deletes the data.

Although some of the issues we mentioned above can be present while not
using CQRS or ES, there are specific problems that pop up when using
CQRS. For example, a write operation results in the creation of a
command. The command will most likely be processed in the background.
Although asynchronous processing is great for scalability and
performance reasons, it also means that the resulting state of a
resource is not yet determined during the POST or PUT web request. Also,
the API consumer does not know when or how the system will process the
command. In these scenarios, it is common to deliver feedback about
processing back to the client via another mechanism, for instance a push
channel by using technology such as Signalr.

A similar issue happens during the validation of the customer state. The
command created by the HTTP action is put on a queue and handled at a
later time. While you can validate the state of the resource before you
create the command, the situation may have changed before the command is
ready to be processed. For example, your application is used to change a
customer address, while somebody else just deleted it. The HTTP request
has already been returned, the client thinks all is well, but the
processing of the last command will fail.

In short, does the creation of an asynchronous application also imply
that we should not use a standard HTTP API? On the contrary; we believe
you can do this, and we'll show you some solutions!

## Possible solutions

As with every problem, there are also various solutions. If we do want
to stick with HTTP verbs, then the options are as follows.

### Option 1. Use PUT

Follow the REST guidelines, and perform updates by using a PUT verb.
This option has all the drawbacks as mentioned before, but it is very
intuitive to update resources using this verb. In this case, you would
update a customer by sending the complete resource representation to the
endpoint.

    PUT /api/customers/{id}
    Content-Type: application/json

    {
      "name": "customername",
      "industry" : "name of industry",
      "telephone" : "123 456 789",
      "active": true,
       etc
      "addresses": [
         {
            "street": "value"
         },
         {
             etc
         }
       ]

    }

Not only can this lead to large payloads as you send the whole
representation of the customer, but it also adds complexity at the
server side. The server needs to discover the intent of the PUT request
in order to convert it into the correct command. It all depends on how
fine-grained you want it to be: building a `ChangeCustomerCommand` might
be simple, but detecting and using state changes to trigger multiple
workflows (like change approval) will be more complicated. Combine this
situation with hierarchical resources, e.g. customers with addresses and
you'll soon have a huge block of complex code...

### Option 2. Use PATCH

You can optimize the change process by using the PATCH verb. Using PATCH
indicates a partial update and allows you to update only specific fields
of the resource. Instead of sending the whole resource, you only send
the changes.

    PATCH /api/customers/{id}
    Content-Type: application/json

    {
      "active": false
    }

Although this makes the payload lighter, it does not solve all
size-related problems. For example; adding a new address is only
possible by providing the entire new set of addresses. As a workaround
for this, you can use "json-patch (RFC 6902)". This approach allows you
to make very specific changes on your resource and as such, it can help
reduce the payload.

    PATCH /api/customers/{id}
    Content-Type: application/json-patch+json

    {
       { "op": "add", "path": "/addresses", "value": [ { "street": "second street" ] },
    }

The JSON-PATCH standard allows the use of operations to make targeted
modifications to a resource. Adding, removing, but also replacing,
copying or moving are valid operations that can be accompanied by a test
condition. Similar to the PUT method, you still need to extract and
generate the command out of the data that is submitted.

### Option 3. The miniput pattern

A possible shortcut is the use of the "miniput pattern", which allows
partial updates to a resource by exposing child resources so we can do a
complete update.

    PUT /api/customers/{id}/telephone
    Content-Type: application/json

    {
        "telephone" : "123 456 789"
    }

Note that the customer attribute named 'telephone' is now part of the
URL. The server replies with the full representation of the resource, as
a result of setting the content-location header value referencing the
customer resource.

    HTTP/1.1 200 OK
    Content-Location: http://example.org/api/customers/{id}
    Content-Type: application/json
    Content-Length: ...

    {
      "name": "customername",
      "industry" : "name of industry",
      "telephone" : "123 456 789",
      "active": true,
       etc
      "addresses": [
         {
            "street": "value"
         },
         {
             etc
         }
       ]
    }

The miniput does not target the parent resource URL, and this makes
cache-invalidation of this customer resource difficult.

The above options provide some alternatives to perform updates on
resources, but they still don't fit very well with the more complex
scenarios like eventual consistency and complex resource graphs.
However, there is a way in which we can interact with the system using a
more "command-driven" approach. (Don't worry, we won't encourage SOAP
web services...)

### Option 4. Command endpoints

In this scenario, we make it very explicit to the caller that we expect
abstract commands by exposing a single command endpoint. The consumer
will send a POST request to this endpoint, and the resulting command
ends up on a command queue and gets processed. You need to specify the
type of command inside the body of the web request.

    POST /api/commands
    Content-Type: application/json

    {
      "Name": "ChangeAddressCommand",
      "Payload": {
        "Address": { "Street": "new street"}
      }
    }

After we enqueue the command, we return a 202 response code. The web
response also includes a location header that points to the endpoint
where the status of the command processing can be retrieved. Note that
it does not point to the resource itself, but to a new resource that
describes the status of the command processing.

    HTTP/1.1 202 OK
    Location: http://example.org/api/command-progress/32453

By performing a GET on the `command-progress` resource, the client can
see whether the command has been processed or rejected. It could also
contain additional details like a webhook location, or a web-socket link
for push notifications over Signalr.

This approach makes it very explicit that commands are needed, as they
are the only way to make changes to the system. All other calls to
resources only support the GET verb, and will query the read-model. The
caller needs to be aware of the command structure available, while at
the same time the types of supported commands are not easily
discoverable.

### Option 5. Content type

Similar to the previous solution, we need the caller to pass on the
intent in the form of commands, using the content type header while
operating on the resource. Queries naturally map to GET methods, while
commands are mapped to POST, PUT, DELETE and PATCH. The command type is
part of the content type header. For example, changing the name of a
customer can be expressed as follows:

    PUT /api/customers/242
    Content-Type: application/json;domain-model=RenameLegalEntityCommand

    {
       "Name": "New Name"
    }

Changing the name is an idempotent operation, which means that executing
the same action multiple times produces the same result. The standard
dictates that we must use the PUT verb in this situation. However, other
commands, e.g. adding a new address, need to be expressed with a POST
verb because they are not idempotent operations.

Removing a customer would be implemented using the DELETE method:

    DELETE /api/customers/242
    Content-Type: application/json;domain-model=DeactivateCustomerCommand

The clear downside of this approach is that the internal domain is now
partially visible on the outside. Callers need to be aware of the
resources, the various operations and even different commands that can
be used. However, this solution does map well to the REST principles, as
it provides operations on resources and uses HTTP semantics correctly.

### Option 6. POST instead of PUT

The final solution we'll discuss is to model all commands as POST
actions to specific resources. As we saw in the above examples, it is
very hard to map a business model to explicit resources. At the same
time it moves the business logic to the client and as such creates a
tight coupling, which is undesirable and can lead to errors.

We can solve the low level CRUD APIs by introducing business process
resources which express the intent of the operation. Consider the
creation of a customer. Most likely this is not a simple process as it
might require sub processes to go along, emails to be sent out, records
to be created etc. The business intent is to enroll the customer, so a
`CustomerEnrollment` endpoint can be used to actually create the
customer itself.

    POST /api/customers/CustomerEnrollment
    Content-Type: application/json

    {
        body
    }
    HTTP/1.1 202 OK
    Location: http://example.org/api/customers/CustomerEnrollment/32453
    Retry-After: 3

The resource returned is a `CustomerEnrollment` entity and tells the
caller the state of the actual enrollment instead of the customer
itself. You can also add an additional header named `Retry-After` that
specifies the amount of seconds it will likely take to change the
resource state at the server side.

Removing an address can be expressed as follows:

    POST /api/customers/421/AddressRemoval
    Content-Type: application/json

    {
      "id": "3",
      "reason": "reason for removal",
      "onBehalfOf": "user name"
    }

The response would look like this:

    HTTP/1.1 202 OK
    Location: http://example.org/api/customers/421/AddressRemoval/631
    Retry-After: 3

Until the Address Removal command is completed, the address is still
present in the customer resource. When completed, this specific instance
of `AddressRemoval` is no longer available. As you can see, the payload
is also tailored to the specific command and not to the actual entity it
should alter.

This is similar to the command endpoint solution, but it is much easier
to discover as it can be advertised in an API definition file like
OpenAPI.

By introducing a slight variation on this solution, you can also model
workflows, like the change-approval we mentioned earlier. Instead of
using a command-based endpoint, you would use one based on a workflow.
For example, to start an approval process you would send this web
request:

    POST /api/customers/421/CustomerChangeApproval
    Content-Type: application/json

    {
      "id": "3",
      "telephone": "new number",
      "onBehalfOf": "user name"
    }

When you regard the workflow as a separate resource, it becomes easy to
manage the workflow by using GET, POST, PUT and DELETE. For instance,
you would use GET to retrieve a workflow, and approve the change with a
PUT request, or reject it by using DELETE. You probably noticed that
this re-introduces the troubles we discussed earlier, but there is a
major difference... Instead of having one PUT endpoint for all workflows
(and changes, and commands), we have narrowed the scope down to manage
just one workflow.

## Conclusion

When building a Web API, there are a lot of strategies to choose from.
In our project we combined HTTP APIs with CQRS and Event Sourcing, and
in that situation our choice was Option 6, the 'POST instead of
PUT'-pattern. This option is particularly suitable for resources where
the GET maps nicely to your data structures, but poorly to your business
domain for mutations. It solves the problems introduced by applying the
CQRS/ES architecture and offers the benefits of a RESTful HTTP design;
it is discoverable, and it uses the correct verbs and endpoints. The
fact that we return a status endpoint (instead of the new resource
state) allows for asynchronous processing of commands.
