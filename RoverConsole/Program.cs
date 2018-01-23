using System;
using RoverCore;

namespace RoverConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            RunRover();
        }

        private static void RunRover()
        {
            var rover = new Rover();

            var commands = new []
            {
                "88",
                "12 E",
                "MMLMRMMRRMML"
            };

            Console.WriteLine(rover.ProcessCommands(commands));

            commands = new[]
            {
                "88",
                "12 E",
                "MLLMRMMRRp;MML"
            };

            Console.WriteLine(rover.ProcessCommands(commands));

            commands = new[]
            {
                "88",
                "12 E",
                "LLLMRRRMRRRRMLLLLMLRMRLMRLLLRRLM"
            };

            Console.WriteLine(rover.ProcessCommands(commands));


            Console.ReadLine();
        }
    }
}
