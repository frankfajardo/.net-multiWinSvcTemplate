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

            this.ServiceName = ConfigurationManager.AppSettings[appSettingPrefix + "ServiceName"] ?? this.GetType().Name;
            this.AutoLog = false;

            string logLevelSetting = ConfigurationManager.AppSettings[appSettingPrefix + "EventLogLevel"].ToString()
                ?? LogLevel.Information.ToString();

            var eventlog = new EventLog();
            eventlog.Log = ConfigurationManager.AppSettings["EventLogName"] ?? "Application"; // Use the common event log name
            eventlog.Source = this.ServiceName;
            this.eventLogger = new EventLogger(eventlog, logLevelSetting.ConvertToLogLevel());

        }

        #region Method Overrides

        protected override async Task DoWorkAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            // Sample work:
            eventLogger.LogInfo(string.Format("This is {0} doing its task.", this.ServiceName));
            await Task.Delay(120 * 1000, token); // 120 seconds or 2 minutes
        }

        #endregion

    }
}