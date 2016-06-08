using System;
using log4net;

namespace Structura.Shared.Utilities
{
    public static class FormatLoggerAccessor
    {
        public static Func<IFormatLogger> Locate;

        public static void Initialize(Func<ILog> getLogger)
        {
            Locate = () => new FormatLogger(getLogger());
        }
    }
}
