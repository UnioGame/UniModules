namespace UniGreenModules.UniNodeSystem.Nodes
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Runtime;
    using Runtime.Extensions;
    using Runtime.Interfaces;
    using Runtime.Runtime;
    using UniCore.Runtime.Interfaces;
    using UniCore.Runtime.ObjectPool;
    using UniStateMachine.Runtime;
    using UniStateMachine.Runtime.Interfaces;
    using UniTools.UniRoutine.Runtime;
    using UnityEngine;

    [Serializable]
    public abstract class UniFlowNode : UniNode
    {
        /// <summary>
        /// output port name
        /// </summary>
        public const string OutputPortName = "Output";
        
        /// <summary>
        /// input port name
        /// </summary>
        public const string InputPortName = "Input";
        
        #region serialized data

        [SerializeField] private RoutineType routineType = RoutineType.UpdateStep;

        #endregion

        #region public properties

        public RoutineType RoutineType => routineType;

        #endregion

        #region state behaviour methods

        /// <summary>
        /// base logic realization
        /// transfer context data to output port value
        /// </summary>
        protected virtual IEnumerator OnExecuteState(IContext context)
        {
            var output = Output;
            output.Add(context);
            yield break;
        }

        protected virtual void OnExit(IContext context){}

        protected virtual void OnInitialize(IContext context){}
        
        protected virtual void OnPostExecute(IContext context){}

        protected IContextState<IEnumerator> CreateState()
        {
            if (behaviourState != null)
                return behaviourState;
            
            var behaviour = ClassPool.Spawn<ProxyState>();
            behaviour.Initialize(OnExecuteState, Initialize, OnExit, OnPostExecute);
            
            behaviour.LifeTime.AddCleanUpAction(() => behaviour.Despawn());
            behaviour.LifeTime.AddCleanUpAction(CleanUpAction);
            
            return behaviour;
        }

        private void CleanUpAction()
        {
            for (var i = 0; i < PortValues.Count; i++)
            {
                var portValue = PortValues[i];
                portValue.CleanUp();
            }

            nodeContext = null;
            behaviourState = null;
        }

#endregion

    }
}