namespace UniGreenModules.UniNodeSystem.Runtime
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Extensions;
    using Interfaces;
    using Runtime;
    using UniCore.Runtime.DataFlow;
    using UniCore.Runtime.Interfaces;
    using UniCore.Runtime.ObjectPool;
    using UniCore.Runtime.ObjectPool.Extensions;
    using UniModule.UnityTools.UniStateMachine;
    using UniModule.UnityTools.UniStateMachine.Interfaces;
    using UniStateMachine.Runtime;
    using UniTools.UniRoutine.Runtime;
    using UnityEngine;

    [Serializable]
    public abstract class UniGraphNode : UniBaseNode, IUniGraphNode
    {
        /// <summary>
        /// output port name
        /// </summary>
        public const string OutputPortName = "Output";
        
        /// <summary>
        /// input port name
        /// </summary>
        public const string InputPortName = "Input";

        #region private fields

        [NonSerialized] private Dictionary<string,UniPortValue> portValuesMap;

        [NonSerialized] private IContextState<IEnumerator> behaviourState;

        [NonSerialized] private List<UniPortValue> portValues;

        [NonSerialized] protected bool isInitialized;
        
        [NonSerialized] protected IContext nodeContext;

        #endregion
        
        #region serialized data

        [SerializeField] private RoutineType routineType = RoutineType.UpdateStep;

        #endregion
        
        #region public properties
        
        public UniPortValue Input => GetPortValue(InputPortName);

        public UniPortValue Output => GetPortValue(OutputPortName);
        
        public RoutineType RoutineType => routineType;

        public IReadOnlyList<UniPortValue> PortValues => portValues;

        public bool IsActive => behaviourState!=null  && behaviourState.IsActive;

        public ILifeTime LifeTime => behaviourState?.LifeTime;
        
        #endregion
        
        #region public methods

        public void Initialize()
        {           
            behaviourState = CreateState();
            
            if (Application.isPlaying && isInitialized)
                return;
            
            isInitialized = true;
            
            portValues = new List<UniPortValue>();
            portValuesMap = new Dictionary<string, UniPortValue>();           
            
            UpdatePortsCache();

            foreach (var portValue in portValues)
            {
                portValue.Initialize();
            }

            OnNodeInitialize();
        }
        
        public virtual bool Validate(IContext context) => true;
        
        public void UpdatePortsCache()
        {
            OnUpdatePortsCache();
            
            var removedPorts = ClassPool.Spawn<List<NodePort>>();
            
            foreach (var port in Ports)
            {
                if(port.IsStatic) continue;
                
                var value = GetPortValue(port.fieldName);
                if (value == null)
                {
                    removedPorts.Add(port);
                }
            }

            foreach (var port in removedPorts)
            {
                RemoveInstancePort(port);
            }

            removedPorts.DespawnCollection();
        }
        
        public void Exit() => behaviourState?.Exit();

        public IEnumerator Execute(IContext context)
        {
            StateLogger.LogState(string.Format("STATE EXECUTE {0} TYPE {1} CONTEXT {2}", 
                name, GetType().Name, context), this);
            
            Initialize();
            
            yield return behaviourState.Execute(context);
            
        }
        
        public void Release() => Exit();

        #region Node Ports operations
 
        public override object GetValue(NodePort port) => GetPortValue(port);
        
        public UniPortValue GetPortValue(NodePort port) => GetPortValue(port.fieldName);

        public UniPortValue GetPortValue(string portName)
        {
            portValuesMap.TryGetValue(portName, out var value);
            return value;
        }

        public bool AddPortValue(UniPortValue portValue)
        {
            if (portValue == null)
            {
                Debug.LogErrorFormat("Try add NULL port value to {0}",this);
                return false;
            }
            
            if (portValuesMap.ContainsKey(portValue.name))
            {
                return false;
            }

            portValuesMap[portValue.name] = portValue;
            portValues.Add(portValue);
            
            return true;
        }

        #endregion

        #endregion

        protected virtual void OnUpdatePortsCache()
        {
            this.UpdatePortValue(OutputPortName, PortIO.Output);
            this.UpdatePortValue(InputPortName, PortIO.Input);
        }

        protected virtual void OnNodeInitialize(){}
        
        #region state behaviour methods

        private void Initialize(IContext stateContext)
        {
            nodeContext = stateContext;
            
            LifeTime.AddCleanUpAction(CleanUpAction);
            
            OnInitialize(stateContext);
        }

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
