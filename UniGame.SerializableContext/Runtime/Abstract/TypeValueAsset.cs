namespace UniGreenModules.UniGame.SerializableContext.Runtime.Abstract
{
    using System;
    using UniCore.Runtime.Common;
    using UniCore.Runtime.DataFlow;
    using UniCore.Runtime.DataFlow.Interfaces;
    using UniCore.Runtime.Interfaces;
    using UniCore.Runtime.ProfilerTools;
    using UnityEngine;

    [Serializable]
    public abstract class TypeValueAsset<TValue,TApiValue> : 
        ScriptableObject,
        IDataValue<TApiValue>,
        ILifeTimeContext
        where TValue : TApiValue
    {
        #region inspector
        
        [SerializeField]
        private TValue defaultValue = default(TValue);
        
        #endregion

        private LifeTimeDefinition lifeTime;
        
        private ContextValue<TApiValue> contextValue = new ContextValue<TApiValue>();

        public ILifeTime LifeTime => lifeTime.LifeTime;
        
        public TApiValue Value => contextValue.Value;

        public bool HasValue => contextValue.HasValue;
        
        #region public methods
        
        public IDisposable Subscribe(IObserver<TApiValue> observer) => contextValue.Subscribe(observer);
        
        public void Dispose() => contextValue.Dispose();
        
        public void SetValue(TApiValue value) => ApplyValue(value);
        
        #endregion
        
        /// <summary>
        /// Initialize when SO loaded
        /// </summary>
        protected void OnEnable()
        {
            contextValue = new ContextValue<TApiValue>();
            lifeTime = new LifeTimeDefinition();
            
            OnInitialize();
        }
        
        /// <summary>
        /// Apply Value to context
        /// </summary>
        /// <param name="value"></param>
        protected virtual void ApplyValue(TApiValue value) => contextValue.SetValue(value);

        /// <summary>
        /// Default context value
        /// </summary>
        /// <returns></returns>
        protected virtual TApiValue GetDefaultValue() => defaultValue;
        
        /// <summary>
        /// initialize default value state
        /// </summary>
        protected virtual void OnInitialize() => SetValue(GetDefaultValue());

        private void OnDisable()
        {
            //end of value LIFETIME
            lifeTime.Terminate();
            
            GameLog.Log($"TypeValueAsset: {GetType().Name} {name} : VALUE {Value} END OF LIFETIME");
        }

    }
}
