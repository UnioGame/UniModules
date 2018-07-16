using System;

namespace Assets.Scripts.MessageQueue
{
    public interface IMessageSystem<TData> : IDisposable
    {
        /// <summary>
        /// add source channel
        /// </summary>
        IDisposable AddPublicher(IObservable<TData> observable);

        ///  <summary>
        ///  register channel to target messages
        ///  </summary>
        /// <param name="observer"></param>
        IResetable Subscribe(IObserver<TData> observer);


    }
}