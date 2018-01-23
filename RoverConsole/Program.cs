using System;
using System.Collections.Generic;
using System.IO;
using RoverCore;

namespace MoveRover
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage dotnet MoveRover.dll FileName");
                return;
            }
            
            RunRoverFile(args[0]);
            var keyInfo = new ConsoleKeyInfo();

            while (keyInfo.KeyChar.ToString().ToUpper() != "X")
            {
                Console.WriteLine("X to quit, any key to run another file");
                keyInfo = Console.ReadKey(true);

                if (keyInfo.KeyChar.ToString().ToUpper() != "X")
                {
                    Console.Write("Please enter a file name: ");
                    var fileName = Console.ReadLine();
                    RunRoverFile(fileName);
                }
            }
        }

        private static void RunRoverFile(string fileName)
        {
            if (string.IsNullOrEmpty(Path.GetDirectoryName(fileName)))
            {
                fileName = $@"{Directory.GetCurrentDirectory()}\{fileName}";
            }

            if (!File.Exists(fileName))
            {
                Console.WriteLine("File not found");
                return;
            }

            var commands = LoadLinesFromFile(fileName);
            var rover = new Rover();
            Console.WriteLine($@"Executing commands from {Path.GetFileName(fileName)}");
            Console.WriteLine("---------------------------");
            Console.WriteLine(rover.ProcessCommands(commands.ToArray()));
            Console.WriteLine("---------------------------");
        }

        public static List<string> LoadLinesFromFile(string fileName)
        {
            var lines = new List<string>();

            var sr = new StreamReader(fileName);
            var line = sr.ReadLine();
            while (line != null)
            {
                lines.Add(line);
                line = sr.ReadLine();
            }
            sr.Close();

            return lines;
        }
    }
}
