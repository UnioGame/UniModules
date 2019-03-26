using System.Collections;
using UniModule.UnityTools.Interfaces;
using UniModule.UnityTools.UniPool.Scripts;
using UniModule.UnityTools.UniStateMachine.Interfaces;
using UniStateMachine;
using UniStateMachine.Nodes;
using UnityEngine;

namespace UniModule.UnityTools.ActorEntityModel
{
    public class ActorComponent : MonoBehaviour, IActor
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
        }

        private void Initialize()
        {
            if (initialized)
                return;

            initialized = true;
            
            behaviour = GetBehaviour();
            actorModel = GetModel();
            
            InitializeContext();
            
            _actor.Initialize(actorModel,behaviour);
        }

        private IContextState<IEnumerator> GetBehaviour()
        {
            var state = ObjectPool.Spawn(behaviourSource, Vector3.zero, Quaternion.identity,transform, false);
            return state;
        }

        private IActorModel GetModel()
        {
            var model = actorInfo?.Create();
            model?.Register(Context);
            return model;
        }
        
        protected virtual void InitializeContext()
        {
        }


    }
}