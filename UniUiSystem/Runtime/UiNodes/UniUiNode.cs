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
    using UniGreenModules.UniCore.Runtime.DataFlow;
    using UniGreenModules.UniCore.Runtime.Interfaces;
    using UniGreenModules.UniCore.Runtime.ObjectPool;
    using UnityEngine.ResourceManagement.AsyncOperations;

    public class UniUiNode : UniNode
    {
        
        private List<UniPortValue> uiInputs;
        private List<UniPortValue> uiOutputs;

        private List<UniPortValue> slotPorts;
        private List<UniPortValue> triggersPorts;

        #region inspector

        public ObjectInstanceData options;

        public AssetLabelReference uiViewLabel;

        public AssetReference viewReference;

        #endregion

        public AsyncOperationHandle<UiModule> UiViewHandle => viewReference.LoadAssetAsync<UiModule>();

        public override bool Validate(IContext context)
        {
            if (UiViewHandle.Status == AsyncOperationStatus.None || UiViewHandle.Status == AsyncOperationStatus.Failed)
            {
                Debug.LogErrorFormat("NULL UI VIEW {0} {1}", UiViewHandle, this);
                return false;
            }

            return base.Validate(context);
        }
        
        protected override IEnumerator OnExecuteState(IContext context)
        {
            var lifetime = LifeTime;

            //wait ui view loading
            while (UiViewHandle.IsDone == false) {
                yield return null;
            }

            if (UiViewHandle.Status == AsyncOperationStatus.None || UiViewHandle.Status == AsyncOperationStatus.Failed) {
                Debug.LogError(UiViewHandle);
                yield break;
            }
            
            var view = CreateView(UiViewHandle.Result);

            ApplyScreenSettings(view, lifetime);
            
            CreateModules(view, lifetime, context);

            BindTriggers(view, lifetime, context);

            yield return base.OnExecuteState(context);
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

            uiInputs = new List<UniPortValue>();
            uiOutputs = new List<UniPortValue>();

#if UNITY_EDITOR
            UpdateUiPorts(UiViewHandle.Result);
#endif

        }

        private void UpdateUiPorts(UiModule uiModule)
        {
            if (!uiModule)
            {
                return;
            }

            uiModule.Initialize();

            UpdateTriggers(uiModule);
            UpdateModulesSlots(uiModule);
        }

        private UiModule CreateView(UiModule source)
        {

            var uiView = ObjectPool.Spawn(source,options.Position,
                Quaternion.identity,options.Parent,options.StayAtWorld);
            
            uiView.Initialize();
            
            return uiView;
        }

        private void ApplyScreenSettings(UiModule uiView, ILifeTime lifetime)
        {
            if (options.Immortal)
            {
                DontDestroyOnLoad(uiView);
            }
            
            //get view context settings
            var viewSettings = Input.Get<UniUiModuleData>();

            if (viewSettings != null)
            {
                var parentDisposable = viewSettings.Transform.
                    Subscribe(uiView.SetParent);
                lifetime.AddDispose(parentDisposable);
            }
            lifetime.AddCleanUpAction(() => uiView?.Despawn());

            uiView.gameObject.SetActive(true);
            uiView.SetState(true);

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

                uiOutputs.Add(values.outputValue);
                uiInputs.Add(values.inputValue);
                
            }
        }

        private void UpdateModulesSlots(IUiModule view)
        {
            var slots = view.Slots.Items;

            for (var i = 0; i < slots.Count; i++)
            {
                var slot = slots[i];

                var outputPort = this.UpdatePortValue(slot.SlotName, PortIO.Output);
                uiOutputs.Add(outputPort.value);
            }
        }
    }
}