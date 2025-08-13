[Failure is not the opposite of success; it's an essential part of it.
It's through failure, in a controlled and tested environment, that we
learn and improve; Trial and error is the way most of us learned how to
work with a computer. We don\'t know any software developer who did not
crash his OS because of the 'oeeee what does this button do' thought. It
is because most of us could experiment safely with a computer, that we
understood how a computer works, and now we fix computers for our entire
family during Christmas because of that.]{.mark}

*[The surprising truth about success (and why some people never learn
from their mistakes), is that it has everything to do with
failure.]{.mark}*[^1] []{.mark}

# Yeah! Science

[In 2017 SpaceX showed for the first time in history how they managed to
land a rocket back on Earth. For many, it was a fantastic event. But
what few people don't see is the process getting to that point. But a
few months later, SpaceX posted a video online called 'How Not to Land
an Orbital Rocket Booster', making fun of the mistakes they made
beginning from 2013 and onwards. The videos show the reason behind their
success, failure! It is nothing new to the space industry because that
industry is using a method build around failure, the scientific
method.]{.mark}

# Falsifiability![](./media/image3.png)

When scientists use the scientific approach, they come up with a
hypothesis and try to find evidence that can falsify that hypotheses.
When we test our code with unit tests, we can confirm that our code
works, and we can create an equivalence test for it that shows it will
fail. When we do so, we test our code with fixed data called fixtures.
By using fixtures we fall into the confirmation bias trap, the test
confirms that our code works, but only with that input. These tests will
only be as robust as the possible arguments or parameters tested against
our code. Quoting Romeu Moura: If we take a String as an argument, then
the works of Shakespeare in Japanese & Korean are ONE valid input. We
can achieve this robustness with parameterised testing. However, this
makes the unit tests so big that it is harder to understand which
behaviour it is validating. We want our unit tests to also serve as
living documentation so they should be comprehensible and to the point.
We can even get into more trouble as our systems evolve, and parameters
can change, making refactoring messy and slow.

# System evolves

Every organisation evolves, transforms itself. The organisation can
start a new business, create a new product from market leads, or even
expand the operations to new countries. The systems which support the
organisation also have a demand to evolve side-by-side with the
organisation, adapting to the new reality, allowing the users to be
competent with their tasks.

A classic example is the expansion of business operations to a new
country, where they create new demand for the system; re-use or creation
of new features can lead to a system evolution to accommodate the new
requirements.

However, with the system evolution, it is also common to have some nasty
side effects, such as unexpected bugs in re-used features. Using the
previous example, where an organisation expanded the business operations
to a new country, the countries have different national holidays; it can
lead to different behaviour on features which are dependent on national
holidays to support business calculations, such as in logistics
operations. How can we leverage Property-based testing to create a
supple design for our system?

# Induce pain or stress the system

Using Lean Principles where we should seek to continue improving the
system, one way is to induce pain or stress the system. There are two
ways to induce pain in the system: in an uncontrolled and controlled
manner.

![](./media/image4.png)


Usually, we develop and test a system, and then deploy it to production.
From the feedback (system observability, user feedback, among others),
we continuously improve the system to provide additional value. How many
times did we felt the pain from production? A nasty bug reported by a
user that corrupts data, or even cripples the system. As a development
team, we are under pressure to fix the issue observed in production,
which is an example of induced pain in an uncontrolled way.

What if we shift left in the system stress, e.g., can we induce pain
during the development phase before deploying the system to production?
In this way, we can stress the system in a controlled manner, where we
can gradually introduce stress, observing the system behaviour. If any
unwanted behaviour arises, it is caught and fixed during the development
phase, increasing the quality of the delivery.

# Enter Property-based testing

Property-based testing is the construction of tests such that, when
these tests are fuzzed, failures in the test reveal problems with the
system under test[^2]. In Property-based testing, we randomly generate
data points within the boundary of a business invariant to verify the
behaviour of the system. The English Oxford Dictionary defines property
as following: "An attribute, quality, or characteristic of something".

Property-based testing not only lets us test edge cases that could
expose unwanted and unexpected errors in the code but also enables us to
make small tests that are readable and clear. Making these tests will
also force us to think harder about the problem at hand and improve our
design and code quality. Using Property-based testing pushes us to think
about the state or the state transitions of the feature under test,
rather than some value to satisfy some conditions. It leads the
development teams to have tests focus on the behaviour of the system,
rather than inputs to fulfil requirements.

