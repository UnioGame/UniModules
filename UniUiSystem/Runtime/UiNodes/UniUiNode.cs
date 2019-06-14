namespace UniGreenModules.UniUiSystem.Runtime.UiNodes
{
    using System.Collections;
    using System.Collections.Generic;
    using Interfaces;
    using Models;
    using UniCore.Runtime.AsyncOperations;
    using UniCore.Runtime.DataFlow;
    using UniCore.Runtime.Interfaces;
    using UniCore.Runtime.ObjectPool;
    using UniNodeSystem.Runtime;
    using UniNodeSystem.Runtime.Extensions;
    using UniNodeSystem.Runtime.NodeData;
    using UniNodeSystem.Runtime.Runtime;
    using UniRx;
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using UnityEngine.ResourceManagement.AsyncOperations;

    public class UniUiNode : UniNode
    {
#region inspector

        public ObjectInstanceData options;

        public AssetReference viewReference;

#endregion

        private List<UniPortValue> uiInputs;
        private List<UniPortValue> uiOutputs;

        private List<UniPortValue> slotPorts;
        private List<UniPortValue> triggersPorts;

        private AsyncOperationHandle<UiModule> UiViewHandle => viewReference.LoadAssetAsync<UiModule>();


        public override bool Validate(IContext context)
        {
            if (UiViewHandle.Status == AsyncOperationStatus.None || UiViewHandle.Status == AsyncOperationStatus.Failed) {
                Debug.LogErrorFormat("NULL UI VIEW {0} {1}", UiViewHandle, this);
                return false;
            }

            return base.Validate(context);
        }

        protected override IEnumerator OnExecuteState(IContext context)
        {
            var lifetime = LifeTime;

            //load view
            //TODO take shared object
            yield return UiViewHandle.Task.AwaitTask();

            if (UiViewHandle.Status == AsyncOperationStatus.None || UiViewHandle.Status == AsyncOperationStatus.Failed) {
                Debug.LogError(UiViewHandle);
                yield break;
            }

            var view = CreateView(UiViewHandle.Result);

            ApplyModuleSettings(view, lifetime);

            CreateModules(view, context,lifetime);

            BindTriggers(view, context,lifetime);

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

            if (trigger.IsActive) {
                portValue.Add(context);
            }
            else {
                portValue.Remove<IContext>();
            }
        }

        protected override void OnUpdatePortsCache()
        {
            base.OnUpdatePortsCache();

            uiInputs  = new List<UniPortValue>();
            uiOutputs = new List<UniPortValue>();

#if UNITY_EDITOR
            UpdateUiPorts(UiViewHandle.Result);
#endif
        }

        private void UpdateUiPorts(UiModule uiModule)
        {
            if (!uiModule) {
                return;
            }

            uiModule.Initialize();

            UpdateTriggers(uiModule);
            UpdateModulesSlots(uiModule);
        }

        private UiModule CreateView(UiModule source)
        {
            var uiView = ObjectPool.Spawn(source, options.Position,
                                          Quaternion.identity, options.Parent, options.StayAtWorld);

            uiView.Initialize();

            return uiView;
        }

        private void ApplyModuleSettings(UiModule uiView, ILifeTime lifetime)
        {
            if (options.Immortal) {
                DontDestroyOnLoad(uiView);
            }

            //get view context settings
            var viewSettings = Input.Get<UniUiModuleData>();

            if (viewSettings != null) {
                var parentDisposable = viewSettings.Transform.Subscribe(uiView.SetParent);
                lifetime.AddDispose(parentDisposable);
            }

            lifetime.AddCleanUpAction(() => uiView?.Despawn());

            uiView.gameObject.SetActive(true);
            uiView.SetState(true);
        }

        private void CreateModules(IUiModule view, IContext context, ILifeTime lifetime)
        {
            var slotContainer = view.Slots;
            var slots         = slotContainer.Items;

            for (int i = 0; i < slots.Count; i++) {
                var slot      = slots[i];
                var portValue = GetPortValue(slot.SlotName);
                slot.ApplySlotData(context, portValue, lifetime);
            }
        }

        private void BindTriggers(IUiModule view, IContext context, ILifeTime lifetime)
        {
            var triggers               = view.Triggers;
            var interactionsDisposable = triggers.TriggersObservable.Subscribe(x => OnUiTriggerAction(x, context));

            lifetime.AddDispose(interactionsDisposable);
        }

        private void UpdateTriggers(IUiModule view)
        {
            var triggers = view.Triggers;

            foreach (var handler in triggers.Items) {
                var values = this.CreatePortPair(handler.ItemName, true);

                uiOutputs.Add(values.outputValue);
                uiInputs.Add(values.inputValue);
            }
        }

        private void UpdateModulesSlots(IUiModule view)
        {
            var slots = view.Slots.Items;

            for (var i = 0; i < slots.Count; i++) {
                var slot       = slots[i];
                var outputPort = this.UpdatePortValue(slot.SlotName, PortIO.Output);
                uiOutputs.Add(outputPort.value);
            }
        }
    }
}