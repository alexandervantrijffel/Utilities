using System;
using System.Diagnostics;
using log4net;

namespace Structura.Shared.Utilities
{
    public interface IFormatLogger
    {
        void Info(string messageFormatString, params object[] formatStringParameters);
        void Info(Exception ex, string messageFormatString, params object[] formatStringParameters);
        void Warn(string messageFormatString, params object[] formatStringParameters);
        void Debug(string messageFormatString, params object[] formatStringParameters);
        void Error(string messageFormatString, params object[] formatStringParameters);
        void Error(Exception ex, string prefix, params object[] formatStringParameters);
        void Fatal(string messageFormatString, params object[] formatStringParameters);
        void Fatal(Exception ex, string prefix, params object[] formatStringParameters);
        void Log(LogLevel level, string messageFormatString, params object[] formatStringParameters);
        void Log(LogLevel level, Exception ex, string prefix, params object[] formatStringParameters);
    }

    public enum LogLevel
    {
        Debug,
        Info,
        Warn,
        Error,
        Fatal
    }

    public class FormatLogger : IFormatLogger
    {
        public FormatLogger(ILog log)
        {
            TheLogger = log;
        }

        public ILog TheLogger { get; set; }

        protected string CallingMethodName => new StackFrame(2, true).GetMethod().Name;

        protected string FormattedString(string messageFormatString, params object[] formatStringParameters)
        {
            return formatStringParameters.Length > 0
                                    ? string.Format(messageFormatString, formatStringParameters)
                                    : messageFormatString;
        }
        /// <summary>
        /// Logs an exception as an info message
        /// </summary>
        public void Info(Exception ex, string messageFormatString, params object[] formatStringParameters)
        {
            var fullMessage =
                $"A business logic error occured: {ex.Message} ({ex.Message.GetType().Name}). {FormattedString(messageFormatString, formatStringParameters)}. At: {ex.StackTrace}";
            TheLogger.Info(CallingMethodName + " " + fullMessage);
        }

        public void Info(string messageFormatString, params object[] formatStringParameters)
        {
            TheLogger.Info(FormattedString(messageFormatString, formatStringParameters));
        }

        public void Warn(string messageFormatString, params object[] formatStringParameters)
        {
            TheLogger.Warn(CallingMethodName + ": " + FormattedString(messageFormatString, formatStringParameters));
        }

        public void Debug(string messageFormatString, params object[] formatStringParameters)
        {
            TheLogger.Debug(CallingMethodName + ": " + FormattedString(messageFormatString, formatStringParameters));
        }

        public void Error(string messageFormatString, params object[] formatStringParameters)
        {
            TheLogger.Error(CallingMethodName + ": " + FormattedString(messageFormatString, formatStringParameters));
        }

        public void Error(Exception ex, string messageFormatString, params object[] formatStringParameters)
        {
            TheLogger.Error(CallingMethodName + ": " + FormattedString(messageFormatString, formatStringParameters), ex);
        }

        public void Fatal(string messageFormatString, params object[] formatStringParameters)
        {
            TheLogger.Fatal(CallingMethodName + ": " + FormattedString(messageFormatString, formatStringParameters));
        }

        public void Fatal(Exception ex, string messageFormatString, params object[] formatStringParameters)
        {
            TheLogger.Fatal(CallingMethodName + ": " + FormattedString(messageFormatString, formatStringParameters), ex);
        }

        public void Log(LogLevel level, string messageFormatString, params object[] formatStringParameters)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    Debug(messageFormatString, formatStringParameters);
                    break;

                case LogLevel.Info:
                    Info(messageFormatString, formatStringParameters);
                    break;

                case LogLevel.Warn:
                    Warn(messageFormatString, formatStringParameters);
                    break;

                case LogLevel.Error:
                    Error(messageFormatString, formatStringParameters);
                    break;

                case LogLevel.Fatal:
                    Fatal(messageFormatString, formatStringParameters);
                    break;

                default:
                    throw new InvalidOperationException("Unknown LogLevel");
            }
        }

        public void Log(LogLevel level, Exception ex, string messageFormatString, params object[] formatStringParameters)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    {
                        TheLogger.Debug(CallingMethodName + " " + FormattedString(messageFormatString, formatStringParameters) + ex.ToFullException()
                            , ex);
                        break;
                    }


                case LogLevel.Info:
                    {
                        TheLogger.Info(CallingMethodName + " " + FormattedString(messageFormatString, formatStringParameters) + ex.ToFullException(), ex);
                        break;
                    }
                case LogLevel.Warn:
                    {
                        TheLogger.Warn(CallingMethodName + " " + FormattedString(messageFormatString, formatStringParameters) + ex.ToFullException(), ex);
                        break;
                    }

                case LogLevel.Error:
                    Error(ex, messageFormatString, formatStringParameters);
                    break;

                case LogLevel.Fatal:
                    Fatal(ex, messageFormatString, formatStringParameters);
                    break;

                default:
                    throw new InvalidOperationException("Unknown LogLevel");
            }
        }
    }
}