namespace UniGame.SceneEditorOnlyAssets.Editor
{
    using System.Collections.Generic;
    using Runtime;
    using UniGreenModules.UniCore.Runtime.DataFlow;
    using UniGreenModules.UniCore.Runtime.Rx.Extensions;
    using UniRx;
    using UnityEditor;
    using UnityEditor.SceneManagement;
    using UnityEngine.SceneManagement;
    using Object = UnityEngine.Object;

    [InitializeOnLoad]
    public static class SceneContainersProcessor
    {
        private static LifeTimeDefinition _lifeTimeDefinition;
        
        static SceneContainersProcessor()
        {
            _lifeTimeDefinition = new LifeTimeDefinition();
            Observable.FromEvent(
                x => EditorApplication.playModeStateChanged += OnPlaymodeChanged,
                x => EditorApplication.playModeStateChanged += OnPlaymodeChanged).
                Subscribe();
            
            Release();
            Initialize();
        }

        private static void OnPlaymodeChanged(PlayModeStateChange modeStateChange)
        {
            switch (modeStateChange) {
                case PlayModeStateChange.EnteredEditMode:
                    Initialize();
                    break;
                case PlayModeStateChange.ExitingPlayMode:
                    Release();
                    CloseAll();
                    break;
                case PlayModeStateChange.ExitingEditMode:
                    break;
            }
        }

        public static void Release() => _lifeTimeDefinition.Release();

        public static void OpenAll()
        {
            foreach (var container in GetContainers()) {
                container.Open();        
            }
        }
        
        public static void CloseAll()
        {
            foreach (var container in GetContainers()) {
                container.Close();        
            }
        }
        
        public static void Save(Scene scene)
        {
            foreach (var container in GetContainers(scene)) {
                container.Save();        
            }
        }

        public static void Close(Scene scene)
        {
            foreach (var container in GetContainers(scene)) {
                container.Save();
                container.Close();
                EditorSceneManager.SaveScene(scene);
            }
        }
        
        public static void Remove(Scene scene)
        {
            foreach (var container in GetContainers(scene)) {
                container.Close();        
            }
        }

        public static void Open(Scene scene)
        {
            foreach (var container in GetContainers(scene)) {
                container.Open();        
            }
        }
        
        private static void Initialize()
        {
            if (EditorApplication.isPlaying) {
                return;
            }
            Release();
            OpenAll();
            
            Observable.FromEvent(x => EditorSceneManager.sceneSaving += OnSceneSaving,
                    x => EditorSceneManager.sceneSaving -= OnSceneSaving).
                Subscribe().
                AddTo(_lifeTimeDefinition);
            
            Observable.FromEvent(x => EditorSceneManager.sceneSaved += OnSceneSaved,
                    x => EditorSceneManager.sceneSaved -= OnSceneSaved).
                Subscribe().
                AddTo(_lifeTimeDefinition);
            Observable.FromEvent(x => EditorSceneManager.sceneClosing += OnSceneClosing,
                    x => EditorSceneManager.sceneClosing -= OnSceneClosing).
                Subscribe().
                AddTo(_lifeTimeDefinition);
            Observable.FromEvent(x => EditorSceneManager.sceneOpened += OnSceneOpened,
                    x => EditorSceneManager.sceneOpened -= OnSceneOpened).
                Subscribe().
                AddTo(_lifeTimeDefinition);
            Observable.FromEvent(x =>EditorSceneManager.sceneLoaded += OnSceneLoaded,
                    x => EditorSceneManager.sceneLoaded += OnSceneLoaded).
                Subscribe().
                AddTo(_lifeTimeDefinition);
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Open(scene);
        }
        
        private static void OnSceneOpened(Scene scene, OpenSceneMode mode)
        {
            Open(scene);
        }
        
        private static void OnSceneClosing(Scene scene, bool removingScene)
        {
            Close(scene);
        }
        
        private static void OnSceneSaved(Scene scene)
        {
            foreach (var container in GetContainers()) {
                container.Save();
            }
        }

        private static void OnSceneSaving(Scene scene,string value)
        {
            foreach (var container in GetContainers()) {
                container.Save();
            }
        }

        private static IEnumerable<SceneEditorAsset> GetContainers(Scene scene)
        {
            var containers = Object.
                FindObjectsOfType<SceneEditorAsset>();
            foreach (var container in containers) {
                if (container.gameObject.scene == scene)
                    yield return container;
            }
        }
        
        private static SceneEditorAsset[] GetContainers()
        {
            var containers = Object.
                FindObjectsOfType<SceneEditorAsset>();
            return containers;
        }
    }
}
