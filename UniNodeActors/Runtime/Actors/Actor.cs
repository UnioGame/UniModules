namespace UniGreenModules.UniNodeActors.Runtime.Actors
{
    using System.Collections;
    using Interfaces;
    using UniContextData.Runtime.Entities;
    using UniCore.Runtime.DataFlow;
    using UniCore.Runtime.Interfaces;
    using UniCore.Runtime.ObjectPool;
    using UniModule.UnityTools.UniStateMachine.Interfaces;
    using UniTools.UniRoutine.Runtime;
    using IActor = Interfaces.IActor;

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
        
        private ILifeTime lifeTime;

#region public properties
        
        public IContextState<IEnumerator> State => _behaviour;

        public IContext Context => _entity;

        public ILifeTime LifeTime => _entity.LifeTime;

#endregion
        
        #region public methods

        public void Initialize(IContextDataSource dataSource, IContextState<IEnumerator> behaviour)
        {

            Release();

            _dataSource = dataSource;
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

            //activate state
            var routine = _behaviour.Execute(_entity);
            var disposableItem = routine.RunWithSubRoutines();
            
            LifeTime.AddDispose(disposableItem);
            LifeTime.AddCleanUpAction(_behaviour.Exit);

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