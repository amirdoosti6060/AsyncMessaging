namespace CommandAPI.Interfaces
{
    public interface ICommand
    {
        string Execute(params int[] parameters);
    }
}
