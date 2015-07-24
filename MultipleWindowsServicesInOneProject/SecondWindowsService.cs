using System.Configuration;
using System.Diagnostics;
using System.ServiceProcess;


namespace MultipleWindowsServicesInOneProject
{
    public partial class SecondWindowsService : ServiceBase
    {

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
            this.eventLog.WriteEntry(string.Format("Started: {0}", this.ServiceName));
        }

        protected override void OnStop()
        {
            this.eventLog.WriteEntry(string.Format("Stopped: {0}", this.ServiceName));
        }

        #endregion

        #region Private Methods

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