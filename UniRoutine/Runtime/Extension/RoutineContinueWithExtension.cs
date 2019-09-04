namespace ZR.Runtime.Utils.Extensions
{
    using System;
    using System.Collections;

    public static class RoutineContinueWithExtension 
    {

        public static IEnumerator ContinueWith(this IEnumerator source, IEnumerator couroutine)
        {
            yield return source;
            yield return couroutine;
        }
    
        public static IEnumerator ContinueWhereWith(this IEnumerator source, IEnumerator couroutine, Func<bool> predicate)
        {
            yield return source;
            if (predicate?.Invoke() ?? false) {
                yield return couroutine;
            }
        }
        
        public static IEnumerator ContinueWith(this IEnumerator source, Action routineFunc)
        {
            yield return source;
            routineFunc?.Invoke();
        }
    }
}
