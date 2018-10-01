using System;
using System.Collections;
using Tools.UniRoutineTask;

namespace UnityToolsModule.Tools.UnityTools.UniRoutine
{
    public static class UniRoutineExtension {

        public static IDisposable RunWithSubRoutines(this IEnumerator enumerator, 
            RoutineType routineType = RoutineType.UpdateStep)
        {
		
            return UniRoutineController.AddWithSubRoutines(enumerator,routineType);
		
        }
	
    }
}
