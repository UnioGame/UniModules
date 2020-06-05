using UnityEngine;

namespace UniModules.UniGame.EditorExtensions.Runtime.Attributes
{
    using System;

    public class DropDownInstanceAttribute : PropertyAttribute, IAssetDropDownInfo
    {
        public Type baseType;
        public bool foldOutOpen;
        
        
        public DropDownInstanceAttribute(Type baseType = null,bool foldOutOpen = false)
        {
            this.baseType    = baseType;
            this.foldOutOpen = foldOutOpen;
        }

        public Type BaseType => baseType;

        public bool FoldOutOpen => foldOutOpen;
    }
}
