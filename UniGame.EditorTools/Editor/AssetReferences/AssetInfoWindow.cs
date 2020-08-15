#if ODIN_INSPECTOR


namespace UniModules.UniGame.EditorTools.Editor.AssetReferences
{
    using Core.EditorTools.Editor;
    using Core.EditorTools.Editor.AssetOperations;
    using Core.EditorTools.Editor.Tools;
    using Sirenix.OdinInspector;
    using Sirenix.OdinInspector.Editor;
    using UnityEditor;
    using UnityEngine;

    public class AssetInfoWindow : OdinEditorWindow
    {
        #region static data

        public const string DefaultToolsLocation = "Editor.Tools/Editor/";
        
        public static string DefaultEditorToolPath =
            EditorFileUtils.Combine(EditorPathConstants.GeneratedContentPath, DefaultToolsLocation);
        
        // Add menu named "My Window" to the Window menu
        [MenuItem("UniGame/Tools/Asset Info Viewer")]
        public static void Open()
        {
            var window = GetWindow<AssetInfoWindow>();
            window.titleContent = new GUIContent("Asset Info Viewer");
            window.minSize      = new Vector2(200, 30);
            window.Show();
        }
        
        #endregion

        #region inspector

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.InlineEditor(InlineEditorModes.GUIOnly,
            InlineEditorObjectFieldModes.CompletelyHidden,Expanded = true)]
#endif
        public AssetInfoEditorAsset infoView;
        
        #endregion


        protected override void OnEnable()
        {
            base.OnEnable();
            infoView = AssetEditorTools.GetAsset<AssetInfoEditorAsset>();
            if (infoView) {
                return;
            }

            infoView = ScriptableObject.CreateInstance<AssetInfoEditorAsset>();
            infoView.SaveAsset(nameof(AssetInfoEditorAsset), DefaultEditorToolPath);
        }
    }
}

#endif