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
        public ServiceOne()
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
            eventlog.Log = ConfigurationManager.AppSettings["EventLogName"] ?? "Application"; // Use the application-wide event log name
            eventlog.Source = this.ServiceName; 

            // Get the event logging level for this service.
            LogLevel logLevel;
            LogLevelHelp.TryParse(logLevelSetting, out logLevel);

            // Create an event logger for this service
            this.eventLogger = new EventLogger(eventlog, logLevel);
        }

        #region Method Overrides

        protected override async Task DoWorkAsync(CancellationToken token)
        {
            if (token.IsCancellationRequested) return;
            
            // Sample work: Delay for 120 seconds (2 minutes).
            eventLogger.LogInfo(string.Format("This is {0} doing its task.", this.ServiceName));
            await Task.Delay(120 * 1000, token); // 120 seconds or 2 minutes
        }

        #endregion

    }
}