# .NET Multiple Windows (Async) Services


### License Info

MIT License.


### Overview

- This sample project includes two windows services that are registered and can be run separately through the Windows Services manager. These services are belonging to a single application so they are considered related in that way. But they can operate differently or perform mutually independent jobs.

- The services are designed to perform their jobs asynchronously. This allows for an elegant way of stopping the service, IMHO.

- The project comes with `ServiceBaseAsync` (abstract) class. This class provides methods for running the main processing recursively in asynchronous manner. It also has helper methods for setting up event logging, and handling or propagating errors during the asynchronous processing.

- The project also comes with `EventLogExtension` which provides extension methods to write event entries based on log level settings that each service has, so each service can log as little or as much as it needs. The log levels are defined in the `app.config` for each service.

- The `app.config` also defines a common event log name used by all the services, as well as the service names and descriptions of the services. The service names are used by the project installer during service registration, and by the services themselves when defining event log source.


### How to Create a New Service

To create a new service that performs asynchronous processing:

- Create a new service class, but inherit from `ServiceBaseAsync` class, instead of `ServiceBase`.

- In your service constructor, define your event log using the `SetEventLog()` base method. Also define your `runIntervalMinutes` property.

- Override the method `DoWorkAsync()`. A typical implementation would look like this:
```
protected override async Task DoWorkAsync(
    CancellationToken token, 
    EventLog eventLog)
{
    await Task.Run(() =>
    {
        // Do your stuff here
    });
}
```
#### About `DoWorkAsync()`

The `DoWorkAsync()` method is intended to perform the main processing of your service. It is called recursively and in intervals by `DoRecursiveWorkAsync()`. As soon as your service runs, `DoWorkAsync()` is run. When it returns, `DoRecursiveWorkAsync()` stays idle---by performing a `Task.Delay()`---for a number of minutes defined by the `runIntervalMinutes` property of your service. Afterwards, it runs `DoWorkAsync()` again.  

In the event that `DoWorkAsync()` throws an exception, `DoRecursiveWorkAsync()` logs the exception details in the event log, and then moves on. `ServiceBaseAsync` has a default implementation of `DoWorkAsync()`. which simply throws a `NotImplementedException`.

`DoWorkAsync()` has two input parameters:

1. *Cancellation Token* 
    
    This is the cancellation token for the service task. It is passed to this method so anytime `OnStop()` runs, this method gets the cancellation request and can end gracefully.

2. *Event Log*
    
     This is the event log the service uses. `DoWorkAsync()` can optionally write logs to this.


#### About the Sample Services

This project includes these services:

- *ServiceOne* 
  
  This implements `DoWorkAsync()`, and all it does is write an entry in the events log. So this is executed recursively.
  
- *ServiceTwo* 
  
  This does not implement `DoWorkAsync()`. So when it runs, it will write the `NotImplementedException` exception in the event log, until you stop the service. 


### How to Add Installer for A New Service

- Update the `ProjectInstaller.cs` and add a service installer and a service process installer for the new service you are adding. 

- Copy the code for either of the two sample services. Revise names accordingly.

- Add the relevant `appSettings` entries in the `app.config` for your service.
 

### Installing the Services

- Build this project using the *Release* configuration.

- Copy the content of `bin/Release` from this project onto a location on your computer where you want the service to run from.

- Launch `cmd.exe`. Go to the directory where you copied the content of `bin/Release`.

- To install (or register) the windows services, run the command: 

 `MultipleWindowsServicesInOneProject.exe /i`


### Un-installing the Services

- Launch `cmd.exe`. Go to the directory where your service runs from.

- To uninstall (or unregister) the windows services, run the command: 

 `MultipleWindowsServicesInOneProject.exe /u`

 