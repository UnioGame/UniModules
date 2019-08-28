namespace UniGreenModules.UniUiNodes.Runtime
{
    using System.Collections;
    using System.Collections.Generic;
    using Interfaces;
    using UiData;
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
    using UniUiSystem.Runtime.Interfaces;
    using Debug = UnityEngine.Debug;

    public class UniUiNode : UniNode
    {
        #region inspector

        [HideInInspector] [SerializeField] private string viewName = "UiNode";

        public ObjectInstanceData options;

        public AssetReferenceGameObject resource;

        #endregion

        private List<UniPortValue> uiTriggersOutputs = new List<UniPortValue>();
        private List<UniPortValue> uiModulesOutputs  = new List<UniPortValue>();

        private List<UniPortValue> slotPorts;
        private List<UniPortValue> triggersPorts;


        private AsyncOperationHandle<GameObject> UiViewHandle => resource.LoadAssetAsync();


        public override string GetName() => viewName;

        public bool Validate(IContext context)
        {
            if (UiViewHandle.Status == AsyncOperationStatus.None || UiViewHandle.Status == AsyncOperationStatus.Failed) {
                Debug.LogErrorFormat("NULL UI VIEW {0} {1}", UiViewHandle, this);
                return false;
            }

            return true;
        }

        protected IEnumerator OnExecuteState(IContext context)
        {
            var lifetime = LifeTime;

            //load view
            //TODO take shared object
            yield return UiViewHandle.Task.AwaitTask();

            if (UiViewHandle.Status == AsyncOperationStatus.None || UiViewHandle.Status == AsyncOperationStatus.Failed) {
                Debug.LogError(UiViewHandle);
                yield break;
            }

            var viewPrefab = UiViewHandle.Result.GetComponent<UiModule>();
            var view       = CreateView(viewPrefab);

            BindModulesPorts(view, context, lifetime);

            BindTriggers(view, context, lifetime);

            //initialize view with input data
            //view.Initialize(Input);

            ApplyViewSettings(view, lifetime);

            //yield return base.OnExecute(context);
            yield break;
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
                portValue.Publish(context);
            }
            else {
                portValue.Remove<IContext>();
            }
        }

        protected void OnRegisterPorts()
        {
            //base.OnRegisterPorts();

            uiModulesOutputs.Clear();
            uiTriggersOutputs.Clear();

#if UNITY_EDITOR

            var viewObject = resource.editorAsset as GameObject;

            if (!viewObject)
                return;

            var view = viewObject.GetComponent<UiModule>();
            if (!view)
                return;

            viewName = view.name;

            UpdateUiPorts(view);
#endif
        }

        private void UpdateUiPorts(UiModule uiModule)
        {
            if (!uiModule) {
                return;
            }

            UpdateTriggers(uiModule);
            UpdateModulesSlots(uiModule);
        }

        private UiModule CreateView(UiModule source)
        {
            //todo add cached version from global manager
            var uiView = ObjectPool.Spawn(source, options.Position,
                Quaternion.identity, options.Parent, options.StayAtWorld);

            //bind main view to input data
            //uiView.Initialize(Input);

            return uiView;
        }

        private void ApplyViewSettings(UiModule uiView, ILifeTime lifetime)
        {
            lifetime.AddCleanUpAction(() => {
                if (uiView != null) {
                    uiView?.Despawn();
                }
            });

            uiView.gameObject.SetActive(true);
            uiView.UpdateView();
        }

        /// <summary>
        /// Bind output ports to ui moduls
        /// </summary>
        private void BindModulesPorts(IUiModule view, IContext context, ILifeTime lifetime)
        {
            var slotContainer = view.Slots;

            var slots = slotContainer.Items;

            for (var i = 0; i < slots.Count; i++) {
                //get associated port value by slot
                var slot      = slots[i];
                var portValue = GetPortValue(slot.SlotName);

                //connect to ui module data
                var connection = slot.Value.Connect(portValue);
                //remove connection, if node stoped
                lifetime.AddCleanUpAction(() => connection.Disconnect(portValue));

                //add new placement value
                portValue.Publish<IUiPlacement>(slot);
                //set node context
                portValue.Publish(context);
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
                //uiTriggersOutputs.Add(values.outputValue);
            }
        }

        private void UpdateModulesSlots(IUiModule view)
        {
            var slots = view.Slots.Items;

            for (var i = 0; i < slots.Count; i++) {
                var slot       = slots[i];
                var outputPort = this.UpdatePortValue(slot.SlotName, PortIO.Output);
                //uiModulesOutputs.Add(outputPort.value);
            }
        }
    }
}