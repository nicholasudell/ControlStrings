namespace ControlStrings
{
    public interface ITransformer
    {
        string Transform(string input);

        bool Matches(string transformString);
    }
}