using System;
using Modules.UnityToolsModule.Tools.UnityTools.Interfaces;

public class CompletionConditionSource : ICompletionStatus , IDisposable{
    
    private Func<bool> _competionFunc;

    public bool IsComplete
    {
        get { return _competionFunc == null || _competionFunc(); }
    }

    public CompletionConditionSource(Func<bool> competionFunc) {
        _competionFunc = competionFunc;
    }

    public void Dispose() {
        _competionFunc = null;
    }
}
