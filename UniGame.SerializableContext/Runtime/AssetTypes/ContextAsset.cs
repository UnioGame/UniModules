namespace UniGreenModules.UniGame.SerializableContext.Runtime.AssetTypes
{
    using UniContextData.Runtime.Entities;
    using UniContextData.Runtime.Interfaces;
    using UniCore.Runtime.DataFlow.Interfaces;
    using UniCore.Runtime.Interfaces;
    using UniRx.Async;
    using UnityEngine;

    [CreateAssetMenu(menuName = "UniGame/GameSystem/Assets/Context" , fileName = nameof(ContextAsset))]
    public class ContextAsset : 
        TypeValueDefaultAsset<EntityContext,IContext>, 
        IContextDataSource,
        IAsyncContextDataSource
    {
        
        public virtual void Register(IContext context)
        {
            context.Publish(Value);
        }

        public virtual async UniTask<IContext> RegisterAsync(IContext context)
        {
            context.Publish(Value);
            return context;
        }
        
        protected override void OnInitialize(ILifeTime lifeTime) => lifeTime.AddDispose(defaultValue);

    }
}
