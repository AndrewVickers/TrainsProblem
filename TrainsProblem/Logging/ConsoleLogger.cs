using System;

namespace TrainsProblem.Logging
{
    public interface IConsoleLogger
    {
        void Info(string message);
    }

    public class ConsoleLogger : IConsoleLogger
    {
        public void Info(string message)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ForegroundColor = color;
        }
    }
}