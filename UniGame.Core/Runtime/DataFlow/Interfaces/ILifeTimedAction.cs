namespace UniGame.Core.Runtime.Common
{
    using System;

    public interface ILifeTimedAction : IDisposable
    {
        void Invoke();
    }
}