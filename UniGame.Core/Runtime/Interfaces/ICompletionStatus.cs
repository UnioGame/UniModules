namespace UniGreenModules.UniCore.Runtime.Interfaces
{
    using System;

    public interface ICompletionStatus : IDisposable
    {

        bool IsComplete { get; }

    }
}
