namespace UniGame.AssetBookmarks.Editor
{
    using System;
    using Sirenix.OdinInspector;
    using Object = UnityEngine.Object;

    [Serializable]
    [InlineProperty]
    public class AssetBookmark
    {
        [HideLabel]
        [HorizontalGroup]
        [InlineButton(action:nameof(UpdatePinned),
            label:"",
            icon:SdfIconType.BookmarkCheck,ShowIf = nameof(_showPinned))]
        public Object asset;
        
        private bool _pinned;
        private bool _showPinned;
        private AssetBookmarksData _data;
        private string _guid;
        private Action _refreshAction;
        
        public void Initialize(AssetBookmarksData data,Object assetItem,string guid,Action refreshAction)
        {
            asset = assetItem;
            
            _data = data;
            _guid = guid;
            _refreshAction = refreshAction;
            _pinned = _data.pinned.Contains(guid);
            _showPinned = !_pinned;
        }

        public void UpdatePinned()
        {
            _showPinned = _pinned;
            _pinned = !_pinned;
            
            var pinnedItems = _data.pinned;
            var contains = pinnedItems.Contains(_guid);
            var isDirty = false;
            
            if (_pinned && !contains)
            {
                pinnedItems.Add(_guid);
                isDirty = true;
            }
            else if (!_pinned && contains)
            {
                isDirty = pinnedItems.Remove(_guid);
            }

            if (isDirty) _refreshAction?.Invoke();
        }
        
    }
}