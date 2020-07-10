using System;
using UnityEngine;

namespace UniModules.UniGame.Core.Runtime.Attributes.FieldTypeDrawer
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class AssetTypeFilterAttribute : PropertyAttribute
    {
        public Type Type;
        
        public AssetTypeFilterAttribute(Type assetType)
        {
            Type = assetType;
        }
    }
}
