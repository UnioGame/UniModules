namespace UniGreenModules.UniGame.Core.EditorTools.Editor.Controls.AssetDropDownControl.Examples
{
    using Runtime.Attributes;
    using UnityEngine;

    [CreateAssetMenu(menuName = "UniGame/Examples/AssetDropDown/AssetDropDownExample")]
    public class AssetDropDownExample : ScriptableObject
    {

        [AssetDropDown]
        public DemoAssetDropDownAsset Item;

        [AssetDropDown]
        public DemoAssetDropDownAsset Item2;
        
    }
}
