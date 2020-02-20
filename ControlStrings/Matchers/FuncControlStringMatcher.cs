namespace ControlStrings
{
    using System;

    public class FuncControlStringMatcher : IControlStringMatcher
    {
        public FuncControlStringMatcher(Func<ControlString, string> matcher, Func<ControlString, bool> canMatch)
        {
            Matcher = matcher;
            CanMatch = canMatch;
        }

        public Func<ControlString, bool> CanMatch { get; set; }

        public Func<ControlString, string> Matcher { get; set; }

        public string Match(ControlString controlString)
        {
            if (!Matches(controlString))
            {
                throw new ArgumentException("Argument cannot be matched by this matcher.", nameof(controlString));
            }

            return Matcher(controlString);
        }

        public bool Matches(ControlString controlString) => CanMatch(controlString);
    }
}