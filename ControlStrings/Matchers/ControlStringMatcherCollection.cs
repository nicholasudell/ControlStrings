namespace ControlStrings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ControlStringMatcherCollection : IControlStringMatcher
    {
        public ControlStringMatcherCollection(IEnumerable<IControlStringMatcher> controlStringMatchers)
        {
            ControlStringMatchers = controlStringMatchers;
        }

        public IEnumerable<IControlStringMatcher> ControlStringMatchers { get; set; }

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

            return ControlStringMatchers.First(x => x.Matches(controlString)).Match(controlString);
        }

        public bool Matches(ControlString controlString)
        {
            if (controlString is null)
            {
                throw new ArgumentNullException(nameof(controlString));
            }

            return ControlStringMatchers.Any(x => x.Matches(controlString));
        }
    }
}