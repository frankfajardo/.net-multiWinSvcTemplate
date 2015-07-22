# .net-multiWinSvcTemplate
Template for creating a project that registers multiple windows services for a single application.

- This template includes two windows services that run are registered and can be run separately through the Windows Services manager.
- The project comes with an `app.config` which allows you to place the name of each service, and this name is picked up by both during registration and by the services during runtime. 
- The `app.config` includes descriptions for the services which is picked up during registration.
- The `app.config` also includes an EventLogName which is shared by the services.

### Service Registration

- To register (install) the services:
   `MultipleWindowsServicesInOneProject.exe /i`
- To unregister (uninstall) the services:
   `MultipleWindowsServicesInOneProject.exe /u

### License Info

MIT License.
