using UniModule.UnityTools.Interfaces;
using UnityEngine;

namespace UniUiSystem {
    using UniGreenModules.UniCore.Runtime.Interfaces;
    using UniGreenModules.UniCore.Runtime.ObjectPool.Interfaces;

    public interface IUiViewBehaviour : IPoolable
    {
        bool IsActive { get; }

        IContext Context { get; }
        
        RectTransform RectTransform { get; }
        
        void UpdateView();
        
        void SetState(bool active);
        
    }
}