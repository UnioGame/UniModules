namespace UniGame.AssetBookmarks.Editor
{
    using System;
    using UnityEngine;

    [Serializable]
    public class AssetBookmarksSettings
    {
        public int MaxBookmarksCount = 10;
        
        public bool IgnoreFolders = true;
        
        public Color regularColor = Color.gray;
        
        public Color oddColor = new Color(0.2f, 0.4f, 0.3f);
    }
}