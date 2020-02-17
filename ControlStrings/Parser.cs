using System.Collections.Generic;
using System.Linq;

namespace ControlStrings
{
    public class Parser
    {
        readonly IControlStringMatcher matcher;
        readonly IEnumerable<ITransformer> transformers;

        public Parser(IControlStringMatcher matcher, IEnumerable<ITransformer> transformers)
        {
            this.matcher = matcher;
            this.transformers = transformers;
        }

        public string Parse(string input)
        {
            var finder = new ControlStringFinder
            (
                controlStringStarter: '{',
                valueSeparator: ':',
                controlStringTerminator: '}',
                specialStringStarter: '[',
                specialStringTerminator: ']',
                transformerSeparator: '|'
            );

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