using System.Collections.Generic;
using System.Linq;

namespace ControlStrings
{
    public class Parser
    {
        readonly IControlStringFinder finder;
        readonly IControlStringMatcher matcher;
        readonly ITransformer transformer;

        public Parser(IControlStringFinder finder, IControlStringMatcher matcher, ITransformer transformer)
        {
            this.finder = finder ?? throw new System.ArgumentNullException(nameof(finder));
            this.matcher = matcher ?? throw new System.ArgumentNullException(nameof(matcher));
            this.transformer = transformer;
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


                    foreach(var transformCode in controlString.Transformers)
                    {
                        if(!transformer.Matches(transformCode))
                        {
                            throw new MissingTransformerException($"Could not find a transformer that matched the string {transformCode}");
                        }

                        matchedString = transformer.Transform(transformCode, matchedString);
                    }

                    result = result.Replace(originalString, matchedString);
                }
            }

            return result;
        }
    }
}