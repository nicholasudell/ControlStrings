namespace ControlStrings
{
    using System;

    public class MissingTransformerException : Exception
    {
        public MissingTransformerException()
        {
        }

        public MissingTransformerException(string message) : base(message)
        {
        }

        public MissingTransformerException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

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