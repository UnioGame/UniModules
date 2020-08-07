namespace UniModules.UniGame.Core.Runtime.Components
{
    using UniCore.Runtime.ProfilerTools;
    using UnityEngine;

    public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static bool _shuttingDown;
        private static readonly object _lock = new object();
        private static T _instance;

        public static T Instance {
            get {
                if (_shuttingDown) {
                    GameLog.LogWarning($"[Singleton]: Instance of {typeof(T)} already destroyed.");
                    return null;
                }

                lock (_lock) {
                    if (_instance == null) {
                        _instance = FindObjectOfType<T>();

                        if (_instance == null) {
                            var singletonObject = new GameObject();
                            _instance = singletonObject.AddComponent<T>();
                            singletonObject.name = $"{typeof(T)} (Singleton)";
                            
                            DontDestroyOnLoad(singletonObject);
                        }
                    }
                }

                return _instance;
            }
        }

        protected virtual void OnApplicationQuit()
        {
            _shuttingDown = true;
        }

        protected virtual void OnDestroy()
        {
            _shuttingDown = true;
        }
    }
}