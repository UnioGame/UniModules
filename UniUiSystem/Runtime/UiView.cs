namespace UniGreenModules.UniUiSystem.Runtime
{
    using System.Collections;
    using Interfaces;
    using JetBrains.Annotations;
    using UniCore.Runtime.Extension;
    using UniCore.Runtime.Interfaces;
    using UniCore.Runtime.Views;
    using UniTools.UniRoutine.Runtime;
    using UnityEngine;

    public class UiView<TModel> : ScheduledViewModel<TModel>, IUiView<TModel>
    {
        [SerializeField]
        private RoutineType updateType = RoutineType.EndOfFrame;
        [SerializeField]
        private bool immediateUpdate = false;

        public RectTransform RectTransform  => transform as RectTransform;

        #region private methods

        //schedule single ui update at next EndOfFrame call
        protected override IDisposableItem ScheduleUpdate()
        {
            return OnScheduledUpdate().
                ExecuteRoutine(updateType,immediateUpdate);
        }

        private IEnumerator OnScheduledUpdate()
        {
            //update ui view
            OnUpdateView();
            
            //cancel disposable item
            updateDisposable.Cancel();
            
            updateDisposable = null;
            
            yield break;
        }

        protected virtual void OnUpdateView(){}

        #endregion

    }
}