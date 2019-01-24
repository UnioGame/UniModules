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
        private LifeTimeDefinition _lifeTimeDefinition = new LifeTimeDefinition();
        private EntityObject _context = new EntityObject();
        private Subject<IInteractionTrigger> _interactionSubject = new Subject<IInteractionTrigger>();
        
        private IDisposableItem _updateDisposableItem;
        
        [SerializeField]
        private List<InteractionTrigger> _triggers = new List<InteractionTrigger>();

        #region inspector

        [SerializeField]
        protected Canvas _canvas;
        [SerializeField]
        protected RectTransform _rectTransform;
        [SerializeField]
        protected CanvasGroup _canvasGroup;

        #endregion

        public IObservable<IInteractionTrigger> InteractionObservable => _interactionSubject;
        
        public ILifeTime LifeTime => _lifeTimeDefinition.LifeTime;
        
        public bool IsActive { get; protected set; }

        public IContext Context => _context;

        public Canvas Canvas => _canvas;

        public RectTransform RectTransform => _rectTransform;

        public CanvasGroup CanvasGroup => _canvasGroup;

        public List<InteractionTrigger> Triggers => _triggers;

        public void UpdateView()
        {
            //is update already scheduled?
            if (_updateDisposableItem != null)
                return;

            //check validation step
            var validationResult = Validate();
            if (!validationResult)
                return;

            //schedule single ui update at next EndOfFrame call
            var routine = OnScheduledUpdate().RunWithSubRoutines(RoutineType.EndOfFrame);
            Context.LifeTime.AddDispose(routine);

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

        public virtual void UpdateTriggers()
        {
            
            GetComponentsInChildren<InteractionTrigger>(true, _triggers);

        }
        
        public void Release()
        {
            Deactivate();
            _context.Release();
            OnReleased();
        }

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
            _lifeTimeDefinition.Terminate();
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

            foreach (var interactionTrigger in _triggers)
            {
                var disposable = interactionTrigger.Subscribe(x => _interactionSubject.OnNext(interactionTrigger));
                LifeTime.AddDispose(disposable);
            }
            
            OnInitialize();
        }
    }
}
