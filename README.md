# .NET Multiple Windows (Async) Services


### License Info

MIT License.


### Overview

- This sample project includes two windows services that are registered and can be run separately through the Windows Services manager. These services are belonging to a single application so they are considered related in that way. But they can operate differently or perform mutually independent jobs.
- The services are designed to perform their jobs asynchronously. This allows for an elegant way of stopping the service, IMHO.
- The project comes with `ServiceBaseAsync` (abstract) class. This class provides methods for running the main processing recursively in asynchronous manner. It also has helper methods for setting up event logging, and handling or propagating errors during the asynchronous processing.
- The project also comes with `EventLogExtension` which provides extension methods to write event entries based on log level settings that each service has, so each service can log as little or as much as it needs. The log levels are defined in the `app.config` for each service.
- The `app.config` also defines a common event log name used by all the services, as well as the service names and descriptions of the services. The service names are used by the project installer during service registration, and by the services themselves when defining event log source.


### How To Create a New Service

To create a new service that runs performs asynchronous processing, the service must:
- inherit from `ServiceBaseAsync` class
- implement the method `DoWorkAsync()`.

The `DoWorkAsync()` method is intended to have the run the main processing of the service. It is called recursively by `DoRecursiveWorkAsync()`, in intervals defined by the service's `runIntervalMinutes` property. 

The `DoWorkAsync()` method receives two parameters:
receives two parameters:
1. **Cancellation Token** - this is the cancellation token for the service task. It is passed to this method so the method can end gracefully when the service is being stopped.
2. **Event Log** - this is the event log the service uses. It can be optinally used within DoWorkAsync.


#### About the Sample Services 

This project includes these services:
- *ServiceOne* implements `DoWorkAsync()`, and all it does is write an entry in the events log. So this is executed recursively.
- *ServiceTwo* does not implement `DoWorkAsync()`. So when it runs, gets a `NotImplementedException`, which is default implementation of the method. This exception is handled by the base class method `DoRecursiveWorkAsync()`, which writes it in the events log. And since `DoWorkAsync()` is called recursively until the service is stopped, that exception will appear in the events log multiple times.


### How To Add Installer for A New Service

- Update the `ProjectInstaller.cs` and add a service installer and a service process installer for the new service you are adding. 
- Copy the code for either of the two sample services.
- Add the relevant entries in the `app.config` for your service.
 

### Installing the Services

- Build this project using the *Release* build.
- Copy the content of `bin/Release` on this projectonto a location on your computer where you want the service to run from.
- Launch `cmd.exe`.
- On the command window, change directory to the location where you copied the content of `bin/Release`.
- To install (or register) the windows services, run the command: `MultipleWindowsServicesInOneProject.exe /i`


### Un-installing the Services

- Launch `cmd.exe`.
- On the command window, change directory to the location where you the service files.
- To uninstall (or un-register) the windows services, run the command: `MultipleWindowsServicesInOneProject.exe /u`

 