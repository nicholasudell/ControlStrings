namespace ControlStrings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ControlStringMatcherCollection : IControlStringMatcher
    {
        public IEnumerable<IControlStringMatcher> ControlStringMatchers { get; set; }

        public ControlStringMatcherCollection(IEnumerable<IControlStringMatcher> controlStringMatchers)
        {
            ControlStringMatchers = controlStringMatchers;
        }

        public bool Matches(ControlString controlString)
        {
            return ControlStringMatchers.Any(x => x.Matches(controlString));
        }

        public string Match(ControlString controlString)
        {
            if (!Matches(controlString))
            {
                throw new ArgumentException("Argument cannot be matched by this matcher.", "controlString");
            }

            return ControlStringMatchers.First(x => x.Matches(controlString)).Match(controlString);
        }
    }
}