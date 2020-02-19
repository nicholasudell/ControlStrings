namespace ControlStrings
{
    public interface ITransformer
    {
        string Transform(string transformCode, string input);

        bool Matches(string transformCode);
    }
}