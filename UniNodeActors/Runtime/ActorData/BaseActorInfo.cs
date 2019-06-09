using UniGreenModules.UniCore.Runtime.Interfaces;
using UniGreenModules.UniCore.Runtime.ScriptableObjects;

namespace UniGreenModules.UniNodeActors.Runtime.ActorData
{
    using System;
    using Interfaces;

    [Serializable]
    public abstract class BaseActorInfo<T> : 
        ScriptableObservable<T>,
        IActorInfo<T>
    {
        public virtual T Create()
        {
            var model = CreateDataSource();
            valueStream.OnNext(model);
            return model;
        }

        protected abstract T CreateDataSource();

    }
}
