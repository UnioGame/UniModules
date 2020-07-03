namespace UniModules.UniGame.SerializableContext.Runtime.Sources
{
    using System.Collections.Generic;
    using Addressables;
    using Context.Runtime.Abstract;
    using UniGreenModules.UniCore.Runtime.Interfaces;
    using UniGreenModules.UniGame.AddressableTools.Runtime.Extensions;
    using UniRx.Async;
    using UnityEngine;
    using UnityEngine.U2D;

    [CreateAssetMenu(menuName = "UniGame/GameSystem/Sources/SpriteAtlasesSource", fileName = nameof(SpriteAtlasesSource))]
    public class SpriteAtlasesSource : AsyncContextDataSource
    {
        
        public List<AssetReferenceSpriteAtlas> Atlases = new List<AssetReferenceSpriteAtlas>();
        
        public override async UniTask<IContext> RegisterAsync(IContext context)
        {
            var lifetime = context.LifeTime;
            var tasks = Atlases.LoadScriptableAssetsTaskAsync<SpriteAtlas>(lifetime);
            var atlases = await tasks;
            context.Publish(atlases);
            return context;
        }
    }
}
