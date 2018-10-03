using System;

namespace Assets.Tools.UnityTools.Interfaces
{
    public interface ICompletionStatus : IDisposable
    {

        bool IsComplete { get; }

    }
}
