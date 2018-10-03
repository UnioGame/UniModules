using System;

namespace Modules.UnityToolsModule.Tools.UnityTools.Interfaces
{
    public interface ICompletionStatus : IDisposable
    {

        bool IsComplete { get; }

    }
}
