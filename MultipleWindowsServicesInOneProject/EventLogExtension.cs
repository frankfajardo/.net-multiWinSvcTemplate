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

    public static class LogLevelExtension
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
        public static LogLevel ConvertToLogLevel(this string logLevelString)
        {
            LogLevel logLevel;
            if (string.IsNullOrWhiteSpace(logLevelString) || !Enum.TryParse<LogLevel>(logLevelString, out logLevel))
            {
                throw new ArgumentException("String is not a valid LogLevel");
            }
            return logLevel;
        }
    }

    /// <summary>
    /// A event logging helper which allows easily controlling the logging level for an application.
    /// </summary>
    public sealed class EventLogger
    {
        private LogLevel loglevel;
        private EventLog eventlog;

        #region Constructor
        public EventLogger(EventLog eventlog)
        {
            this.eventlog = eventlog;
            this.loglevel = LogLevel.Undefined;
        }
        public EventLogger(EventLog eventlog, LogLevel loglevel)
        {
            this.eventlog = eventlog;
            this.loglevel = loglevel;
        }

        #endregion Constructor

        /// <summary>
        /// Defines the logging level to use when this EventLogger receives request to log an event.
        /// </summary>
        public LogLevel LogLevel
        {
            get { return this.loglevel; }
            set { this.loglevel = value; }
        }

        /// <summary>
        /// Defines the event log to use.
        /// </summary>
        public EventLog EventLog
        {
            get { return this.eventlog; }
            set { this.eventlog = value; }
        }

        /// <summary>
        /// Writes an Error event to the event log, provided the LogLevel allows for error events.
        /// </summary>
        /// <param name="msg">The event text to write</param>
        public void LogError(string msg)
        {
            if (this.loglevel.IncludesEventType(EventLogEntryType.Error))
                this.eventlog.WriteEntry(msg, EventLogEntryType.Error);
        }

        /// <summary>
        /// Writes a Warning event to the event log, provided the LogLevel allows for warning events
        /// </summary>
        /// <param name="msg">The event text to write</param>
        public void LogWarning(string msg)
        {
            if (this.loglevel.IncludesEventType(EventLogEntryType.Warning))
                this.eventlog.WriteEntry(msg, EventLogEntryType.Warning);
        }

        /// <summary>
        /// Writes an Information event to the event log, provided the LogLevel allows for information events
        /// </summary>
        /// <param name="msg">The event text to write</param>
        public void LogInfo(string msg)
        {
            if (this.loglevel.IncludesEventType(EventLogEntryType.Information))
                this.eventlog.WriteEntry(msg, EventLogEntryType.Information);
        }


    }

    //public static class EventLogExtension
    //{
    //    public static void WriteErrorEntry(this EventLog log, string msg, LogLevel loglevel = LogLevel.Undefined)
    //    {
    //        //if (loglevel.IncludesEventType(EventLogEntryType.Error))
    //        if (loglevel == LogLevel.Undefined || (int)loglevel >= (int)EventLogEntryType.Error)
    //            log.WriteEntry(msg, EventLogEntryType.Error);
    //    }

    //    public static void WriteWarningEntry(this EventLog log, string msg, LogLevel loglevel = LogLevel.Undefined)
    //    {
    //        //if (loglevel.IncludesEventType(EventLogEntryType.Warning))
    //        if (loglevel == LogLevel.Undefined || (int)loglevel >= (int)EventLogEntryType.Warning)
    //            log.WriteEntry(msg, EventLogEntryType.Warning);
    //    }

    //    public static void WriteInfoEntry(this EventLog log, string msg, LogLevel loglevel = LogLevel.Undefined)
    //    {
    //        //if (loglevel.IncludesEventType(EventLogEntryType.Information))
    //        if (loglevel == LogLevel.Undefined || (int)loglevel >= (int)EventLogEntryType.Information)
    //            log.WriteEntry(msg, EventLogEntryType.Information);
    //    }

    //    public static bool IncludesEventType(this LogLevel loglevel, EventLogEntryType eventType)
    //    {
    //        return (loglevel == LogLevel.Undefined || (int)loglevel >= (int)eventType);
    //    }
    //}
}
