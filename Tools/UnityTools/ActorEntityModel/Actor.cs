using System;
using System.Collections;
using Assets.Tools.UnityTools.ActorEntityModel.Interfaces;
using Assets.Tools.UnityTools.StateMachine.Interfaces;
using Assets.Tools.UnityTools.UniRoutine;

namespace Assets.Tools.UnityTools.ActorEntityModel
{
    public class Actor : BehaviourObject
    {
        protected IContextState<IEnumerator> _stateBehaviour;
        private IDisposable _routineDisposable;
        private IEntity _entity;

        public IContextState<IEnumerator> State => _stateBehaviour;

        #region public methods


        public void SetEntity(IEntity entity)
        {
            _entity = entity;
        }

        public void SetBehaviour(IContextState<IEnumerator> behaviour)
        {
            _stateBehaviour = behaviour;
            SetBehaviourState(IsActive);
        }
      
        #endregion

        #region private methods

        protected void SetBehaviourState(bool activate)
        {
            if (activate == false)
            {
                StopBehaviour();
            }

            //already active
            if (activate == false || State == null)
                return;
   
            //activate state
            var routine = _stateBehaviour.Execute(_entity);
            _routineDisposable = routine.RunWithSubRoutines();

        }

        private void StopBehaviour()
        {
            if (_routineDisposable != null)
            {
                _stateBehaviour?.Exit(_entity);
            }
            //release current routine
            _routineDisposable?.Dispose();
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