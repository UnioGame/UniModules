using System;
using System.Collections;
using Assets.Tools.UnityTools.Interfaces;
using Assets.Tools.UnityTools.ObjectPool.Scripts;
using Assets.Tools.UnityTools.StateMachine.Interfaces;
using Assets.Tools.UnityTools.UniRoutine;
using UnityEngine;

namespace UnityTools.ActorEntityModel
{
    public class Actor : BehaviourObject
    {
        private IDisposable _routineDisposable;
        private ActorModel _model;
        private EntityObject _entity = new EntityObject();

        protected IContextState<IEnumerator> _stateBehaviour;

        public IContextState<IEnumerator> State => _stateBehaviour;

        public IContext Context => _entity;

        #region public methods

        public void SetModel(ActorModel model)
        {

            if (_model != null)
            {
                Release();
            }

            _model = model;
            _model.RegisterContext(_entity);
            _stateBehaviour = _model.Behaviour;

        }
      
        #endregion

        #region private methods

        protected override void OnReleased()
        {

            _model?.Despawn();
            _model = null;
            
            _entity.Release();
            _stateBehaviour = null;

        }
        
        protected void SetBehaviourState(bool activate)
        {
            if (_stateBehaviour == null || 
                _stateBehaviour.IsActive(_entity) == activate)
                return;
            
            if (activate == false)
            {
                StopBehaviour();
                return;
            }
  
            //activate state
            var routine = _stateBehaviour.Execute(_entity);
            _routineDisposable = routine.RunWithSubRoutines();

        }

        private void StopBehaviour()
        {
            _stateBehaviour?.Exit(_entity);
            //release current routine
            _routineDisposable?.Despawn();
            _routineDisposable = null;

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