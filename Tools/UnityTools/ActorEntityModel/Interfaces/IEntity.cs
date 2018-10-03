using System;
using Assets.Tools.UnityTools.Interfaces;

namespace Assets.Tools.UnityTools.ActorEntityModel.Interfaces
{
    public interface IEntity : IContext,IDisposable
    {
        
        int Id { get; }

    }
}