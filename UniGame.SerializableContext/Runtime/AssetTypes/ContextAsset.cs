namespace UniGreenModules.UniGame.SerializableContext.Runtime.AssetTypes
{
    using Abstract;
    using UniContextData.Runtime.Entities;
    using UniContextData.Runtime.Interfaces;
    using UniCore.Runtime.Interfaces;
    using UniRx.Async;
    using UnityEngine;

    [CreateAssetMenu(menuName = "UniGame/GameSystem/Assets/SharedContext" , fileName = nameof(ContextAsset))]
    public class ContextAsset : 
        TypeDataDefaultAsset<EntityContext,IContext>, 
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
        
    }
}
