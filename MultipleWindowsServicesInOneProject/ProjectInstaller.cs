using System.ComponentModel;
using System.Configuration;
using System.Configuration.Install;
using System.ServiceProcess;
using System.Diagnostics;

namespace Abcm.WindowsServices
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();

            // Define the event log name that will be used by all our services.
            var eventLogName = ConfigurationManager.AppSettings["EventLogName"];

            // Set service installer and service process installer for our first service
            this.svcInstaller1.ServiceName = ConfigurationManager.AppSettings["ServiceOne.ServiceName"];
            this.svcInstaller1.Description = ConfigurationManager.AppSettings["ServiceOne.ServiceDescription"];
            this.svcInstaller1.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            this.svcpInstaller1.Account = ServiceAccount.LocalSystem;

            // Remove the default event log installer. We want to use custom events log name and source.
            this.svcInstaller1.Installers.Clear();
            
            // Create event source and event log
            EventLogInstaller logInstaller1 = new EventLogInstaller();
            logInstaller1.Source = this.svcInstaller1.ServiceName;
            logInstaller1.Log = eventLogName;

            // Remove event log source registration, if already existing.
            if (EventLog.SourceExists(logInstaller1.Source))
                EventLog.DeleteEventSource(logInstaller1.Source);


            // Set service installer and service process installer for our second service
            this.svcInstaller2.ServiceName = ConfigurationManager.AppSettings["ServiceTwo.ServiceName"];
            this.svcInstaller2.Description = ConfigurationManager.AppSettings["ServiceTwo.ServiceDescription"];
            this.svcInstaller2.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            this.svcpInstaller2.Account = ServiceAccount.LocalSystem;

            // Remove the default event log installer. We want to use custom events log name and source.
            this.svcInstaller2.Installers.Clear();

            // Create event source and event log
            EventLogInstaller logInstaller2 = new EventLogInstaller();
            logInstaller2.Source = this.svcInstaller2.ServiceName;
            logInstaller2.Log = eventLogName;

            // Remove event log source registration, if already existing.
            if (EventLog.SourceExists(logInstaller2.Source))
                EventLog.DeleteEventSource(logInstaller2.Source);

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
