namespace UniGreenModules.UniActors.Runtime.ActorData
{
    using System;
    using Interfaces;
    using UniCore.Runtime.ScriptableObjects;

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
