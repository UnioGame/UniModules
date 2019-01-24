using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniModule.UnityTools.Interfaces;
using UniModule.UnityTools.ObjectPool.Scripts;
using UniModule.UnityTools.UiViews;
using UniModule.UnityTools.UniStateMachine.Extensions;
using UniStateMachine.NodeEditor.UiNodes;
using UnityEngine;
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

            _context.UpdateValue<IUiViewBehaviour>(context,uiView);

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
            
            if (trigger.IsActive)
            {
                portValue.UpdateValue(context,context);
            }
            else
            {
                portValue.RemoveContext(context);
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

        protected override void OnUpdatePortsCache()
        {
            base.OnUpdatePortsCache();

            if (!UiView)
            {
                //todo remove ui ports
                return;
            }
            
            UiView.UpdateTriggers();
            
            foreach (var handler in UiView.Triggers)
            {
                
                this.UpdatePortValue(handler.Name, PortIO.Output);

                var inputName = GetFormatedInputName(handler.Name);

                this.UpdatePortValue(inputName, PortIO.Input);
                
            }
        }
    }
}
