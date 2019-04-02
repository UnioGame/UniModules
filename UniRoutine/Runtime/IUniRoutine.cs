using System.Collections;
using UniModule.UnityTools.Interfaces;

namespace UniModule.UnityTools.UniRoutine
{
    public interface IUniRoutine
    {
        IDisposableItem AddRoutine(IEnumerator enumerator);
        void Update();
    }
}