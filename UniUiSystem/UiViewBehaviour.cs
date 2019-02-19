using System;
using System.Collections;
using System.Collections.Generic;
using UniModule.UnityTools.ActorEntityModel;
using UniModule.UnityTools.DataFlow;
using UniModule.UnityTools.Interfaces;
using UniModule.UnityTools.UniRoutine;
using UniRx;
using UniStateMachine.NodeEditor.UiNodes;
using UnityEngine;

namespace UniModule.UnityTools.UiViews
{
    public class UiViewBehaviour : MonoBehaviour, IUiViewBehaviour
    {
        private EntityObject _context = new EntityObject();
        private IDisposableItem _updateDisposable;

        #region inspector

        [SerializeField]
        protected Canvas _canvas;
        [SerializeField]
        protected RectTransform _rectTransform;
        [SerializeField]
        protected CanvasGroup _canvasGroup;

        #endregion

        #region public property

        public ILifeTime LifeTime => _context.LifeTime;
        
        public bool IsActive { get; protected set; }

        public IContext Context => _context;

        public Canvas Canvas => _canvas;

        public RectTransform RectTransform => _rectTransform;

        public CanvasGroup CanvasGroup => _canvasGroup;

        #endregion
        
        #region public methods
        
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

        protected void Awake()
        {
            if (!_canvas)
                _canvas = GetComponent<Canvas>();
            if (!_rectTransform)
                _rectTransform = GetComponent<RectTransform>();
            if (!_canvasGroup)
                _canvasGroup = GetComponent<CanvasGroup>();

            OnInitialize();
        }
    }
}
