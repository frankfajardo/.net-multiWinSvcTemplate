# .net-multiWinSvcTemplate

Template for creating a project that registers multiple windows services for a single application.

- This template includes two windows services that are registered and can be run separately through the Windows Services manager.
- The project comes with an `app.config` which allows you to place the name of each service, so the service names could be referred to, by the installer, and also by the service itself (as the name the installer uses must match the name the service uses). 
- The `app.config` includes descriptions for the services which is picked up by the installer during registration of the services.
- The `app.config` also includes an EventLogName which is shared by the services. So all services log events using the same Event Log Name, and only the Event Sources are different. Each service uses its name as the Event Source.


### Service Registration

The project `.exe` can register and unregister the services.

- To register (install) the services:
   `MultipleWindowsServicesInOneProject.exe /i`
- To unregister (uninstall) the services:
   `MultipleWindowsServicesInOneProject.exe /u`


### License Info

MIT License.

