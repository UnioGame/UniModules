namespace UniGreenModules.UniNodeSystem.Runtime
{
    using System;
    using System.Collections.Generic;
    using Interfaces;
    using Runtime;
    using UniCore.Runtime.DataFlow;
    using UniCore.Runtime.Extension;
    using UniRx;
    using UniStateMachine.Runtime;
    using UnityEngine;

    [Serializable]
    public abstract class UniNode : UniBaseNode, IUniNode
    {
        #region private fields

        [NonSerialized] private LifeTimeDefinition lifeTimeDefinition;

        [NonSerialized] private Dictionary<string, UniPortValue> portValuesMap;

        [NonSerialized] private List<UniPortValue> portValues;

        [NonSerialized] private bool isInitialized;

        [NonSerialized] private bool isActive = false;

        #endregion

        #region public properties

        public bool IsActive => isActive;

        public IReadOnlyList<IPortValue> PortValues => portValues;

        public ILifeTime LifeTime => lifeTimeDefinition.LifeTime;

        public string ItemName => name;

        #endregion

        #region public methods

        public void Initialize()
        {
            //check initialization status
            if (isInitialized)
                return;

            isInitialized = true;
            portValues    = new List<UniPortValue>();
            portValuesMap = new Dictionary<string, UniPortValue>();

            RegisterPorts();
            OnNodeInitialize();
        }

        public void RegisterPortHandler<TValue>(
            IPortValue portValue,
            IObserver<TValue> observer,
            bool oneShot = false)
        {
            //subscribe to port value observable
            portValue.GetObservable<TValue>().Finally(() => {
                    //if node stoped or 
                    if (!oneShot || !IsActive) return;
                    //resubscribe to port values
                    RegisterPortHandler(portValue, observer, true);
                }).
                Subscribe(observer). //subscribe to port value changes
                AddTo(LifeTime);     //stop all subscriptions when node deactivated
        }

        /// <summary>
        /// stop execution state
        /// </summary>
        public void Exit()
        {
            isActive = false;
            lifeTimeDefinition.Terminate();
        }

        public void Execute()
        {
            //node already active
            if (isActive) {
                StateLogger.LogState(string.Format("STATE ALREADY ACTIVE {0} TYPE {1}",
                    name, GetType().Name), this);
                return;
            }

            StateLogger.LogState(string.Format("STATE EXECUTE {0} TYPE {1}",
                name, GetType().Name), this);
            //mark as active
            isActive = true;
            
            //restart lifetime
            lifeTimeDefinition.Release();

            //initialize
            Initialize();

            //cleanup ports on exit
            LifeTime.AddCleanUpAction(CleanUpPorts);

            //user defined logic
            OnExecute();
        }

        /// <summary>
        /// stop node execution
        /// </summary>
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
            if (portValue == null) {
                Debug.LogErrorFormat("Try add NULL port value to {0}", this);
                return false;
            }

            if (portValuesMap.ContainsKey(portValue.name)) {
                return false;
            }

            portValuesMap[portValue.name] = portValue;
            portValues.Add(portValue);

            return true;
        }

        #endregion

        #endregion


        /// <summary>
        /// Register node port values
        /// </summary>
        private void RegisterPorts()
        {
            //custom registration action
            OnRegisterPorts();
            //remove deleted ports
            Ports.RemoveItems(IsExistsPort, RemoveInstancePort);
        }


        private bool IsExistsPort(NodePort port)
        {
            if (port.IsStatic) return false;
            var value = GetPortValue(port.fieldName);
            return value == null;
        }

        /// <summary>
        /// Custom port registration method
        /// </summary>
        protected virtual void OnRegisterPorts(){}

        /// <summary>
        /// Call once on node initialization
        /// </summary>
        protected virtual void OnNodeInitialize(){}

        /// <summary>
        /// base logic realization
        /// </summary>
        protected virtual void OnExecute(){}

        /// <summary>
        /// cleanup all ports values
        /// </summary>
        private void CleanUpPorts()
        {
            for (var i = 0; i < PortValues.Count; i++) {
                var portValue = PortValues[i];
                portValue.CleanUp();
            }
        }
    }
}