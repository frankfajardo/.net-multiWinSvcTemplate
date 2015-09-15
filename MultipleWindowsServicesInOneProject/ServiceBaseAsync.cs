using System;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MultipleWindowsServicesInOneProject
{
    public class ServiceBaseAsync : ServiceBase
    {
        protected Task serviceTask;
        protected CancellationTokenSource cancellationTokenSource;
        protected EventLogger eventLogger;
        protected int runIntervalSeconds; 

        public ServiceBaseAsync()
        {
            this.ServiceName = this.GetType().Name;
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
            try
            {
                serviceTask.Wait();
            }
            catch (ObjectDisposedException)
            {
                eventLogger.LogInfo("Task was abruptly terminated. Check previous error logs for details.");
            }
            catch (AggregateException ae)
            {
                if (!(ae.InnerException is TaskCanceledException))
                {
                    var eventMsg = string.Format("Task encountered an error: {0}", ae.InnerException.ToString());
                    eventLogger.LogError(eventMsg);
                }
            }
            eventLogger.LogInfo("Service is stopped.");
        }

        protected virtual async Task DoRecursiveWorkAsync(CancellationToken token)
        {
            while (true)
            {
                token.ThrowIfCancellationRequested();
                eventLogger.LogInfo("Task starts.");

                // Call the main (async) method. But catch any errors 
                // as we do not want the exception to bubble up and stop the service.
                try
                {
                    await DoWorkAsync(token);
                }
                catch (Exception e)
                {
                    eventLogger.LogError(e.Message);
                }

                eventLogger.LogInfo("Task ends. Service is now idle.");
                await SleepTillNextRun(token);
            }
        }

        protected virtual async Task DoWorkAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            await Task.Run(() => 
            { 
                throw new NotImplementedException(String.Format("The \"{0}\" service has not implemented the DoWorkAsync() method inherited from the ServiceBaseAsync class.", this.ServiceName));
            });
        }

        protected virtual async Task SleepTillNextRun(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            this.runIntervalSeconds = 300; // 5 minutes
            await Task.Delay(this.runIntervalSeconds * 1000, token);
        }
    }

}
