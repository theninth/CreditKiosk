using System;

namespace CreditKiosk.Exceptions
{
    /// <summary>
    /// Custom exception for when log directory could not be created.
    /// </summary>
    public class CouldNotCreateLogDirException : Exception
    {
        public CouldNotCreateLogDirException()
        {
        }

        public CouldNotCreateLogDirException(string message)
            : base(message)
        {
        }

        public CouldNotCreateLogDirException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
