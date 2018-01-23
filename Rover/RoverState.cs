using System;
using System.Collections.Generic;

namespace RoverCore
{
    public class RoverState
    {
        internal LocationVertex MaxExtent = new LocationVertex();
        internal Direction CurrentDirection { get; set; } = Direction.Undefined;
        internal LocationVertex CurrentLocation { get; } = new LocationVertex();
        internal List<MoveResult> MoveErrors { get; } = new List<MoveResult>();

        internal void SetPositionAndDirection(int x, int y, Direction direction)
        {
            CurrentLocation.X = x;
            CurrentLocation.Y = y;
            CurrentDirection = direction;
        }

        internal void ConfigureExtent(int maxX, int maxY)
        {
            MaxExtent.X = maxX;
            MaxExtent.Y = maxY;
        }

        public override string ToString()
        {
            return GetFormattedState();
        }

        internal string GetFormattedState()
        {
            return $@"{CurrentLocation.X} {CurrentLocation.Y} {(char)CurrentDirection}";
        }

        internal MoveResult ValidateMove()
        {
            var moveResult = MoveResult.Ok;
            switch (CurrentDirection)
            {
                case Direction.Undefined:
                    throw new InitialDirectionNotDefinedException();
                case Direction.North:
                    if (CurrentLocation.Y == MaxExtent.Y)
                    {
                        moveResult = MoveResult.YMaxBoundaryExceeded;
                    }
                    break;
                case Direction.East:
                    if (CurrentLocation.X == MaxExtent.X)
                    {
                        moveResult = MoveResult.XMaxBoundaryExceeded;
                    }
                    break;
                case Direction.South:
                    if (CurrentLocation.Y == 0)
                    {
                        moveResult = MoveResult.YMinBoundaryExceeded;
                    }
                    break;
                case Direction.West:
                    if (CurrentLocation.X == 0)
                    {
                        moveResult = MoveResult.XMinBoundaryExceeded;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return moveResult;
        }

        public RoverState CreateCopy()
        {
            var roverState = new RoverState();

            roverState.CurrentLocation.X = CurrentLocation.X;
            roverState.CurrentLocation.Y = CurrentLocation.Y;
            roverState.CurrentDirection = CurrentDirection;
            roverState.MaxExtent.X = MaxExtent.X;
            roverState.MaxExtent.Y = MaxExtent.Y;
            
            return roverState;
        }
    }
}