using System;

namespace Assets.Scripts.Interfaces
{
    public interface ICompletionSource : IDisposable
    {

        bool IsComplete { get; }

    }
}
