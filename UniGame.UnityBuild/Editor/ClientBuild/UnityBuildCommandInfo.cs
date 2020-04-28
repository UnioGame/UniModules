namespace UniGreenModules.UniGame.UnityBuild.Editor.ClientBuild {
    using System;
    using Interfaces;
    using UnityEngine;

    [Serializable]
    public class UnityBuildCommandInfo : IUnityBuildCommandInfo 
    {
        #region inspector data
        
        [SerializeField]
        public int priority;

        [SerializeField]
        public bool isActive = true;

        [SerializeField]
        public string name = string.Empty;
        
        #endregion

        public string Name => name;
        
        public int Priority => priority;

        public bool IsActive => this.isActive;
    }
}