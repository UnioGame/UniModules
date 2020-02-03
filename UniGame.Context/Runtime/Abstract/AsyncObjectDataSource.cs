using UnityEngine;

namespace UniGreenModules.UniGame.Context.Runtime.Context
{
    using UniContextData.Runtime.Interfaces;
    using UniCore.Runtime.Interfaces;
    using UniRx.Async;

    public abstract class AsyncObjectDataSource : MonoBehaviour, IAsyncContextDataSource
    {

        public abstract UniTask<IContext> RegisterAsync(IContext context);
    }
}
