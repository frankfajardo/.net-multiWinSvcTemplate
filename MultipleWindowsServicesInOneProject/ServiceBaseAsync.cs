using System;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Configuration;

namespace MultipleWindowsServicesInOneProject
{
    /// <summary>
    /// Base class for a service that performs asynchronously processing in loops until the service is stopped.
    /// </summary>
    public class ServiceBaseAsync : ServiceBase
    {
        protected string internalName;
        protected Task serviceTask;
        protected CancellationTokenSource cancellationTokenSource;
        protected EventLogger eventLogger;
        protected int runIntervalSeconds; 

        public ServiceBaseAsync()
        {
            this.internalName = this.GetType().Name;
            this.ServiceName = this.internalName;
            var eventlog = new EventLog();
            eventlog.Log = "Application";
            eventlog.Source = this.ServiceName;
            this.eventLogger = new EventLogger(eventlog, LogLevel.Information);
        }

        protected override void OnStart(string[] args)
        {
            cancellationTokenSource = new CancellationTokenSource();
            serviceTask = Task.Run(() => DoRecursiveWorkAsync(cancellationTokenSource.Token));
            eventLogger.LogInfo("Service is started");
        }

        protected override void OnStop()
        {
            cancellationTokenSource.Cancel();
            serviceTask.Wait();
            eventLogger.LogInfo("Service is stopped.");
        }

        protected virtual async Task DoRecursiveWorkAsync(CancellationToken token)
        {
            while (true)
            {
                if (token.IsCancellationRequested) return;

                // Call the main (async) method. But catch any errors 
                // as we do not want the exception to bubble up and stop the service.
                try
                {
                    await DoWorkAsync(token);
                    await SleepTillNextRun(token);
                }
                catch (Exception excp)
                {
                    // Log any errors returned (except TaskCanceledException).
                    if (!(excp is TaskCanceledException))
                    {
                        eventLogger.LogError(string.Format("{0}. {1} Stack trace: {2}", excp.GetType().Name, excp.Message, excp.StackTrace));
                    }
                }

            }
        }

        /// <summary>
        /// Performs the main processing asyncronously
        /// </summary>
        /// <param name="token">Cancellation token</param>
        protected virtual async Task DoWorkAsync(CancellationToken token)
        {
            if (token.IsCancellationRequested) return;
            await Task.Run(() => 
            { 
                throw new NotImplementedException(String.Format("The \"{0}\" service has not implemented the DoWorkAsync() method inherited from the ServiceBaseAsync class.", this.ServiceName));
            });
        }

        protected virtual async Task SleepTillNextRun(CancellationToken token)
        {
            // If this task has been cancelled, do not proceed any further.
            if (token.IsCancellationRequested) return;

            var appSettingsKey = !string.IsNullOrWhiteSpace(this.internalName) ? this.internalName + ".RunIntervalSeconds" : "RunIntervalSeconds";

            // Check the configuration file for the run interval
            ConfigurationManager.RefreshSection("appSettings");
            string x = ConfigurationManager.AppSettings[appSettingsKey] ?? "0";

            // If RunIntervalSeconds config settings is available and valid, use it. Else, do nothing.
            if (int.TryParse(x, out runIntervalSeconds) && runIntervalSeconds > 0)
            {
                var statusMsg = string.Format("Sleeping for {0} {1}.", this.runIntervalSeconds, (this.runIntervalSeconds != 1 ? "seconds" : "second"));
                eventLogger.LogInfo(statusMsg);
                await Task.Delay(runIntervalSeconds * 1000, token);
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (eventLogger != null)
                    eventLogger.Dispose();
                if (cancellationTokenSource != null)
                    cancellationTokenSource.Dispose();
            }
            base.Dispose(disposing);
        }
    }

}
