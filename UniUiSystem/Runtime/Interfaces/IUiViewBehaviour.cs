namespace UniGreenModules.UniUiSystem.Runtime.Interfaces {
    using UniCore.Runtime.Interfaces;
    using UniCore.Runtime.ObjectPool.Interfaces;
    using UnityEngine;

    public interface IUiViewBehaviour : IPoolable
    {
        bool IsActive { get; }

        IContext Context { get; }
        
        RectTransform RectTransform { get; }
        
        void UpdateView();
        
        void SetState(bool active);
        
    }
}