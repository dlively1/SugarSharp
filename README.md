SugarSharp
==========

Sugar7 C# Rest Client


Todo
* Finish data models for core objects
* Unit Testing


### Creating Connection

```c#
string url = "https://instanceurl/rest/v10/";
string username = "username";
string password = "password";

SugarClient Sugar = new SugarClient(url, username, password);

```


### Create Record

Create records with anonymous objects
```c#
string returnID = "";

List<string> multiselect_example = new List<string>();
multiselect_example.Add("A");
multiselect_example.Add("B");
multiselect_example.Add("C");

try
{
  returnID = Sugar.Create("Accounts", new
  {
      name = "Acme Inc",
      description = "full text description",
      phone_office = "555-555-5555",
      multiselect_example_c = SugarList.CreateMultiSelect(multiselect_example)
  });
}
catch (SugarException E)
{
  //handle exception
}
```

Create records asynchronously

```c#
SugarClient Sugar = new SugarClient(url, username, passsword);
            
object bean = new
{
  name = "Async Testing",
  phone_office = "333-443-3552",
  description = "Testing out async method calls from C#"
};


Sugar.CreateAsync("Accounts", bean, (success, recordId) =>
{
    if (success)
    {
      Console.WriteLine("1st result {0}", recordId);
    }

});
            
```


### Update Record

```c#
String recordID = "<<record_id>>";

try
{
  Sugar.Update("Accounts",recordID, new {
    description = "Add fields to update in annonymous object",
    phone_office = "555-333-3333"
  });
}
catch (SugarException E)
{
  //handle Exception
}

```

Update Records asynchronously

```c#

String recordID = "<<record_id>>";

object bean = new
{
  name = "Async Update Test Account",
  phone_office = "333-443-3552",
  description = "Update the record description"
};

Sugar.UpdateAsync("Accounts",recordID, bean, (success, recordId) =>
{
    if (success)
    {
        Console.WriteLine("1st result {0}", recordId);
    }

});

```

Check out the [Wiki Pages](https://github.com/dlively1/SugarSharp/wiki/_pages) page for more examples

