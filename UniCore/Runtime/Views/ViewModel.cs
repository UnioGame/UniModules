﻿namespace UniGreenModules.UniCore.Runtime.Views
{
    using System;
    using DataFlow;
    using Interfaces;
    using Rx;
    using Rx.Extensions;
    using UniRx;

    public class ViewModel<TModel> : IViewModel<TModel>
    {
        private LifeTimeDefinition lifeTimeDefinition = new LifeTimeDefinition();
        private RecycleReactiveProperty<TModel> contextModel = new RecycleReactiveProperty<TModel>();

        public ILifeTime LifeTime => lifeTimeDefinition.LifeTime;

        public Type Type { get; } = typeof(TModel);

        public IReadOnlyReactiveProperty<TModel> Model => contextModel;

        public bool IsInitialized => LifeTime.IsTerminated;

        public void Initialize(TModel model)
        {
            Release();  
            
            //restart lifetime
            lifeTimeDefinition.Release();

            contextModel.Value = model;

            LifeTime.AddDispose(contextModel);

            OnInitialize(model);
        }

        public void Release()
        {
            lifeTimeDefinition.Terminate();
            OnRelease();
        }

        
        protected virtual void OnInitialize(TModel model) {}

        protected virtual void OnRelease(){}
    }
}
