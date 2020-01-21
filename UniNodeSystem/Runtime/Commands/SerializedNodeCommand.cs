namespace UniGreenModules.UniNodeSystem.Runtime.Commands
{
    using System;
    using Interfaces;
    using UniCore.Runtime.Interfaces;

    [Serializable]
    public abstract class SerializedNodeCommand 
    {

        public abstract ILifeTimeCommand Create(IUniNode node);
        
    }
}
