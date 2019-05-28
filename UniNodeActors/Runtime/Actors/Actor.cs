namespace UniGreenModules.UniNodeActors.Runtime.Actors
{
    using System.Collections;
    using Interfaces;
    using UniContextData.Runtime.Entities;
    using UniCore.Runtime.DataFlow;
    using UniCore.Runtime.ObjectPool;
    using UniModule.UnityTools.UniStateMachine.Interfaces;
    using UniRx;
    using UniTools.UniRoutine.Runtime;
    using IActor = Interfaces.IActor;

    /// <summary>
    /// single computation actor
    /// </summary>
    public class Actor : IActor
    {
        /// <summary>
        /// data container
        /// </summary>
        private EntityContext _entity = new EntityContext();

        /// <summary>
        /// active actor behaviour
        /// </summary>
        protected IContextState<IEnumerator> _behaviour;
        
#region public properties

        public IMessageBroker MessageBroker => _entity;

        public ILifeTime LifeTime => _entity.LifeTime;

#endregion
        
#region public methods

        public void Initialize(IContextDataSource dataSource, IContextState<IEnumerator> behaviour)
        {
            Release();
            
            dataSource.Register(_entity);
            _behaviour = behaviour;
        }

        public void Execute()
        {
            
            //activate state
            var routine        = _behaviour.Execute(_entity);
            var disposableItem = routine.RunWithSubRoutines();
            
            LifeTime.AddDispose(disposableItem);
            LifeTime.AddCleanUpAction(_behaviour.Exit);
        }
        
        public void Dispose()
        {
            Release();
            this.Despawn();
        }
        
        public void Release()
        {
            _entity.Release();
            _behaviour = null;
        }

#endregion

    }
}