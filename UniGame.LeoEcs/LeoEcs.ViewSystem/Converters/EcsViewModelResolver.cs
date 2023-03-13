namespace UniGame.LeoEcs.ViewSystem.Converters
{
    using System;
    using Behavriour;
    using Context.Runtime.Extension;
    using Core.Runtime;
    using Core.Runtime.ReflectionUtils;
    using Cysharp.Threading.Tasks;
    using Leopotam.EcsLite;
    using UniGame.ViewSystem.Runtime;
    using UniModules.UniGame.ViewSystem.Runtime.Abstract;
    using UnityEngine;

    [Serializable]
    public class EcsViewModelResolver : IViewModelResolver
    {
        #region inspector

        [SerializeField] 
        public DefaultConstructorViewModelFactory
            defaultConstructorFactory = new DefaultConstructorViewModelFactory();

        #endregion

        public bool IsValid(Type modelType) => defaultConstructorFactory.IsValid(modelType) &&
                                               AssignableTypeValidator<IEcsViewModel>.Validate(modelType);

        public async UniTask<IViewModel> CreateViewModel(IContext context, Type type)
        {
            var world = await context.ReceiveFirstAsync<EcsWorld>();
            var lifeTime = context.LifeTime;
            var viewModel = await defaultConstructorFactory
                .CreateViewModel(context, type);

            if (viewModel is not IEcsViewModel model) 
                return viewModel;

            await model.InitializeAsync(world,context)
                .AttachExternalCancellation(lifeTime.AsCancellationToken());
                
            return model;
        }
    }
}