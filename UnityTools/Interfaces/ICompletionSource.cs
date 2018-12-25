using System;

namespace Assets.Tools.UnityTools.Interfaces
{
    public interface ICompletionSource : ICompletionStatus ,IDisposable
    {

        void Complete();

    }
}
