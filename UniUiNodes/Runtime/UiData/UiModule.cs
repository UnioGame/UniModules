namespace UniGreenModules.UniUiNodes.Runtime.UiData
{
    using Interfaces;
    using Triggers;
    using UniCore.Runtime.Interfaces;
    using UniCore.Runtime.Rx.Extensions;
    using UniRx;
    using UnityEngine;
    using UniUiSystem.Runtime;
    using UniUiSystem.Runtime.Interfaces;

    public class UiModule : UiView<IValueReceiver>, IUiModule
    {
        #region inspector data

        [SerializeField] private UiModuleSlotsContainer slots = new UiModuleSlotsContainer();

        [SerializeField] private UiTriggersContainer triggers = new UiTriggersContainer();

        #endregion

        #region public properties

        public IContainer<IUiModuleSlot> Slots => slots;

        public ITriggersContainer Triggers => triggers;

        #endregion

        #region public methods

        public void AddTrigger(IInteractionTrigger trigger)
        {
            triggers.Add(trigger);
        }

        public void AddSlot(IUiModuleSlot slot)
        {
            slots.Add(slot);
        }

        #endregion


        protected override void OnInitialize(IValueReceiver receiver)
        {
            var lifeTime = LifeTime;

            var placement = receiver.Get<IUiPlacement>();
            placement?.Insert(RectTransform);

            receiver.Receive<IUiPlacement>().
                Subscribe(x => 
                    x.Insert(RectTransform)).
                AddTo(lifeTime);
        }
        
        #region Editor Validation

        protected virtual void OnModuleValidate(){}

        public void OnValidate()
        {
            var slotsItems    = GetComponentsInChildren<UiModuleSlot>(true);
            var triggersItems = GetComponentsInChildren<InteractionTrigger>(true);

            triggers.Release();
            triggers.AddRange(triggersItems);

            slots.Release();
            slots.AddRange(slotsItems);

            OnModuleValidate();
        }
        
        #endregion
    }
}