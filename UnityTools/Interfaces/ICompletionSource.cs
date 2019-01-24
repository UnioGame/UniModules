using System;

namespace UniModule.UnityTools.Interfaces
{
    public interface ICompletionSource : ICompletionStatus ,IDisposable
    {

        void Complete();

    }
}
