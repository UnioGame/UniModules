using System.Collections;
using System.Collections.Generic;

namespace UniModule.UnityTools.UniRoutine
{

    public class UniRoutineTask : IEnumerator<IEnumerator>
    {
        private bool _isCompleted = false;
        private Stack<IEnumerator> _awaiters = new Stack<IEnumerator>();

        public bool IsCompleted => _isCompleted;
        
        public void Initialize(IEnumerator enumerator,bool moveNextImmediately = false)
        {
            _awaiters.Clear();
            Current = enumerator;
            _isCompleted = false;
            
            if (moveNextImmediately)
            {
                MoveNext();
            }
            
        }

        public bool MoveNext()
        {
            if (_isCompleted)
                return false;
            
            var moveNext = MoveNextInner();
            _isCompleted = !moveNext;

            return moveNext;
        }

        public void Reset()
        {
        }

        public IEnumerator Current { get; protected set; }

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            _awaiters.Clear();
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
                if (_awaiters.Count == 0){
                    return false;
                }
                Current = _awaiters.Pop();
                return true;
            }

            while (moveNext && Current.Current is IEnumerator awaiter)
            {
                _awaiters.Push(Current);
                Current = awaiter;
                moveNext = Current.MoveNext();
            }

            return true;
        }
    }

}