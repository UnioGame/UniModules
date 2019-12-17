namespace UniGreenModules.UniGame.UnityBuild.Editor.ClientBuild {
    using System;
    using Interfaces;
    using UnityEngine;

    [Serializable]
    public class UnityBuildCommandInfo : IUnityBuildCommandInfo 
    {
        #region inspector data
        
        [SerializeField]
        protected int order;

        [SerializeField]
        protected bool isActive = true;

        [SerializeField]
        protected string name = string.Empty;
        
        #endregion

        public string Name => name;
        
        public int Order => order;

        public bool IsActive => this.isActive;
    }
}