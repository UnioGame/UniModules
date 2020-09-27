namespace UniGreenModules.UniGame.Core.EditorTools.Editor.Controls.AssetDropDownControl.Examples
{
    using Runtime.Attributes;
    using UnityEngine;

    [CreateAssetMenu(menuName = "UniGame/Examples/AssetDropDown/AssetDropDownExample")]
    public class AssetDropDownExample : ScriptableObject
    {

        [AssetFilter]
        public DemoAssetDropDownAsset Item;

        [AssetFilter]
        public DemoAssetDropDownAsset Item2;
        
    }
}
