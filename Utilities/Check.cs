using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace Structura.SharedComponents.Utilities
{
    /// <summary>
    /// Each method generates an exception or
    /// a trace assertion statement if the contract is broken.
    /// </summary>
    /// <remarks>
    /// This example shows how to call the Require method.
    /// Assume DBC_CHECK_PRECONDITION is defined.
    /// <code>
    /// public void Test(int x)
    /// {
    /// 	try
    /// 	{
    ///			Check.Require(x > 1, "x must be > 1");
    ///		}
    ///		catch (System.Exception ex)
    ///		{
    ///			Console.WriteLine(ex.ToString());
    ///		}
    ///	}
    /// </code>
    /// If you wish to use trace assertion statements, intended for Debug scenarios,
    /// rather than exception handling then set 
    /// 
    /// <code>Check.UseAssertions = true</code>
    /// 
    /// You can specify this in your application entry point and maybe make it
    /// dependent on conditional compilation flags or configuration file settings, e.g.,
    /// <code>
    /// #if DBC_USE_ASSERTIONS
    /// Check.UseAssertions = true;
    /// #endif
    /// </code>
    /// You can direct output to a Trace listener. For example, you could insert
    /// <code>
    /// Trace.Listeners.Clear();
    /// Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
    /// </code>
    /// 
    /// or direct output to a file or the Event Log.
    /// 
    /// (Note: For ASP.NET clients use the Listeners collection
    /// of the Debug, not the Trace, object and, for a Release build, only exception-handling
    /// is possible.)
    /// </remarks>
    /// 
    public static class Check
    {
        /// <summary>
        /// Precondition check - should run regardless of preprocessor directives.
        /// </summary>
        public static void Require(bool assertion)
        {
            Require(assertion, string.Empty);
        }
        public static void Require(bool assertion, string messageFormatString, params object[] formatStringParameters)
        {
            Require<PreconditionException>(assertion, messageFormatString, formatStringParameters);
        }

        /// <summary>
        /// Precondition check - should run regardless of preprocessor directives. Throws exception of type T. T must contain
        /// a default constructor and a constructor that accepts a string argument for the message.
        /// </summary>
        public static void Require<T>(bool assertion, string messageFormatString, params object[] formatStringParameters) where T : Exception, new()
        {
            PerformCheck(assertion,
                () => ThrowExceptionOfType<T>(messageFormatString, formatStringParameters),
                CheckType.Precondition,
                messageFormatString,
                formatStringParameters);
        }

        /// <summary>
        /// Precondition check - should run regardless of preprocessor directives.
        /// </summary>

        public static void RequireNotNull(object o)
        {
            RequireNotNull(o, string.Empty);
        }

        public static void RequireNotNull(dynamic o, string messageFormatString, params object[] formatStringParameters)
        {
            Require(o != null, messageFormatString, formatStringParameters);
        }

        public static void RequireNotNullOrEmpty(ICollection value, string messageFormatString, params object[] formatStringParameters)
        {
            Require(value != null && value.Count > 0, messageFormatString, formatStringParameters);
        }

        public static void RequireNotNull<TException, TObject>(dynamic o, string messageFormatString, params object[] formatStringParameters) where TException : Exception, new()
        {
            bool assertion = o != null;
            PerformCheck(assertion,
                () => ThrowExceptionOfType<TException>(messageFormatString + ". Object of type '" + typeof(TObject).FullName + "' cannot be null.", formatStringParameters),
                CheckType.Precondition,
                messageFormatString,
                formatStringParameters);
        }

        private static string GetStackFrameString()
        {
            var frames = new StackTrace().GetFrames();
            var method = "?";
            var declaringType = "?";
            // Frames: 
            //     0: Current method (GetStackFrameString)
            //     1: Caller
            //     2: Caller of Caller 
            //     etc
            for (var frameNum = 5; frameNum < frames.Count() && frameNum < 15; frameNum++)
            {
                var theMethod = frames[frameNum].GetMethod();
                if (theMethod.DeclaringType != null && theMethod.DeclaringType.Name != "Check" && theMethod.DeclaringType.Name != "UpdateDelegates")
                {
                    method = theMethod.Name;
                    declaringType = theMethod.DeclaringType.FullName;
                    break;
                }
            }
            return string.Format("{0}.{1}.", declaringType, method);
        }


        /// <summary>
        /// Postcondition check.
        /// </summary>
        public static void Ensure(bool assertion, string messageFormatString, params object[] formatStringParameters)
        {
            PerformCheck(assertion,
                () => ThrowExceptionOfType<PostconditionException>(messageFormatString, formatStringParameters),
                CheckType.PostCondition,
                messageFormatString,
                formatStringParameters);
        }

        /// <summary>
        /// Postcondition check. Throws exception of type T. T must contain
        /// a default constructor and a constructor that accepts a string argument for the message.
        /// </summary>
        public static void Ensure<T>(bool assertion, string messageFormatString, params object[] formatStringParameters) where T : Exception, new()
        {
            PerformCheck(assertion,
                () => ThrowExceptionOfType<T>(messageFormatString, formatStringParameters),
                CheckType.PostCondition,
                messageFormatString,
                formatStringParameters);
        }

        /// <summary>
        /// Postcondition check.
        /// </summary>
        public static void Ensure(bool assertion)
        {
            Ensure(assertion, string.Empty);
        }

        /// <summary>
        /// Invariant check.
        /// </summary>
        public static void Invariant(bool assertion, string messageFormatString, params object[] formatStringParameters)
        {
            PerformCheck(assertion,
                            () => ThrowExceptionOfType<PostconditionException>(messageFormatString, formatStringParameters),
                            CheckType.Invariant,
                            messageFormatString,
                            formatStringParameters);
        }

        /// <summary>
        /// Invariant check.
        /// </summary>
        public static void Invariant(bool assertion)
        {
            PerformCheck(assertion,
                () => ThrowExceptionOfType<PostconditionException>(string.Empty),
                CheckType.Invariant,
                string.Empty);
        }

        /// <summary>
        /// Assertion check.
        /// </summary>
        public static void Assert(bool assertion, string messageFormatString, params object[] formatStringParameters)
        {
            PerformCheck(assertion,
                () => ThrowExceptionOfType<PostconditionException>(messageFormatString, formatStringParameters),
                CheckType.Assertion,
                messageFormatString,
                formatStringParameters);
        }

        /// <summary>
        /// Assertion check.
        /// </summary>
        public static void Assert(bool assertion)
        {
            PerformCheck(assertion,
                () => ThrowExceptionOfType<PostconditionException>(string.Empty),
                CheckType.PostCondition,
                string.Empty);
        }

        /// <summary>
        /// Set this if you wish to use Trace Assert statements 
        /// instead of exception handling. 
        /// (The Check class uses exception handling by default.)
        /// </summary>
        public static bool UseAssertions
        {
            get
            {
                return _useAssertions;
            }
            set
            {
                _useAssertions = value;
            }
        }

        private static string FormattedString(string messageFormatString, params object[] formatStringParameters)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}. At method {1}",
                                        formatStringParameters.Length > 0
                                    ? String.Format(messageFormatString, formatStringParameters)
                                    : messageFormatString
                               , GetStackFrameString());
        }

        private static void PerformCheck(bool assertion, Action exceptionImpl, CheckType checkType,
            string messageFormatString, params object[] formatStringParameters)
        {
            if (UseExceptions)
            {
                if (!assertion) exceptionImpl();
            }
            else if (!assertion)
                Trace.Assert(assertion, Enum.GetName(typeof(CheckType), checkType) +
                    " failed: " + FormattedString(messageFormatString, formatStringParameters));
        }

        private static void ThrowExceptionOfType<T>(string messageFormatString,
                params object[] formatStringParameters) where T : Exception, new()
        {
            var message = FormattedString(messageFormatString, formatStringParameters);
            var classType = typeof(T);
            var classConstructor = classType.GetConstructor(new[] { message.GetType() });
            if (null == classConstructor)
            {
                throw new Exception(
                    string.Format(
                        "Cannot instantiate an exception of type '{0}' because no appropriate constructor is found. " +
                        "Add a constructor with a string argument to your exception class."
                            , classType.Name));
            }
            T classInstance = (T)classConstructor.Invoke(new object[] { message });
            throw classInstance;
        }


        /// <summary>
        /// Is exception handling being used?
        /// </summary>
        private static bool UseExceptions
        {
            get
            {
                return !_useAssertions;
            }
        }

        // Are trace assertion statements being used? 
        // Default is to use exception handling.
        private static bool _useAssertions;

        private enum CheckType
        {
            Precondition,
            Invariant,
            PostCondition,
            Assertion
        }
    } // End Check

    #region Exceptions

    /// <summary>
    /// Exception raised when a contract is broken.
    /// Catch this exception type if you wish to differentiate between 
    /// any DesignByContract exception and other runtime exceptions.
    ///  
    /// </summary>
    public class DesignByContractException : ApplicationException
    {
        protected DesignByContractException() { }
        protected DesignByContractException(string message) : base(message) { }
        protected DesignByContractException(string message, Exception inner) : base(message, inner) { }
    }

    /// <summary>
    /// Exception raised when a precondition fails.
    /// </summary>
    public class PreconditionException : DesignByContractException
    {
        /// <summary>
        /// Precondition Exception.
        /// </summary>
        public PreconditionException() { }
        /// <summary>
        /// Precondition Exception.
        /// </summary>
        public PreconditionException(string message) : base(message) { }
        /// <summary>
        /// Precondition Exception.
        /// </summary>
        public PreconditionException(string message, Exception inner) : base(message, inner) { }
    }

    /// <summary>
    /// Exception raised when a postcondition fails.
    /// </summary>
    public class PostconditionException : DesignByContractException
    {
        /// <summary>
        /// Postcondition Exception.
        /// </summary>
        public PostconditionException() { }
        /// <summary>
        /// Postcondition Exception.
        /// </summary>
        public PostconditionException(string message) : base(message) { }
        /// <summary>
        /// Postcondition Exception.
        /// </summary>
        public PostconditionException(string message, Exception inner) : base(message, inner) { }
    }

    /// <summary>
    /// Exception raised when an invariant fails.
    /// </summary>
    public class InvariantException : DesignByContractException
    {
        /// <summary>
        /// Invariant Exception.
        /// </summary>
        public InvariantException() { }
        /// <summary>
        /// Invariant Exception.
        /// </summary>
        public InvariantException(string message) : base(message) { }
        /// <summary>
        /// Invariant Exception.
        /// </summary>
        public InvariantException(string message, Exception inner) : base(message, inner) { }
    }

    /// <summary>
    /// Exception raised when an assertion fails.
    /// </summary>
    public class AssertionException : DesignByContractException
    {
        /// <summary>
        /// Assertion Exception.
        /// </summary>
        public AssertionException() { }
        /// <summary>
        /// Assertion Exception.
        /// </summary>
        public AssertionException(string message) : base(message) { }
        /// <summary>
        /// Assertion Exception.
        /// </summary>
        public AssertionException(string message, Exception inner) : base(message, inner) { }
    }

    #endregion // Exception classes
}
