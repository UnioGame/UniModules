namespace UniGreenModules.UniUiSystem.Runtime.UiData
{
    using UniCore.Runtime.Interfaces;
    using UniCore.Runtime.Rx.Extensions;
    using UniRx;

    public class UiModuleT<TModel> : UiModule
    {
        public TModel UiModel { get; protected set; }

        /// <summary>
        /// base module initialization data
        /// </summary>
        /// <param name="receiver">data provider</param>
        protected sealed override void OnInitialize(IValueReceiver receiver)
        {
            base.OnInitialize(receiver);
            
            var lifeTime = LifeTime;

            receiver.Receive<TModel>().Do(x => OnReleaseModel()). //release current data if new comming
                Do(x => UiModel = x).                             //set current model data
                Subscribe(OnInitialize).                          //initialize new data
                AddTo(lifeTime);

            if (receiver.Contains<TModel>()) {
                UiModel = receiver.Get<TModel>();
                OnInitialize(UiModel);
            }

            lifeTime.AddCleanUpAction(() => UiModel = default(TModel));
            lifeTime.AddCleanUpAction(OnReleaseModel);
        }

        protected virtual void OnInitialize(TModel model){}

        protected virtual void OnReleaseModel(){}
    }
}