namespace UniGreenModules.UniCore.Runtime.Common
{
    using System;
    using ObjectPool.Runtime.Interfaces;

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
