namespace UniGreenModules.UniGame.SerializableContext.Runtime.Abstract
{
    using System;
    using UniCore.Runtime.Common;
    using UniCore.Runtime.Interfaces;
    using UnityEngine;

    [Serializable]
    public abstract class TypeDataAsset<TValue,TApiValue> : 
        ScriptableObject,
        IDataValue<TApiValue>
        where TValue : TApiValue
    {
        [SerializeField]
        private TValue defaultValue = default(TValue);
        
        private ContextValue<TApiValue> contextValue = new ContextValue<TApiValue>();

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
            if(contextValue == null) contextValue = new ContextValue<TApiValue>();
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
        
    }
}
