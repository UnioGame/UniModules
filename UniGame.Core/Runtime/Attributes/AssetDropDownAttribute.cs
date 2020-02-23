using UnityEngine;

namespace UniGreenModules.UniGame.Core.Runtime.Attributes
{
    using System;

    public class AssetDropDownAttribute : PropertyAttribute
    {
        public Type FilterType;
        public bool FoldOutOpen;
        public string FolderFilter = String.Empty;
        
        public AssetDropDownAttribute(Type filterType = null,bool foldOutOpen = false, string folderFilter = "")
        {
            this.FilterType = filterType;
            this.FoldOutOpen = foldOutOpen;
            this.FolderFilter = folderFilter;
        }
        
    }
}
