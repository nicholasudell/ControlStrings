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
}