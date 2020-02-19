using System.Linq;

namespace ControlStrings
{

    public class StartUpperTransformer : ITransformer
    {
        public bool Matches(string transformCode)
        {
            if (transformCode is null)
            {
                throw new System.ArgumentNullException(nameof(transformCode));
            }

            return transformCode.Equals("StartUpper");
        }

        public string Transform(string transformCode, string input)
        {
            if (input is null)
            {
                throw new System.ArgumentNullException(nameof(input));
            }

            if(input == string.Empty)
            {
                return string.Empty;
            }

            return input.First().ToString().ToUpper() + input.Substring(1);
        }
    }
}