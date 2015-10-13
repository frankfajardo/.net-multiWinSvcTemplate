using System.Configuration;
using System.Diagnostics;
using System.ServiceProcess;


namespace MultipleWindowsServicesInOneProject
{
    public partial class ServiceTwo : ServiceBaseAsync
    {
        private string internalName = string.Empty;
        private string appSettingPrefix = "ServiceTwo.";

        #region Constructors

        public ServiceTwo()
        {
            InitializeComponent();

            this.internalName = this.GetType().Name;

            // Use custom event logging for this service.
            this.AutoLog = false;

            // Get the service name from the application config, if available.
            this.ServiceName = ConfigurationManager.AppSettings[this.internalName + ".Name"] ?? this.GetType().Name ?? this.internalName;

            // Get the event logging level for this service from the application config, if available.
            string logLevelSetting = ConfigurationManager.AppSettings[this.internalName + "EventLogLevel"].ToString()
                ?? LogLevel.Information.ToString();

            // Set the event log for this service
            var eventlog = new EventLog();
            eventlog.Log = ConfigurationManager.AppSettings["EventLogName"] ?? "Application"; // Use the common event log name
            eventlog.Source = this.ServiceName;

            // Get the event logging level for this service.
            LogLevel logLevel;
            LogLevelHelp.TryParse(logLevelSetting, out logLevel);

            // Create an event logger for this service
            this.eventLogger = new EventLogger(eventlog, logLevel);
        }

        #endregion Constructors       

        #region Method Overrides

        

        #endregion

        #region Private Methods


        #endregion
    }
}