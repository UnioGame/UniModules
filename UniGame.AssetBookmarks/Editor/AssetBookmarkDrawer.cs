namespace UniGame.AssetBookmarks.Editor
{
    using System;
    using System.Collections.Generic;
    using Runtime.ObjectPool;
    using Runtime.ObjectPool.Extensions;
    using Sirenix.OdinInspector;
    using Sirenix.OdinInspector.Editor;
    using UniModules.Editor;
    using UnityEditor;
    using UnityEngine;
    using Object = UnityEngine.Object;

    [Serializable]
    public class AssetBookmarkDrawer
    {
        public const string EditorPrefsKey = "UniGame.AssetBookmarksData.Key";
        
        [HideInInspector]
        public AssetBookmarksData bookmarksData;

        [TabGroup("Bookmarks")]
        [ListDrawerSettings(HideAddButton = true, 
            ShowPaging = false,
            CustomRemoveIndexFunction = nameof(RemovePinned))]
        [ShowIf(nameof(HasPinned))]
        public List<AssetBookmark> pinned = new();
        
        [TabGroup("Bookmarks")]
        [ListDrawerSettings(HideAddButton = true,
            ShowPaging = false,
            HideRemoveButton = true,
            ElementColor = nameof(GetElementColor))]
        public List<AssetBookmark> bookmarks = new();

        [TabGroup("Settings")]
        [OnValueChanged(nameof(Save))]
        [InlineProperty]
        [HideLabel]
        public AssetBookmarksSettings settings = new ();
        
        public bool HasPinned => pinned.Count > 0;
        
        public void Initialize()
        {
            AssetBookmarksData data = null;

            var value = EditorPrefs.GetString(EditorPrefsKey, string.Empty);
            try
            {
                data = string.IsNullOrEmpty(value)
                    ? new AssetBookmarksData()
                    : JsonUtility.FromJson<AssetBookmarksData>(value);
            }
            catch (Exception e)
            {
                data = new AssetBookmarksData();
            }

            data ??= new AssetBookmarksData();
            bookmarksData = data;
            
            UpdateBookmarks();
        }

        [ButtonGroup(GroupID = "Bookmarks")]
        [Button(Name = "",Icon = SdfIconType.Save)]
        [PropertyOrder(-1)]
        public void Save()
        {
            if (bookmarksData == null) return;
            var value = JsonUtility.ToJson(bookmarksData);
            EditorPrefs.SetString(EditorPrefsKey, value);
        }

        [ButtonGroup(GroupID = "Bookmarks")]
        [Button(Name = "",Icon = SdfIconType.Trash)]
        [PropertyOrder(-1)]
        public void Reset()
        {
            bookmarksData.pinned.Clear();
            bookmarksData.bookmarks.Clear();
            Save();
            UpdateBookmarks();
        }
        
        public bool AddBookmark(Object assetValue)
        {
            var ignoreFolders = settings.IgnoreFolders;
            var path = AssetDatabase.GetAssetPath(assetValue);
            if (string.IsNullOrEmpty(path)) return false;
            
            if (ignoreFolders && AssetDatabase.IsValidFolder(path)) return false;
            
            var guid = assetValue.GetGUID();
            if(string.IsNullOrEmpty(guid))return false;
            
            return AddBookmark(guid);
        }

        public void UpdateBookmarks()
        {
            if(bookmarksData == null) return;
            
            settings = bookmarksData.settings;

            bookmarks.DespawnItems();
            pinned.DespawnItems();

            foreach (var selection in bookmarksData.bookmarks)
            {
                AddBookmarkView(selection,bookmarks);
            }
            
            foreach (var pined in bookmarksData.pinned)
            {
                AddBookmarkView(pined,pinned);
            }

        }
        
        public void AddBookmarkView(string guid,List<AssetBookmark> collection)
        {
            var view = CreateBookmarkView(guid);
            if (view == null) return;
            
            collection.Add(view);
        }

        public AssetBookmark CreateBookmarkView(string guid)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var assetValue = AssetDatabase.LoadAssetAtPath<Object>(path);
            if (assetValue == null) return null;
            
            var view = ClassPool.Spawn<AssetBookmark>();
            view.Initialize(bookmarksData,assetValue,guid,UpdateBookmarks);
            view.asset = assetValue;
            return view;
        }
        
        public void ValidateData()
        {
            bookmarksData.bookmarks.RemoveAll(x =>
            {
                if (string.IsNullOrEmpty(x)) return true;
                var path = AssetDatabase.GUIDToAssetPath(x);
                return string.IsNullOrEmpty(path);
            });
            
            var removedCount = bookmarksData.pinned.RemoveAll(x =>
            {
                if (string.IsNullOrEmpty(x)) return true;
                var path = AssetDatabase.GUIDToAssetPath(x);
                return string.IsNullOrEmpty(path);
            });
            
            if (removedCount > 0) Save();
        }

        public void RemovePinned(int index)
        {
            var data = bookmarksData.pinned;
            var guid = data[index];
            data.RemoveAt(index);
            Save();
            UpdateBookmarks();
        }
        
        public bool AddPinned(string guid)
        {
            var pinnedData = bookmarksData.pinned;
            if(pinnedData.Contains(guid)) return false;
            pinnedData.Add(guid);
            Save();
            return true;
        }
        
        public bool AddBookmark(string guid)
        {
            if(bookmarksData == null) return false;
            
            ValidateData();
            
            var amount = bookmarksData.settings.MaxBookmarksCount;
            var bookmarksItems = bookmarksData.bookmarks;
            var currentCount = bookmarksItems.Count;
            if(currentCount > 0 && bookmarksItems[0] == guid) return false;
            
            bookmarksItems.RemoveAll(x => string.Equals(x, guid));
            bookmarksItems.Insert(0,guid);
            
            var activeCount = bookmarksItems.Count;
            if(activeCount > amount)
                bookmarksItems.RemoveRange(amount, activeCount - amount);

            UpdateBookmarks();
            
            return true;
        }
        
        public Color GetElementColor(int index)
        {
            return index % 2 == 0 ? settings.regularColor : settings.oddColor;
        }

    }
}