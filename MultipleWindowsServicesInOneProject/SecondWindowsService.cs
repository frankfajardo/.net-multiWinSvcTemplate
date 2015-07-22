using System.Configuration;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;

namespace MultipleWindowsServicesInOneProject
{
    public partial class SecondWindowsService : ServiceBase
    {
        #region Private Members

        private bool runRightAfterLaunch = true;
        private CancellationTokenSource cancelTokenSrc;

        #endregion Private Members

        #region Constructors

        public SecondWindowsService()
        {
            this.InitializeComponent();
            // Override service name set by the InitializeComponent() with the one in the app.config; this is also used by the project installer.
            this.ServiceName = ConfigurationManager.AppSettings["SecondWindowsService.ServiceName"];

            this.DefineEventLogging();
        }

        #endregion Constructors       

        #region Method Overrides

        protected override void OnStart(string[] args)
        {
            this.eventLog.WriteEntry("Service is started.");
            if (this.runRightAfterLaunch)
            {
                
            }
        }

        protected override void OnStop()
        {
            if (cancelTokenSrc != null)
            {
                cancelTokenSrc.Cancel();
            }
            this.eventLog.WriteEntry("Service is stopped.");
        } 

        #endregion


        #region Private Methods

        private async Task MainTask()
        {
            cancelTokenSrc = new CancellationTokenSource();
            var cancelToken = cancelTokenSrc.Token;
            await Task.Run(() =>
            {
                for (long i = 0; i < 10000000000; i++)
                {
                    this.eventLog.WriteEntry(string.Format("Processing...  i = {0}", i));

                    for (long j = 0; j < 1000000000000000000; j++)
                    {

                    }
                    if (cancelToken.IsCancellationRequested)
                    {
                        this.eventLog.WriteEntry("Aborting...");
                        cancelToken.ThrowIfCancellationRequested();
                    }
                }
                //MainTask();
            }, cancelToken);
        }


        private void DefineEventLogging()
        {
            // All services logs event to the same Event Log Name.
            string eventLogName = ConfigurationManager.AppSettings["EventLogName"];
            string eventSource = this.ServiceName;
            this.AutoLog = false;
            if (!EventLog.SourceExists(eventSource))
            {
                EventLog.CreateEventSource(eventSource, eventLogName);
            }
            this.eventLog.Source = eventSource;
            this.eventLog.Log = eventLogName;
        }

        #endregion
    }
}