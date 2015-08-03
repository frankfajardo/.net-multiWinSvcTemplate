using System;
using System.Configuration;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;


namespace MultipleWindowsServicesInOneProject
{
    public partial class ServiceOne : ServiceBaseAsync
    {
        private string appSettingPrefix = "ServiceOne.";

        public ServiceOne()
        {
            InitializeComponent();

            // Override service name set by the InitializeComponent() with the one in the app.config; this is also used by the project installer.
            this.ServiceName = ConfigurationManager.AppSettings[appSettingPrefix + "ServiceName"];

            // All services logs event to the same Event Log Name.
            string eventLogName = ConfigurationManager.AppSettings["EventLogName"];
            string eventSource = this.ServiceName;
            string eventLogLevel = ConfigurationManager.AppSettings[appSettingPrefix + "EventLogLevel"];
            SetEventLog(eventLogName, eventSource, eventLogLevel);

            this.runIntervalMinutes = 5;
        }

        #region Method Overrides

        protected override void DoWork(EventLog eventLog)
        {
            eventLog.WriteInfoEntry("This is ServiceOne doing its task.");
        }


        #endregion

    }
}