using System.Collections;
using Assets.Scripts.Tools.StateMachine;
using UniStateMachine;

namespace Tools.ActorModel
{
    public class ActorEntity<TModel, TView> : EntityObject
    {
        protected IEnumerator _progress;
        protected IStateBehaviour<IEnumerator> _stateBehaviour;

        #region public properties
        
        public TView View { get; protected set; }

        public TModel Model { get; protected set; }

        #endregion

        #region public methods

        /// <summary>
        /// set actor view
        /// </summary>
        /// <param name="view">view object</param>
        public void SetView(TView view)
        {
            View = view;
        }

        /// <summary>
        /// set actor model data
        /// </summary>
        public void SetModel(TModel model)
        {
            Model = model;
        }

        public void SetBehaviour(IStateBehaviour<IEnumerator> behaviour)
        {
            if (_stateBehaviour != null)
                _stateBehaviour.Exit();
            _progress = null;

            _stateBehaviour = behaviour;
        }

        #endregion

        #region private methods

        protected override void OnUpdate(float time)
        {
            if (_progress == null)
            {
                _progress = _stateBehaviour?.Execute();
            }

            if (_progress == null)
                return;

            _progress.MoveNext();
        }

        #endregion
    }
}