The first framework implementing to use Property-based testing with was
QuickCheck[^3] for Haskell. A Property-based testing framework has 3
main components: (1) a fuzzer, generating pseudo-randomly values, (2) a
sinker, which reduces in an algorithmic way the number of hypothesis for
the input dataset, and (3) the tools for making the construction of the
property-based tests with the fuzzer and the sinker.

# Property-based testing in C#

In the .NET world (C#, F# and VB.NET) the framework of choice nowadays
is FsCheck[^4].

FsCheck ticks all the three boxes and offers integration with the 2 of
the main .NET unit testing frameworks, xUnit and NUnit. This integration
allows for a faster learning curve for the development teams since they
do not need to learn yet another new tool, keep them focused on
delivering value for the system.

# FsCheck in action

Imagine the following scenario: our team developed a system to handle
the costs of a parcel shipment. The rules are straightforward; if the
total cost of the parcel equals or is higher than 20 euros, then the
parcel is entitled to free shipment.

The generation of the input datasets for our property-based tests can be
based on 2 methods, **Primitive Generation** or **Model Generation**.

# Primitive generation

The Primitive Generation is for the primitives offered by the language.
With C# we have bool, byte, sbyte, char, decimal, double, float, int,
uint, long, ulong, short, ushort and string (we are ignoring object,
given it is the base for the complex data structures).

To test the parcel shipment scenario, we will start testing the parcels
which have a price below 20 euro, thus are not entitled to free
shipment:

+-----------------------------------------------------------------------+
| public class WhenCalculatingParcelShipment                            |
|                                                                       |
| {                                                                     |
|                                                                       |
| \[Property(Arbitrary = new\[\] {typeof(ParcelPriceBelow20Euros)})\]   |
|                                                                       |
| public void                                                           |
| GivenParcelPriceIsBelow20Euros_ParcelShipmentIsNotFree(decimal        |
| parcelPrice)                                                          |
|                                                                       |
| {                                                                     |
|                                                                       |
| var postalService = new PostalService();                              |
|                                                                       |
| var isFreeShipment = postalService.IsFreeShipment(parcelPrice);       |
|                                                                       |
| Assert.Equal(false, isFreeShipment);                                  |
|                                                                       |
| }                                                                     |
|                                                                       |
| }                                                                     |
+=======================================================================+
+-----------------------------------------------------------------------+

Notice the Property attribute as an arbitrary for the test, where we
explicitly set the context for the input dataset generation. The
Arbitrary is responsible for generating the values for the feature under
test, and for this case is defined as:

+-----------------------------------------------------------------------+
| public class ParcelPriceBelow20Euros                                  |
|                                                                       |
| {                                                                     |
|                                                                       |
| public static Arbitrary\<decimal\> ParcelPrice() =\>                  |
|                                                                       |
| Arb.Default.Decimal().Generator.                                      |
|                                                                       |
| Where(x =\> x \> 0 && x \< 20).ToArbitrary();                         |
|                                                                       |
| }                                                                     |
+=======================================================================+
+-----------------------------------------------------------------------+

To complete the behaviour testing of the feature, we have a second test
focused on the parcels that are entitled to free shipment:

+-----------------------------------------------------------------------+
| public class WhenCalculatingParcelShipment                            |
|                                                                       |
| {                                                                     |
|                                                                       |
| \[Property(Arbitrary = new\[\]                                        |
| {typeof(ParcelPriceEqualOrAbove20Euros)})\]                           |
|                                                                       |
| public void                                                           |
| GivenParcelPriceIsEqualOrAbove20Euros_ParcelShipmentFree(decimal      |
| parcelPrice)                                                          |
|                                                                       |
| {                                                                     |
|                                                                       |
| var postalService = new PostalService();                              |
|                                                                       |
| var isFreeShipment = postalService.IsFreeShipment(parcelPrice);       |
|                                                                       |
| Assert.Equal(true, isFreeShipment);                                   |
|                                                                       |
| }                                                                     |
|                                                                       |
| }                                                                     |
|                                                                       |
| public class ParcelPriceEqualOrAbove20Euros                           |
|                                                                       |
| {                                                                     |
|                                                                       |
| public static Arbitrary\<decimal\> ParcelPrice() =\>                  |
|                                                                       |
| Arb.Default.Decimal().Generator.                                      |
|                                                                       |
| Where(x =\> x \>= 20).ToArbitrary();                                  |
|                                                                       |
| }                                                                     |
+=======================================================================+
+-----------------------------------------------------------------------+

# Model generation

Often our domain logic is implemented using domain models which is an
abstraction of the real world. For this we need to use model generation
(note that some properties and behaviour were omitted for brevity):

+-----------------------------------------------------------------------+
| public class Parcel                                                   |
|                                                                       |
| {                                                                     |
|                                                                       |
|    private readonly IEnumerable\<Item\> \_items;                      |
|                                                                       |
|    public double TotalPrice =\> \_items.Sum(x =\> x.Price);           |
|                                                                       |
|    public Parcel(IEnumerable\<Item\> items)                           |
|                                                                       |
|    {                                                                  |
|                                                                       |
|        \_items = items;                                               |
|                                                                       |
|    }                                                                  |
|                                                                       |
| }                                                                     |
|                                                                       |
| public struct Item                                                    |
|                                                                       |
| {                                                                     |
|                                                                       |
|    public double Price { get; }                                       |
|                                                                       |
|                                                                       |
|                                                                       |
|    public Item(double price)                                          |
|                                                                       |
|    {                                                                  |
|                                                                       |
|        Price = price;                                                 |
|                                                                       |
|    }                                                                  |
|                                                                       |
| }                                                                     |
|                                                                       |
| public class PostalService                                            |
|                                                                       |
| {                                                                     |
|                                                                       |
|    public bool IsFreeShipment(Parcel parcel)                          |
|                                                                       |
|    {                                                                  |
|                                                                       |
|        return parcel.TotalPrice \>= 20;                               |
|                                                                       |
|    }                                                                  |
|                                                                       |
| }                                                                     |
+=======================================================================+
+-----------------------------------------------------------------------+

Again, our first test will target the parcels that are not entitled to
free shipment:

+-----------------------------------------------------------------------+
| public class WhenCalculatingParcelShipment                            |
|                                                                       |
| {                                                                     |
|                                                                       |
|    \[Property(Arbitrary = new\[\]                                     |
| {typeof(ParcelPriceBelow20Euros)})\]                                  |
|                                                                       |
|    public void                                                        |
| GivenParcelPriceIsBelow20Euros_ParcelShipmentIsNotFree(Parcel parcel) |
|                                                                       |
|    {                                                                  |
|                                                                       |
|        var postalService = new PostalService();                       |
|                                                                       |
|        var isFreeShipment = postalService.IsFreeShipment(parcel);     |
|                                                                       |
|        Assert.Equal(false, isFreeShipment);                           |
|                                                                       |
|    }                                                                  |
|                                                                       |
| }                                                                     |
+=======================================================================+
+-----------------------------------------------------------------------+

With the dataset generator for the test as:

+-----------------------------------------------------------------------+
| public class ParcelPriceBelow20Euros                                  |
|                                                                       |
| {                                                                     |
|                                                                       |
|    public static Arbitrary\<Parcel\> Parcel()                         |
|                                                                       |
|    {                                                                  |
|                                                                       |
|        var input = from prices in Arb.Generate\<double\[\]\>()        |
|                                                                       |
|                    where prices.Sum() \> 0 && prices.Sum() \< 20      |
|                                                                       |
|                    select new Parcel(prices.Select(x =\> new          |
| Item(x)).ToArray());                                                  |
|                                                                       |
|                                                                       |
|                                                                       |
|        return input.ToArbitrary();                                    |
|                                                                       |
|    }                                                                  |
|                                                                       |
| }                                                                     |
+=======================================================================+
+-----------------------------------------------------------------------+

The complementary test, parcels that are entitled to free shipment:

+-----------------------------------------------------------------------+
| public class WhenCalculatingParcelShipment                            |
|                                                                       |
| {                                                                     |
|                                                                       |
|    \[Property(Arbitrary = new\[\]                                     |
| {typeof(ParcelPriceEqualOrAbove20Euros)})\]                           |
|                                                                       |
|    public void                                                        |
| GivenParcelPriceIsEqualOrAbove20Euros_ParcelShipmentFree(Parcel       |
| parcel)                                                               |
|                                                                       |
|    {                                                                  |
|                                                                       |
|        var postalService = new PostalService();                       |
|                                                                       |
|        var isFreeShipment = postalService.IsFreeShipment(parcel);     |
|                                                                       |
|        Assert.Equal(true, isFreeShipment);                            |
|                                                                       |
|    }                                                                  |
|                                                                       |
| }                                                                     |
|                                                                       |
| public class ParcelPriceEqualOrAbove20Euros                           |
|                                                                       |
| {                                                                     |
|                                                                       |
|    public static Arbitrary\<Parcel\> Parcel()                         |
|                                                                       |
|    {                                                                  |
|                                                                       |
|        var input = from prices in Arb.Generate\<double\[\]\>()        |
|                                                                       |
|                    where prices.Sum() \>= 20                          |
|                                                                       |
|                    select new Parcel(prices.Select(x =\> new          |
| Item(x)).ToArray());                                                  |
|                                                                       |
|                                                                       |
|                                                                       |
|        return input.ToArbitrary();                                    |
|                                                                       |
|    }                                                                  |
|                                                                       |
| }                                                                     |
+=======================================================================+
+-----------------------------------------------------------------------+

# Delayed feedback

For each time we run the tests, FsCheck will, by default, create 100
different inputs for one Property test. Because each tests get run
multiple times, this means we get delayed feedback on our unit tests.
The amount of time depends on the number of tests we use. FsCheck will
not linearly increase the unit test time so that it won't increase the
tests times a 100. Using the dotnet test \--logger:trx we can verify the
time that the tests take. On a MacBook Pro from 2017, we get:

![](./media/image6.png)


Not every Property-based test needs to be run 100 times either. In
FsCheck we can quickly change the default 100 times to another number of
our likings:

+-----------------------------------------------------------------------+
| \[Property(**MaxTest = 50**, Arbitrary = new\[\]                      |
| {typeof(ParcelPriceBelow20Euros)})\]                                  |
|                                                                       |
| public void                                                           |
| GivenParcelPriceIsBelow20Euros_ParcelShipmentIsNotFree(Parcel parcel) |
|                                                                       |
| {                                                                     |
|                                                                       |
| var postalService = new PostalService();                              |
|                                                                       |
| var isFreeShipment = postalService.IsFreeShipment(parcel);            |
|                                                                       |
| Assert.Equal(false, isFreeShipment);                                  |
|                                                                       |
| }                                                                     |
+=======================================================================+
+-----------------------------------------------------------------------+

Also, we don't need all tests to be FsCheck tests. Since FsCheck
integrates with xUnit and NUnit, we can combined the standard unit tests
with Property-based tests. Decide on every single test if we will use
FsCheck with the default value, a custom run value or just a standard
unit test.

# Deterministic vs Non-deterministic

A basic rule in testing is that we want our test to be deterministic,
meaning that the tests will always result in the same outcome with the
same input. With Property-based testing we are generating the data for
our tests, so the tests are non-deterministic. Non-deterministic tests
are not a problem as long as we can reproduce the errors that showed up.
So if we change our test to fail:

+-----------------------------------------------------------------------+
| public class PostalService                                            |
|                                                                       |
| {                                                                     |
|                                                                       |
| public bool IsFreeShipment(Parcel parcel)                             |
|                                                                       |
| {                                                                     |
|                                                                       |
| return parcel.TotalPrice **\>= 10**;                                  |
|                                                                       |
| }                                                                     |
|                                                                       |
| }                                                                     |
+=======================================================================+
+-----------------------------------------------------------------------+

We will get the following error:

+-----------------------------------------------------------------------+
| PropertyBasedTesting.Tests.Unit.WhenCalculating                       |
| ParcelShipment.GivenParcelPriceIsBelow20Euros_ParcelShipmentIsNotFree |
|                                                                       |
| FsCheck.Xunit.PropertyFailedException :                               |
|                                                                       |
| Falsifiable, after 5 tests (0 shrinks) (StdGen                        |
| (**610985339,296499972**)):                                           |
|                                                                       |
| Original:                                                             |
|                                                                       |
| PropertyBasedTesting.Tests.Unit.Parcel                                |
|                                                                       |
| \-\-\-- Assert.Equal() Failure                                        |
|                                                                       |
| Expected: False                                                       |
|                                                                       |
| Actual: True                                                          |
|                                                                       |
| \-\-\-\-- Inner Stack Trace \-\-\-\--                                 |
|                                                                       |
| at                                                                    |
| PropertyBasedTesting.Tests.Unit.WhenCalculatingParcelS                |
| hipment.GivenParcelPriceIsBelow20Euros_ParcelShipmentIsNotFree(Parcel |
| parcel) in                                                            |
| /Users/joaorosa/Documents/code/P                                      |
| ropertyBasedTesting/PropertyBasedTesting.Tests.Unit/UnitTest1.cs:line |
| 37                                                                    |
|                                                                       |
| \-\-- End of stack trace from previous location where exception was   |
| thrown \-\--                                                          |
|                                                                       |
| at FsCheck.Runner.invokeAndThrowInner@318-1.Invoke(Object\[\] o)      |
|                                                                       |
| at \<StartupCode\$FSharp-Core\>.\$Reflect.Invoke@820-4.Invoke(T1 inp) |
|                                                                       |
| at FsCheck.Testable.evaluate\[a,b\](FSharpFunc\`2 body, a a)          |
+=======================================================================+
+-----------------------------------------------------------------------+

Now we can use the StdGen tuple to seed an replay the failing test as:

+-----------------------------------------------------------------------+
| \[Property(Arbitrary = new\[\] {typeof(ParcelPriceBelow20Euros)}**,   |
| Replay = \"610985339,296499972\")**\]                                 |
|                                                                       |
| public void                                                           |
| GivenParcelPriceIsBelow20Euros_ParcelShipmentIsNotFree(Parcel parcel) |
|                                                                       |
| {                                                                     |
|                                                                       |
| var postalService = new PostalService();                              |
|                                                                       |
| var isFreeShipment = postalService.IsFreeShipment(parcel);            |
|                                                                       |
| Assert.Equal(false, isFreeShipment);                                  |
|                                                                       |
| }                                                                     |
+=======================================================================+
+-----------------------------------------------------------------------+

This way we can rerun our failed tests easily and fix the bug we just
uncovered, a bug we usually would not find until maybe perhaps a user or
even worse a hacker found!

# Works on my machine!

Using Property-based testing also means that a test someone else wrote
works on their machine, but fails on our machine, or on the build
server. A good practice is that we as a team will fix this and own all
the unit tests. Pairing or even when needed mob program the error and
learn from it as a team!

In this example, we used a simplistic view of the logistics domain. In
the real world, the domain logic usually is a lot more complicated, with
a lot of invariants. Modelling a domain is always essential, and we
would advise you to keep it simple. An approach you can use for this is
Domain Driven Design. Still writing code for generators can take a lot
of effort and time, definitely, but it will repay itself. When we create
generators for our domain model, the code will get written faster
because we don't need to be concerned with test data anymore. We only
need to configure the Property-based test to put the domain models in a
specific state. Property-based testing will increase the teams\' domain
knowledge, create more clear living documentation through the unit
tests, and improve design and code quality.

You can find the code of this article at
[[https://github.com/joaoasrosa/xpirit-magazine-property-based-testing]{.underline}](https://github.com/joaoasrosa/xpirit-magazine-property-based-testing).

[^1]: ^Black\ Box\ Thinking:\ Matthew\ Syed.\ [ISBN13]{.mark}\ 9781473613775^

[^2]: [[https://hypothesis.works/articles/what-is-property-based-testing/]{.underline}](https://hypothesis.works/articles/what-is-property-based-testing/)

[^3]: [[http://www.cse.chalmers.se/\~rjmh/QuickCheck/]{.underline}](http://www.cse.chalmers.se/~rjmh/QuickCheck/) -
    QuickCheck resources, visited on 31/08/2018

[^4]: [[https://fscheck.github.io/FsCheck/]{.underline}](https://fscheck.github.io/FsCheck/) -
    FsCheck resources, visited on 31/08/2018
