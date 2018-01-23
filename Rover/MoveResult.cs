namespace RoverCore
{
    public enum MoveResult
    {
        Undefined = 0,
        Ok = 1,
        XMaxBoundaryExceeded = 2,
        XMinBoundaryExceeded = 3,
        YMaxBoundaryExceeded = 4,
        YMinBoundaryExceeded = 5,
        UnknownError = 6,
        CommandError = 7,
        SimulationError = 8
    }
}