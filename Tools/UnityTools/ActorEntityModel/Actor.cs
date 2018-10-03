using System;
using System.Collections;
using Assets.Tools.UnityTools.ActorEntityModel.Interfaces;
using Assets.Tools.UnityTools.StateMachine.Interfaces;
using Assets.Tools.UnityTools.UniRoutine;

namespace Assets.Tools.UnityTools.ActorEntityModel
{
    public class Actor : BehaviourObject
    {
        protected IContextStateBehaviour<IEnumerator> _stateBehaviour;
        private IDisposable _routineDisposable;
        private IEntity _entity;

        #region public methods


        public void SetEntity(IEntity entity)
        {
            _entity = entity;
        }

        public void SetBehaviour(IContextStateBehaviour<IEnumerator> behaviour)
        {
            if (_stateBehaviour != null)
            {
                _stateBehaviour.Exit(_entity);
            }
            
            _stateBehaviour = behaviour;
            
            SetBehaviourState(IsActive);
        }

        public void SetState(bool state)
        {
            if (IsActive == state)
                return;
            if (state)
            {
                Activate();
            }
            else
            {
                Deactivate();
            }

            IsActive = state;
        }

        
        #endregion

        #region private methods

        protected void SetBehaviourState(bool activate)
        {
            //already active
            if (activate == (_routineDisposable!=null))
                return;

            if (!activate && _routineDisposable != null)
            {
                _stateBehaviour?.Exit(_entity);
                _stateBehaviour?.Dispose();
            }
            
            //release current routine
            _routineDisposable?.Dispose();
            _routineDisposable = null;

            if (_stateBehaviour == null)
                return;

            var routine = _stateBehaviour.Execute(_entity);
            _routineDisposable = routine.RunWithSubRoutines();

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