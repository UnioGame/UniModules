namespace UniTools.UniRoutine.Runtime
{
    using System.Collections;
    using System.Collections.Generic;

    public class UniRoutineTask : IEnumerator<IEnumerator>
    {
        private bool isCompleted = false;
        private Stack<IEnumerator> awaiters = new Stack<IEnumerator>();

        public bool IsCompleted => isCompleted;
        
        public void Initialize(IEnumerator enumerator,bool moveNextImmediately = false)
        {
            awaiters.Clear();
            Current = enumerator;
            isCompleted = false;
            
            if (moveNextImmediately)
            {
                MoveNext();
            }
            
        }

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
        }

        public IEnumerator Current { get; protected set; }

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            awaiters.Clear();
        }

        private bool MoveNextInner()
        {
            if (Current == null)
            {
                Dispose();
                return false;
            }

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
                awaiters.Push(Current);
                Current = awaiter;
                moveNext = Current.MoveNext();
            }

            return true;
        }
    }

}