namespace Plavalaguna.Joy.Modules.UnityBuild {
    using UnityEngine;

    public abstract class UnityBuildData : ScriptableObject, IUnityBuildData 
    {
        #region inspector data
        
        [SerializeField]
        protected int order;

        [SerializeField]
        protected bool isActive = true;
        
        #endregion

        public string Name => name;
        
        public int Order => order;

        public bool IsActive => this.isActive;
    }
}