namespace UniGreenModules.UniCore.Runtime.Extension
{
    using System;
    using System.Collections.Generic;

    public static class DisposableExtension
    {
        public static void DisposeItems<TTarget>(this List<TTarget> disposables)
            where  TTarget : IDisposable
        {
            for (int i = 0; i < disposables.Count; i++)
            {
                var item = disposables[i];
                item.Dispose();
            }
            disposables.Clear();
        }
    }
}
