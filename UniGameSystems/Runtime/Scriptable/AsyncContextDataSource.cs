using UniGreenModules.UniContextData.Runtime.Interfaces;
using UniGreenModules.UniCore.Runtime.Interfaces;
using UniRx.Async;
using UnityEngine;

namespace UniGreenModules.UniGameSystems.Runtime.Scriptable
{
    public abstract class AsyncContextDataSource : 
        ScriptableObject, 
        IAsyncContextDataSource
    {
        
        public abstract UniTask<IContext> Register(IContext context);

    }
}
