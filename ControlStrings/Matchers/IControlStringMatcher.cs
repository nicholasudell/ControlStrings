namespace ControlStrings
{
    public interface IControlStringMatcher
    {
        string Match(ControlString controlString);

        bool Matches(ControlString controlString);
    }
}