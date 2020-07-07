namespace UniModules.UniGame.AddressableTools.Editor.AddressableSpriteAtlasManager
{
    using System.Collections.Generic;
    using System.Linq;
    using AddressableExtensions.Editor;
    using Runtime.AssetReferencies;
    using Runtime.SpriteAtlases;
    using UniCore.Runtime.ProfilerTools;
    using UniGreenModules.UniCore.EditorTools.Editor.AssetOperations;
    using UniGreenModules.UniCore.EditorTools.Editor.Utility;
    using UnityEditor;
    using UnityEngine.U2D;

    public class AddressableSpriteAtlasesEditorHandler 
    {
        [MenuItem("UniGame/Addressables/Reimport Atlases")]
        public static void Reimport()
        {
            var atlases = AssetEditorTools.GetAssets<SpriteAtlas>();
            var addressableAtlases = atlases.
                Where(x => x.IsInAnyAddressableAssetGroup()).
                Select(x => new AssetReferenceSpriteAtlas(AssetEditorTools.GetGUID(x))).
                ToList();

            var atlasesManagers = AssetEditorTools.GetAssets<AddressableSpriteAtlasConfiguration>();

            foreach (var manager in atlasesManagers) {
                SetupMap(manager,addressableAtlases);
                manager.MarkDirty();
            }
        }
        
        public static void SetupMap(AddressableSpriteAtlasConfiguration handler, IReadOnlyList<AssetReferenceSpriteAtlas> atlases)
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
