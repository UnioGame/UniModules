namespace UniTools.UniRoutine.Runtime
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Interfaces;
    using UniGreenModules.UniCore.Runtime.Interfaces;
    using UnityEngine;
    using UnityEngine.Profiling;

    public static class UniRoutineManager
    {
        private static RoutineValue                  emptyRoutineValue = new RoutineValue(0);
        private static int                              activeRoutineId      = 1;
        private static Dictionary<RoutineValue, IUniRoutineTask> activeRoutines       = new Dictionary<RoutineValue, IUniRoutineTask>();

        private static Lazy<UniRoutineRootObject> routineObject = new Lazy<UniRoutineRootObject>(CreateRoutineManager);

        private static List<Lazy<IUniRoutine>> uniRoutines = new List<Lazy<IUniRoutine>>() {
            new Lazy<IUniRoutine>(() => CreateRoutine(RoutineType.Update)),
            new Lazy<IUniRoutine>(() => CreateRoutine(RoutineType.FixedUpdate)),
            new Lazy<IUniRoutine>(() => CreateRoutine(RoutineType.EndOfFrame)),
            new Lazy<IUniRoutine>(() => CreateRoutine(RoutineType.LateUpdate)),
        };

        /// <summary>
        /// start uniroutine interator
        /// </summary>
        /// <param name="enumerator">target enumerator</param>
        /// <param name="routineType">routine type</param>
        /// <param name="moveNextImmediately"></param>
        /// <returns>cancelation</returns>
        public static RoutineValue RunUniRoutine(IEnumerator enumerator,
            RoutineType routineType = RoutineType.Update,
            bool moveNextImmediately = true)
        {
            //get routine
            var routine = uniRoutines[(int) routineType];
            //add enumerator to routines
            var routineItem = routine.Value;
            var routineTask = routineItem.AddRoutine(enumerator, moveNextImmediately);
            if (routineTask == null)
                return emptyRoutineValue;
            
            var id = activeRoutineId++;
            
            Profiler.BeginSample("CreateRoutineValue");
            var routineValue = new RoutineValue(id);
            activeRoutines[routineValue] = routineTask;
            Profiler.EndSample();
            
            Profiler.BeginSample("AddCleanUpAction");
            routineTask.lifeTime.AddCleanUpAction(() => TryStopRoutine(routineValue));
            Profiler.EndSample();
            
            return routineValue;
        }

        public static bool IsRoutineActive(RoutineValue value) => activeRoutines.ContainsKey(value);

        private static UniRoutineRootObject CreateRoutineManager()
        {
            //create routine object and mark as immortal
            var gameObject        = new GameObject("UniRoutineManager");
            var routineGameObject = gameObject.AddComponent<UniRoutineRootObject>();
            UnityEngine.Object.DontDestroyOnLoad(gameObject);
            return routineGameObject;
        }


        private static IUniRoutine CreateRoutine(RoutineType routineType)
        {
            //create uni routine
            var routine = new UniRoutine();
            //run coroutine for target update type
            ExecuteUniRoutines(routine, routineType);
            return routine;
        }

        private static IEnumerator ExecuteOnUpdate(IUniRoutine routine, RoutineType routineType)
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
            switch (routineType) {
                case RoutineType.Update:
                    return null;
                case RoutineType.EndOfFrame:
                    return new WaitForEndOfFrame();
                case RoutineType.FixedUpdate:
                    return new WaitForFixedUpdate();
                case RoutineType.LateUpdate:
                    return new WaitForFixedUpdate();
            }

            return null;
        }

        private static void ExecuteUniRoutines(IUniRoutine routine, RoutineType routineType)
        {
            var routineContainer = routineObject.Value;
            if (routineType == RoutineType.LateUpdate) {
                routineContainer.AddLateRoutine(routine);
                return;
            }

            routineContainer.StartCoroutine(ExecuteOnUpdate(routine, routineType));
        }

        public static bool TryStopRoutine(RoutineValue value)
        {
            if (!activeRoutines.TryGetValue(value, out var routine)) {
                return false;
            }

            activeRoutines.Remove(value);
            routine.Dispose();
            return true;

        }
    }
}