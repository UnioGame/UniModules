namespace UniGreenModules.UniGame.SerializableContext.Runtime.Abstract
{
    using System;
    using UniCore.Runtime.Common;
    using UniCore.Runtime.Interfaces;
    using UnityEngine;

    [Serializable]
    public abstract class TypeDataAsset<TValue> : 
        ScriptableObject,
        IDataValue<TValue>
    {
        [SerializeField]
        private TValue defaultValue = default(TValue);
        
        private ContextValue<TValue> contextValue = new ContextValue<TValue>();

        public TValue Value => contextValue.Value;

        public bool HasValue => contextValue.HasValue;
        
        #region public methods
        
        public IDisposable Subscribe(IObserver<TValue> observer) => contextValue.Subscribe(observer);
        
        public void Dispose() => contextValue.Dispose();
        
        public void SetValue(TValue value) => ApplyValue(value);
        
        #endregion
        
        /// <summary>
        /// Initialize when SO loaded
        /// </summary>
        protected void OnEnable()
        {
            if(contextValue == null) contextValue = new ContextValue<TValue>();
            OnInitialize();
        }
        
        /// <summary>
        /// Apply Value to context
        /// </summary>
        /// <param name="value"></param>
        protected virtual void ApplyValue(TValue value) => contextValue.SetValue(value);

        /// <summary>
        /// initialize default value state
        /// </summary>
        protected virtual void OnInitialize() => SetValue(defaultValue);
    }
}
