using UniRx;

namespace UniModule.UnityTools.Interfaces
{
    public interface ICopyableData<TContext>
    {
        /// <summary>
        /// copy context values to new container
        /// </summary>
        /// <param name="context">key context</param>
        /// <param name="writer">container writer</param>
        void CopyTo(TContext context, IMessagePublisher writer);
    }
}