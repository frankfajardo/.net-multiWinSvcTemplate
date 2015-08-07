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

            string eventLogName = ConfigurationManager.AppSettings["EventLogName"] ?? "Application";
            string eventLogLevel = ConfigurationManager.AppSettings[appSettingPrefix + "EventLogLevel"] ?? LogLevel.Information.ToString();
            SetEventLog(eventLogName, this.ServiceName, eventLogLevel);

            string runIntervals = this.ServiceName = ConfigurationManager.AppSettings[appSettingPrefix + "RunIntervalMinutes"] ?? "1";
            if (!int.TryParse(runIntervals, out this.runIntervalMinutes))
            {
                this.runIntervalMinutes = 1;
            }
        }

        #endregion Constructors       

        #region Method Overrides

        

        #endregion

        #region Private Methods


        #endregion
    }
}