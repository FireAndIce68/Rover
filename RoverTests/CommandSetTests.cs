using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoverCore;

namespace RoverTests
{
    [TestClass]
    public class CommandSetTests
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

            var commandSet = new CommandSet();
            commandSet.Parse(commands);

            Debug.Assert(commandSet.IsValid, "Command Set should be valid");
            Debug.Assert(commandSet.ExtentX == 8, "Extent X should be 8");
            Debug.Assert(commandSet.ExtentY == 8, "Extent X should be 8");
            Debug.Assert(commandSet.InitialX == 1, "Initial X should be 1");
            Debug.Assert(commandSet.InitialY == 2, "Initial X should be 2");
            Debug.Assert(commandSet.InitialDirection == Direction.East, "Initial Direction should be East");
            Debug.Assert(commandSet.MovementCommands == "MMLMRMMRRMML", "MovementCommands should be MMLMRMMRRMML");
        }

        [TestMethod]
        public void TestExtentCommandLength()
        {
            var commands = new[]
            {
                "998", //3 characters in initial setup
                "12 E",
                "MMLMRMMRRMML"
            };

            var commandSet = new CommandSet();
            var result = commandSet.Parse(commands);

            Debug.Assert(!commandSet.IsValid, "Command Set should not be valid");
            Debug.Assert(result.Count == 1,"Only one error is expected");
            Debug.Assert(result[0] == ParseResult.InvalidExtentCommand, $"{ParseResult.InvalidExtentCommand} Expected");
        }

        [TestMethod]
        public void TestInitialMinBoundary()
        {
            var commands = new[]
            {
                "88",
                "00 E", //00 is lower left
                "MMLMRMMRRMML"
            };

            var commandSet = new CommandSet();
            commandSet.Parse(commands);

            Debug.Assert(commandSet.IsValid, "Command Set should be valid");
            Debug.Assert(commandSet.ExtentX == 8, "Extent X should be 8");
            Debug.Assert(commandSet.ExtentY == 8, "Extent X should be 8");
            Debug.Assert(commandSet.InitialX == 0, "Initial X should be 0");
            Debug.Assert(commandSet.InitialY == 0, "Initial X should be 0");
            Debug.Assert(commandSet.InitialDirection == Direction.East, "Initial Direction should be East");
            Debug.Assert(commandSet.MovementCommands == "MMLMRMMRRMML", "MovementCommands should be MMLMRMMRRMML");
        }

        [TestMethod]
        public void TestInitialMaxBoundary()
        {
            var commands = new[]
            {
                "88",
                "88 E", //88 is upper right
                "MMLMRMMRRMML"
            };

            var commandSet = new CommandSet();
            commandSet.Parse(commands);

            Debug.Assert(commandSet.IsValid, "Command Set should be valid");
            Debug.Assert(commandSet.ExtentX == 8, "Extent X should be 8");
            Debug.Assert(commandSet.ExtentY == 8, "Extent X should be 8");
            Debug.Assert(commandSet.InitialX == 8, "Initial X should be 8");
            Debug.Assert(commandSet.InitialY == 8, "Initial X should be 8");
            Debug.Assert(commandSet.InitialDirection == Direction.East, "Initial Direction should be East");
            Debug.Assert(commandSet.MovementCommands == "MMLMRMMRRMML", "MovementCommands should be MMLMRMMRRMML");
        }

        [TestMethod]
        public void TestExtentFormat()
        {
            var commands = new[]
            {
                "9A8",  //Invalid character in command
                "12 E",
                "MMLMRMMRRMML"
            };

            var commandSet = new CommandSet();
            var result = commandSet.Parse(commands);

            Debug.Assert(!commandSet.IsValid, "Command Set should not be valid");
            Debug.Assert(result.Count == 1, "Only one error is expected");
            Debug.Assert(result[0] == ParseResult.InvalidExtentCommand, $"{ParseResult.InvalidExtentCommand} Expected");
        }


        [TestMethod]
        public void TestExtentSize()
        {
            var commands = new[]
            {
                "11",  //1 extent >= 2 squares
                "12 E",
                "MMLMRMMRRMML"
            };

            var commandSet = new CommandSet();
            var result = commandSet.Parse(commands);

            Debug.Assert(!commandSet.IsValid, "Command Set should not be valid");
            Debug.Assert(result.Count == 1, "Only one error is expected");
            Debug.Assert(result[0] == ParseResult.ExtentTooSmall, $"{ParseResult.ExtentTooSmall} Expected");
        }

        [TestMethod]
        public void TestInvalidInitialPositionAndOrientationCommandLength()
        {
            var commands = new[]
            {
                "88",
                "132 E", //Command length > 4 characters
                "MMLMRMMRRMML"
            };

            var commandSet = new CommandSet();
            var result = commandSet.Parse(commands);

            Debug.Assert(!commandSet.IsValid, "Command Set should not be valid");
            Debug.Assert(result.Count == 1, "Only one error is expected");
            Debug.Assert(result[0] == ParseResult.InvalidOrientationCommand, $"{ParseResult.InvalidOrientationCommand} Expected");
        }

        [TestMethod]
        public void TestInvalidInitialXYExtents()
        {
            
            var commands = new[]
            {
                "88",
                "99 E", //99 is beyond the boundary
                "MMLMRMMRRMML"
            };

            var commandSet = new CommandSet();
            var result = commandSet.Parse(commands);

            Debug.Assert(!commandSet.IsValid, "Command Set should not be valid");
            Debug.Assert(result.Count == 2, "Two errors are expected");
            Debug.Assert(result.Contains(ParseResult.InitialXGtExtentX), $"{ParseResult.InitialXGtExtentX} Expected");
            Debug.Assert(result.Contains(ParseResult.InitialYGtExtentY), $"{ParseResult.InitialYGtExtentY} Expected");
        }

        [TestMethod]
        public void TestInvalidInitialOrientation()
        {
            var commands = new[]
            {
                "88",
                "12 Z",
                "MMLMRMMRRMML"
            };

            var commandSet = new CommandSet();
            var result = commandSet.Parse(commands);

            Debug.Assert(!commandSet.IsValid, "Command Set should not be valid");
            Debug.Assert(result.Count == 1, "One error is expected");
            Debug.Assert(result[0] == ParseResult.InvalidOrientation, $"{ParseResult.InvalidOrientation} Expected");
        }

        [TestMethod]
        public void TestInvalidMovementsCommand()
        {
            var commands = new[]
            {
                "88",
                "12 E",
                "MMLMRZMRRMML" //Z is an invalid character
            };

            var commandSet = new CommandSet();
            var result = commandSet.Parse(commands);

            Debug.Assert(!commandSet.IsValid, "Command Set should not be valid");
            Debug.Assert(result.Count == 1, "One error is expected");
            Debug.Assert(result[0] == ParseResult.InvalidMovementCommand, $"{ParseResult.InvalidMovementCommand} Expected");
        }

        [TestMethod]
        public void TestMovementOptimisations()
        {
            var commands = new[]
            {
                "88",
                "12 E",
                "LLLMRRRMRRRRMLLLLMLRMRLMRLLLRRLM" //Z is an invalid character
            };

            var commandSet = new CommandSet();
            commandSet.Parse(commands);

            Debug.Assert(commandSet.IsValid, "Command Set should be valid");
            Debug.Assert(commandSet.MovementCommands == "RMLMMMMMLM", $"Unexpected result after optimisation");
        }
    }
}
