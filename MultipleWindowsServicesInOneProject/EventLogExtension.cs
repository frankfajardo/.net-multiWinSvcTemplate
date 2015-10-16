using System;
using System.Diagnostics;

namespace MultipleWindowsServicesInOneProject
{
    /// <summary>
    /// Defines event logging level. Log levels are assigned specific values to denote hierarchy. 
    /// A greater value indicates a more inclusive log level. However, an Undefined LogLevel indicates no hierarchy and therefore all inclusive logging.
    /// </summary>
    public enum LogLevel
    {
        Undefined = 0,
        Error = 1,
        Warning = 2,
        Information = 4
    }

    /// <summary>
    /// Provides static methods relating to LogLevel
    /// </summary>
    public static class LogLevelHelp
    {
        /// <summary>
        /// Determines if the current LogLevel includes the specified event type.
        /// </summary>
        /// <returns>True, if the LogLevel includes the event type.</returns>
        public static bool IncludesEventType(this LogLevel loglevel, EventLogEntryType eventType)
        {
            return (loglevel == LogLevel.Undefined || (int)loglevel >= (int)eventType);
        }

        /// <summary>
        /// Converts a string to a LogLevel
        /// </summary>
        public static LogLevel Parse(string logLevelString)
        {
            LogLevel logLevel;
            if (string.IsNullOrWhiteSpace(logLevelString) || !Enum.TryParse<LogLevel>(logLevelString, out logLevel))
            {
                throw new ArgumentException("String is not a valid LogLevel");
            }
            return logLevel;
        }

        /// <summary>
        /// Converts the string
        /// </summary>
        /// <param name="logLevelString"></param>
        /// <param name="logLevel"></param>
        /// <returns></returns>
        public static bool TryParse(string logLevelString, out LogLevel logLevel)
        {
            bool success = false;
            try
            {
                logLevel = LogLevelHelp.Parse(logLevelString);
                success = true;
            }
            catch
            {
                logLevel = LogLevel.Undefined;
            }
            return success;
        }
    }

    /// <summary>
    /// A event logging helper which allows easily controlling the logging level for an application.
    /// </summary>
    public sealed class EventLogger : IDisposable
    {
        private LogLevel loglevel;
        private EventLog eventlog;
        private bool ownsEventLog = false;

        #region Constructor

        /// <summary>
        /// Initialise a new instance of the EventLogger class that uses an existing EventLog to log any type of event. 
        /// This EventLogger does not assume ownership of the EventLog and therefore will not dispose of it in its Dispose() method.
        /// </summary>
        /// <param name="eventlog">The eventlog to use</param>
        /// <exception cref="ArgumentNullException">When eventLog is null</exception>
        public EventLogger(EventLog eventlog)
        {
            if (eventlog == null)
                throw new ArgumentNullException("Eventlog cannot be null.");
            this.eventlog = eventlog;
            this.loglevel = LogLevel.Undefined;
        }

        /// <summary>
        /// Initialises a new instance of the EventLogger class that uses an existing EventLog, to log events based on the specified LogLevel.
        /// This EventLogger does not assume ownership of the EventLog and therefore will not dispose of it in its Dispose() method.
        /// </summary>
        /// <param name="eventlog">The eventlog to use</param>
        /// <param name="loglevel">The loglevel to follow</param>
        /// <exception cref="ArgumentNullException">When eventLog is null</exception>
        public EventLogger(EventLog eventlog, LogLevel loglevel)
        {
            if (eventlog == null)
                throw new ArgumentNullException("Eventlog cannot be null.");
            this.eventlog = eventlog;
            this.loglevel = loglevel;
        }

        /// <summary>
        /// Initialises a new instance of the EventLogger class to log any type of event.
        /// This EventLogger will create an EventLog with the specified log name and source. 
        /// It assumes ownership of the EventLog and will dispose of it in its Dispose() method.
        /// </summary>
        /// <param name="logname">The log name to use when creating the event log</param>
        /// <param name="logsource">The source for the events</param>
        /// <exception cref="ArgumentNullException">When either log name or source is null or empty</exception>
        public EventLogger(string logname, string logsource)
        {
            if (string.IsNullOrWhiteSpace(logname))
                throw new ArgumentNullException("Log name is required.");
            if (string.IsNullOrWhiteSpace(logsource))
                throw new ArgumentNullException("Log source is required.");
            this.eventlog = new EventLog(logname, ".", logsource);
            this.ownsEventLog = true;
            this.loglevel = LogLevel.Undefined;
        }

        /// <summary>
        /// Initialises a new instance of the EventLogger class to log events based on the specified LogLevel.
        /// This EventLogger will create an EventLog with the specified log name and source. 
        /// It assumes ownership of the EventLog and will dispose of it in its Dispose() method.
        /// </summary>
        /// <param name="logname">The log name to use when creating the event log</param>
        /// <param name="logsource">The source for the events</param>
        /// <param name="loglevel">The loglevel to follow</param>
        /// <exception cref="ArgumentNullException">When either log name or source is null or empty</exception>
        public EventLogger(string logname, string logsource, LogLevel loglevel)
        {
            if (string.IsNullOrWhiteSpace(logname))
                throw new ArgumentNullException("Log name is required.");
            if (string.IsNullOrWhiteSpace(logsource))
                throw new ArgumentNullException("Log source is required.");
            this.eventlog = new EventLog(logname, ".", logsource);
            this.ownsEventLog = true;
            this.loglevel = loglevel;
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Gets or sets the loglevel to follow when logging events.
        /// </summary>
        public LogLevel LogLevel
        {
            get { return this.loglevel; }
            set { this.loglevel = value; }
        }

        /// <summary>
        /// Gets the event log to used by this event logger.
        /// </summary>
        public EventLog EventLog
        {
            get { return this.eventlog; }
        }

        #endregion  Properties

        #region Methods

        /// <summary>
        /// Writes an Error event to the event log, provided the LogLevel allows for error events.
        /// </summary>
        /// <param name="msg">The event text to write</param>
        public void LogError(string msg)
        {
            // Log event message based on logging level
            if (this.LogLevel.IncludesEventType(EventLogEntryType.Error))
                this.EventLog.WriteEntry(msg, EventLogEntryType.Error);
        }

        /// <summary>
        /// Writes a Warning event to the event log, provided the LogLevel allows for warning events
        /// </summary>
        /// <param name="msg">The event text to write</param>
        public void LogWarning(string msg)
        {
            // Log event message based on logging level
            if (this.LogLevel.IncludesEventType(EventLogEntryType.Warning))
                this.EventLog.WriteEntry(msg, EventLogEntryType.Warning);
        }

        /// <summary>
        /// Writes an Information event to the event log, provided the LogLevel allows for information events
        /// </summary>
        /// <param name="msg">The event text to write</param>
        public void LogInfo(string msg)
        {
            // Log event message based on logging level
            if (this.LogLevel.IncludesEventType(EventLogEntryType.Information))
                this.EventLog.WriteEntry(msg, EventLogEntryType.Information);
        }

        /// <summary>
        /// Disposes the EventLogger. It disposes of the EventLog it uses, provided that EventLog was created by this EventLogger.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.ownsEventLog && this.eventlog != null)
                    this.eventlog.Dispose();
            }
        }

        #endregion Methods
    }
    
}
