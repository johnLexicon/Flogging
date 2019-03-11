using System;
using System.Configuration;
using System.Data.SqlClient;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace Flogging.Core
{

    public static class Flogger
    {

        private static readonly ILogger _perfLogger;
        private static readonly ILogger _usageLogger;
        private static readonly ILogger _errorLogger;
        private static readonly ILogger _diagnosticLogger;

        static Flogger()
        {
            _perfLogger = new LoggerConfiguration()
                .WriteTo.File(path: "~/flogger/perf.txt")
                .CreateLogger();

            _usageLogger = new LoggerConfiguration()
                .WriteTo.File(path: "~/flogger/usage.txt")
                .CreateLogger();

            _errorLogger = new LoggerConfiguration()
                .WriteTo.File(path: "~/flogger/error.txt")
                .CreateLogger();

            _diagnosticLogger = new LoggerConfiguration()
                .WriteTo.File(path: "~/flogger/diagnostic.txt")
                .CreateLogger();
        }

        public static void WritePerf(FlogDetail infoToLog)
        {
            _perfLogger.Write(LogEventLevel.Information, "{@FlogDetail}", infoToLog);
        }

        public static void WriteUsage(FlogDetail infoToLog)
        {
            _usageLogger.Write(LogEventLevel.Information, "{@FlogDetail}", infoToLog);
        }

        public static void WriteError(FlogDetail infoToLog)
        {
            if(infoToLog.Exception != null)
            {
                var procName = FindProcName(infoToLog.Exception);
                infoToLog.Location = String.IsNullOrEmpty(procName) ? infoToLog.Location : procName;
                infoToLog.Message = GetMessageFromException(infoToLog.Exception);
            }

            _errorLogger.Write(LogEventLevel.Information, "{@FlogDetail}", infoToLog);
        }

        private static string GetMessageFromException(Exception exception)
        {
            if(exception.InnerException != null)
            {
                return GetMessageFromException(exception.InnerException);
            }
            return exception.Message;
        }

        private static string FindProcName(Exception exception)
        {
            var sqlException = exception as SqlException;

            if(sqlException != null)
            {
                var procName = sqlException.Procedure;
                if (!string.IsNullOrEmpty(procName))
                {
                    return procName;
                }
            }

            if (!string.IsNullOrEmpty((string)exception.Data["Procedure"]))
            {
                return (string) exception.Data["Procedure"];
            }

            if(exception.InnerException != null)
            {
                return FindProcName(exception.InnerException);
            }
        }

        public static void WriteDiagnostic(FlogDetail infoToLog)
        {
            //var writeDiagnostics = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableDiagnostics"]);
            //if (!writeDiagnostics)
            //{
            //    return;
            //}

            _diagnosticLogger.Write(LogEventLevel.Information, "{@FlogDetail}", infoToLog);
        }

    }
}
