namespace UniGreenModules.UniRoutine.Runtime.Extension
{
    using System;
    using System.Collections;
    using UniTools.UniRoutine.Runtime;
    using UniTools.UniRoutine.Runtime.Extension;
    using UnityEngine;

    public static class RoutineActionExtension
    {

        public static void Do(this object source,Action action, float delay)
        {
            if (delay <= 0) {
                source.DoDelayed(action).
                    ExecuteRoutine();
            }
        }

        public static IEnumerator OnUpdate(this GameObject asset, Action action)
        {
            if (action == null) {
                yield break;
            }
            
            while (asset) {
                action();
                yield return null;
            }
        }
        
    }
}
