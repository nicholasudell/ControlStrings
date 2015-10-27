namespace ControlStrings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CollectionControlStringMatcher<T> : IControlStringMatcher where T : IControlStringMatcher
    {
        readonly Func<IEnumerable<T>> source;
        string beginningText;
        string separator;
        string terminatingText;

        public CollectionControlStringMatcher(Func<IEnumerable<T>> source, string beginningText, string separator, string terminatingText)
        {
            this.source = source;
            this.beginningText = beginningText;
            this.separator = separator;
            this.terminatingText = terminatingText;
        }

        public string Match(ControlString controlString)
        {
            return beginningText + string.Join(separator, source().Select(x => x.Match(controlString)).ToArray()) + terminatingText;
        }

        public bool Matches(ControlString controlString)
        {
            return source().All(x => x.Matches(controlString));
        }
    }
}