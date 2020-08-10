namespace UniModules.UniGame.Core.Runtime.Components
{
    using UniCore.Runtime.ProfilerTools;
#if UNITY_EDITOR
    using UnityEditor;
#endif
    using UnityEngine;

    public class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
    {
        private static bool _shuttingDown;
        private static readonly object _lock = new object();
        private static T _instance;

        public static bool Exists => _instance != null;
        
        public static T Instance {
            get {
                if (_shuttingDown) {
                    GameLog.LogWarning($"[Singleton]: Instance of {typeof(T)} already destroyed.");
                    return null;
                }

                lock (_lock) {
                    if (_instance == null) {
                        _instance = FindObjectOfType<T>() ?? CreateInstance();
                    }
                }

                return _instance;
            }
        }

        public static void DestroySingleton()
        {
            if (Exists) {
                DestroyImmediate(Instance.gameObject);
                _instance = null;
            }
        }

        protected virtual void OnInstanceCreated()
        {
        }

        protected virtual void Awake()
        {
            if (_instance != null && _instance != this) {
                DestroyImmediate(gameObject);
                return;
            }
            
            _instance = this as T;
        }

        protected virtual void OnApplicationQuit()
        {
            _shuttingDown = true;
        }

        protected virtual void OnDestroy()
        {
            _shuttingDown = true;
        }

        protected virtual void OnEnable()
        {
#if UNITY_EDITOR
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
#endif
        }

        protected virtual void OnDisable()
        {
#if UNITY_EDITOR
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
#endif
        }

        protected virtual string GetGameObjectName() => typeof(T).Name;

        private static T CreateInstance()
        {
            var singletonObject = new GameObject();

            if (Application.isPlaying) {
                DontDestroyOnLoad(singletonObject);
                singletonObject.hideFlags = HideFlags.DontSave;
            }
            else {
                singletonObject.hideFlags = HideFlags.HideAndDontSave;
            }

            var instance = singletonObject.AddComponent<T>();
            singletonObject.name = instance.GetGameObjectName();
            
            instance.OnInstanceCreated();

            return instance;
        }
        
#if UNITY_EDITOR
        private void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingPlayMode) {
                DestroySingleton();
            }
        }
#endif
    }
}