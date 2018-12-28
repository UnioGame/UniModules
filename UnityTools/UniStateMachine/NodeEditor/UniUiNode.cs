using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Tools.UnityTools.Interfaces;
using Assets.Tools.UnityTools.ObjectPool.Scripts;
using UniStateMachine.NodeEditor.UiNodes;
using UnityEngine;
using UnityTools.UiViews;
using UniRx;
using XNode;

namespace UniStateMachine
{
    public class UniUiNode : UniNode
    {
        #region inspector

        [HideInInspector]
        public UiViewBehaviour UiView;

        #endregion

        protected override IEnumerator ExecuteState(IContext context)
        {
            var lifetime = GetLifeTime(context);
            var uiView = ObjectPool.Spawn(UiView);
            
            lifetime.AddCleanUpAction(uiView.Release);

            _context.AddValue<IUiViewBehaviour>(context,uiView);

            var interactionsDisposable = uiView.InteractionObservable.
                Subscribe(x => OnUiTriggerAction(x,context));
            lifetime.AddDispose(interactionsDisposable);
            
            uiView.gameObject.SetActive(true);
            uiView.SetState(true);

            return base.ExecuteState(context);
        }

        protected override void OnExit(IContext context)
        {
            var view = _context.Get<IUiViewBehaviour>(context);
            view?.Despawn();
            base.OnExit(context);
        }

        /// <summary>
        /// add ui output context
        /// </summary>
        /// <param name="trigger"></param>
        /// <param name="context"></param>
        protected void OnUiTriggerAction(IInteractionTrigger trigger,IContext context)
        {

            var port = GetPort(trigger.Name);
            var portValue = GetPortValue(port);
            var value = portValue.Value;

            if (trigger.IsActive)
            {
                value.AddValue(context,context);
            }
            else
            {
                value.RemoveContext(context);
            }

        }
        
        public override bool Validate(IContext context)
        {
            if (!UiView)
            {
                Debug.LogErrorFormat("NULL UI VIEW {0} {1}",UiView,this);
                return false;
            }
            return base.Validate(context);
        }

    }
}
