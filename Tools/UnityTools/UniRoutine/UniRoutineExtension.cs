using System.Collections;
using System.Collections.Generic;
using Tools.UniRoutineTask;
using UnityEngine;

public static class UniRoutineExtension {

	public static void RunWithSubRoutines(this IEnumerator enumerator, RoutineType routineType = RoutineType.UpdateStep)
	{
		
		UniRoutineController.AddWithSubRoutines(enumerator,routineType);
		
	}
	
}
