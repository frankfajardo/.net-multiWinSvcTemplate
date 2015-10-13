using System.ComponentModel;
using System.Configuration;
using System.Configuration.Install;
using System.ServiceProcess;
using System.Diagnostics;

namespace MultipleWindowsServicesInOneProject
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();

            // Set service installer and service process installer for our first service
            var svc1 = typeof(ServiceOne).Name;
            this.svcInstaller1.ServiceName = ConfigurationManager.AppSettings[svc1 + ".Name"] ?? svc1;
            this.svcInstaller1.DisplayName = ConfigurationManager.AppSettings[svc1 + ".DisplayName"] ?? svc1;
            this.svcInstaller1.Description = ConfigurationManager.AppSettings[svc1 + ".Description"] ?? string.Empty;
            this.svcInstaller1.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            this.svcpInstaller1.Account = ServiceAccount.LocalSystem;

            // Remove the default event log installer. We want to use custom events log name and source.
            this.svcInstaller1.Installers.Clear();

            // Define the event log name that will be used by all our services.
            var eventLogName = ConfigurationManager.AppSettings["EventLogName"] ?? "Application";
            var eventSource = this.svcInstaller1.ServiceName;

            // Delete event log and source, if already existing. Then re-create.
            if (EventLog.Exists(eventLogName))
                EventLog.Delete(eventLogName);
            if (EventLog.SourceExists(eventSource))
                EventLog.DeleteEventSource(eventSource);
            EventLog.CreateEventSource(eventSource, eventLogName);
            
            // Create event source and event log
            EventLogInstaller logInstaller1 = new EventLogInstaller();
            logInstaller1.Source = this.svcInstaller1.ServiceName;
            logInstaller1.Log = eventLogName;

            // Set service installer and service process installer for our second service
            var svc2 = typeof(ServiceTwo).Name;
            this.svcInstaller2.ServiceName = ConfigurationManager.AppSettings[svc2 + ".Name"] ?? svc2;
            this.svcInstaller2.DisplayName = ConfigurationManager.AppSettings[svc2 + ".DisplayName"] ?? svc2;
            this.svcInstaller2.Description = ConfigurationManager.AppSettings[svc2 + ".Description"] ?? string.Empty;
            this.svcInstaller2.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            this.svcpInstaller2.Account = ServiceAccount.LocalSystem;

            // Remove the default event log installer. We want to use custom events log name and source.
            this.svcInstaller2.Installers.Clear();

            // Delete event source, if already existing. Then re-create.
            eventSource = this.svcInstaller2.ServiceName;
            if (EventLog.SourceExists(eventSource))
                EventLog.DeleteEventSource(eventSource);
            EventLog.CreateEventSource(eventSource, eventLogName);

            // Create event source and event log
            EventLogInstaller logInstaller2 = new EventLogInstaller();
            logInstaller2.Source = this.svcInstaller2.ServiceName;
            logInstaller2.Log = eventLogName;

            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(
                new Installer[] {
                this.svcpInstaller1,
                this.svcInstaller1,
                logInstaller1,
                this.svcpInstaller2,
                this.svcInstaller2,
                logInstaller2
            });

        }
    }
}
