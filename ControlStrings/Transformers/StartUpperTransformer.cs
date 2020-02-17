using System.Linq;

namespace ControlStrings
{
    public class StartUpperTransformer : ITransformer
    {
        public bool Matches(string transformString) => transformString.Equals("StartUpper");
        public string Transform(string input) => input.First().ToString().ToUpper() + input.Substring(1);
    }
}