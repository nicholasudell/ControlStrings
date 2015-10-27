namespace ControlStrings
{
    using System;
    using System.Runtime.Serialization;

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

    public class ContextControlStringMatcher : IControlStringMatcher
    {
        public string Context { get; set; }

        public IControlStringMatcher Matcher { get; private set; }

        public ContextControlStringMatcher(string context, IControlStringMatcher matcher)
        {
            if (matcher == null)
            {
                throw new ArgumentNullException("matcher");
            }

            Context = context;
            Matcher = matcher;
        }

        public bool Matches(ControlString controlString)
        {
            try
            {
                return controlString.Values.Peek().Equals(Context) && Matcher.Matches(controlString.NextControlString);
            }
            catch (Exception e)
            {
                throw new ControlStringParseException("Error processing controlString " + string.Join(":", controlString.Values.ToArray()) + ".\n\r" + e.Message, e);
            }
        }

        public string Match(ControlString controlString)
        {
            if (!Matches(controlString))
            {
                throw new ArgumentException("Argument cannot be matched by this matcher.", "controlString");
            }

            return Matcher.Match(controlString.NextControlString);
        }
    }
}
