using Assets.Tools.UnityTools.Interfaces;

namespace Assets.Tools.UnityTools.Common
{
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
