using UniStateMachine.Nodes;

namespace UniModule.UnityTools.ActorEntityModel
{
    using System.Collections;
    using Sirenix.OdinInspector;
    using UniModule.UnityTools.Interfaces;
    using UniModule.UnityTools.UniPool.Scripts;
    using UniModule.UnityTools.UniStateMachine.Interfaces;
    using UniStateMachine;
    using UnityEngine;

    public class ActorComponent : SerializedMonoBehaviour, IActor
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
        /// behaviour SO
        /// </summary>
        [SerializeField] private UniGraph behaviourSource;

        /// <summary>
        /// actor model data
        /// </summary>
        [SerializeField] private ActorInfo actorInfo;

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

        private IActorModel GetModel()
        {
            var model = actorInfo?.Create();
            model?.Register(Context);
            return model;
        }

    }
}