#Glimpse for SignalR

---

### What does this package do?

This package adds SignalR diagnostics to Glimpse. After installing Glimpse for SignalR, two tabs will be added to the Glimpse UI. The first tab contains type information for all your SignalR hubs, the second tab lists diagnostics of all invocations performed on your SignalR hubs.

### What kind of diagnostics does this package give me?

##### SignalR - Hubs

This tab display type information for all your hubs, this includes:

- The hub name and the name of the class that represents the hub
- A list of its methods
- For each method:
	- The return type
	- Its parameters, including parameter names and types

![](https://raw.github.com/stevenlauwers22/Glimpse.SignalR/master/Documentation/Hubs.png)

##### SignalR - Invocations

This tab show a list of all invocation that were performed on your SignalR hubs. The information you get is the following:

- The hub on which an invocation is performed
- The method that was invoked
- The value returned by the method
- Any arguments passed to the method
- A date indicating the time of invocation
- The duration of the invocation in milliseconds
- The identifier of the connection that invoked the method

![](https://raw.github.com/stevenlauwers22/Glimpse.SignalR/master/Documentation/Invocations.png)

### How do I install this package?

* From NuGet: `Install-Package Glimpse.SignalR`

### Any extension points?

Yes, there is one!  
By default all invocation diagnostics are stored in an in-memory list. In some cases this will not always yield correct results (e.g. in a load balanced environment).

In a next version of this pluging we might be able to piggy back on the persistence mechanism that's built into Glimpse. For now, you'll have to provide your own persistence logic. Luckily this is very easy. A little example:

    Glimpse.SignalR.Invocations.PluginSettings.GetInvocations = () =>
    {
		// TODO: put in your own persistence logic, could look something this:
        var invocations = Database.ReadAllInvocations();
        return invocations;
    };

    Glimpse.SignalR.Invocations.PluginSettings.StoreInvocation = invocation =>
    {
		// TODO: put in your own persistence logic, could look something this:
        Database.StoreInvocation(invocation);
    };

The application_start would be a good place to register your own implementation of these delegates.
