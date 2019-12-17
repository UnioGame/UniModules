namespace UniGreenModules.UniRoutine.Runtime
{
    public struct RoutineHandler
    {
        public readonly int Id;
        public readonly RoutineType Type;

        public RoutineHandler(int id, RoutineType routineType)
        {
            Id = id;
            Type = routineType;
        }

        public override int GetHashCode() => Id;

        public bool Equals(RoutineHandler obj) => Id == obj.Id;

        public override bool Equals(object obj) 
        {
            if (obj is RoutineHandler value)
                return Id == value.Id;

            // compare elements here
            return false;
        }
    }
}