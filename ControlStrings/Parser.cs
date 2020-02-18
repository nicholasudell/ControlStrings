namespace ControlStrings
{
    public class Parser
    {
        readonly IControlStringFinder finder;
        readonly IControlStringMatcher matcher;

        public Parser(IControlStringFinder finder, IControlStringMatcher matcher)
        {
            this.finder = finder ?? throw new System.ArgumentNullException(nameof(finder));
            this.matcher = matcher ?? throw new System.ArgumentNullException(nameof(matcher));
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

                    var newString = matcher.Match(controlString);

                    newString = string.IsNullOrEmpty(newString) ? newString : specialPrepending + newString + specialPostpending;

                    result = result.Replace(originalString, newString);
                }
            }

            return result;
        }
    }
}