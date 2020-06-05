namespace UniModules.UniGame.Core.Runtime.ScriptableObjects
{
    using System.Diagnostics;
    using UniRx;

    public static class LifetimeObjectData
    {
        private static ReactiveCollection<LifetimeScriptableObject> _lifetimes = new ReactiveCollection<LifetimeScriptableObject>();

        public static IReadOnlyReactiveCollection<LifetimeScriptableObject> LifeTimes => _lifetimes;

        static LifetimeObjectData()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.playModeStateChanged += x => _lifetimes.Clear();
#endif
        }
        
        [Conditional("UNITY_EDITOR")]
        public static void Add(LifetimeScriptableObject lifetime)
        {
            if (_lifetimes.Contains(lifetime)) return;
            _lifetimes.Add(lifetime);
        }
        
        [Conditional("UNITY_EDITOR")]
        public static void Remove(LifetimeScriptableObject lifetime)
        {
            _lifetimes.Remove(lifetime);
        }

    }
}
