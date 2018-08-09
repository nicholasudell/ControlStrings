namespace ControlStrings
{
    public interface IFactory<out T>
    {
        T Create();
    }

    public interface IFactory<out T, in V>
    {
        T Create(V param);
    }
}