namespace ControlStrings
{
    public interface IFactory<T>
    {
        T Create();
    }

    public interface IFactory<T, V>
    {
        T Create(V param);
    }
}