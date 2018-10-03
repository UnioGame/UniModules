using System;
using Modules.UnityToolsModule.Tools.UnityTools.Interfaces;

namespace Tools.ActorModel
{
    public interface IEntity : IContext,IDisposable
    {
        
        int Id { get; }

    }
}