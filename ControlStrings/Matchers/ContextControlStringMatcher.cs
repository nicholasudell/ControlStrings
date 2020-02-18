namespace ControlStrings
{
    using System;

    public class ContextControlStringMatcher : IControlStringMatcher
    {
        public ContextControlStringMatcher(string context, IControlStringMatcher matcher)
        {
            if (string.IsNullOrEmpty(context))
            {
                throw new ArgumentException("Argument cannot be null or the empty string.", nameof(context));
            }

            Context = context;
            Matcher = matcher ?? throw new ArgumentNullException(nameof(matcher));
        }

        public string Context { get; set; }

        public IControlStringMatcher Matcher { get; }

        public string Match(ControlString controlString)
        {
            if (controlString is null)
            {
                throw new ArgumentNullException(nameof(controlString));
            }

            if (!Matches(controlString))
            {
                throw new ArgumentException("Argument cannot be matched by this matcher.", nameof(controlString));
            }

            return Matcher.Match(controlString.NextControlString);
        }

        public bool Matches(ControlString controlString)
        {
            if (controlString is null)
            {
                throw new ArgumentNullException(nameof(controlString));
            }

            try
            {
                return controlString.Values.Peek().Equals(Context) && Matcher.Matches(controlString.NextControlString);
            }
            catch (Exception e)
            {
                throw new ControlStringParseException("Error processing controlString " + string.Join(":", controlString.Values.ToArray()) + ".\n\r" + e.Message, e);
            }
        }
    }
}