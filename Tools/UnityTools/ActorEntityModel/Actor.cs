using System;
using System.Collections;
using UniStateMachine;
using UnityToolsModule.Tools.UnityTools.ActorEntityModel;
using UnityToolsModule.Tools.UnityTools.UniRoutine;

namespace Tools.ActorModel
{
    public class Actor : BehaviourObject
    {
        
        protected IStateBehaviour<IEnumerator> _stateBehaviour;
        
        private IDisposable _routineDisposable;
        
            
        #region public methods
        
        public void SetBehaviour(IStateBehaviour<IEnumerator> behaviour)
        {
            if (_stateBehaviour != null)
            {
                _stateBehaviour.Exit();
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
                _stateBehaviour?.Exit();
            }
            
            //release current routine
            _routineDisposable?.Dispose();
            _routineDisposable = null;

            if (_stateBehaviour == null)
                return;

            _routineDisposable = _stateBehaviour.
                Execute().RunWithSubRoutines();

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