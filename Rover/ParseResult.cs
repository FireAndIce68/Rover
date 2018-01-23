namespace RoverCore
{
    public enum ParseResult
    {
        Undefined = 0,
        Ok,
        InvalidExtentCommand ,
        InvalidOrientation,
        InvalidMovementCommand,
        InitialXGtExtentX,
        InitialYGtExtentY,
        InvalidCommandCount,
        InvalidOrientationCommand,
        ExtentTooSmall
    }
}