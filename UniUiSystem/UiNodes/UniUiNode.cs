using System.Collections;
using System.Collections.Generic;
using UniModule.UnityTools.Interfaces;
using UniModule.UnityTools.ObjectPool.Scripts;
using UniModule.UnityTools.UniStateMachine.Extensions;
using UniModule.UnityTools.UniVisualNodeSystem.Connections;
using UniStateMachine;
using UniStateMachine.Nodes;
using UnityEngine;
using UnityEngine.AddressableAssets;
using XNode;
using UniRx;
using UnityEngine.Assertions;
using UniTools.UniUiSystem;

namespace UniUiSystem
{
    public class UniUiNode : UniNode
    {
        private List<UniPortValue> _uiInputs;
        private List<UniPortValue> _uiOutputs;

        private List<UniPortValue> _slotPorts;
        private List<UniPortValue> _triggersPorts;

        #region inspector

        [HideInInspector]
        public UiModule UiView;

        public AssetLabelReference UiViewLabel;

        public AssetReference ViewReference;
        
        #endregion

        protected override IEnumerator ExecuteState(IContext context)
        {
            var lifetime = GetLifeTime(context);
            var uiView = ObjectPool.Spawn(UiView);
            
            lifetime.AddCleanUpAction(uiView.Release);

            _context.UpdateValue<IUiViewBehaviour>(context,uiView);

            var triggers = uiView.Triggers;
            
            var interactionsDisposable = triggers.TriggersObservable.
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

            _uiInputs = new List<UniPortValue>();
            _uiOutputs = new List<UniPortValue>();

            if(Application.isPlaying)
                Assert.IsFalse(UiView,$"UiNode {name} UiView is EMPTY");

            if (!UiView)
                return;
            
            UiView.Initialize();
            
            var triggers = UiView.Triggers;
            
            foreach (var handler in triggers.Items)
            {
                
                var outputPort = this.UpdatePortValue(handler.Name, PortIO.Output);
                _uiOutputs.Add(outputPort.value);
                
                var inputName = GetFormatedInputName(handler.Name);

                var port = this.UpdatePortValue(inputName, PortIO.Input);
                var portValue = port.value;
                _uiInputs.Add(portValue);

                BindInputOutputValues(portValue, outputPort.value);
            }
            
        }

        private void BindInputOutputValues(UniPortValue input, UniPortValue output)
        {
            var contextObservable = new ContextObservable<IContext>();
            
            contextObservable.SubscribeOnContextChanged(x =>
            {
                output.RemoveContext(x);
                input.RemoveContext(x);
            });
            
            input.Add(contextObservable);
        }

        private void UpdateTriggers()
        {
            
        }

        private void UpdateModules()
        {
            
        }
    }
}
