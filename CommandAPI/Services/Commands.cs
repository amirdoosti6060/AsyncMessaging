using CommandAPI.Interfaces;

namespace CommandAPI.Services
{
    public class Adder : ICommand
    {
        public string Execute(params int[] parameters)
        {
            return parameters.Sum().ToString();
        }
    }

    public class Subtractor : ICommand
    {
        public string Execute(params int[] parameters)
        {
            return parameters.Skip(1).Aggregate(parameters[0], (result, next) => result - next).ToString();
        }
    }

    public class Multiplier : ICommand
    {
        public string Execute(params int[] parameters)
        {
            return parameters.Skip(1).Aggregate(parameters[0], (result, next) => result * next).ToString();
        }
    }

    public class Devider : ICommand
    {
        public string Execute(params int[] parameters)
        {
            return parameters.Skip(1).Aggregate(parameters[0], (result, next) => result / next).ToString();
        }
    }

    public class Saver : ICommand
    {
        public string Execute(params int[] parameters)
        {
            return string.Join(",", parameters) + " Saved!";
        }
    }

    public class Sorter : ICommand
    {
        public string Execute(params int[] parameters)
        {
            return string.Join(",", parameters.OrderBy(e => e));
        }
    }
}
