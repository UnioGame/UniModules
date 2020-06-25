namespace UniModules.UniGame.Core.Runtime.ScriptableObjects
{
    using System;
    using System.Diagnostics;
    using DataFlow.Interfaces;
    using UniGreenModules.UniCore.Runtime.DataFlow.Interfaces;
    using UniRx;

    public static class LifetimeObjectData
    {
        private static ReactiveCollection<WeakReference<ILifeTime>> _lifetimes = new ReactiveCollection<WeakReference<ILifeTime>>();

        public static IReadOnlyReactiveCollection<WeakReference<ILifeTime>> LifeTimes => _lifetimes;

        static LifetimeObjectData()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.playModeStateChanged += x => _lifetimes.Clear();
#endif
        }
        
        [Conditional("UNITY_EDITOR")]
        public static void Add(LifetimeScriptableObject lifetime)
        {
            if (Find(lifetime) != null)
                return;
            
            _lifetimes.Add(new WeakReference<ILifeTime>(lifetime));
            return;
        }
        
        [Conditional("UNITY_EDITOR")]
        public static void Remove(LifetimeScriptableObject lifetime)
        {
            var reference = Find(lifetime);
            if (reference != null)
                _lifetimes.Remove(reference);
        }

        public static WeakReference<ILifeTime> Find(ILifeTime lifeTime)
        {
            foreach (var reference in _lifetimes) {
                if (!reference.TryGetTarget(out var referenceLifetime)) {
                    continue;
                }

                if (referenceLifetime == lifeTime)
                    return reference;
            }

            return null;
        }
    }
}
