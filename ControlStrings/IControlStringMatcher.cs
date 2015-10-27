namespace ControlStrings
{
    public interface IControlStringMatcher
    {
        bool Matches(ControlString controlString);

        string Match(ControlString controlString);
    }
}