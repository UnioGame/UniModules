using System;
using System.Collections.Generic;

namespace Assets.Tools.UnityTools.Extension
{
    public static class RxExtension
    {
        public static void Cancel(this IDisposable disposable)
        {
            disposable?.Dispose();
        }
        
        public static void Cancel<TItem>(this List<TItem> disposables)
            where TItem : IDisposable
        {
            if (disposables == null)
                return;
            for (var i = 0; i < disposables.Count; i++)
            {
                disposables[i]?.Dispose();
            }
        }

        public static void Cancel(this List<IDisposable> disposables)
        {
            if (disposables == null)
                return;
            for (var i = 0; i < disposables.Count; i++)
            {
                disposables[i]?.Dispose();
            }
        }
    }
}
