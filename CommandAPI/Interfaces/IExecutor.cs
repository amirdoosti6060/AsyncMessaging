namespace CommandAPI.Interfaces
{
    public interface IExecutor
    {
        string Execute(string command, params int[] parameters);
    }
}
