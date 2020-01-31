namespace UniGreenModules.UniUiSystem.Editor
{
    using System;
    using Runtime.Data;
    using UniCore.Runtime.Interfaces;
    using UniCore.Runtime.Utils;
    using UnityEditor;
    using UnityEngine;

    public class UiScreensMapAssetProcessor : AssetModificationProcessor
    {
        private static string[] OnWillSaveAssets(string[] paths)
        {

            foreach (var path in paths) {
                ProcessAsset(path);
            }
            
            return paths;
            
        }

        private static void ProcessAsset(string path)
        {
            var asset = AssetDatabase.LoadAssetAtPath<UiScreenMapData>(path);
            if (!asset) return;

            var screens = asset.screens;

            for (var i = 0; i < screens.Count; i++) {
                var screen = screens[i];
                var reference = screen.Screen.editorAsset as GameObject;
                
                if (!reference) {
                    screen.ScreenName = String.Empty;
                    screen.TypeId = 0;
                    continue;
                }

                var screenView = reference.GetComponent<ITypeViewModel>();
                screen.ScreenName = reference.name;
                screen.TypeId = screenView.Type.GetTypeId();
                
            }
        }
    }
}
