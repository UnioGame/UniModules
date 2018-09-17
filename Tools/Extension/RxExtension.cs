using System;
using System.Collections.Generic;

namespace Assets.Scripts.Extensions
{
    public static class RxExtension
    {
        public static void Cancel(this IDisposable disposable)
        {
            disposable?.Dispose();
        }

        public static void Cancel(this List<IDisposable> disposable)
        {
            if (disposable == null)
                return;
            for (var i = 0; i < disposable.Count; i++)
            {
                disposable[i]?.Dispose();
            }
            disposable.Clear();
        }
    }
}
