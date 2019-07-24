namespace UniTools.UniRoutine.Runtime
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using Interfaces;
	using UniGreenModules.UniCore.Runtime.Interfaces;
	using UnityEngine;

	public static class UniRoutineManager
	{
		
		private static Lazy<UniRoutineRootObject> routineObject = new Lazy<UniRoutineRootObject>(CreateRoutineManager);
		
		private static Dictionary<RoutineType,Lazy<IUniRoutine>> uniRoutines = new Dictionary<RoutineType,Lazy<IUniRoutine>>()
		{
			
			{RoutineType.UpdateStep,new Lazy<IUniRoutine>(() => CreateRoutine(RoutineType.UpdateStep))},
			{RoutineType.FixedUpdate,new Lazy<IUniRoutine>(() => CreateRoutine(RoutineType.FixedUpdate))},
			{RoutineType.EndOfFrame,new Lazy<IUniRoutine>(() => CreateRoutine(RoutineType.EndOfFrame))},
			
		};

		/// <summary>
		/// start uniroutine interator
		/// </summary>
		/// <param name="enumerator">target enumerator</param>
		/// <param name="routineType">routine type</param>
		/// <param name="moveNextImmediately"></param>
		/// <returns>cancelation</returns>
		public static IDisposableItem RunUniRoutine(IEnumerator enumerator, 
			RoutineType routineType = RoutineType.UpdateStep,
			bool moveNextImmediately = true)
		{
			
			//get routine
			var routine = uniRoutines[routineType];
			//add enumerator to routines
			var routineItem = routine.Value;
			var result = routineItem.AddRoutine(enumerator,moveNextImmediately);
			return result;

		}


		private static UniRoutineRootObject CreateRoutineManager()
		{
			//create routine object and mark as immortal
			var gameObject = new GameObject("UniRoutineManager");
			var routineGameObject = gameObject.AddComponent<UniRoutineRootObject>();
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			return routineGameObject;
		}


		private static IUniRoutine CreateRoutine(RoutineType routineType)
		{
			//create uni routine
			var routine = new UniRoutine();
			//run coroutine for target update type
			ExecuteUniRoutines(routine,routineType);
			return routine;
		}

		private static IEnumerator ExecuteOnUpdate(IUniRoutine routine,RoutineType routineType)
		{

			var awaiter = GetRoutineAwaiter(routineType);
			while (true) {

				routine.Update();
				//wait time before next update
				yield return awaiter;
			}

		}

		private static YieldInstruction GetRoutineAwaiter(RoutineType routineType)
		{
			switch (routineType)
			{
				case RoutineType.UpdateStep:
					return null;
				case RoutineType.EndOfFrame:
					return new WaitForEndOfFrame();
					break;
				case RoutineType.FixedUpdate:
					return new WaitForFixedUpdate();
			}

			return null;
		}
		
		private static void ExecuteUniRoutines(IUniRoutine routine,RoutineType routineType) {

			routineObject.Value.StartCoroutine(ExecuteOnUpdate(routine,routineType));

		}
	}
}
