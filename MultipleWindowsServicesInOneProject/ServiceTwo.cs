using System.Configuration;
using System.Diagnostics;
using System.ServiceProcess;


namespace MultipleWindowsServicesInOneProject
{
    public partial class ServiceTwo : ServiceBaseAsync
    {
        private string appSettingPrefix = "ServiceTwo.";

        #region Constructors

        public ServiceTwo()
        {
            this.InitializeComponent();

            // Override service name set by the InitializeComponent() with the one in the app.config; this is also used by the project installer.
            this.ServiceName = ConfigurationManager.AppSettings[appSettingPrefix + "ServiceName"];

            // All services logs event to the same Event Log Name.
            string eventLogName = ConfigurationManager.AppSettings["EventLogName"];
            string eventSource = this.ServiceName;
            string eventLogLevel = ConfigurationManager.AppSettings[appSettingPrefix + "EventLogLevel"];
            SetEventLog(eventLogName, eventSource, eventLogLevel);

            this.runIntervalMinutes = 2;
        }

        #endregion Constructors       

        #region Method Overrides

        

        #endregion

        #region Private Methods


        #endregion
    }
}