using UnityEngine;

namespace UniGreenModules.UniGame.Context.Runtime.Context
{
    using Cysharp.Threading.Tasks;
    using UniContextData.Runtime.Interfaces;
    using UniCore.Runtime.Interfaces;

    public abstract class AsyncObjectDataSource : MonoBehaviour, IAsyncContextDataSource
    {

        public abstract UniTask<IContext> RegisterAsync(IContext context);
    }
}
