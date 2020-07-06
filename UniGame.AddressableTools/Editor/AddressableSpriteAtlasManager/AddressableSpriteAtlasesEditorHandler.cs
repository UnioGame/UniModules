namespace UniModules.UniGame.AddressableTools.Editor.AddressableSpriteAtlasManager
{
    using System.Linq;
    using AddressableExtensions.Editor;
    using Runtime.SpriteAtlases;
    using SerializableContext.Runtime.Addressables;
    using UniGreenModules.UniCore.EditorTools.Editor.AssetOperations;
    using UniGreenModules.UniCore.EditorTools.Editor.Utility;
    using UnityEditor;
    using UnityEngine.U2D;

    public class AddressableSpriteAtlasesEditorHandler 
    {
        [MenuItem("UniGame/Addressables/Validate SpriteAtlasManager")]
        public static void ValidateAtlasManager()
        {
            var atlases = AssetEditorTools.GetAssets<SpriteAtlas>();
            var addressablesAtlases = atlases.
                Where(x => x.IsInAnyAddressableAssetGroup()).
                Select(x => new AssetReferenceSpriteAtlas(AssetEditorTools.GetGUID(x))).
                ToList();

            var atlaseManagers = AssetEditorTools.GetAssets<AddressableSpriteAtlasHandler>();

            foreach (var manager in atlaseManagers) {
                manager.Set(addressablesAtlases);
                manager.MarkDirty();
            }
        }
        
    }
}
