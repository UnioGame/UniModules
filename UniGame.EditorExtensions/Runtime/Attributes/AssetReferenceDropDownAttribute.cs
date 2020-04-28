using System;

namespace UniModules.UniGame.EditorExtensions.Runtime.Attributes
{
    using UnityEngine;

    public class AssetReferenceDropDownAttribute : PropertyAttribute, IAssetReferenceDropDownInfo
    {
        public Type   baseType;
        public bool   foldOutOpen;


        public AssetReferenceDropDownAttribute(Type baseType = null,bool foldOutOpen = false)
        {
            this.baseType = baseType;
            this.foldOutOpen  = foldOutOpen;
        }

        public Type BaseType => baseType;

        public bool FoldOutOpen => foldOutOpen;
    }
}
