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
            InitializeComponent();

            this.ServiceName = ConfigurationManager.AppSettings[appSettingPrefix + "ServiceName"] ?? this.GetType().Name;
            this.AutoLog = false;

            string logLevelSetting = ConfigurationManager.AppSettings[appSettingPrefix + "EventLogLevel"].ToString()
                ?? LogLevel.Information.ToString();

            var eventlog = new EventLog();
            eventlog.Log = ConfigurationManager.AppSettings["EventLogName"] ?? "Application"; // Use the common event log name
            eventlog.Source = this.ServiceName;
            this.eventLogger = new EventLogger(eventlog, logLevelSetting.ConvertToLogLevel());
        }

        #endregion Constructors       

        #region Method Overrides

        

        #endregion

        #region Private Methods


        #endregion
    }
}