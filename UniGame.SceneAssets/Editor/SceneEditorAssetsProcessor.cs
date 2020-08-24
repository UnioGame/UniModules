namespace UniGame.SceneEditorOnlyAssets.Editor
{
    using System.Collections.Generic;
    using Runtime;
    using UniGreenModules.UniCore.Runtime.DataFlow;
    using UniGreenModules.UniCore.Runtime.Rx.Extensions;
    using UniModules.UniGame.Core.EditorTools.Editor.AssetOperations;
    using UniRx;
    using UnityEditor;
    using UnityEditor.SceneManagement;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using Object = UnityEngine.Object;
    using SceneAsset = Runtime.SceneAsset;

    [InitializeOnLoad]
    public static class SceneEditorAssetsProcessor
    {
        private static LifeTimeDefinition _lifeTimeDefinition;
        private const string SceneAssetName = "SceneAsset";
        
        static SceneEditorAssetsProcessor()
        {
            _lifeTimeDefinition = new LifeTimeDefinition();
            
            Observable.FromEvent(
                x => EditorApplication.playModeStateChanged += OnPlaymodeChanged,
                x => EditorApplication.playModeStateChanged += OnPlaymodeChanged).
                Subscribe();
            
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
                case PlayModeStateChange.ExitingEditMode:
                case PlayModeStateChange.EnteredPlayMode:
                    CloseAllCommands();
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
        
        [MenuItem("GameObject/EditorOnlyAssets/Create Asset", false, 0)]
        public static void CreateAsset()
        {
            var gameObject = new GameObject(SceneAssetName, typeof(SceneAsset));
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
            foreach (var container in GetContainers(scene,Object.FindObjectsOfType<SceneAsset>())) {
                container.Save();        
            }
        }

        public static void Close(Scene scene)
        {
            if (!AssetEditorTools.IsPureEditorMode)
                return;
            foreach (var container in GetContainers(scene,Object.FindObjectsOfType<SceneAsset>())) {
                container.Save();
                container.Close();
                EditorSceneManager.SaveScene(scene);
            }
        }
        
        public static void Remove(Scene scene)
        {
            foreach (var container in GetContainers(scene,Object.FindObjectsOfType<SceneAsset>())) {
                container.Close();        
            }
        }

        public static void Open(Scene scene)
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode || IsActive.Value == false)
                return;
            foreach (var container in GetContainers(scene,Object.FindObjectsOfType<SceneAsset>())) {
                Open(container);
            }
        }
        
        private static void Initialize()
        {
            Release();

            Observable.FromEvent(x => EditorSceneManager.sceneSaving += OnSceneSaving,
                    x => EditorSceneManager.sceneSaving -= OnSceneSaving).
                Where(x => !EditorApplication.isPlayingOrWillChangePlaymode).
                Subscribe().
                AddTo(_lifeTimeDefinition);
            
            Observable.FromEvent(x => EditorSceneManager.sceneSaved += OnSceneSaved,
                    x => EditorSceneManager.sceneSaved -= OnSceneSaved).
                Where(x => !EditorApplication.isPlayingOrWillChangePlaymode).
                Subscribe().
                AddTo(_lifeTimeDefinition);
            
            Observable.FromEvent(x => EditorSceneManager.sceneClosing += OnSceneClosing,
                    x => EditorSceneManager.sceneClosing -= OnSceneClosing).
                Where(x => !EditorApplication.isPlayingOrWillChangePlaymode).
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
            var containers = Object.
                FindObjectsOfType<SceneAsset>();
            foreach (var container in GetContainers(scene,containers)) {
                container.Save();
                container.Close();
            }
        }

        private static IEnumerable<SceneAsset> GetContainers(Scene scene, SceneAsset[] assets)
        {
            foreach (var container in assets) {
                if (container.gameObject.scene == scene)
                    yield return container;
            }
        }
        
        private static SceneAsset[] GetContainers()
        {
            var containers = Object.
                FindObjectsOfType<SceneAsset>();
            return containers;
        }
    }
}
