using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Tools.UnityTools.Interfaces;
using UnityEngine;

namespace Assets.Tools.UnityTools.UniRoutine
{
	public enum RoutineType : byte
	{
		UpdateStep = 0,
		EndOfFrame = 1,
		FixedUpdate,
	}
	
	public static class UniRoutineController
	{
		
		private static Lazy<UniRoutineManager> _routineManager = new Lazy<UniRoutineManager>(CreateRoutineManager);
		
		private static Dictionary<RoutineType,Lazy<IUniRoutine>> _uniRoutines = new Dictionary<RoutineType,Lazy<IUniRoutine>>()
		{
			
			{RoutineType.UpdateStep,new Lazy<IUniRoutine>(() => CreateRoutine(RoutineType.UpdateStep))},
			{RoutineType.FixedUpdate,new Lazy<IUniRoutine>(() => CreateRoutine(RoutineType.FixedUpdate))},
			{RoutineType.EndOfFrame,new Lazy<IUniRoutine>(() => CreateRoutine(RoutineType.EndOfFrame))},
			
		};

		public static IDisposableItem AddWithSubRoutines(IEnumerator enumerator, 
			RoutineType routineType = RoutineType.UpdateStep)
		{
			
			//get routine
			var routine = _uniRoutines[routineType];
			//add enumerator to routines
			var routineItem = routine.Value;
			var result = routineItem.AddRoutine(enumerator);
			return result;

		}


		private static UniRoutineManager CreateRoutineManager()
		{
			
			//create routine object and mark as immortal
			var gameObject = new GameObject("UniRoutineManager");
			var routineObject = gameObject.AddComponent<UniRoutineManager>();
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			return routineObject;
			
		}


		private static IUniRoutine CreateRoutine(RoutineType routineType)
		{
			
			//create uni routine
			var routine = new UniRoutine();
			
			//run coroutine for target update type
			ExecuteUniRoutines(routine,routineType);
			
			return routine;
			
		}

		private static IEnumerator ExecuteOnUpdate(IUniRoutine routine,RoutineType routineType) {

			while (true) {

				routine.Update();

				switch (routineType)
				{
					case RoutineType.UpdateStep:
						yield return null;
						break;
					case RoutineType.EndOfFrame:
						yield return new WaitForEndOfFrame();
						break;
					case RoutineType.FixedUpdate:
						yield return new WaitForFixedUpdate();
						break;
				}

			}

		}
		
		private static void ExecuteUniRoutines(IUniRoutine routine,RoutineType routineType) {

			_routineManager.Value.StartCoroutine(ExecuteOnUpdate(routine,routineType));

		}
	}
}
