using System.Collections;
using UniModule.UnityTools.Interfaces;
using UnityEngine;

namespace UniUiSystem
{
    using UniGreenModules.UniContextData.Runtime.Entities;
    using UniGreenModules.UniCore.Runtime.Interfaces;
    using UniTools.UniRoutine.Runtime;

    public class UiViewBehaviour : MonoBehaviour, IUiViewBehaviour
    {
        private EntityObject _context = new EntityObject();
        private IDisposableItem _updateDisposable;

        #region public property

        public bool IsActive { get; protected set; }

        public IContext Context => _context;

        public RectTransform RectTransform => transform as RectTransform;

        #endregion
        
        #region public methods

        public void Initialize()
        {
            OnInitialize();
        }
        
        public void UpdateView()
        {
            //is update already scheduled?
            if (_updateDisposable != null && _updateDisposable?.IsDisposed == false)
                return;

            //release dispose items
            _updateDisposable?.Dispose();
            
            //check validation step
            var validationResult = Validate();
            if (!validationResult)
                return;

            //schedule single ui update at next EndOfFrame call
            _updateDisposable = OnScheduledUpdate().
                RunWithSubRoutines(RoutineType.EndOfFrame);
            
        }

        public void SetState(bool active)
        {
            if (IsActive == active)
                return;

            if (active)
            {
                Activate();
            }
            else
            {
                Deactivate();
            }
        }
        
        public void Release()
        {
            Deactivate();
            _updateDisposable?.Dispose();
            _context.Release();
            OnReleased();
        }

        #endregion
        
        protected virtual void OnReleased()
        {

        }

        protected IEnumerator OnScheduledUpdate()
        {
            
            OnUpdateView();
            yield break;

        }

        protected virtual void Activate()
        {

        }

        protected virtual void Deactivate()
        {
            
        }

        protected virtual bool Validate()
        {
            return isActiveAndEnabled;
        }

        protected virtual void OnUpdateView()
        {

        }

        protected virtual void OnInitialize()
        {

        }

        protected virtual void OnEnable()
        {
            UpdateView();
        }

        protected virtual void OnDestroy()
        {
            Release();
        }

    }
}
