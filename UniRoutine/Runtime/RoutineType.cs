namespace UniTools.UniRoutine.Runtime
{
    public enum RoutineType : byte
    {
        UpdateStep = 0,
        EndOfFrame = 1,
        FixedUpdate = 2,
        LateUpdate = 3,
    }
}