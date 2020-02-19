using System.Collections.Generic;

namespace ControlStrings
{
    public interface IControlStringFinder
    {
        IEnumerable<ControlString> FindAllControlStrings(string input);
        string FindPostpendingSpecial(string input);
        string FindPrependingSpecial(string input);
    }
}