using System;
using Modules.UnityToolsModule.Tools.UnityTools.Interfaces;

namespace Tools.ActorModel
{
    public interface IEntity : IContextProvider,IDisposable
    {
        
        int Id { get; }

        void AddContext<TData>(TData data);
        
    }
}