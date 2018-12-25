using System.Collections;
using Assets.Tools.UnityTools.Interfaces;

namespace Assets.Tools.UnityTools.UniRoutine
{
    public static class UniRoutineExtension {

        public static IDisposableItem RunWithSubRoutines(this IEnumerator enumerator, 
            RoutineType routineType = RoutineType.UpdateStep)
        {
		
            return UniRoutineController.AddWithSubRoutines(enumerator,routineType);
		
        }
	
    }
}
