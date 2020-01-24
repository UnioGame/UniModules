namespace UniGreenModules.UniCore.Runtime.Common
{
    using Interfaces;

    public class CompletionSource : ICompletionSource
    {
        public bool IsComplete { get; protected set; }

        public void Complete()
        {
            IsComplete = true;
        }

        public void Dispose()
        {
            IsComplete = false;
        }
    }
}
