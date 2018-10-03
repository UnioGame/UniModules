using System;
using System.Collections;
using Assets.Modules.UnityToolsModule.Tools.UnityTools.Interfaces;

namespace Tools.UniRoutineTask
{
    public interface IUniRoutine
    {
        IDisposableItem AddRoutine(IEnumerator enumerator);
        void Update();
    }
}