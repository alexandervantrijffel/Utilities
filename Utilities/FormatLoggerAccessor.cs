using System;
using log4net;

namespace Structura.SharedComponents.Utilities
{
    public class FormatLoggerAccessor
    {
        private static readonly FormatLoggerAccessor instance;

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static FormatLoggerAccessor()
        {
        }

        public static Func<IFormatLogger> Instance;

        public static void Initialize(Func<ILog> getLogger)
        {
            Instance = () => new FormatLogger(getLogger());
        }
    }
}
