namespace ControlStrings
{
    using System;

    public class FuncControlStringMatcher : IControlStringMatcher
    {
        public Func<ControlString, bool> CanMatch { get; set; }

        public Func<ControlString, string> Matcher { get; set; }

        public FuncControlStringMatcher(Func<ControlString, string> matcher, Func<ControlString, bool> canMatch)
        {
            Matcher = matcher;
            CanMatch = canMatch;
        }

        public bool Matches(ControlString controlString)
        {
            return CanMatch(controlString);
        }

        public string Match(ControlString controlString)
        {
            if (!Matches(controlString))
            {
                throw new ArgumentException("Argument cannot be matched by this matcher.", "controlString");
            }

            return Matcher(controlString);
        }
    }
}