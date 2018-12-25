using System;
using Assets.Modules.UnityToolsModule.Tools.UnityTools.DataFlow;
using Assets.Tools.UnityTools.Interfaces;
using Assets.Tools.UnityTools.ObjectPool.Scripts;
using UniStateMachine.NodeEditor.UiNodes;
using UnityEngine;

namespace UnityTools.UiViews {
    public interface IUiViewBehaviour : IPoolable
    {
        IObservable<IInteractionTrigger> InteractionObservable { get; }
        
        ILifeTime LifeTime { get; }
        
        bool IsActive { get; }
        IContext Context { get; }
        Canvas Canvas { get; }
        RectTransform RectTransform { get; }
        CanvasGroup CanvasGroup { get; }
        void UpdateView();

        void UpdateTriggers();
        
        void SetState(bool active);
        
    }
}