using System;

namespace SharedUtilities
{
    public static class ExceptionExtensions
    {
        public static string ToFullException(this Exception ex)
        {
            String message = String.Format("{0}", ex.Message);
            if (ex.InnerException != null)
            {
                message += String.Format(", {0}", ToFullException(ex.InnerException));
            }
            return message;
        }
    }
}