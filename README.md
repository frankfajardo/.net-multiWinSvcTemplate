# .NET Multiple Windows (Async) Services

Sample project which registers multiple windows services.

- This sample project includes two windows services that are registered and can be run separately through the Windows Services manager.
- The project comes with `ServiceBaseAsync` class which two services inherit from. This class provides methods for running the main processing in asynchronous manner. 
- The project also comes with `EventLogExtension` which provides extension methods to write event entries based on log level settings that each service has. The log levels are defined in the `app.config` for each service.
- The `app.config` also defines a common event log name used by all the services, as well as the service names and descriptions of the services. 
The service names are used both by the project installer during service registration, and by the services themselves to define the event source.


### How To Use

To create a new service that runs is processing asynchronously, the service must:
- inherit from `ServiceBaseAsync` class
- implement the method `DoWorkAsync()`.

The `DoWorkAsync` method receives two parameters:
- **Cancellation Token** - this is the cancellation token for the service task. It is passed to this method so the method can end gracefully when the service is being stopped.
- **Event Log** - this is the event log the service uses. It can be optinally used within DoWorkAsync.

In the sample project, there are two **sample services**:
- **ServiceOne** implements `DoWorkAsync()`, by writing an event log. So this is executed recursively.
- **ServiceTwo** does not implement `DoWorkAsync()`. So when it is started, it runs the `DoWorkAsync()` method from the base class---which simply throws a `NotImplementedException`. This exception is by default written in the event log by the method `DoRecursiveWorkAsync()` in the base class.

### Service Registration

The project `.exe` can register and unregister the services.

- To register (install) the services:
   `MultipleWindowsServicesInOneProject.exe /i`
- To unregister (uninstall) the services:
   `MultipleWindowsServicesInOneProject.exe /u`


### License Info

MIT License.

