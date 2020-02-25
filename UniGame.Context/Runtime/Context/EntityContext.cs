namespace UniGreenModules.UniContextData.Runtime.Entities
{
    using System;
    using System.Collections.Generic;
    using UniCore.Runtime.Common;
    using UniCore.Runtime.DataFlow;
    using UniCore.Runtime.DataFlow.Interfaces;
    using UniCore.Runtime.Interfaces;
    using UniCore.Runtime.Interfaces.Rx;
    using UniCore.Runtime.ObjectPool;
    using UniCore.Runtime.ObjectPool.Runtime.Extensions;
    using UniCore.Runtime.ProfilerTools;
    using UniCore.Runtime.Rx;
    using UniRx;

    public class EntityContext : IContext
    {
        private TypeData           data;
        private LifeTimeDefinition lifeTimeDefinition;
        private bool hasValue;

        public EntityContext()
        {
            //context data container
            data = new TypeData();
            //context lifetime
            lifeTimeDefinition = new LifeTimeDefinition();
        }

        
#region public properties

        public ILifeTime LifeTime => lifeTimeDefinition.LifeTime;

        public bool HasValue => data.HasValue;
        
#endregion
        
#region public methods

        public bool Contains<TData>()
        {
            return data.Contains<TData>();
        }

        public virtual TData Get<TData>()
        {
            return data.Get<TData>();
        }

        public bool Remove<TData>()
        {
            return data.Remove<TData>();
        }

        public void Release()
        {
            data.Release();
            lifeTimeDefinition.Release();
        }

        public virtual void Dispose()
        {
            Release();
            this.Despawn();
        }

#region rx 

        public void Publish<T>(T message)
        {
            data.Publish(message);
        }

        public IObservable<T> Receive<T>()
        {
            return data.Receive<T>();
        }

#endregion

#endregion

#region Unity Editor Api
#if UNITY_EDITOR
        
        public IReadOnlyDictionary<Type, IValueContainerStatus> EditorValues => data.EditorValues;
        
#endif
#endregion

    }
}