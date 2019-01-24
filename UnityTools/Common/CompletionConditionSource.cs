using System;
using UniModule.UnityTools.Interfaces;
using UniModule.UnityTools.ObjectPool.Scripts;

namespace UniModule.UnityTools.Common
{
    public class CompletionConditionSource : ICompletionStatus , IPoolable
    {
    
        private Func<bool> _competionFunc;

        public bool IsComplete
        {
            get { return _competionFunc == null || _competionFunc(); }
        }

        public void Initialize(Func<bool> competionFunc) {
            _competionFunc = competionFunc;
        }

        public void Dispose()
        {
            Release();
        }
    
        public void Release()
        {
            _competionFunc = null;
        }
    }
}
