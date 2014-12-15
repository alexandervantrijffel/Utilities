using System;
using System.Diagnostics;
using log4net;
using SharedUtilities;

namespace Structura.SharedComponents.Utilities
{
    public class FormatLogger : IFormatLogger
    {
        public FormatLogger(ILog log)
        {
            TheLogger = log;
        }

        public ILog TheLogger { get; set; }

        protected string CallingMethodName
        {
            get { return new StackFrame(2, true).GetMethod().Name; }
        }
        protected string FormattedString(string messageFormatString, params object[] formatStringParameters)
        {
            return formatStringParameters.Length > 0
                                    ? String.Format(messageFormatString, formatStringParameters)
                                    : messageFormatString;
        }
        /// <summary>
        /// Logs an exception as an info message
        /// </summary>
        public void Info(Exception ex, string messageFormatString, params object[] formatStringParameters)
        {
            string fullMessage = String.Format("A business logic error occured: {0} ({1}). {2}. At: {3}"
                                               , ex.Message, ex.Message.GetType().Name
                                               , FormattedString(messageFormatString, formatStringParameters)
                                               , ex.StackTrace);
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