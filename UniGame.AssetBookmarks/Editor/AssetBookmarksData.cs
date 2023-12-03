namespace UniGame.AssetBookmarks.Editor
{
    using System;
    using System.Collections.Generic;
    using Sirenix.OdinInspector;
    using UnityEngine.Serialization;

    [Serializable]
    public class AssetBookmarksData
    {
        [FormerlySerializedAs("pinnedBookmarks")]
        [TabGroup("bookmarks")]
        public List<string> pinned = new List<string>();
        
        [TabGroup("bookmarks")]
        [PropertySpace(8)]
        public List<string> bookmarks = new List<string>();
        
        [TabGroup("settings")]
        [InlineProperty]
        [HideLabel]
        public AssetBookmarksSettings settings = new AssetBookmarksSettings();
    }
}