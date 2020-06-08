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
    public static class SceneEditorAssetsProcessor
    {
        private static LifeTimeDefinition _lifeTimeDefinition;
        
        static SceneEditorAssetsProcessor()
        {
            _lifeTimeDefinition = new LifeTimeDefinition();
            
            // Observable.FromEvent(
            //     x => EditorApplication.playModeStateChanged += OnPlaymodeChanged,
            //     x => EditorApplication.playModeStateChanged += OnPlaymodeChanged).
            //     Subscribe();
            
            Release();
            Initialize();
        }

        public static BoolReactiveProperty IsActive { get; private set; } = new BoolReactiveProperty(false);

        public static void SetActive(bool active)
        {
            IsActive.Value = active;
            if (active) {
                OpenAll();
            }
            else {
                CloseAll();
            }
        }

        private static void OnPlaymodeChanged(PlayModeStateChange modeStateChange)
        {
            switch (modeStateChange) {
                case PlayModeStateChange.ExitingPlayMode:
                case PlayModeStateChange.ExitingEditMode:
                case PlayModeStateChange.EnteredPlayMode:
                    Release();
                    CloseAll();
                    break;
                case PlayModeStateChange.EnteredEditMode:
                    Initialize();
                    break;
            }
        }

        public static void Release() => _lifeTimeDefinition.Release();

        [MenuItem("GameObject/EditorOnlyAssets/Open All", false, 0)]
        public static void OpenAllCommand()
        {
            SetActive(true);
            OpenAll();
        }
        
        [MenuItem("GameObject/EditorOnlyAssets/Close All", false, 0)]
        public static void CloseAllCommands()
        {
            SetActive(false);
            CloseAll();
        }
        
        public static void OpenAll()
        {
            foreach (var container in GetContainers()) {
                Open(container);  
            }
        }
        
        [MenuItem("GameObject/EditorOnlyAssets/Close All", false, 0)]
        public static void CloseAll()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
                return;
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
            if (EditorApplication.isPlayingOrWillChangePlaymode)
                return;
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
            if (EditorApplication.isPlayingOrWillChangePlaymode || IsActive.Value == false)
                return;
            foreach (var container in GetContainers(scene)) {
                Open(container);
            }
        }
        
        private static void Initialize()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode) {
                return;
            }
            
            Release();

            IsActive.
                Where(x => x).
                Subscribe(x => OpenAll()).
                AddTo(_lifeTimeDefinition);

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
            
            // Observable.FromEvent(x => EditorSceneManager.sceneOpened += OnSceneOpened,
            //         x => EditorSceneManager.sceneOpened -= OnSceneOpened).
            //     Subscribe().
            //     AddTo(_lifeTimeDefinition);
            //
            // Observable.FromEvent(x =>EditorSceneManager.sceneLoaded += OnSceneLoaded,
            //         x => EditorSceneManager.sceneLoaded += OnSceneLoaded).
            //     Subscribe().
            //     AddTo(_lifeTimeDefinition);
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

        private static void Open(ISceneEditorAsset asset)
        {
            if (IsActive.Value == false) {
                asset.Close();
                return;
            }
            asset.Open();
        }
        
        private static void OnSceneSaved(Scene scene)
        {
            if (IsActive.Value) {
                Open(scene);
            }
        }

        private static void OnSceneSaving(Scene scene,string value)
        {
            foreach (var container in GetContainers(scene)) {
                container.Save();
                container.Close();
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
