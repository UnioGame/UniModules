using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Tools.Utils;

namespace Tools.UniRoutineTask {

	public class UniRoutine : IUniRoutine
	{

		private List<UniRoutineTask> _routines = new List<UniRoutineTask>();
		private List<DisposableAction<int>> _routineDisposable = new List<DisposableAction<int>>();
		private Stack<int> _unusedSlots = new Stack<int>();
		
		private static UniRoutineManager _routineObject;

		public IDisposable AddRoutine(IEnumerator enumerator) {

			if (enumerator == null) return null;

			//get routine from pool
			var routine = ClassPool.Spawn<UniRoutineTask>();
			var disposable = ClassPool.Spawn<DisposableAction<int>>();

			routine.Initialize(enumerator);

			var slotIndex = _routines.Count;
			
			if (_unusedSlots.Count > 0) {
				var index = _unusedSlots.Pop();
				_routines[index] = routine;
				_routineDisposable[index] = disposable;
				slotIndex = index;
			}
			else {			
				_routines.Add(routine);
				_routineDisposable.Add(disposable);
			}

			disposable.Initialize(ReleaseRoutine,slotIndex);

			return disposable;
			
		}

		public void Update() {

			for (var i = 0; i < _routines.Count; i++) {

				//execute routine
				var routine = _routines[i];
				
				//if routine slot is empty skip it
				if(routine== null)
					continue;
				
				var moveNext = routine.MoveNext();
				if (moveNext) continue;

				//
				ReleaseRoutine(i);
				
			}

		}

		//stop routine and release resources
		private void ReleaseRoutine(int index)
		{
			var routine = _routines[index];
			var disposable = _routineDisposable[index];
			
			_routines[index] = null;
			_routineDisposable[index] = null;
			
			//routine complete, return it to pool
			routine.Dispose();
			routine.Despawn();
			//cleanup disposable data
			disposable.Reset();
			//mark slot as empty
			_unusedSlots.Push(index);
			
		}

	}
}
