using System;
using System.IO;
using Serilog;

namespace Divy.Common
{
    /// <summary>
    /// Tracing Container that will wrap the tracing solution keeping it
    /// limited if it needs to swapped for another solution later
    /// </summary>
    public class Tracing
    {
        public static ILogger Trace = Log.Logger;

        public Tracing()
        {
            CreateLogPath();
            Trace = new LoggerConfiguration()
                .ReadFrom.AppSettings().CreateLogger();
        }

        public static void Verbose(string message, Exception ex = null)
        {
            if(ex != null)
                Trace.Verbose(ex, message);
            else
                Trace.Verbose(message);
        }

        public static  void Debug(string message, Exception ex = null)
        {
            if (ex != null)
                Trace.Debug(ex, message);
            else
                Trace.Debug(message);
        }

        public static void Info(string message, Exception ex = null)
        {
            if (ex != null)
                Trace.Information(ex, message);
            else
                Trace.Information(message);
        }

        public static void Warning(string message, Exception ex = null)
        {
            if (ex != null)
                Trace.Warning(ex, message);
            else
                Trace.Warning(message);
        }

        public static void Error(string message, Exception ex = null)
        {
            if (ex != null)
                Trace.Error(ex, message);
            else
                Trace.Error(message);
        }
        public static void Error(Exception ex)
        {
            Trace.Error(ex,ex.Message);
        }
        public static void Fatal(string message, Exception ex = null)
        {
            if (ex != null)
                Trace.Fatal(ex, message);
            else
                Trace.Fatal(message);
        }

        /// <summary>
        /// Creates the folder that will contain the logs 
        /// </summary>
        private static void CreateLogPath()
        {
            var FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                "Divy");
            if (!System.IO.Directory.Exists(FilePath))
            {
                System.IO.Directory.CreateDirectory(FilePath);
            }
        }
    }
}
