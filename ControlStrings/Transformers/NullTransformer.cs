using System.Linq;

namespace ControlStrings
{
    public class NullTransformer : ITransformer
    {
        public bool Matches(string transformCode) => true;
        public string Transform(string transformCode, string input) => input;
    }
}