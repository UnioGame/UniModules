namespace UniTools.UniRoutine.Runtime {
	using System.Collections;
	using System.Collections.Generic;
	using Interfaces;
	using UniGreenModules.UniCore.Runtime.Common;
	using UniGreenModules.UniCore.Runtime.Interfaces;
	using UniGreenModules.UniCore.Runtime.ObjectPool;

	public class UniRoutine : IUniRoutine
	{
		 
		private List<RoutineItem> routines = new List<RoutineItem>(200);
		private List<RoutineItem> bufferRoutines = new List<RoutineItem>(200);
		
		public IDisposableItem AddRoutine(IEnumerator enumerator,bool moveNextImmediately = true) {

			if (enumerator == null) return null;

			//get routine from pool
			var routine = new UniRoutineTask(enumerator, moveNextImmediately);
			var disposable = ClassPool.Spawn<DisposableAction>();

			var slotIndex = routines.Count;
			
			var routineItem = new RoutineItem() {
				Disposable = disposable,
				Task = routine
			};
			
			routines.Add(routineItem);
			disposable.Initialize(() => ReleaseRoutine(slotIndex));

			return disposable;
			
		}

		/// <summary>
		/// update all registered routine tasks
		/// </summary>
		public void Update() {

			bufferRoutines.Clear();
			
			for (var i = 0; i < routines.Count; i++) {

				//execute routine
				var routine = routines[i];
				var moveNext = routine.MoveNext();

				//copy to buffer routine
				if (moveNext) {
					bufferRoutines.Add(routine);
					continue;
				}
				
				ReleaseRoutine(i);
			}

			var swapBuffer = bufferRoutines;
			bufferRoutines = routines;
			routines = swapBuffer;
			
		}

		//stop routine and release resources
		private void ReleaseRoutine(int index)
		{
			var routine = routines[index];
			routine.Release();
			routines[index] = routine;
		}

	}
}
