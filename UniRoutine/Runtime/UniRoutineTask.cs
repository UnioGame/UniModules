namespace UniTools.UniRoutine.Runtime
{
    using System.Collections;
    using System.Collections.Generic;

    public class UniRoutineTask : IEnumerator<IEnumerator>
    {
        private bool isCompleted = false;
        private IEnumerator rootEnumerator;
        private Stack<IEnumerator> awaiters = new Stack<IEnumerator>();

        public bool IsCompleted => isCompleted;
        
        public void Initialize(IEnumerator enumerator,bool moveNextImmediately = false)
        {
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
            if (isCompleted)
                return false;
            
            var moveNext = MoveNextInner();
            isCompleted = !moveNext;

            return moveNext;
        }

        public void Reset()
        {
            rootEnumerator.Reset();
            SetupEnumerator(rootEnumerator);
        }

        public void Dispose()
        {
            awaiters.Clear();
            rootEnumerator = null;
            Current = null;
            isCompleted = true;
        }

        public IEnumerator Current { get; protected set; }

        object IEnumerator.Current => Current;

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
            isCompleted    = false;
            
            awaiters.Clear();
        }
    }

}