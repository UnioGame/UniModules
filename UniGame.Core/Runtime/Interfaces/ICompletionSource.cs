namespace UniGreenModules.UniCore.Runtime.Interfaces
{
    using System;

    public interface ICompletionSource : ICompletionStatus
    {

        void Complete();

    }
}
