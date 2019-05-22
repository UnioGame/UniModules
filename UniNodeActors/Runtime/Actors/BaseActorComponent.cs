namespace UniGreenModules.UniNodeActors.Runtime.Actors
{
    using System.Collections;
    using Interfaces;
    using Sirenix.OdinInspector;
    using UniModule.UnityTools.Interfaces;
    using UniModule.UnityTools.UniPool.Scripts;
    using UniModule.UnityTools.UniStateMachine.Interfaces;
    using UniStateMachine.Nodes;
    using UnityEngine;
    using IActor = Interfaces.IActor;

    public abstract class BaseActorComponent : SerializedMonoBehaviour, IActor
    {

        /// <summary>
        /// is actor component ready
        /// </summary>
        private bool initialized = false;
        
        /// <summary>
        /// actor behaviour instance
        /// </summary>
        private IContextState<IEnumerator> behaviour;

        /// <summary>
        /// actor model data
        /// </summary>
        private IActorModel actorModel;
        
        /// <summary>
        /// actor source
        /// </summary>
        private Actor _actor = new Actor();

        #region inspector data

        /// <summary>
        /// is activate actor on onenable
        /// </summary>
        [SerializeField] private bool activateOnStart;

        /// <summary>
        /// behaviour
        /// </summary>
        [SerializeField] private UniGraph behaviourSource;

        #endregion

        #region public properties

        public bool IsActive => _actor.IsActive;

        public IContext Context => _actor.Context;
        
        #endregion
        
        public void SetEnabled(bool state)
        {
            _actor.SetEnabled(state);
        }
        
        public void Release()
        {
            _actor.Release();
        }

        // Use this for initialization
        private void OnEnable()
        {
            Initialize();
            if (activateOnStart)
            {
                SetEnabled(true);
            }
        }

        private void Initialize()
        {
            if (initialized)
                return;

            initialized = true;
            
            behaviour = GetBehaviour();
            actorModel = GetModel();
                       
            _actor.Initialize(actorModel,behaviour);
        }

        private IContextState<IEnumerator> GetBehaviour()
        {
            var actorTransform = transform;
            var state = ObjectPool.Spawn(behaviourSource, Vector3.zero, Quaternion.identity,actorTransform, false);
            state.gameObject.SetActive(true);
            
            return state;
        }

        protected abstract IActorModel GetModel();

    }
}