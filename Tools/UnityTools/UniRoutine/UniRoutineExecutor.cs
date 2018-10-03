using System;
using System.Collections;
using Assets.Modules.UnityToolsModule.Tools.UnityTools.Interfaces;
using Assets.Scripts.Extensions;
using Modules.UnityToolsModule.Tools.UnityTools.Interfaces;
using UnityToolsModule.Tools.UnityTools.UniRoutine;

public class UniRoutineExecutor : IContextExecutor<IEnumerator>
{

    public IDisposableItem Execute(IEnumerator data)
    {
        var disposable = data.RunWithSubRoutines();
        return disposable;
    }

}
