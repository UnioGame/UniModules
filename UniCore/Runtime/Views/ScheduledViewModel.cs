using UniGreenModules.UniCore.Runtime.Extension;

namespace UniGreenModules.UniCore.Runtime.Views
{
    using Interfaces;

    public abstract class ScheduledViewModel<TModel> : ComponentViewModel<TModel>
    {
        protected IDisposableItem updateDisposable;

        protected TModel ModelValue => Model.Value;

        public void UpdateView()
        {
            //is update already scheduled?
            if (updateDisposable != null && updateDisposable.IsDisposed == false)
                return;

            //release dispose items
            updateDisposable?.Dispose();
            updateDisposable = null;
            
            //check validation step
            var validationResult = Validate();
            if (!validationResult)
                return;
            
            updateDisposable = ScheduleUpdate();
            
        }

#region private methods

        protected virtual bool Validate() => Model.HasValue &&
                                             isActiveAndEnabled;

        protected override void OnRelease()
        {
            updateDisposable.Cancel();
            updateDisposable = null;
        }

        protected abstract IDisposableItem ScheduleUpdate();

        protected void OnDestroy() => Release();

        protected virtual void OnEnable() => UpdateView();
        
#endregion

    }
}
