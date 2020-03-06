using UnityEditor;
using UnityEngine;

namespace UniGreenModules.AssetBundleManager.Editor.AssetReferenceViewerWindow
{
    using UnityEditor.UIElements;
    using UnityEngine.UIElements;
    using Object = UnityEngine.Object;

    public class AssetReferenceViewerWindow : EditorWindow
    {
        private TextField guidField;
        private ObjectField objectField;
        
        private Object  targetAsset;
        private string  guidValue;

        private VisualElement root;

        // Add menu named "My Window" to the Window menu
        [MenuItem("UniGame/Tools/Asset Info Viewer")]
        static void Open()
        {
            // Get existing open window or if none, make a new one:
            var window = (AssetReferenceViewerWindow) EditorWindow.GetWindow(typeof(AssetReferenceViewerWindow));
            window.titleContent = new GUIContent("Asset Info Viewer");
            window.minSize      = new Vector2(200, 30);
            window.Show();
        }

        private void OnEnable()
        {
            root = rootVisualElement;
            var scroll = new ScrollView(ScrollViewMode.Vertical);
            {
                DrawInfoView(scroll);
            }
            root.Add(scroll);
            
        }


        public void DrawInfoView(VisualElement container)
        {
            guidField = new TextField("guid:") {
                value = string.IsNullOrEmpty(guidValue) ? string.Empty : guidValue,
                style = {
                    marginTop = 6
                }
            };
            guidField.RegisterValueChangedCallback(x => UpdateGuidData(x.newValue));

            objectField = new ObjectField("asset") {
                objectType = typeof(Object),
                allowSceneObjects = true,
                style = {
                    marginTop = 6
                }
            };
            objectField.RegisterValueChangedCallback(x => UpdateGuidData(x.newValue));            

            container.Add(guidField);
            container.Add(objectField);
        }

        private void UpdateGuidData(string guid)
        {
            var assetPath = AssetDatabase.GUIDToAssetPath(guid);
            targetAsset = AssetDatabase.LoadAssetAtPath<Object>(assetPath);

            guidField.value = guid;
            objectField.value = targetAsset;
        }

        private void UpdateGuidData(Object asset)
        {
            var assetPath = AssetDatabase.GetAssetPath(asset);
            var guid      = AssetDatabase.AssetPathToGUID(assetPath);
            UpdateGuidData(guid);
        }
    }
}