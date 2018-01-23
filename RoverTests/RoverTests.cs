using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoverCore;

namespace RoverTests
{
    [TestClass]
    public class RoverTests
    {
        [TestMethod]
        public void TestValidCommand()
        {
            var commands = new[]
            {
                "88",
                "12 E",
                "MMLMRMMRRMML"
            };

            var rover = new Rover();
            rover.ProcessCommands(commands);

            Debug.Assert(rover.State.MoveErrors.Count == 0, "Zero errors are expected");
            Debug.Assert(rover.State.CurrentLocation.X == 3, "X Should be 3");
            Debug.Assert(rover.State.CurrentLocation.Y == 3, "Y Should be 3");
            Debug.Assert(rover.State.CurrentDirection == Direction.South, $@"Direction should be {Direction.South}");
        }

        [TestMethod]
        public void TestInvalidCommand()
        {
            var commands = new[]
            {
                "88",
                "12 E",
                "MMLMRMMRRMMGL" //G is invalid
            };

            var rover = new Rover();
            rover.ProcessCommands(commands);

            Debug.Assert(rover.State.MoveErrors.Count == 1, "One errors is expected");
            Debug.Assert(rover.State.MoveErrors[0] == MoveResult.CommandError);
        }

        [TestMethod]
        public void TestXMaxExtentLimit()
        {
            var commands = new[]
            {
                "44",
                "00 E",
                "MMMMM"
            };

            var rover = new Rover();
            rover.ProcessCommands(commands);

            Debug.Assert(rover.State.MoveErrors.Count == 2, "Only 2 errors expected" );
            Debug.Assert(rover.State.MoveErrors.Contains(MoveResult.XMaxBoundaryExceeded));
        }

        [TestMethod]
        public void TestXMinExtentLimit()
        {
            var commands = new[]
            {
                "44",
                "00 W",
                "M"
            };

            var rover = new Rover();
            rover.ProcessCommands(commands);

            Debug.Assert(rover.State.MoveErrors.Count == 2, "Only 2 errors expected");
            Debug.Assert(rover.State.MoveErrors.Contains(MoveResult.XMinBoundaryExceeded));
        }

        [TestMethod]
        public void TestYMaxExtentLimit()
        {
            var commands = new[]
            {
                "44",
                "00 N",
                "MMMMM"
            };

            var rover = new Rover();
            rover.ProcessCommands(commands);

            Debug.Assert(rover.State.MoveErrors.Count == 2, "Only 2 errors expected");
            Debug.Assert(rover.State.MoveErrors.Contains(MoveResult.YMaxBoundaryExceeded));
        }

        [TestMethod]
        public void TestYMinExtentLimit()
        {
            var commands = new[]
            {
                "44",
                "00 S",
                "M"
            };

            var rover = new Rover();
            rover.ProcessCommands(commands);

            Debug.Assert(rover.State.MoveErrors.Count == 2, "Only 2 errors expected");
            Debug.Assert(rover.State.MoveErrors.Contains(MoveResult.YMinBoundaryExceeded));
        }
    }
}
