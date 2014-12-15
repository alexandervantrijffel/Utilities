using System;

namespace Structura.SharedComponents.Utilities
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
}