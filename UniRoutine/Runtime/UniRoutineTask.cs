namespace UniTools.UniRoutine.Runtime
{
    using System.Collections;
    using System.Collections.Generic;
    using UniGreenModules.UniCore.Runtime.ObjectPool;

    public struct UniRoutineTask : IEnumerator<IEnumerator>
    {
        private RoutineState state;
        private IEnumerator rootEnumerator;
        private Stack<IEnumerator> awaiters;

        public bool IsCompleted => state == RoutineState.Complete;
        
        public IEnumerator Current { get; private set; }

        object IEnumerator.Current => Current;

        public UniRoutineTask(IEnumerator enumerator,bool moveNextImmediately = false)
        {
            state = RoutineState.None;
            rootEnumerator = enumerator;
            Current        = enumerator;
            awaiters = ClassPool.Spawn<Stack<IEnumerator>>();
            
            SetupEnumerator(enumerator);
            
            if (moveNextImmediately)
            {
                MoveNext();
            }
        }

        /// <summary>
        /// iterate all enumerator steps with inner iterators
        /// </summary>
        /// <returns>is iteration completed</returns>
        public bool MoveNext()
        {
            if (state == RoutineState.Complete)
                return false;
            if (state == RoutineState.Paused)
                return true;
            
            state = RoutineState.Active;
            
            var moveNext = MoveNextInner();
            state = !moveNext ? RoutineState.Complete : RoutineState.Active;

            return moveNext;
        }

        public void Pause()
        {
            if (IsCompleted) return;
            state = RoutineState.Paused;
        }

        public void Unpause()
        {
            if (IsCompleted) return;
            state = RoutineState.Active;
        }

        public void Reset()
        {
            rootEnumerator.Reset();
            SetupEnumerator(rootEnumerator);
        }

        public void Dispose()
        {
            awaiters.Clear();
            awaiters.Despawn();
            rootEnumerator = null;
            Current = null;
            state = RoutineState.Complete;
        }

        private bool MoveNextInner()
        {
            //if current already null - stop execution
            if (Current == null)
            {
                Dispose();
                return false;
            }

            //cacl nect execution step
            var moveNext = Current.MoveNext();

            //if current enumerator motion finished try get next one from stack
            if (!moveNext)
            {
                if (awaiters.Count == 0){
                    return false;
                }
                Current = awaiters.Pop();
                return true;
            }

            while (moveNext && Current.Current is IEnumerator awaiter)
            {
                //add new inner ienumerator to stack
                awaiters.Push(Current);
                Current = awaiter;
                //for new root enumerator calculate first step
                moveNext = Current.MoveNext();
            }

            return true;
        }

        private void SetupEnumerator(IEnumerator enumerator)
        {
            rootEnumerator = enumerator;
            Current        = enumerator;
            state    = RoutineState.Active;
            
            awaiters.Clear();
        }
    }

}