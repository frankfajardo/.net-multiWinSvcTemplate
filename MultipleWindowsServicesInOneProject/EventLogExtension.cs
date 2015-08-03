using System.Diagnostics;

namespace MultipleWindowsServicesInOneProject
{
    public enum LogLevel
    {
        Undefined = 0,
        Error = 1,
        Warning = 2,
        Information = 4
    }
    
    public static class EventLogExtension
    {
        public static void WriteErrorEntry(this EventLog log, string msg, LogLevel loglevel = LogLevel.Undefined)
        {
            //if (loglevel.IncludesEventType(EventLogEntryType.Error))
            if (loglevel == LogLevel.Undefined || (int)loglevel >= (int)EventLogEntryType.Error)
                log.WriteEntry(msg, EventLogEntryType.Error);
        }

        public static void WriteWarningEntry(this EventLog log, string msg, LogLevel loglevel = LogLevel.Undefined)
        {
            //if (loglevel.IncludesEventType(EventLogEntryType.Warning))
            if (loglevel == LogLevel.Undefined || (int)loglevel >= (int)EventLogEntryType.Warning)
                log.WriteEntry(msg, EventLogEntryType.Warning);
        }

        public static void WriteInfoEntry(this EventLog log, string msg, LogLevel loglevel = LogLevel.Undefined)
        {
            //if (loglevel.IncludesEventType(EventLogEntryType.Information))
            if (loglevel == LogLevel.Undefined || (int)loglevel >= (int)EventLogEntryType.Information)
                log.WriteEntry(msg, EventLogEntryType.Information);
        }

        public static bool IncludesEventType(this LogLevel loglevel, EventLogEntryType eventType)
        {
            return (loglevel == LogLevel.Undefined || (int)loglevel >= (int)eventType);
        }
    }
}
