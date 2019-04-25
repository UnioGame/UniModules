using System;
using System.Collections;
using UniModule.UnityTools.Interfaces;
using UniModule.UnityTools.UniPool.Scripts;
using UniModule.UnityTools.UniStateMachine.Interfaces;

namespace UniModule.UnityTools.ActorEntityModel
{
    using UniTools.UniRoutine.Runtime;

    public class Actor : BehaviourObject, IActor
    {
        /// <summary>
        /// data container
        /// </summary>
        private EntityObject _entity = new EntityObject();

        /// <summary>
        /// context data source
        /// </summary>
        private IContextDataSource _dataSource;
        
        /// <summary>
        /// active actor behaviour
        /// </summary>
        protected IContextState<IEnumerator> _behaviour;

        #region public properties
        
        public IContextState<IEnumerator> State => _behaviour;

        public IContext Context => _entity;

        #endregion
        
        #region public methods

        public void Initialize(IActorModel model, IContextState<IEnumerator> behaviour)
        {

            Release();

            _dataSource = model;
            _dataSource.Register(_entity);
            _behaviour = behaviour;

        }
      
        #endregion

        #region private methods

        protected override void OnReleased()
        {
   
            _entity.Release();
            _behaviour = null;

            _dataSource?.Despawn();
            _dataSource = null;

        }
        
        protected void SetBehaviourState(bool activate)
        {
            if (_behaviour == null || 
                _behaviour.IsActive == activate)
                return;
            
            if (activate == false)
            {
                StopBehaviour();
                return;
            }

            var lifeTime = _entity.LifeTime;
            //activate state
            var routine = _behaviour.Execute(_entity);
            var disposableItem = routine.RunWithSubRoutines();
            
            lifeTime.AddDispose(disposableItem);
            lifeTime.AddCleanUpAction(_behaviour.Exit);

        }

        private void StopBehaviour()
        {
            _entity.Release();
        }

        protected override void Deactivate()
        {
            base.Deactivate();
            SetBehaviourState(false);
        }

        protected override void Activate()
        {
            base.Activate();
            SetBehaviourState(true);
        }

        #endregion
    }
}