using System;
using System.Collections;
using Assets.Scripts.Extensions;
using Modules.UnityToolsModule.Tools.UnityTools.Interfaces;
using UnityToolsModule.Tools.UnityTools.UniRoutine;

public class UniRoutineExecutor : IRoutineExecutor<IEnumerator>
{

    private IDisposable _disposable;

    public bool IsActive { get; protected set; }

    public void Execute(IEnumerator data)
    {
        IsActive = true;
        _disposable = data.RunWithSubRoutines();

    }

    public void Stop()
    {
        IsActive = false;
        _disposable.Cancel();
    }


}
