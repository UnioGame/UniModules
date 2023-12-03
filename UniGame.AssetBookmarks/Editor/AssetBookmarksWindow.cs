namespace UniGame.AssetBookmarks.Editor
{
    using Sirenix.OdinInspector;
    using Sirenix.OdinInspector.Editor;
    using UnityEditor;

    public class AssetBookmarksWindow : OdinEditorWindow
    {
#region static data

        [MenuItem("UniGame/Tools/Bookmarks")]
        public static void Open()
        {
            var window = GetWindow<AssetBookmarksWindow>();
            window.InitializeDrawer();
            window.Show();
            window.Focus();
        }

#endregion
        
        [InlineProperty]
        [HideLabel]
        public AssetBookmarkDrawer drawer = new();

        protected override void Initialize()
        {
            InitializeDrawer();
            base.Initialize();
        }

        [Button("Reload")]
        [OnInspectorInit]
        public void InitializeDrawer()
        {
            drawer.Initialize();
            drawer.UpdateBookmarks();
            Selection.selectionChanged -= UpdateSelection;
            Selection.selectionChanged += UpdateSelection;
        }

        public void UpdateSelection()
        {
            drawer.AddBookmark(Selection.activeObject);
        }
        
    }
}
