using System.Collections;
using Assets.Tools.UnityTools.Interfaces;

namespace Assets.Tools.UnityTools.UniRoutine
{
    public interface IUniRoutine
    {
        IDisposableItem AddRoutine(IEnumerator enumerator);
        void Update();
    }
}