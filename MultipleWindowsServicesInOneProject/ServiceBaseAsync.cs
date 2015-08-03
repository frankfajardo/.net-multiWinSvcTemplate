using System;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MultipleWindowsServicesInOneProject
{
    public abstract class ServiceBaseAsync : ServiceBase
    {
        protected Task serviceTask;
        protected CancellationTokenSource cancellationTokenSource;

        protected EventLog eventLog;
        protected LogLevel logLevel;

        protected int runIntervalMinutes;

        public ServiceBaseAsync()
        {

        }

        protected override void OnStart(string[] args)
        {
            cancellationTokenSource = new CancellationTokenSource();
            serviceTask = Task.Run(() => DoWorkAsync(cancellationTokenSource.Token, runIntervalMinutes));

            eventLog.WriteInfoEntry("Service is started", logLevel);
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
                eventLog.WriteWarningEntry("Task was abruptly terminated. Check previous error logs for details.", logLevel);
            }
            catch (AggregateException ae)
            {
                if (!(ae.InnerException is TaskCanceledException))
                {
                    var eventMsg = string.Format("Task encountered an error: {0}", ae.InnerException.ToString());
                    eventLog.WriteErrorEntry(eventMsg, logLevel);
                }
            }
            eventLog.WriteInfoEntry("Service is stopped.", logLevel);
        }

        protected virtual async Task DoWorkAsync(CancellationToken token, int runIntervalMinutes)
        {
            while (true)
            {
                eventLog.WriteInfoEntry("Task is started.", logLevel);
                Task task1 = Task.Factory.StartNew(() => { DoWork(eventLog); });
                try
                {
                    task1.Wait();
                }
                catch (AggregateException ae)
                {
                    ae.Handle(x =>
                    {
                        // If our task is cancelled, stop and return so that our service stops.
                        if (x is TaskCanceledException)
                        {
                            return false;
                        }
                        // For any other exception, log it on the event log. Then simply continue to next run interval.
                        var eventMsg = string.Format("Task received an exception: {0}", ae.InnerException.ToString());
                        eventLog.WriteErrorEntry(eventMsg, logLevel);
                        return true;
                    });
                }

                eventLog.WriteInfoEntry("Task is finished. Service is now idle.", logLevel);

                // Delay before the next run of our task.
                await Task.Delay(TimeSpan.FromMinutes(runIntervalMinutes), token);
            }
        }

        protected virtual void DoWork(EventLog eventLog)
        {
            throw new NotImplementedException();
        }

        protected virtual void SetEventLog(string eventLogName, string eventSource, string eventLogLevel)
        {
            if (String.IsNullOrWhiteSpace(eventLogName))
                throw new ArgumentNullException("eventLogName");

            if (String.IsNullOrWhiteSpace(eventLogName))
                throw new ArgumentNullException("eventSource");

            if (this.eventLog == null)
                this.eventLog = new EventLog();

            this.AutoLog = false;
            this.eventLog.Source = eventSource;
            this.eventLog.Log = eventLogName;

            if (String.IsNullOrWhiteSpace(eventLogLevel) || !Enum.TryParse<LogLevel>(eventLogLevel, out this.logLevel))
            {
                this.logLevel = LogLevel.Information;
            }
        }
    }

}
