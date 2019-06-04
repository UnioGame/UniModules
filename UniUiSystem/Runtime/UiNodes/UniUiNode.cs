using System.Collections;
using System.Collections.Generic;
using Modules.UniTools.UniUiSystem.Interfaces;
using UniModule.UnityTools.Interfaces;
using UniModule.UnityTools.ResourceSystem;
using UniModule.UnityTools.UniStateMachine.Extensions;
using UniModule.UnityTools.UniVisualNodeSystem.Connections;
using UniStateMachine;
using UniStateMachine.Nodes;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UniNodeSystem;
using UniRx;
using UnityEngine.Assertions;
using UniTools.UniUiSystem;
using UniUiSystem.Models;

namespace UniUiSystem
{
    using UniGreenModules.UniCore.Runtime.Attributes;
    using UniGreenModules.UniCore.Runtime.DataFlow;
    using UniGreenModules.UniCore.Runtime.Interfaces;
    using UniGreenModules.UniCore.Runtime.ObjectPool;

    public class UniUiNode : UniNode
    {
        
        private List<UniPortValue> _uiInputs;
        private List<UniPortValue> _uiOutputs;

        private List<UniPortValue> _slotPorts;
        private List<UniPortValue> _triggersPorts;

        #region inspector

        public ObjectInstanceData Options;

        [TargetType(typeof(UiModule))]
        public ResourceItem UiResource;
//        public AssetLabelReference UiViewLabel;
//
//        public AssetReference ViewReference;

        #endregion

        public UiModule UiView
        {
            get { return UiResource.Load<UiModule>(); }
        }

        public override string GetName()
        {
            var targetName = UiResource.ItemName;
            return string.IsNullOrEmpty(targetName) ? name : targetName;
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
        
        protected override IEnumerator ExecuteState(IContext context)
        {
            var lifetime = LifeTime;

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
                portValue.Add(context);
            }
            else
            {
                portValue.Remove<IContext>();
            }
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

        private UiModule CreateView(ILifeTime lifetime, IContext context)
        {
            //get view context settings
            var viewSettings = Input.Get<UniUiModuleData>();

            var uiView = ObjectPool.Spawn(UiView,Options.Position,Quaternion.identity,Options.Parent,Options.StayAtWorld);
            if (Options.Immortal)
            {
                DontDestroyOnLoad(uiView);
            }
            
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

                var values =this.CreatePortPair(handler.ItemName, true);

                _uiOutputs.Add(values.outputValue);
                _uiInputs.Add(values.inputValue);
                
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