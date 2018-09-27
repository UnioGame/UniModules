using System;
using System.Collections;
using System.Collections.Generic;
using Tools.UniRoutineTask;
using UnityEngine;

public static class UniRoutineExtension {

	public static IDisposable RunWithSubRoutines(this IEnumerator enumerator, 
		RoutineType routineType = RoutineType.UpdateStep)
	{
		
		return UniRoutineController.AddWithSubRoutines(enumerator,routineType);
		
	}
	
}
