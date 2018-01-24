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
                Console.WriteLine();
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
            if (string.IsNullOrWhiteSpace(fileName))
            {
                Console.WriteLine("Filename not provided");
                return;
            }

            if (string.IsNullOrEmpty(Path.GetDirectoryName(fileName)))
            {
                fileName = $@"{Directory.GetCurrentDirectory()}\{fileName}";
            }

            if (!File.Exists(fileName))
            {
                Console.WriteLine("File not found");
                return;
            }

            Console.WriteLine($@"Executing commands from {Path.GetFileName(fileName)}");
            Console.WriteLine("----------------------------------------------------");
            var commands = LoadLinesFromFile(fileName);

            var rover = new Rover();
            var commandSet = new List<string>();
            foreach (var command in commands)
            {
                commandSet.Add(command);
                if (commandSet.Count % 3 == 0)
                {
                    RunCommandSet(rover, commandSet);
                    commandSet.Clear();
                }
            }
        }

        private static void RunCommandSet(Rover rover, List<string> commands)
        {
            Console.WriteLine();
            Console.WriteLine("====================================================");
            Console.WriteLine("Input Commands");
            Console.WriteLine("----------------------------------------------------");
            foreach (var command in commands)
            {
                Console.WriteLine(command);
            }
            Console.WriteLine("----------------------------------------------------");
            Console.WriteLine($"Navigation Result {rover.ProcessCommands(commands.ToArray())}");
            Console.WriteLine("====================================================");
        }

        public static List<string> LoadLinesFromFile(string fileName)
        {
            var lines = new List<string>();

            var sr = new StreamReader(fileName);
            var line = sr.ReadLine();
            while (line != null)
            {
                if (!line.StartsWith("#"))
                {
                    lines.Add(line);
                }
                line = sr.ReadLine();
            }
            sr.Close();

            return lines;
        }
    }
}
