using System.Collections;
using System.Collections.Generic;
using UniModule.UnityTools.UniPool.Scripts;

namespace UniModule.UnityTools.CoroutineTools
{
    public class CoroutineIterator : IPoolable
    {

        private IEnumerator _activeEnumerator;
        private IEnumerator _sourceEnumerator;
        private Stack<IEnumerator> _enumeratorStack = new Stack<IEnumerator>();

        public bool IsDone { get; protected set; }

        public void Initialize(IEnumerator enumerator)
        {
            Release();
            _sourceEnumerator = enumerator;
            _activeEnumerator = _sourceEnumerator;
            
        }

        public bool MoveNext()
        {
            _activeEnumerator = MakeStep(_activeEnumerator);
            IsDone = _activeEnumerator == null;
            return !IsDone;
        }

        public void Release()
        {
            IsDone = false;
            _enumeratorStack.Clear();
            _sourceEnumerator = null;
            _activeEnumerator = null;

        }

        private IEnumerator MakeStep(IEnumerator enumerator)
        {
            if (enumerator != null && enumerator.MoveNext()) {
                var current = enumerator.Current;
                var currentEnumerator = current as IEnumerator;
                if (currentEnumerator != null)
                {
                    _enumeratorStack.Push(_activeEnumerator);
                    return currentEnumerator;
                }
                return enumerator;
            }
            else if (_enumeratorStack.Count > 0)
            {
                return _enumeratorStack.Pop();
            }

            return null;
        }

    }
}
