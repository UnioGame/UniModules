namespace UniGame.LeoEcs.ViewSystem.Behavriour
{
    using System;
    using Core.Runtime;
    using Cysharp.Threading.Tasks;
    using Leopotam.EcsLite;
    using UiSystem.Runtime;

    [Serializable]
    public class EcsViewModel : ViewModelBase, IEcsViewModel
    {

        public virtual async UniTask InitializeAsync(EcsWorld world, IContext context)
        {
            
        }
        
    }
}