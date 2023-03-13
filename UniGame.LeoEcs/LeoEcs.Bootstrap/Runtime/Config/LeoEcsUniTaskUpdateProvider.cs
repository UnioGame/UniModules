using System;
using Sirenix.OdinInspector;
using UniGame.LeoEcs.Bootstrap.Runtime.Abstract;

namespace UniGame.LeoEcs.Bootstrap.Runtime.Config
{
    [Serializable]
    [InlineProperty]
    public class LeoEcsUniTaskUpdateProvider : ILeoEcsUpdateOrderProvider
    {
        public LeoEcsPlayerUpdateType updateType = LeoEcsPlayerUpdateType.Update;

        public LeoEcsPlayerUpdateType UpdateType => updateType;
        
        public ILeoEcsExecutor Create()
        {
            return new LeoEcsExecutor(updateType);
        }
    }
}