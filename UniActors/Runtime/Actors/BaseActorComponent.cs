namespace UniGreenModules.UniActors.Runtime.Actors
{
    using System.Collections;
    using Interfaces;
    using UniCore.Runtime.DataFlow;
    using UniCore.Runtime.ObjectPool;
    using UniNodeSystem.Runtime;
    using UniRx;
    using UniStateMachine.Runtime.Interfaces;
    using UnityEngine;
    using IActor = Interfaces.IActor;

    public abstract class BaseActorComponent : MonoBehaviour, IActor
    {
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
        [SerializeField] private UniNode behaviourSource;

#endregion

        #region public properties

        public IMessageBroker MessageBroker => _actor.MessageBroker;

        public ILifeTime LifeTime => _actor.LifeTime;

        public bool IsActive => _actor.IsActive;
        
        #endregion
        
        public void Release()
        {
            _actor.Release();
        }

        // Use this for initialization
        private void OnEnable()
        {
            if (activateOnStart)
            {
                Execute();
            }
        }

        private void Initialize()
        {
            if (IsActive)
                return;

            var behaviour = GetBehaviour();
            var actorModel = GetModel();
                       
            _actor.Initialize(actorModel,behaviour);
            _actor.LifeTime.AddCleanUpAction(actorModel.MakeDespawn);
        }

        private IContextState<IEnumerator> GetBehaviour()
        {
            var actorTransform = transform;
            var state = ObjectPool.Spawn(behaviourSource, 
                Vector3.zero, Quaternion.identity,actorTransform, false);
            state.gameObject.SetActive(true);
            
            _actor.LifeTime.AddCleanUpAction(() => state.Despawn());
            
            return state;
        }

        protected abstract IActorModel GetModel();


        public void Execute()
        {
            Initialize();
            _actor.Execute();
        }

        public void Stop()
        {
            _actor.Stop();
        }
    }
}