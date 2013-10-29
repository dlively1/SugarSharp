SugarSharp
==========

Sugar7 C# Rest Client


Todo
* Finish data models for core objects
* Unit Testing


###Creating Connection

```c#
string url = "https://instanceurl/rest/v10/";
string username = "username";
string password = "password";

SugarClient Sugar = new SugarClient(url, username, password);

```


###Create Record

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

Create records with defined object types

```c#
string returnID = "";

Account Account = new Account();
Account.name = "Acme Inc";
Account.description = "full text description";
Account.phone_office = "555-555-5555";

try
{
  returnID = Sugar.Create("Accounts", Account);
}
catch (SugarException E)
{
  //handle exception
}
```


###Update Record

```c#
string recordID = "<<record_id>>";

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

Check out the [Wiki Pages](https://github.com/dlively1/SugarSharp/wiki/_pages) page for more examples

