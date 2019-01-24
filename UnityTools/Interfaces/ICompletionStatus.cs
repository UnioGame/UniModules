using System;

namespace UniModule.UnityTools.Interfaces
{
    public interface ICompletionStatus : IDisposable
    {

        bool IsComplete { get; }

    }
}
