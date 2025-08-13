## Extending Entity Framework Core

Author Victor de Baare

Entity Framework Core offers a broad framework to help create your
database schema, and then store and access your data in said database.
But what if you want to do more than what Entity Framework Core offers
out of the box? What if you have an edge case that needs support?
Extending Entity Framework Core might be a solution.

## What is Entity Framework Core?

Entity Framework Core (EF core) is a Microsoft supported open-source
object-relational mapping (ORM) framework. As the name implies it helps
developers with persisting and accessing data which resides in a
database without being encumbered with converting it from the database
DataSet\`s to .NET objects. Additionally, it also helps with creating a
database schema based on the .NET objects, or the other way around,
creating .NET objects based on your database schema. In summary, it
eliminates most of the data-access and data-persisting code and frees up
the developer's time to focus on more important areas of the code.

We will now focus on the persistence of data in your database,
particularly a SQL database. When you decide to save data to your
database, EF core will do this by using an insert, update or delete SQL
statement. Often this is a \"good enough" approach, but in some
situations you might want to use a merge statement instead. For example,
on every insert, update or delete statement a round-trip to the database
is performed. With a few records this won\'t impose any issue. When the
amount of records increases the numerous round-trips will slow down the
application. To avoid the performance degration a merge statement could
be used. A merge statemant would create a single round-trip to the
database for every type of object which needs to be inserted, updated or
deleted regardless of the amount of records.

To avoid going back to writing the code yourself for every object, you
can write an extension on EF core. This way you would only need to write
the code once and afterwards you\'ll let EF core do the heavy lifting.
This article will dive deeper into how you can extend EF core. In the
next article we will dive deeper into using merge statements. To do this
you\'ll first need to know how a migration in EF core works.

## Migrations

EF core uses migrations to update the database Schema based on the data
model changes that the developer made. EF core compares the data model
changes that the developer made with a snapshot of the old model that is
known to EF core. Based on the snapshot ,a migration file is generated
to describe the operations which are needed to move from the old model
towards the new model. The individual migration files are saved to a
history table in the database. This way EF core can track which
migrations have been applied and which have not, which is important if
you want to update your database and have multiple migrations to apply.

The migration takes place in two steps: the creation of a migration file
based on your changes and updating your database schema. To create a
migration file you can use the .NET EF core console commands. This will
result in calling the CSharp part of the migration, the
CSharpMigrationGenerator. We will discuss CSharpMigrationGenerator later
in the article. The CSharpMigrationGenerator will generate migration
operations based on the changes that have been made. The changes will be
persisted in a migration.cs file in which you can add extra operations
or extra custom SQL. Updating the database is often performed during
startup of your application. We will first discuss how we can extend the
SQL script generation of EF core.

![](./media/image1.png)


## Extending the MigrationBuilder API.

The MigrationBuilder builds the migration and contains many different
operations. But it cannot contain everything a developer might want to
do. To accommodate the option of creating your own operations, the
MigrationBuilder can be extended! You can use the \`\`\`sql()\`\`\`
method to write your own little piece of code into a migration. The
\`\`\`sql()\`\`\` method can be useful if a developer wants to update
some records before or after a migration is performed. Another
possibility is writing your own custom migration operations to extend
the migrations.

If you decide to create your own custom migrations operations, the first
step is declaring your own CustomMigrationSqlGenerator and making it
inherit from the SqlServerMigrationSqlGenerator.

\`\`\`

public class CustomMigrationSqlGenerator:
SqlServerMigrationsSqlGenerator

{

public CustomMigrationSqlGenerator(

MigrationsSqlGeneratorDependencies dependencies,

ICommandBatchPreparer commandBatchPreparer)

: base(dependencies, commandBatchPreparer)

{

}

protected override void Generate(

MigrationOperation operation,

IModel model,

MigrationCommandListBuilder builder)

{

case CreateMergeOperation createoperation:

builder.AppendLine(\"\-\--Write your SQL code here\");

builder.EndCommand();

break;

case DropMergeOperation dropoperation:

builder.AppendLine(\"\-\--Write your SQL code here\");

builder.EndCommand();

break;

default:

base.Generate(operation, model, builder);

break;

}

}

\`\`\`

The next step is registering your custom implementation of the SQL
generator in your EF core DbContext. After registering your custom
implementation and running a migration, your custom generator will be
used instead of the default generator for generating your migration
code.

\`\`\`

protected override void OnConfiguring(DbContextOptionsBuilder options)

=\> options

.UseSqlServer(\_connectionString)

.ReplaceService\<IMigrationsSqlGenerator, MyMigrationsSqlGenerator\>();

\`\`\`

Now, all the necessary steps are done to implement your own custom
operations. For example, if a merge operation needs to be created in the
database you can create a CreateMergeOperation and add this to the
CustomMigrationSqlGenerator.Generate method. Then you can add the code
to an existing migration so the operation is added to the SQL script
when the migration is executed.

\`\`\`

///Generated Migration.cs

protected override void Up(MigrationBuilder migrationBuilder)

{

migrationBuilder.CreateMerge('TableName', (columnsBuilder) =\> new
{\...});

}

\`\`\`

\`\`\`

///CustomMigrationBuilderExtensions.cs

public static OperationBuilder\<CreateMergeOperation\> CreateMerge(

this MigrationBuilder migrationBuilder,

string tableName,

Func\<ColumnsBuilder, TColumns\> columns)

{

var operation = new CreateMergeOperation(tableName, columns);

migrationBuilder.Operations.Add(operation);

return new OperationBuilder\<CreateMergeOperation\>(operation);

}

\`\`\`

The implementation still requires the developer to add custom code after
performing the first part of the migration. Otherwise the migration
wouldn\'t know another operation should be executed. A better
implementation would be to add annotations to the entities you want to
get custom operations on. During the generation of the operations you
can check if an entity has a specific annotation. If the annotation is
present you can execute the custom code.

With the SQL generator approach it still only works in the second part
of the migrations: the SQL script generation. The preference would be to
automatically add custom operations based on annotations to the first
part of the migration. That way you would have nothing to do besides
adding an annotation in the configuration of the model. Afterwards you
would be able to see the new operations in the migration class that is
generated in the first step.

To achieve this you would need to extend more services that are used for
the migration. The MigrationsModelDiffer.cs and the
CSharpMigrationOperationGenerator.cs are the two services that you would
need to extend. We will discuss how to extend these services in the
following paragraphs.

## MigrationsModelDiffer

As the name implies this part of the migration will check for any
differences between the new model and the old model. The beautiful part
is you can actually include the check on potential custom annotations
for the differences, if you want to include custom code when an
annotation is present. You can also use this diff method to remove the
custom code when the annotation is no longer present. For implementation
you would have to create a class inheriting from the
MigrationsModelDiffer.cs class and then override the following method:

\`\`\`

/// CustomMigrationsModelDiffer.cs

public override IReadOnlyList\<MigrationOperation\>
GetDifferences(IRelationalModel? source, IRelationalModel? target)

\`\`\`

The method is called by the migration to get the differences between the
target and source entities. By overriding this we can check the target
and source for the annotation and if changes are present we can instruct
it to create an operation to update the database schema with the changes
that we would like to see. The operations that are created here will be
passed towards the next step in the migration; the
CSharpMigrationOperationGenerator.

An example of the implementation for the custom operations is as
follows:

\`\`\`

public override IReadOnlyList\<MigrationOperation\>
GetDifferences(IRelationalModel? source, IRelationalModel? target)

{

var sourceTypes = GetEntityTypesContainingMergeAnnotation(source);

var targetTypes = GetEntityTypesContainingMergeAnnotation(target);

var diffContext = new DiffContext();

var customOperations = DiffCollection(source, target, diffContext, Diff,
Add, Remove, (x, y, diff) =\> x.Name.Equals(y.Name,
StringComparison.CurrentCultureIgnoreCase));

return base.GetDifferences(source,
target).Concat(customOperations).ToList();

}

\`\`\`

The DiffCollections is an internal API method that helps make sure an
operation is only created once for the target and source combination.
The add and remove parameters in the DiffCollection are the functions
which contain the knowledge on how to create the custom operations.

The last step that we need to do is to register the ModelDiffer in the
service collection. This service must be registered with the DbContext
as well. From here we can extend the previous code to include the model
differ service.

\`\`\`

protected override void OnConfiguring(DbContextOptionsBuilder options)

=\> options

.UseSqlServer(\_connectionString)

.ReplaceService\<IMigrationsModelDiffer, CustomMigrationsModelDiffer\>()

.ReplaceService\<IMigrationsSqlGenerator, MyMigrationsSqlGenerator\>();

\`\`\`

## CSharp Migration Operation Generator

The CSharpMigrationOperationGenerator will provide the .NET code which
is parsed in the migration class. With the new operations included, the
Csharp generator can now be extended to know what to do when such an
operation is encountered. An important part here to remember is that the
migrations are done in two parts which both are run at different times.
For example the migration of the Csharp code is run during design time.
The SQL migration is often performed during run time.

For the SQL migration this means we can use the DbContext
OnConfiguration method to register the needed overrides to create the
correct SQL script. The CsharpMigrationOperationGenerator, which runs
during design time, is implemented differently. To make sure that your
customer CsharpMigrationOperationGenerator can be called, adjust the
imported Microsoft.EntityFrameworkCore.Design NuGet package.

When you import this package it automatically ensures that certain
design time services are not exposed for your code. The
CSharpMigrationOperationGenerator is part of the services which are
filtered away. To actually be able to use all the services you should
adjust the code in your .csproj file a bit.

For example when you import the package you get the following:

\`\`\`

\<PackageReference Include=\"Microsoft.EntityFrameworkCore.Design\"
Version=\"7.0.2\"\>

\<PrivateAssets\>all\</PrivateAssets\>

\<IncludeAssets\>runtime; build; native; contentfiles; analyzers;
buildtransitive

\</IncludeAssets\>

\</PackageReference\>

\`\`\`

Now you will need to adjust this to the following:

\`\`\`

\<PackageReference Include=\"Microsoft.EntityFrameworkCore.Design\"
Version=\"7.0.2\"\>

\<PrivateAssets\>all\</PrivateAssets\>

\</PackageReference\>

\`\`\`

Once done you can extend the CSharpMigrationOperationGenerator with your
own code. Before we start extending the generator, we must make sure the
generator can be found during a migration. For the SQL generator we
could use the OnConfiguration method of the DbContext. Due to the fact
that the CSharp generator is a design time implementation, the SQL
generator solution won\'t work. An alternative for this is using the
DesignTimeServices.

For DesignTimeServices, Entity Framework has a separate interface that
needs to be included. The interface exposes the \`\`\`

ConfigureDesignTimeServices(IServiceCollection)

\`\`\`

method. EF core scans the start-up project for this interface to
register potential design time services. If you want to ship potential
extensions in a NuGet package the scanning for an interface might be a
problem. The solution to this issue is using an assembly attribute in
your startup project which contains the name of the class which
implements the interface and the full namespace.

\`\`\`

\[DesignTimeServicesReference(\"TypeName\", \"ForProvider\")\]

\`\`\`

The typename parameter is the assembly-qualified name to be added to the
servicecollection. The \`ForProvider\` parameter is the name of the
provider for which the DesignTimeServices should be used. This parameter
is nullable, when left empty the service is added for all the present
providers.\
With the interface in place we can start implementing the CSharp
generator. The implementation is very similar to that of the SQL
generator. We will be overriding the \`\`\`\`protected virtual void
Generate(MigrationOperation operation, IndentedStringBuilder
builder)\`\`\` method. The MigrationsModelDiffer will be doing the work
to provide the custom operations so the CSharp generator should work in
providing the CSharp code of the implementation. In this case when we
get an operation of the \`\`\`CreateMergeOperation\`\`\` type we want to
perform the following additional code:

\`\`\`

private static void Generate(CreateMergeOperation operation,
IndentedStringBuilder builder)

{

builder.AppendLine(\$\".CreateMerge(");

using(builder.Indent()) {

builder.AppendLine(\$"\.....");

}

}

\`\`\`

If you generate a new migration, you will see the migration will contain
the CreateMerge statement now.

With this the whole chain is completed. We can now just add an
annotation to the entities in the modelbuilder method on the DbContext.
This will result in the merge statements being generated on the next
migration. An example of the source code for generating customer
operations can be found here:
https://github.com/VictordeBaare/EntityFrameworkExtensions
