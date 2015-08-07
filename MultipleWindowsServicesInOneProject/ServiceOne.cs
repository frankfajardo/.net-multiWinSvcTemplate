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

            string eventLogName = ConfigurationManager.AppSettings["EventLogName"] ?? "Application";
            string eventLogLevel = ConfigurationManager.AppSettings[appSettingPrefix + "EventLogLevel"] ?? LogLevel.Information.ToString();
            SetEventLog(eventLogName, this.ServiceName, eventLogLevel);

            string runIntervals = this.ServiceName = ConfigurationManager.AppSettings[appSettingPrefix + "RunIntervalMinutes"] ?? "1";
            if (!int.TryParse(runIntervals, out this.runIntervalMinutes))
            { 
                this.runIntervalMinutes = 1;
            }
        }

        #region Method Overrides

        protected override async Task DoWorkAsync(CancellationToken token, EventLog eventLog)
        {
            await Task.Run(() =>
            {
                eventLog.WriteInfoEntry("This is ServiceOne doing its task.");
            });
        }

        #endregion

    }
}