# .NET Multiple Windows (Async) Services

Sample project which registers multiple windows services.

- This sample project includes two windows services that are registered and can be run separately through the Windows Services manager.
- The project comes with `ServiceBaseAsync` class which two services inherit from. This class provides methods for running the main processing in asynchronous manner. 
- The project also comes with `EventLogExtension` which provides extension methods to write event entries based on log level settings that each service has. The log levels are defined in the `app.config` for each service.
- The `app.config` also defines a common event log name used by all the services, as well as the service names and descriptions of the services. 
The service names are used both by the project installer during service registration, and by the services themselves to define the event source.


### How To Use

A service inheriting from the ServiceBaseAsync class must implement the DoWorkAsync method. This method is aimed to contain the main processing done by the service. This method receives two parameters:
- Cancellation Token - this is the cancellation token for the service task. It is passed to this method so the method can end gracefully when the service is being stopped.
- Event Log - this is the event log the service uses. It can be optinally used within DoWorkAsync.

In the sample project, `ServiceOne` implements the DoWorkAsync, whilst ServiceTwo does not. So when `ServiceTwo` is started, it runs the `DoWorkAsync()` method from the `ServiceBaseAsync` class, and all that does is log throw a `NotImplementedException`. This is by default written in the event log by the `DoRecursiveWorkAsync()` method in the `ServiceBaseAsync` class.

### Service Registration

The project `.exe` can register and unregister the services.

- To register (install) the services:
   `MultipleWindowsServicesInOneProject.exe /i`
- To unregister (uninstall) the services:
   `MultipleWindowsServicesInOneProject.exe /u`


### License Info

MIT License.

