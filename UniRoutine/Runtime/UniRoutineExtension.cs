namespace UniTools.UniRoutine.Runtime
{
    using System.Collections;
    using UniGreenModules.UniCore.Runtime.Interfaces;

    public static class UniRoutineExtension {

        public static IDisposableItem RunWithSubRoutines(this IEnumerator enumerator, 
            RoutineType routineType = RoutineType.UpdateStep)
        {
            return UniRoutineManager.RunUniRoutine(enumerator,routineType);   
        }
        
        public static IDisposableItem ExecuteRoutine(
            this IEnumerator enumerator, 
            RoutineType routineType = RoutineType.UpdateStep,
            bool moveNextImmediately = true)
        {
		
            return UniRoutineManager.RunUniRoutine(enumerator,routineType,moveNextImmediately);
		
        }
	
    }
}
