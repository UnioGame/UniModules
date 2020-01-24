using UniGreenModules.UniContextData.Runtime.Entities;
using UniGreenModules.UniCore.Runtime.Interfaces;
using UniGreenModules.UniGame.SerializableContext.Runtime.Abstract;

namespace UniGreenModules.UniGameSystems.Runtime.SharedData
{
    using UniContextData.Runtime.Interfaces;
    using UnityEngine;

    [CreateAssetMenu(menuName = "UniGame/GameSystem/Assets/SharedContext" , fileName = nameof(SharedContext))]
    public class SharedContext : TypeDataAsset<EntityContext,IContext>, IContextDataSource {
        
        public void Register(IContext context)
        {
            context.Publish(Value);
        }
        
    }
}
