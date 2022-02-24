using System;
using UnityEngine;

namespace UnrealEngine.Core
{
    public static class UELog
    {
        public static void Log(FLogCategory category, ELogVerbosity verbosity, FString message)
        {
            message = $"[{category}] {message}";
            switch (verbosity)
            {
                case ELogVerbosity.NoLogging: return;
                case ELogVerbosity.Fatal:
                    FatalLogException exception = new FatalLogException(message);
                    Console.Error.WriteLine(exception);
                    Debug.LogException(exception);
                    break;
                case ELogVerbosity.Error:
                    Console.Error.WriteLine(message);
                    Debug.LogError(message);
                    break;
                case ELogVerbosity.Warning:
                    Console.WriteLine(message);
                    Debug.LogWarning(message);
                    break;
                case ELogVerbosity.Display:
                    Console.WriteLine(message);
                    Debug.Log(message);
                    break;
                case ELogVerbosity.Log:
                    Console.WriteLine(message);
                    break;
                case ELogVerbosity.Verbose:
                    Console.WriteLine(message);
                    Debug.Log(message);
                    break;
                case ELogVerbosity.VeryVerbose:
                    Console.WriteLine(message);
                    Debug.Log(message);
                    break;
            }
        }

        internal class FatalLogException : Exception
        {
            public FatalLogException(string message) : base(message)
            {
                //
            }
        }
    }

    public enum ELogVerbosity
    {
        /** Not used */
        NoLogging = 0,

        /** Always prints a fatal error to console (and log file) and crashes (even if logging is disabled) */
        Fatal,

        /** 
		 * Prints an error to console (and log file). 
		 * Commandlets and the editor collect and report errors. Error messages result in commandlet failure.
		 */
        Error,

        /** 
		 * Prints a warning to console (and log file).
		 * Commandlets and the editor collect and report warnings. Warnings can be treated as an error.
		 */
        Warning,

        /** Prints a message to console (and log file) */
        Display,

        /** Prints a message to a log file (does not print to console) */
        Log,

        /** 
		 * Prints a verbose message to a log file (if Verbose logging is enabled for the given category, 
		 * usually used for detailed logging) 
		 */
        Verbose,

        /** 
		 * Prints a verbose message to a log file (if VeryVerbose logging is enabled, 
		 * usually used for detailed logging that would otherwise spam output) 
		 */
        VeryVerbose,

        All = VeryVerbose,
    }

    public partial class FLogCategory
    {
        private string _name = default;

        public static readonly FLogCategory LogCore = new FLogCategory(nameof(LogCore));

        protected FLogCategory(string name) => _name = name;

        public override string ToString()
        {
            return _name;
        }
    }
}
