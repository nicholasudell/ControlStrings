namespace ControlStrings
{
    using System;

    public class ControlStringParseException : Exception
    {
        public ControlStringParseException()
        {
        }

        public ControlStringParseException(string message) : base(message)
        {
        }

        public ControlStringParseException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}