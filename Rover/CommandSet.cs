using System;
using System.Collections.Generic;
using System.Linq;

namespace RoverCore
{
    public class CommandSet
    {
        private readonly List<Tuple<string, string>> _optimisations = new List<Tuple<string, string>>();

        public int ExtentX;
        public int ExtentY;

        public int InitialX;
        public int InitialY;
        public Direction InitialDirection { get; internal set; }
        public bool IsValid { get; internal set; }

        public string MovementCommands;

        public CommandSet()
        {
            CreateOptimisations();
        }

        private void CreateOptimisations()
        {
            _optimisations.Add(new Tuple<string, string>("LLLL", ""));
            _optimisations.Add(new Tuple<string, string>("RRRR", ""));
            _optimisations.Add(new Tuple<string, string>("LLL","R"));
            _optimisations.Add(new Tuple<string, string>("RRR", "L"));
            _optimisations.Add(new Tuple<string, string>("LR", ""));
            _optimisations.Add(new Tuple<string, string>("RL", ""));
        }

        public List<ParseResult> Parse(string[] commands)
        {
            IsValid = false;
            commands = commands.Select(o => o.ToUpper()).ToArray();

            var parseResults = new List<ParseResult>();

            if (commands.Length > 3)
            {
                parseResults.Add(ParseResult.InvalidCommandCount);
                return parseResults;
            }

            var gridResult = ParseExtent(commands[0]);
            if (gridResult != ParseResult.Ok)
            {
                parseResults.Add(gridResult);
                return parseResults;
            }

            var initialPositionResults = ParseInitialXYAndOrientation(commands[1]);
            parseResults.AddRange(initialPositionResults);

            var movesResult = ParseMovements(commands[2]);
            if (movesResult != ParseResult.Ok)
            {
                parseResults.Add(movesResult);
            }

            IsValid = parseResults.Count == 0;
            return parseResults;
        }

        private ParseResult ParseExtent(string command)
        {
            try
            {
                if (command.Length > 2)
                {
                    return ParseResult.InvalidExtentCommand;
                }

                ExtentX = int.Parse(command[0].ToString());
                ExtentY = int.Parse(command[1].ToString());

                if (ExtentX * ExtentX < 2)
                {
                    return ParseResult.ExtentTooSmall;
                }

                return ParseResult.Ok;
            }
            catch
            {
                return ParseResult.InvalidExtentCommand;
            }
        }

        private List<ParseResult> ParseInitialXYAndOrientation(string command)
        {
            var parseResults = new List<ParseResult>();
            try
            {
                if (command.Length > 4)
                {
                    parseResults.Add(ParseResult.InvalidOrientationCommand);
                    return parseResults;
                }

                InitialX = int.Parse(command[0].ToString());
                if (InitialX > ExtentX)
                {
                    parseResults.Add(ParseResult.InitialXGtExtentX);
                }

                InitialY = int.Parse(command[1].ToString());
                if (InitialY > ExtentY)
                {
                    parseResults.Add(ParseResult.InitialYGtExtentY);
                }

                var initialDirection = command[3];
                var validDirections = new[] { 'N', 'E', 'S', 'W' };
                if (!validDirections.Contains(initialDirection))
                {
                    parseResults.Add(ParseResult.InvalidOrientation);
                }
                InitialDirection = (Direction)initialDirection;
            }
            catch
            {
                //by design
            }

            return parseResults;
        }

        private ParseResult ParseMovements(string command)
        {
            var validCommands = new[] { 'M', 'R', 'L' };

            if (command.Any(instruction => !validCommands.Contains(instruction)))
            {
                return ParseResult.InvalidMovementCommand;
            }


            MovementCommands = OptimizeMovements(command);

            return ParseResult.Ok;
        }

        private string OptimizeMovements(string command)
        {
            var newCommand = command;
            var commandLength = newCommand.Length;
            var oldLength = 0;

            while (commandLength != oldLength)
            {
                oldLength = newCommand.Length;
                foreach (var optimisation in _optimisations)
                {
                    newCommand = newCommand.Replace(optimisation.Item1, optimisation.Item2);
                }
                commandLength = newCommand.Length;
            }

            return newCommand;
        }
    }
}