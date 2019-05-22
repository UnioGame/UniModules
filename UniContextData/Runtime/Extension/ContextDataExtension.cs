namespace UniGreenModules.UniContextData.Runtime.Extension
{
    using System;
    using UniCore.Runtime.Interfaces;
    using UniCore.Runtime.ObjectPool;

    public static class ContextDataExtension
    {
        public static TValue GetOrCreateNew<TContext, TValue>(this IContextData<TContext> source, TContext context)
            where TValue : class, new()
        {
            var item = source.Get<TValue>(context);
            if (item == null) {
                item = ClassPool.Spawn<TValue>();
                source.UpdateValue(context, item);
            }

            return item;
        }

        public static TValue GetOrCreateNew<TContext, TValue>(this IContextData<TContext> source, TContext context, Func<TValue> factory)
            where TValue : class
        {
            var item = source.Get<TValue>(context);
            if (item == null) {
                item = factory();
                source.UpdateValue(context, item);
            }

            return item;
        }
    }
}