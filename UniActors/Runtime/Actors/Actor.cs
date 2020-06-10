namespace UniGreenModules.UniActors.Runtime.Actors
{
    using System;
    using System.Collections;
    using Interfaces;
    using UniContextData.Runtime.Entities;
    using UniContextData.Runtime.Interfaces;
    using UniCore.Runtime.DataFlow;
    using UniCore.Runtime.DataFlow.Interfaces;
    using UniRoutine.Runtime;
    using UniRx;
    using UniStateMachine.Runtime.Interfaces;
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
        private IContextState<IEnumerator> _behaviour;

        /// <summary>
        /// execution cancelation
        /// </summary>
        private IDisposable _cancelationOperation;
        
#region public properties

        public IMessageBroker MessageBroker => _entity;

        public ILifeTime LifeTime => _entity.LifeTime;

        public bool IsActive { get; protected set; }

#endregion
        
#region public methods

        public void Initialize(IContextDataSource dataSource, IContextState<IEnumerator> behaviour)
        {          
            Stop();

            //register data context 
            //dataSource.Register(_entity);
            //set current behaviour
            _behaviour = behaviour;

        }
        
        public void Execute()
        {
            if (IsActive || _behaviour == null)
                return;

            IsActive = true;
            
            //activate state
            var routine        = _behaviour.Execute(_entity);
            var disposableItem = routine.RunWithSubRoutines();
            
            //add lifetime cleanup actions
            LifeTime.AddDispose(disposableItem);
            LifeTime.AddCleanUpAction(_behaviour.Exit);
            
        }


        public void Stop()
        {
            Release();
        }
        
        public void Release()
        {
            _entity.Release();
            _behaviour = null;
            _cancelationOperation = null;
            IsActive = false;
        }

#endregion

    }
}