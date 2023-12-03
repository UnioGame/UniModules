namespace UniGame.AssetBookmarks.Editor
{
    using Sirenix.OdinInspector;
    using UniModules.UniGame.Core.Editor.EditorProcessors;

    public class AssetBookmarksAsset : GeneratedAsset<AssetBookmarksAsset>
    {
        [InlineProperty]
        [HideLabel]
        public AssetBookmarksData data = new AssetBookmarksData();
    }
}