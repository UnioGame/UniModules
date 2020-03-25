namespace UniGreenModules.UniGame.SerializableContext.Runtime.Abstract
{
    using System;
    using global::UniCore.Runtime.ProfilerTools;
    using UniCore.Runtime.Common;
    using UniCore.Runtime.DataFlow;
    using UniCore.Runtime.DataFlow.Interfaces;
    using UniCore.Runtime.ProfilerTools;
    using UnityEngine;

    [Serializable]
    public abstract class TypeValueAsset<TValue,TApiValue> : 
        ScriptableObject,
        ITypeValueAsset<TValue,TApiValue>
        where TValue : TApiValue
    {
        #region inspector

        public TValue defaultValue = default(TValue);
        
        #endregion

        private TypeValueAsset<TValue, TApiValue> source;
   
        private LifeTimeDefinition lifeTime;
        
        private ContextValue<TApiValue> contextValue = new ContextValue<TApiValue>();

        protected TValue _activeValue;

        public ILifeTime LifeTime => lifeTime.LifeTime;

        public TApiValue Value {

            get {
                if (contextValue.IsValueType == false && contextValue.Value == null) {
                    SetValue(GetDefaultValue());
                }
                return contextValue.Value;
            }
            
        }

        public bool HasValue => contextValue.HasValue;
        
        #region public methods
        
        public IDisposable Subscribe(IObserver<TApiValue> observer) => contextValue.Subscribe(observer);
        
        public void Dispose() => contextValue.Dispose();

        public void SetValue(TValue value)
        {
            _activeValue = value;
            ApplyValue(value);
        }
        
        #endregion
        
        /// <summary>
        /// Initialize when SO loaded
        /// </summary>
        protected void OnEnable()
        {
            contextValue = new ContextValue<TApiValue>();
            lifeTime = new LifeTimeDefinition();
            
            SetValue(GetDefaultValue());
            
            OnInitialize(lifeTime.LifeTime);
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
        protected virtual TValue GetDefaultValue() => defaultValue;

        /// <summary>
        /// initialize default value state
        /// </summary>
        protected virtual void OnInitialize(ILifeTime lifetime) {}

        private void OnDisable()
        {
            if (!Application.isPlaying)
                return;
            
            //end of value LIFETIME
            lifeTime.Terminate();

            GameLog.Log($"TypeValueAsset: {GetType().Name} {name} : VALUE {Value} END OF LIFETIME");
        }

    }
}
