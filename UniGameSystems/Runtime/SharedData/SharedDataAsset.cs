using UnityEngine;

namespace UniGreenModules.UniGameSystems.Runtime.SharedData
{
    using System;
    using UniCore.Runtime.Common;
    using UniCore.Runtime.Interfaces;

    public abstract class SharedDataAsset<TValue> : 
        ScriptableObject,
        IDataValue<TValue>
    {
        private ContextValue<TValue> contextValue = new ContextValue<TValue>();

        public IDisposable Subscribe(IObserver<TValue> observer) => throw new NotImplementedException();

        public TValue Value => contextValue.Value;

        public bool HasValue => contextValue.HasValue;
        
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void SetValue(TValue value)
        {
            throw new NotImplementedException();
        }

        protected virtual void OnEnable()
        {
            
        }
    }
}
