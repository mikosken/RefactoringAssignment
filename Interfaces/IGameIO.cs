namespace MyNaiveGameEngine
{
    public interface IGameIO
    {
        string? ReadLine();
        void WriteLine(string value);
        public T? ListSelect<T>(List<T> items);
    }
}