namespace UniGreenModules.UniCore.Runtime.Interfaces
{
    using System;
    using System.Collections.Generic;

    public interface IContextData<TContext> : IContextDataWriter<TContext>
    {
        IReadOnlyList<TContext> Contexts { get; }

        int Count { get; }
        
        bool HasValue(TContext context,Type type);
        
        bool HasValue<TValue>(TContext context);
             
        bool HasContext(TContext context);
        
        TData Get<TData>(TContext context);


    }
}
