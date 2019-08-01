namespace UniTools.UniRoutine.Runtime {
	using System.Collections;
	using System.Collections.Generic;
	using Interfaces;
	using UniGreenModules.UniCore.Runtime.Common;
	using UniGreenModules.UniCore.Runtime.DataStructure;
	using UniGreenModules.UniCore.Runtime.Interfaces;
	using UniGreenModules.UniCore.Runtime.ObjectPool;

	public class UniRoutine : IUniRoutine
	{
		private List<UniRoutineTask> routines = new List<UniRoutineTask>();
		private List<DisposableAction<int>> routineDisposable = new List<DisposableAction<int>>();
		private Stack<int> unusedSlots = new Stack<int>();

		public IDisposableItem AddRoutine(IEnumerator enumerator,bool moveNextImmediately = true) {

			if (enumerator == null) return null;

			//get routine from pool
			var routine = ClassPool.Spawn<UniRoutineTask>();
			var disposable = ClassPool.Spawn<DisposableAction<int>>();

			routine.Initialize(enumerator,moveNextImmediately);

			var slotIndex = routines.Count;
			
			if (unusedSlots.Count > 0) {
				var index = unusedSlots.Pop();
				routines[index] = routine;
				routineDisposable[index] = disposable;
				slotIndex = index;
			}
			else {			
				routines.Add(routine);
				routineDisposable.Add(disposable);
			}

			disposable.Initialize(ReleaseRoutine,slotIndex);

			return disposable;
			
		}

		/// <summary>
		/// update all registered routine tasks
		/// </summary>
		public void Update() {

			for (var i = 0; i < routines.Count; i++) {

				//execute routine
				var routine = routines[i];
				
				//if routine slot is empty skip it
				if(routine== null)
					continue;
				
				var moveNext = routine.MoveNext();
				if (moveNext) continue;

				//return routine task to pool
				ReleaseRoutine(i);
				
			}

		}

		//stop routine and release resources
		private void ReleaseRoutine(int index)
		{
			var routine = routines[index];
			var disposable = routineDisposable[index];
			
			routines[index] = null;
			routineDisposable[index] = null;
			
			//routine complete, return it to pool
			if (routine != null) {
				routine.Dispose();
				routine.Despawn();
			}

			//cleanup disposable data
			disposable.Reset();
			
			//mark slot as empty
			unusedSlots.Push(index);
			
		}

	}
}
