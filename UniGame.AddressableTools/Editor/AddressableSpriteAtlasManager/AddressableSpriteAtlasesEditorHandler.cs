namespace UniModules.UniGame.AddressableTools.Editor.AddressableSpriteAtlasManager
{
    using System.Collections.Generic;
    using System.Linq;
    using AddressableExtensions.Editor;
    using Runtime.AssetReferencies;
    using Runtime.SpriteAtlases;
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
            var addressablesAtlases = atlases.
                Where(x => x.IsInAnyAddressableAssetGroup()).
                Select(x => new AssetReferenceSpriteAtlas(AssetEditorTools.GetGUID(x))).
                ToList();

            var atlaseManagers = AssetEditorTools.GetAssets<AddressableSpriteAtlasConfiguration>();

            foreach (var manager in atlaseManagers) {
                SetupMap(manager,addressablesAtlases);
                manager.MarkDirty();
            }
        }
        
        public static void SetupMap(AddressableSpriteAtlasConfiguration handler,IReadOnlyList<AssetReferenceSpriteAtlas> atlases)
        {
            var map = handler.atlasesTagsMap;
            map.Clear();

            foreach (var atlasRef in atlases) {
                var atlas = atlasRef.editorAsset;
                map[atlas.tag] = atlasRef;
            }
            
            handler.Validate();
        }
        
    }
}
