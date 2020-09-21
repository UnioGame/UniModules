namespace UniModules.UniGame.AddressableTools.Runtime.SpriteAtlases.Abstract
{
    using Cysharp.Threading.Tasks;

    public interface IAddressablesAtlasesLoader
    {
        UniTask<bool> RequestSpriteAtlas(string guid);
    }
}