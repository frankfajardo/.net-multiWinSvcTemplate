# .NET Multiple Windows (Async) Services

Template for creating a project that registers multiple windows services for a single application.

- This template includes two windows services that are registered and can be run separately through the Windows Services manager.
- The project comes with `ServiceBaseAsync` class which two services inherit from. This class provides methods for running the main processing in asynchronous manner. 
- The project also comes with `EventLogExtension` which provides extension methods to write event entries based on log level settings that each service has. The log levels are defined in the `app.config` for each service.
- The `app.config` also defines a common event log name used by all the services, as well as the service names and descriptions of the services. 
The service names are used both by the project installer during service registration, and by the services themselves to define the event source.


### Service Registration

The project `.exe` can register and unregister the services.

- To register (install) the services:
   `MultipleWindowsServicesInOneProject.exe /i`
- To unregister (uninstall) the services:
   `MultipleWindowsServicesInOneProject.exe /u`


### License Info

MIT License.

