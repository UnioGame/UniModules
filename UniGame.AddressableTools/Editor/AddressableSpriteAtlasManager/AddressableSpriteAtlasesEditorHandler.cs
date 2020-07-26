namespace UniModules.UniGame.AddressableTools.Editor.AddressableSpriteAtlasManager
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using AddressableExtensions.Editor;
    using Runtime.AssetReferencies;
    using Runtime.SpriteAtlases;
    using UniCore.Runtime.ProfilerTools;
    using UniGreenModules.UniCore.EditorTools.Editor.AssetOperations;
    using UniGreenModules.UniCore.EditorTools.Editor.Utility;
    using Unity.EditorCoroutines.Editor;
    using UnityEditor;
    using UnityEditor.AddressableAssets;
    using UnityEditor.AddressableAssets.Settings;
    using UnityEngine.U2D;
    
    public static class AddressableSpriteAtlasesEditorHandler 
    {
        private const int FastModeIndex = 0;

        [MenuItem("UniGame/Addressables/Reimport Atlases")]
        public static void Reimport()
        {
            var atlases = AssetEditorTools.GetAssets<SpriteAtlas>();
            var addressableAtlases = atlases.
                Where(x => x.IsInAnyAddressableAssetGroup()).
                Select(x => new AssetReferenceSpriteAtlas(AssetEditorTools.GetGUID(x))).
                ToList();

            var atlasManagers = AssetEditorTools.GetAssets<AddressableSpriteAtlasConfiguration>();

            foreach (var manager in atlasManagers) {
                SetupMap(manager,addressableAtlases);
                manager.MarkDirty();
            }
        }

        [InitializeOnLoadMethod]
        private static void SubscribeOnAddressableMode()
        {
            if (!AddressableAssetSettingsDefaultObject.SettingsExists) {
                GameLog.LogError("Addressable Asset Settings doesn't exist!");
                return;
            }

            EditorCoroutineUtility.StartCoroutineOwnerless(UpdateMode());

            AddressableAssetSettings.OnModificationGlobal += OnModification;
        }

        private static IEnumerator UpdateMode()
        {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            while (settings == null) {
                settings = AddressableAssetSettingsDefaultObject.Settings;

                yield return null;
            }
            
            var isFastMode = settings.ActivePlayModeDataBuilderIndex == FastModeIndex;
            SetFastModeToManagers(isFastMode);
        }

        private static void OnModification(AddressableAssetSettings settings, AddressableAssetSettings.ModificationEvent modificationEvent, object o)
        {
            if (modificationEvent == AddressableAssetSettings.ModificationEvent.ActivePlayModeScriptChanged) {
                var isFastMode = settings.ActivePlayModeDataBuilderIndex == FastModeIndex;
                SetFastModeToManagers(isFastMode);
            }
        }

        private static void SetFastModeToManagers(bool isFastMode)
        {
            var atlasManagers = AssetEditorTools.GetAssets<AddressableSpriteAtlasConfiguration>();
            
            foreach (var manager in atlasManagers) {
                manager.isFastMode = isFastMode;
                manager.MarkDirty();
                
                GameLog.Log($"Set fast mode [{isFastMode}] to {manager.name}");
            }
        }

        private static void SetupMap(AddressableSpriteAtlasConfiguration handler, IReadOnlyList<AssetReferenceSpriteAtlas> atlases)
        {
            var map = handler.atlasesTagsMap;
            map.Clear();

            foreach (var atlasRef in atlases) {
                var atlas = atlasRef.editorAsset;

                if (map.ContainsKey(atlas.tag)) {
                    GameLog.LogError($"{atlas.tag} [{atlasRef.AssetGUID}] is already contained in map ({handler.name})!", atlas);
                    continue;
                }

                map[atlas.tag] = atlasRef;
            }
            
            handler.Validate();
        }
    }
}
