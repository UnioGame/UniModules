namespace UniGame.LeoEcs.ViewSystem.Converters
{
    using System.Threading;
    using Converter.Runtime;
    using Converter.Runtime.Abstract;
    using Leopotam.EcsLite;
    using UiSystem.Runtime;
    using UniGame.ViewSystem.Runtime;
    using UnityEngine;

    [RequireComponent(typeof(LeoEcsMonoConverter))]
    [RequireComponent(typeof(EcsViewConverter))]
    public abstract class EcsUiView<TViewModel> : UiView<TViewModel>,
        ILeoEcsComponentConverter,IEcsView
        where TViewModel : class, IViewModel
    {
        private EcsViewDataConverter<TViewModel> _dataConverter = new EcsViewDataConverter<TViewModel>();
        
        public void Apply(GameObject target, EcsWorld world, 
            int entity, CancellationToken cancellationToken = default)
        {
            _dataConverter.Apply(target,world,entity,cancellationToken);
            
            OnApply(world,entity);
        }
        
        protected virtual void OnApply(EcsWorld world, int entity){}
    }
}