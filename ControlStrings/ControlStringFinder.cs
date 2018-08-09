namespace ControlStrings
{
    using System;
    using System.Collections.Generic;

    public class ControlStringFinder
    {
        readonly char controlStringStarter;
        readonly char controlStringTerminator;
        readonly char valueSeparator;
        readonly char specialStringStarter;
        readonly char specialStringTerminator;
        public ControlStringFinder(char controlStringStarter, char valueSeparator, char controlStringTerminator, char specialStringStarter, char specialStringTerminator)
        {
            this.controlStringStarter = controlStringStarter;
            this.valueSeparator = valueSeparator;
            this.controlStringTerminator = controlStringTerminator;
            this.specialStringStarter = specialStringStarter;
            this.specialStringTerminator = specialStringTerminator;
        }

        public IEnumerable<ControlString> FindAllControlStrings(string input)
        {
            for (var i = 0; i < input.Length; i++)
            {
                if (input[i] == controlStringStarter)
                {
                    var end = -1;

                    for (var j = i + 1; j < input.Length; j++)
                    {
                        if (input[j] == controlStringTerminator)
                        {
                            end = j;
                            break;
                        }
                    }
                    if (end == -1)
                    {
                        throw new FormatException("Input string has opening control string starter with no matching control string terminator.");
                    }

                    var internalString = input.Substring(i + 1, end - i - 1);

                    var prependSpecial = FindPrependingSpecial(internalString);
                    var postpendSpecial = FindPostpendingSpecial(internalString);

                    int prependLength = prependSpecial.Length == 0 ? 0 : prependSpecial.Length + 2;
                    int postpendLength = postpendSpecial.Length == 0 ? 0 : postpendSpecial.Length + 2;


                    var values = new Queue<string>(internalString.Substring(prependLength, internalString.Length - postpendLength - prependLength).Split(valueSeparator));

                    yield return new ControlString(i, end - i + 1, values);

                    i = end;
                }
            }
        }

        public string FindPrependingSpecial(string input)
        {
            if (input[0] == specialStringStarter)
            {
                int length = -1;

                for (var x = 1; x < input.Length; x++)
                {
                    if (input[x] == specialStringTerminator)
                    {
                        length = x;
                        break;
                    }
                }

                if (length != -1)
                {
                    return input.Substring(1, length - 1);
                }
            }

            return string.Empty;
        }

        public string FindPostpendingSpecial(string input)
        {
            if (input[input.Length - 1] == specialStringTerminator)
            {
                int start = -1;

                for (var x = input.Length - 2; x >= 0; x--)
                {
                    if (input[x] == specialStringStarter)
                    {
                        start = x;
                        break;
                    }
                }

                if (start != -1)
                {
                    return input.Substring(start + 1, input.Length - start - 2);
                }
            }

            return string.Empty;
        }
    }
}