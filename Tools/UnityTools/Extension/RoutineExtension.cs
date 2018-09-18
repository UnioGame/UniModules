using System;
using System.Collections;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Extension
{
    public static class RoutineExtension
    {

        public static IEnumerator RoutineWaitUntil(this ICompletionSource source) {

            if(source == null)yield break;
            
            while (source.IsComplete == false) {
                yield return null;
            }

        }
        
    }
}
