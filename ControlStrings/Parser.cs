using System.Collections.Generic;
using System.Linq;

namespace ControlStrings
{
    public class Parser
    {
        readonly IControlStringFinder finder;
        readonly IControlStringMatcher matcher;
        readonly IEnumerable<ITransformer> transformers;

        public Parser(IControlStringFinder finder, IControlStringMatcher matcher, IEnumerable<ITransformer> transformers)
        {
            this.finder = finder ?? throw new System.ArgumentNullException(nameof(finder));
            this.matcher = matcher ?? throw new System.ArgumentNullException(nameof(matcher));
            this.transformers = transformers;
        }

        public string Parse(string input)
        {
            if (input is null)
            {
                throw new System.ArgumentNullException(nameof(input));
            }

            var controlStrings = finder.FindAllControlStrings(input);

            string result = input;

            foreach (var controlString in controlStrings)
            {
                if (matcher.Matches(controlString))
                {
                    var originalString = input.Substring(controlString.Index, controlString.Length);

                    var specialPrepending = finder.FindPrependingSpecial(originalString.Substring(1, originalString.Length - 2));
                    var specialPostpending = finder.FindPostpendingSpecial(originalString.Substring(1, originalString.Length - 2));

                    var matchedString = matcher.Match(controlString);

                    matchedString = string.IsNullOrEmpty(matchedString) ? matchedString : specialPrepending + matchedString + specialPostpending;

                    foreach(var transformString in controlString.Transformers)
                    {
                        var transformer = transformers.SingleOrDefault(x => x.Matches(transformString));

                        if(transformer == null)
                        {
                            throw new MissingTransformerException($"Could not find a transformer that matched the string {transformString}");
                        }

                        matchedString = transformer.Transform(matchedString);
                    }

                    result = result.Replace(originalString, matchedString);
                }
            }

            return result;
        }
    }
}