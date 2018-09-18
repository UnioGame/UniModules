using System;

namespace Assets.Scripts.Interfaces
{
    public interface ICompletionSource : ICompletionStatus ,IDisposable
    {

        void Complete();

    }
}
