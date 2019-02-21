using System.Collections;
using System.Collections.Generic;
using UniModule.UnityTools.DataFlow;
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
using UniUiSystem.Models;

namespace UniUiSystem
{
    public class UniUiNode : UniNode
    {
        private List<UniPortValue> _uiInputs;
        private List<UniPortValue> _uiOutputs;

        private List<UniPortValue> _slotPorts;
        private List<UniPortValue> _triggersPorts;

        #region inspector

        public UiModule UiView;

//        public AssetLabelReference UiViewLabel;
//
//        public AssetReference ViewReference;

        #endregion

        protected override IEnumerator ExecuteState(IContext context)
        {
            var lifetime = GetLifeTime(context);

            var view = CreateView(lifetime, context);

            CreateModules(view, lifetime, context);

            BindTriggers(view, lifetime, context);

            return base.ExecuteState(context);
        }

        /// <summary>
        /// add ui output context
        /// </summary>
        /// <param name="trigger"></param>
        /// <param name="context"></param>
        protected void OnUiTriggerAction(IInteractionTrigger trigger, IContext context)
        {
            var portValue = GetPortValue(trigger.ItemName);

            if (trigger.IsActive)
            {
                portValue.UpdateValue(context, context);
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
                Debug.LogErrorFormat("NULL UI VIEW {0} {1}", UiView, this);
                return false;
            }

            return base.Validate(context);
        }

        protected override void OnUpdatePortsCache()
        {
            base.OnUpdatePortsCache();

            _uiInputs = new List<UniPortValue>();
            _uiOutputs = new List<UniPortValue>();

            if (!UiView)
            {
                return;
            }

            UiView.Initialize();

            UpdateTriggers(UiView);
            UpdateModulesSlots(UiView);
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

        private UiModule CreateView(ILifeTime lifetime, IContext context)
        {
            //get view context settings
            var viewSettings = Input.Get<UniUiModuleData>(context);

            var uiView = ObjectPool.Spawn(UiView);
            uiView.Initialize();

            if (viewSettings != null)
            {
                var parentDisposable = viewSettings.Transform.
                    Subscribe(uiView.SetParent);
                lifetime.AddDispose(parentDisposable);
            }

            lifetime.AddCleanUpAction(() => uiView?.Despawn());

            uiView.gameObject.SetActive(true);
            uiView.SetState(true);

            return uiView;
        }

        private void CreateModules(IUiModule view, ILifeTime lifetime, IContext context)
        {
            var slotContainer = view.Slots;
            var slots = slotContainer.Items;
            for (int i = 0; i < slots.Count; i++)
            {
                var slot = slots[i];
                var portValue = GetPortValue(slot.SlotName);

                slot.ApplySlotData(context, portValue, lifetime);
            }
        }

        private void BindTriggers(IUiModule view, ILifeTime lifetime, IContext context)
        {
            var triggers = view.Triggers;
            var interactionsDisposable = triggers.TriggersObservable.Subscribe(x => OnUiTriggerAction(x, context));

            lifetime.AddDispose(interactionsDisposable);
        }

        private void UpdateTriggers(IUiModule view)
        {
            var triggers = view.Triggers;

            foreach (var handler in triggers.Items)
            {
                var outputPort = this.UpdatePortValue(handler.ItemName, PortIO.Output);
                _uiOutputs.Add(outputPort.value);

                var inputName = GetFormatedInputName(handler.ItemName);

                var port = this.UpdatePortValue(inputName, PortIO.Input);
                var portValue = port.value;
                _uiInputs.Add(portValue);

                BindInputOutputValues(portValue, outputPort.value);
            }
        }

        private void UpdateModulesSlots(IUiModule view)
        {
            var slots = view.Slots.Items;

            for (var i = 0; i < slots.Count; i++)
            {
                var slot = slots[i];

                var outputPort = this.UpdatePortValue(slot.SlotName, PortIO.Output);
                _uiOutputs.Add(outputPort.value);
            }
        }
    }
}