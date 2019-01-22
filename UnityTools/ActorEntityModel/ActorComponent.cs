using System.Collections;
using Assets.Tools.UnityTools.ObjectPool.Scripts;
using Assets.Tools.UnityTools.StateMachine.Interfaces;
using UniStateMachine;
using UniStateMachine.Nodes;
using UnityEngine;

namespace UnityTools.ActorEntityModel
{
    public class ActorComponent : EntityComponent, IPoolable {
		
        private Actor _actor = new Actor();

        #region inspector data

        [SerializeField]
        private bool _launchOnStart = true;
    
        /// <summary>
        /// behaviour SO
        /// </summary>
        [SerializeField]
        private UniNodesGraph _stateObject;

        /// <summary>
        /// behaviour component
        /// </summary>
        [SerializeField]
        private UniStateComponent _stateComponent;

        /// <summary>
        /// actor model data
        /// </summary>
        [SerializeField]
        private ActorInfo _actorInfo;

        #endregion

        public IContextState<IEnumerator> State { get; protected set; }

        public Actor Actor => _actor;

        public void Release() {

            Actor.Release();

        }
    
        protected IContextState<IEnumerator> GetState()
        {
            var model = Context.Get<ActorModel>();
            var parameterBehaviour = _stateObject ? 
                (IContextState<IEnumerator>) _stateObject : 
                _stateComponent;

            if (model?.Behaviour == null && parameterBehaviour == null)
                return null;

            var state = model?.Behaviour == null ?
                parameterBehaviour:
                model.Behaviour;

            return state;

        }

        protected virtual void Activate()
        {
            Actor.SetEnabled(true);
        }

        protected virtual void Deactivate() 
        {
            Actor.SetEnabled(false);
        }

        private void OnDisable()
        {
            Deactivate();
        }

        private void OnEnable() {

            Activate();

        }

        private void Start()
        {
            if (_launchOnStart)
            {
                var state = GetState();
            }
        }

        // Use this for initialization
        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            var model = _actorInfo?.Create();
            model?.RegisterContext(Context);
            InitializeContext();
            Actor.SetModel(model);
        }

        protected virtual void InitializeContext(){}

        private void OnDestroy() {
            Release();
        }

    }
}
