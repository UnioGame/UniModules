using Assets.Tools.UnityTools.Interfaces;
using Assets.Tools.UnityTools.ObjectPool.Scripts;

namespace Modules.Tools.UnityTools.Extension
{
    public static class ContextDataExtension 
    {
       
        public static TValue GetOrCreateNew<TContext,TValue>(this IContextData<TContext> source,TContext context)
            where TValue : class, new()
        {
            
            var item = source.Get<TValue>(context);
            if (item == null)
            {
                item = ClassPool.Spawn<TValue>();
                source.UpdateValue(context,item);
            }

            return item;
            
        }
        
        public static TValue GetOrCreateDefault<TContext,TValue>(this IContextData<TContext> source,TContext context)
        {
            
            var item = source.HasContext()<TValue>(context);
            if (item == null)
            {
                item = 
                source.UpdateValue(context,item);
            }

            return item;
            
        }
        
    }
}
