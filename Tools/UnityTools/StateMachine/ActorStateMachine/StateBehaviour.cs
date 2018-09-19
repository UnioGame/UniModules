using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Extensions;
using Assets.Scripts.Tools.StateMachine;

public class StateBehaviour<TData> : IStateBehaviour<TData, IEnumerator> 
{
    
    protected readonly List<IDisposable> _disposables = new List<IDisposable>();

    public bool IsActive { get; protected set; }
       
    #region public methods

    public IEnumerator Execute(TData data)
    {

        if (IsActive)
            yield return Wait();
            
        IsActive = true;
            
        Initialize();

        yield return ExecuteState(data);

    }

    public void Exit() {
            
        IsActive = false;
        _disposables.Cancel();
        OnStateStop();
            
    }



    #endregion

    protected virtual IEnumerator Wait()
    {
        while (IsActive)
        {
            yield return null;
        }
    }
        
    protected virtual void OnStateStop()
    {
    }
        
    protected virtual void Initialize()
    {
    }

    protected virtual IEnumerator ExecuteState(TData data)
    {
        yield break;
    }
}