using System;

namespace UniModules.UniGame.EditorExtensions.Runtime.Attributes
{
    using UnityEngine;

    public class ImplementationDropDownAttribute : PropertyAttribute, IAssetDropDownInfo
    {
        public Type   baseType;
        public bool   foldOutOpen;
        
        public ImplementationDropDownAttribute(Type baseType = null,bool foldOutOpen = false)
        {
            this.baseType = baseType;
            this.foldOutOpen  = foldOutOpen;
        }

        public Type BaseType => baseType;

        public bool FoldOutOpen => foldOutOpen;
    }
}
