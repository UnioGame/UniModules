namespace UniTools.UniRoutine.Runtime.Interfaces
{
    using System.Collections;
    using UniModule.UnityTools.Interfaces;

    public interface IUniRoutine
    {
        IDisposableItem AddRoutine(IEnumerator enumerator);
        void Update();
    }
}