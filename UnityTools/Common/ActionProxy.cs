using System;
using Assets.Tools.UnityTools.ObjectPool.Scripts;

namespace Assets.Tools.UnityTools.Common
{
    public class ActionProxy<T> : IPoolable
    {
        private Action _action;

        public void Initialize(Action action) {
            _action = action;
        }

        public virtual void Release() {
            _action?.Invoke();
        }
    }
}
