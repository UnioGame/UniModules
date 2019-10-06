namespace UniTools.UniRoutine.Runtime {
	
	using System.Collections;
	using System.Collections.Generic;
	using Interfaces;
	using UniGreenModules.UniCore.Runtime.Common;
	using UniGreenModules.UniCore.Runtime.Interfaces;
	using UniGreenModules.UniCore.Runtime.ObjectPool;
	using UniGreenModules.UniCore.Runtime.ProfilerTools;

	public class UniRoutine : IUniRoutine, IResetable
	{
		private List<UniRoutineTask> routines = new List<UniRoutineTask>(200);
		private List<UniRoutineTask> bufferRoutines = new List<UniRoutineTask>(200);
		
		public IDisposableItem AddRoutine(IEnumerator enumerator,bool moveNextImmediately = true) {

			if (enumerator == null) return null;

			//create disposable token
			var disposable = ClassPool.Spawn<DisposableAction>();
			var routine = ClassPool.Spawn<UniRoutineTask>();

#if UNITY_EDITOR
			if (routine.IsCompleted == false) {
				GameLog.LogError("ROUTINE: routine task is not completed");
			}
			
			if (disposable.IsDisposed == false) {
				GameLog.LogError("ROUTINE: disposable is not completed");
			}	
#endif
			
			//get routine from pool
			routine.Initialize(enumerator,disposable.Release, moveNextImmediately);
			disposable.Initialize(routine.Complete);

			routines.Add(routine);
			
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
				
				routine.Despawn();
			}

			var swapBuffer = bufferRoutines;
			bufferRoutines = routines;
			routines = swapBuffer;
			
		}

		public void Reset()
		{
			for (var i = 0; i < routines.Count; i++) {
				var routine = routines[i];
				routine.Complete();
				routine.Despawn();
			}
			
			routines.Clear();
			bufferRoutines.Clear();
		}
	}
}
