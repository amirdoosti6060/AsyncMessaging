using CommandAPI.Interfaces;

namespace CommandAPI.Services
{
    public class Executor : IExecutor
    {
        public string Execute(string command, params int[] parameters)
        {
            var result = command.ToLower() switch
            {
                "add" => new Adder().Execute(parameters),
                "subtract" => new Subtractor().Execute(parameters),
                "multiply" => new Multiplier().Execute(parameters),
                "devide" => new Devider().Execute(parameters),
                "save" => new Saver().Execute(parameters),
                "sort" => new Sorter().Execute(parameters),
                _ => throw new NotImplementedException()
            };

            return result;
        }
    }
}
