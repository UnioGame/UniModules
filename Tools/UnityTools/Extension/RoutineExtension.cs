using System;
using System.Collections;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Extension
{
    public static class RoutineExtension
    {

        public static IEnumerator RoutineWaitUntil(this ICompletionStatus status) {

            if(status == null)yield break;
            
            while (status.IsComplete == false) {
                yield return null;
            }

        }
        
    }
}
