using System;
using Assets.Tools.UnityTools.Interfaces;

namespace UnityTools.ActorEntityModel.Interfaces
{
    public interface IEntity : IContext
    {
        
        int Id { get; }

    }
}