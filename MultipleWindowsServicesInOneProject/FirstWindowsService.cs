using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Configuration;

namespace MultipleWindowsServicesInOneProject
{
    public partial class FirstWindowsService : ServiceBase
    {
        public FirstWindowsService()
        {
            this.InitializeComponent();
            // Override service name set by the InitializeComponent() with the one in the app.config; this is also used by the project installer.
            this.ServiceName = ConfigurationManager.AppSettings["FirstWindowsService.ServiceName"];

            this.DefineEventLogging();
        }

        protected override void OnStart(string[] args)
        {
            this.eventLog.WriteEntry(string.Format("Started: {0}", this.ServiceName));
        }

        protected override void OnStop()
        {
            this.eventLog.WriteEntry(string.Format("Stopped: {0}", this.ServiceName));
        }


        private void DefineEventLogging()
        {
            // All services logs event to the same Event Log Name.
            string eventLogName = ConfigurationManager.AppSettings["EventLogName"];
            string eventSource = this.ServiceName;
            this.AutoLog = false;
            if (!EventLog.SourceExists(eventSource))
            {
                EventLog.CreateEventSource(eventSource, eventLogName);
            }
            this.eventLog.Source = eventSource;
            this.eventLog.Log = eventLogName;
        }
    }
}