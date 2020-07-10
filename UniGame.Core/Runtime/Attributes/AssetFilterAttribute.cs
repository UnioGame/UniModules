using UnityEngine;

namespace UniGreenModules.UniGame.Core.Runtime.Attributes
{
    using System;

    public class AssetFilterAttribute : PropertyAttribute
    {
        public Type FilterType;
        public bool FoldOutOpen;
        public string FolderFilter = String.Empty;
        public bool DrawWithOdin = true;
        
        public AssetFilterAttribute(Type filterType = null,bool foldOutOpen = false, string folderFilter = "",bool drawWithOdin = true)
        {
            this.FilterType = filterType;
            this.FoldOutOpen = foldOutOpen;
            this.FolderFilter = folderFilter;
            this.DrawWithOdin = drawWithOdin;
        }
        
    }
}
