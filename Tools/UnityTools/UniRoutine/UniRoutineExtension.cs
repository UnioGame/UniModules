using System;
using System.Collections;
using Assets.Modules.UnityToolsModule.Tools.UnityTools.Interfaces;
using Tools.UniRoutineTask;

namespace UnityToolsModule.Tools.UnityTools.UniRoutine
{
    public static class UniRoutineExtension {

        public static IDisposableItem RunWithSubRoutines(this IEnumerator enumerator, 
            RoutineType routineType = RoutineType.UpdateStep)
        {
		
            return UniRoutineController.AddWithSubRoutines(enumerator,routineType);
		
        }
	
    }
}
