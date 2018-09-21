using System.Collections;
using System.Collections.Generic;
using Assets.Tools.Utils;
using UnityEngine;

namespace Tools.UniRoutineTask {

	public class UniRoutine : IUniRoutine
	{

		private static List<UniRoutineTask> _routines = new List<UniRoutineTask>();
		private static Stack<int> _unusedSlots = new Stack<int>();
		private static UniRoutineManager _routineObject;

		public void AddRoutine(IEnumerator enumerator) {

			if (enumerator == null) return;

			//get routine from pool
			var routine = ClassPool.Spawn<UniRoutineTask>();

			routine.Initialize(enumerator);

			if (_unusedSlots.Count > 0) {
				var index = _unusedSlots.Pop();
				_routines[index] = routine;
			}
			else {			
				_routines.Add(routine);
			}

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
				_routines[i] = null;
				//routine complete, return it to pool
				routine.Dispose();
				routine.Despawn();
				_unusedSlots.Push(i);
				
			}

		}

	}
}
