using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Tools.StateMachine;
using UnityEngine;

public class ReactiveStateMachine<TData,TState> : StateBehaviour<TData> {

    private IStateSelector<TData,TState> _stateSelector;
    private IStateManager<TState> _stateManager;
        
    public void Initialize(
        IStateSelector<TData,TState> stateSelector,
        IStateManager<TState> stateManager)
    {
        
        if(IsActive)
            Exit();
            
        _stateSelector = stateSelector;
        _stateManager = stateManager;      

    }
        

    #region private methods

    protected override IEnumerator ExecuteState(TData data)
    {
        while (IsActive)
        {
            var state = _stateSelector.Select(data);
                
            _stateManager.SetState(state);
                
            yield return null;
        }
    }

    protected override void OnStateStop()
    {
        base.OnStateStop();
        _stateManager?.Stop();
        _stateManager = null;
        _stateSelector = null;
    }

    #endregion
}
