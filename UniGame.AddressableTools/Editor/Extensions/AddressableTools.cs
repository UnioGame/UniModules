namespace UniModules.UniGame.AddressableExtensions.Editor
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.EditorTools.Editor.AssetOperations;
    using UnityEditor;
    using UnityEditor.AddressableAssets;
    using UnityEditor.AddressableAssets.Settings;
    using UnityEngine;
    using UnityEngine.AddressableAssets;

    public static class AddressableTools
    {
        private static AddressableAssetSettings addressableAssetSettings;
        
        public static AssetReference FindReferenceByAddress(this AddressableAssetSettings settings,string address)
        {
            var entries = new List<AddressableAssetEntry>();
            settings.GetAllAssets(entries,true,null,x => x.address == address);
            var asset = entries.FirstOrDefault();
            
            if (asset == null) {
                Debug.LogWarning($"Not found asset with address :: {address}");
                return null;
            }
            return new AssetReference(asset.guid);
        }

        public static AssetReference FindReferenceByAddress(string address)
        {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            return FindReferenceByAddress(settings, address);
        }
        
        public static string EvaluateActiveProfileString(this string key)
        {
            addressableAssetSettings = addressableAssetSettings ? addressableAssetSettings :  AssetEditorTools.GetAsset<AddressableAssetSettings>();
            if (!addressableAssetSettings) return key;
            var activeprofile = addressableAssetSettings.activeProfileId;
            var result = addressableAssetSettings.profileSettings.EvaluateString(activeprofile, key);
            return result;
        }
        
        public static AddressableAssetEntry CreateAssetEntry<T>(T source, string groupName, string label) where T : Object
        {
            var entry = CreateAssetEntry(source, groupName);
            if (source != null) {
                source.AddAddressableAssetLabel(label);
            }

            return entry;
        }

        public static AddressableAssetEntry CreateAssetEntry<T>(T source, string groupName) where T : Object
        {
            if (source == null || string.IsNullOrEmpty(groupName) || !AssetDatabase.Contains(source))
                return null;
            
            var addressableSettings = AddressableAssetSettingsDefaultObject.Settings;
            var sourcePath = AssetDatabase.GetAssetPath(source);
            var sourceGuid = AssetDatabase.AssetPathToGUID(sourcePath);
            var group = !GroupExists(groupName) ? CreateGroup(groupName) : GetGroup(groupName);

            var entry = addressableSettings.CreateOrMoveEntry(sourceGuid, group);
            entry.address = sourcePath;
            
            addressableSettings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entry, true);

            return entry;
        }

        public static AddressableAssetEntry CreateAssetEntry<T>(T source) where T : Object
        {
            if (source == null || !AssetDatabase.Contains(source))
                return null;
            
            var addressableSettings = AddressableAssetSettingsDefaultObject.Settings;
            var sourcePath = AssetDatabase.GetAssetPath(source);
            var sourceGuid = AssetDatabase.AssetPathToGUID(sourcePath);
            var entry = addressableSettings.CreateOrMoveEntry(sourceGuid, addressableSettings.DefaultGroup);
            entry.address = sourcePath;
            
            addressableSettings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entry, true);

            return entry;
        }

        public static AddressableAssetGroup GetGroup(string groupName)
        {
            if (string.IsNullOrEmpty(groupName))
                return null;
            
            var addressableSettings = AddressableAssetSettingsDefaultObject.Settings;
            return addressableSettings.FindGroup(groupName);
        }

        public static AddressableAssetGroup CreateGroup(string groupName)
        {
            if (string.IsNullOrEmpty(groupName))
                return null;
            
            var addressableSettings = AddressableAssetSettingsDefaultObject.Settings;
            var group = addressableSettings.CreateGroup(groupName, false, false, false, addressableSettings.DefaultGroup.Schemas);
            
            addressableSettings.SetDirty(AddressableAssetSettings.ModificationEvent.GroupAdded, group, true);

            return group;
        }

        public static bool GroupExists(string groupName)
        {
            var addressableSettings = AddressableAssetSettingsDefaultObject.Settings;
            return addressableSettings.FindGroup(groupName) != null;
        }
    }
}