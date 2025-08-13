## Implementing SQL Merge functionality into Entity Framework Core

In the previous article [Extending Entity Framework Core](https://xebia.com/wp-content/uploads/2024/02/Xebia_Xpirit_XPRT_magazine_Final.pdf), we touched upon the subject of extending Entity Framework Core (EF core) to automatically generate custom statements. We wrapped up the previous article with a basic solution that you can build upon to create your own custom code to be generated. In this article we will be implementing a SQL merge statement in EF core. By leveraging EF core you can avoid writing the SQL merge code for every object you want to use it for. The SQL merge statement is an alternative to the Insert, Update and Delete statement when dealing with large datasets. In combination with EF core the SQL merge statement can lower the amount of round trips needed to the database to a minimum. 

### SQL Merge
The SQL merge statement is used to effectively synchronize two different datasets. An example can be found in data warehousing where updates to bigger datasets are often performed. With the SQL merge statement a target table can be synchronized with a source table. The power of the merge statement is it combines the Insert, Update and delete statement in a single atomic statement. This way it is easier to maintain the integrity and consistency of the data. 
The merge statement consists of the following elements:
*	Target and Source Table
*	Condition on which to join the tables (For example, primary key)
*	The action to take (Insert, Update, Delete)

The target and source tables can be SQL tables or, for example, a table type. The combination of a Table and a Table type is what we will use in the current example. The table type we need to declare is almost a copy of the table in the database. It contains an additional column that will be used for the potential deletion of data. The next elements are the Condition and the Action to take.

```
MERGE INTO TargetTable AS Target
USING SourceTable AS Source
ON Target.Id = Source.Id
-- Update existing rows
WHEN MATCHED AND Source.MustDelete = 0 THEN
    UPDATE SET
    Target.Counter = Source.Counter,
    Target.CreationDate = Source.CreationDate
-- Delete rows from target if MustDelete is true
WHEN MATCHED AND Source.MustDelete = 1 THEN
    DELETE
-- Insert new rows from source to target
WHEN NOT MATCHED BY TARGET THEN
    INSERT (Id, Counter, CreationDate)
    VALUES (Source.Id, Source.Counter, Source.CreationDate);

```

This example shows the following condition: On Target.Id = Source.Id. This is the condition on which the merge statement will try to match the data. In this case it is the primary key. The next part of the code are the actions. These consists of `When Matched (Potential additional condition)` and a `When Not Matched`. You can see that when the id can be matched and the additional Must delete condition is false, the found record will be updated. When the ```Source.MustDelete = 1``` condition is true then the row will be deleted from the target table. The last action is inserting a record when the condition cannot be matched. 
To ensure the merge statement can be used together with EF Core, its output is also needed. The output contains possible created ids or other database generated fields. This can be done by using the ```OUTPUT``` statement in SQL. 

```
-- Capture the output of the merge operation
OUTPUT 
    $action, 
    INSERTED.Id AS NewId, 
    DELETED.Id AS OldId, 
    INSERTED.Counter, 
    INSERTED.CreationDate;
```

The output example contains the inserted fields, the potential delete, and an ```$action```. The $action is a field resulting from the Merge statement telling us if the action was an Insert, Update, or Delete. This can be useful for logic in your output statement. I prefer to avoid additional logic here and let the C# code resolve any additional logic I want to perform. 
Now there are a couple of advantages to a merge statement. The single atomic action which in combination with EF Core is an even greater advantage because you only need a single round trip to the database. This will result in an even better performance boost. 
With every advantage comes a disadvantage. The merge is not only advantageous. One big risk when using big datasets is locking the table. Especially when the Join condition is not properly indexed then the performance can greatly suffer which in turn increases the risk of deadlocks. Another issue is portability. Merge statement syntax and working tends to differ between different types of databases. 

### Implementing the Merge statement in EF Core.
To implement a merge statement into EF COre. The following classes need to be adjusted: ```CSharpMergeMigrationOperationGenerator``` and the ```SQLServerMergeMigrationSQLGenerator```. The ```CSharpMergeMigrationOperationGenerator``` is responsible for the Migration file. The ```SQLServerMergeMigrationSQLGenerator``` is responsible for the SQL script. In the previous article the skeleton was already provided for these generators, this will now be further expanded upon. The difficulty lies mainly in the Migration generator. The migration generator created the migration.cs file and should include the merge statements with all the necessary information. Such as column names and table name. That means that the migration code should contain the statement to create the columns with their specific information. To achieve this a function can be used. The function will contain a column builder as output and as input the column information.

```
Func<ColumnsBuilder, TColumn> columns
```

This will look as follows in the migration file:

```
migrationBuilder.CreateMerge(
   name: "Forecasts",
   columns: table => new {
      Id = table.Column<System.Guid>(type: "uniqueidentifier", nullable: false),
      Date = table.Column<System.DateTime>(type: "datetime2", nullable: false),
      Summary = table.Column<System.String>(type: "nvarchar(max)", nullable: false),
      TemperatureC = table.Column<System.Int32>(type: "int", nullable: false)
   });

```

The migrationBuilder extension method ‘CreateMerge’ will look as follows:

```
public static OperationBuilder<CreateMergeOperation> CreateMerge<TColumns>(
    this MigrationBuilder migrationBuilder,
    string name,
    Func<ColumnsBuilder, TColumns> columns)
{
    var operation = new CreateMergeOperation(name, new List<AddColumnOperation>());
    var builder = new ColumnsBuilder(operation);
    var columnsObject = columns(builder);
    foreach (var property in typeof(TColumns).GetTypeInfo().DeclaredProperties)
    {
        var addColumnOperation = ((AddColumnOperation)property.GetMethod!.Invoke(columnsObject, null)!);
        addColumnOperation.Name = property.Name;
        operation.Columns.Add(addColumnOperation);
    }
    migrationBuilder.Operations.Add(operation);
    return new OperationBuilder<CreateMergeOperation>(operation);
}

```

The ColumnsBuilder contains a Column method which is called here using a reflection implementation. The column method returns the AddColumnOperation which contains all the necessary information to create a new column. In our case, specifically for the table type and the merge statement.
Now during the migration of the database we will get the MergeOperations which the ```SQLServerMergeMigrationSQLGenerator``` can convert into SQL script.

### Implementing the SQL generator.
This is the easier part of the process. In the previous article, we adjusted this generator to be able to handle the MergeOperations. Now that the merge operation is implemented in the Migration we can actually implement the SQL generator. Using the table name and the columns from the merge operation we have all the information to create the stored procedure. Here follows a short example of how the SQL generator can be futher implemented.

```
builder.AppendLine("CREATE STORED PROCEDURE [dbo].[Merge_" + operation.TableName + "]");
using (builder.Indent())
{
    builder.AppendLine($"@SourceTable dbo.{operation.TableName}Type READONLY");
}
builder.AppendLine("AS");
builder.AppendLine("BEGIN");
using (builder.Indent())
{
    builder.AppendLine("MERGE INTO " + operation.TableName + " AS Target");
    builder.AppendLine("USING @SourceTable AS Source");
    builder.AppendLine("ON Target.Id = Source.Id");
    builder.AppendLine("WHEN MATCHED AND Source.ShouldDelete = 0 THEN");
    builder.AppendLine("UPDATE SET");
………
    builder.AppendLine();
    builder.AppendLine("OUTPUT $action, Inserted.Id, Deleted.Id;");
}

builder.EndCommand();

```

### Calling the merge during runtime.
The SQL merge statement extension we just implemented into EF core will be created during the design time command:  (```dotnet ef migrations add merge```). Afterwards, when you migrate your database these new SQL merge statements will be added to your database. If you prefer to first take a look at the SQL script which will be generated then you can first call ```dotnet ef migrations script```.
With the current implementation this will give the following output:
```
IF TYPE_ID('dbo.ForecastsType') IS NOT NULL
BEGIN
    DROP TYPE dbo.ForecastsType;
GO

IF OBJECT_ID('dbo.Merge_Forecasts') IS NOT NULL
BEGIN
    DROP PROCEDURE dbo.Merge_Forecasts;
GO

CREATE TYPE dbo.ForecastsType AS TABLE
(
    Id uniqueidentifier NOT NULL,
    Date datetime2 NOT NULL,
    Summary nvarchar(max) NOT NULL,
    TemperatureC int NOT NULLShouldDelete bit
);
GO

CREATE STORED PROCEDURE [dbo].[Merge_Forecasts]
    @SourceTable dbo.ForecastsType READONLY
AS
BEGIN
    MERGE INTO Forecasts AS Target
    USING @SourceTable AS Source
    ON Target.Id = Source.Id
    WHEN MATCHED AND Source.ShouldDelete = 0 THEN
    UPDATE SET
        Target.Id = Source.Id,
        Target.Date = Source.Date,
        Target.Summary = Source.Summary,
        Target.TemperatureC = Source.TemperatureC
    WHEN MATCHED AND Source.ShouldDelete = 1 THEN
        DELETE
    WHEN NOT MATCHED BY TARGET THEN
        INSERT (Id, Name, Description)
        VALUES (Source.Id,Source.Date,Source.Summary,Source.TemperatureC);

    OUTPUT $action, Inserted.Id, Deleted.Id;
GO
```
To avoid complexity the code first drops the table type and stored procedure to avoid complex alter statements. The next step is to call the SQL merge statements during run time. To achieve that an extension on the DbContext called Merge and MergeAsync can be a solution. I would advise against overriding the SaveChanges method. The merge statement should be used when there is a dataset large enough to make it useful to perform. The SaveChanges could be overridden to perform a merge when the dataset becomes large enough. But I would advise against that. It is arbitrary on which number a merge would be faster than a standard insert, update or delete. I prefer to leave that decision with the implementing party. 
The current implementation is very similar to the ExecuteUpdate and ExecuteDelete which are available since EF Core version 7.0. But the merge statement can be further extended than the Execute methods. The merge statement can be chained, so that the whole tree can be saved. This can be done by first saving the child objects, easily recognizable using the foreign keys. Updating the foreign keys in the root objects, then saving the root objects. Finally, updating the foreign keys in the child collection objects, then saving the child collection objects. In order to correctly update the foreign keys, an internal Id will need to be sent with the merge statement as well. This way a mapping can be maintained to update the foreign keys with their correct root objects. 

### Wrapping Up: SQL Merge Meets EF Core

Alright, we've come a long way! From just talking about extending EF Core to actually getting our hands dirty with SQL Merge. This article was about making life easier when dealing with big chunks of data. We showed how SQL Merge can be a game-changer, combining inserting, updating, and deleting into one neat package. This means less back and forth with the database.

We dove into how to set this up in EF Core, tweaking some classes like ```CSharpMergeMigrationOperationGenerator``` and ```SQLServerMergeMigrationSQLGenerator```. It was all about getting these parts to generate the SQL Merge statement, to avoid writing that code yourself.

Finally, we touched on how this all comes together at runtime. It’s not just about setting things up; it’s about using them effectively. We suggested some smart ways to do this, like the Merge and MergeAsync methods in DbContext.