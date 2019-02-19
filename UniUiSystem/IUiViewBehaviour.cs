using System;
using UniModule.UnityTools.DataFlow;
using UniModule.UnityTools.Interfaces;
using UniModule.UnityTools.ObjectPool.Scripts;
using UniStateMachine.NodeEditor.UiNodes;
using UnityEngine;

namespace UniModule.UnityTools.UiViews {
    public interface IUiViewBehaviour : IPoolable
    {
        bool IsActive { get; }

        IContext Context { get; }
        
        Canvas Canvas { get; }
        
        RectTransform RectTransform { get; }
        
        CanvasGroup CanvasGroup { get; }
        
        void UpdateView();
        
        void SetState(bool active);
        
    }
}