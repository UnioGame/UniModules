using System;

namespace Modules.UnityToolsModule.Tools.UnityTools.Interfaces
{
    public interface ICompletionSource : ICompletionStatus ,IDisposable
    {

        void Complete();

    }
}
