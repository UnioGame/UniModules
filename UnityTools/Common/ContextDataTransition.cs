using Assets.Tools.UnityTools.Common;
using UnityTools.Common;

public class ContextDataTransition<TContext> : 
    IDataTransition<ContextDataProvider<TContext>,ContextDataProvider<TContext>>
{
    public void Move(ContextDataProvider<TContext> fromData, ContextDataProvider<TContext> data)
    {
        
    }
}
