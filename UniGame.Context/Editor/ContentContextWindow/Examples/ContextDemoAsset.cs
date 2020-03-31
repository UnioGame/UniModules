namespace UniGreenModules.UniGame.Core.EditorTools.Editor.Controls.AssetDropDownControl.Examples
{
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(menuName = "UniGame/GameSystem/Examples/ContextDataWindowData",fileName = "ContextDemoAsset")]
    public class ContextDemoAsset : ScriptableObject
    {
        public int OneInt;
        public float FloatValue;
        public Texture TextureValue;
        
        public List<int> Values = new List<int>(){5,4,3,2,1};
    }
}