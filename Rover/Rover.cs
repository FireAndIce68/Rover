using System;
using System.Linq;

namespace RoverCore
{
    public class Rover
    {
        private readonly RoverState _roverState = new RoverState();

        public string ProcessCommands(string[] commands)
        {
            var commandSet = new CommandSet();
            try
            {
                commandSet.Parse(commands);
                if (commandSet.IsValid)
                {
                    var simulatedState = SimulateCommands(commandSet);
                    if (simulatedState.MoveErrors.Count == 0)
                    {
                        ExecuteCommands(commandSet);
                    }
                    else
                    {
                        _roverState.MoveErrors.Add(MoveResult.SimulationError);
                        _roverState.MoveErrors.AddRange(simulatedState.MoveErrors);
                    }
                }
                else
                {
                    _roverState.MoveErrors.Add(MoveResult.CommandError);
                }
            }
            catch 
            {
                _roverState.MoveErrors.Add(MoveResult.UnknownError);
            }

            var errorCount = _roverState.MoveErrors.Count;
            var errorString = $@"Errors({string.Join(",", _roverState.MoveErrors.Select(o => o.ToString()))}";
            var orientationString = GetCurrentLocationandOrientation();

            return errorCount == 0
                ? $@"0:{orientationString}"
                : $@"{errorCount}:{orientationString}:{errorString}";
        }

        private string GetCurrentLocationandOrientation()
        {
            return _roverState.GetFormattedState();
        }

        private RoverState SimulateCommands(CommandSet commandSet)
        {
            var roverState = _roverState.CreateCopy();
            roverState.ConfigureExtent(commandSet.ExtentX, commandSet.ExtentY);
            roverState.SetPositionAndDirection(commandSet.InitialX, commandSet.InitialY,commandSet.InitialDirection);
            ExecuteMovementCommands(roverState, commandSet.MovementCommands);

            return roverState;
        }

        private void ExecuteCommands(CommandSet commandSet)
        {
            _roverState.MoveErrors.Clear();
            _roverState.ConfigureExtent(commandSet.ExtentX, commandSet.ExtentY);
            _roverState.SetPositionAndDirection(commandSet.InitialX, commandSet.InitialY, commandSet.InitialDirection);

            ExecuteMovementCommands(_roverState, commandSet.MovementCommands);
        }

        private void ExecuteMovementCommands(RoverState roverState, string movementCommands)
        {
            var moveResult = MoveResult.Ok;
            foreach (var command in movementCommands)
            {
                switch (command)
                {
                    case 'M':
                        moveResult = MoveForward(roverState);
                        break;
                    case 'R':
                        RotateRight(roverState);
                        break;
                    case 'L':
                        RotateLeft(roverState);
                        break;
                    default:
                        throw new InvalidCommandException();
                }

                if (moveResult != MoveResult.Ok)
                {
                    roverState.MoveErrors.Add(moveResult);
                    break;
                }
            }
        }

        private void RotateLeft(RoverState roverState)
        {
            switch (roverState.CurrentDirection)
            {
                case Direction.Undefined:
                    throw new InitialDirectionNotDefinedException();
                case Direction.North:
                    roverState.CurrentDirection = Direction.West;
                    break;
                case Direction.East:
                    roverState.CurrentDirection = Direction.North;
                    break;
                case Direction.South:
                    roverState.CurrentDirection = Direction.East;
                    break;
                case Direction.West:
                    roverState.CurrentDirection = Direction.South;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(roverState.CurrentDirection));
            }
        }

        private void RotateRight(RoverState roverState)
        {
            switch (roverState.CurrentDirection)
            {
                case Direction.Undefined:
                    throw new InitialDirectionNotDefinedException();
                case Direction.North:
                    roverState.CurrentDirection = Direction.East;
                    break;
                case Direction.East:
                    roverState.CurrentDirection = Direction.South;
                    break;
                case Direction.South:
                    roverState.CurrentDirection = Direction.West;
                    break;
                case Direction.West:
                    roverState.CurrentDirection = Direction.North;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(roverState.CurrentDirection));
            }
        }

        private MoveResult MoveForward(RoverState roverState)
        {
            var moveResult = roverState.ValidateMove();

            if (moveResult != MoveResult.Ok)
            {
                return moveResult;
            }

            switch (roverState.CurrentDirection)
            {
                case Direction.Undefined:
                    throw new InitialDirectionNotDefinedException();
                case Direction.North:
                    roverState.CurrentLocation.Y++;
                    break;
                case Direction.East:
                    roverState.CurrentLocation.X++;
                    break;
                case Direction.South:
                    roverState.CurrentLocation.Y--;
                    break;
                case Direction.West:
                    roverState.CurrentLocation.X--;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(moveResult));
            }

            return moveResult;
        }
    }
}
