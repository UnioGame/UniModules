using System;
using System.Collections;
using Assets.Tools.UnityTools.Interfaces;
using UnityEngine;

namespace Assets.Tools.UnityTools.Extension
{
    public static class RoutineExtension
    {

        public static IEnumerator RoutineWaitUntil(this ICompletionStatus status) {

            if(status == null)yield break;
            
            while (status.IsComplete == false) {
                yield return null;
            }

        }

        public static IEnumerator RoutineWaitUntil(this Func<bool> completeFunc)
        {

            if (completeFunc == null) yield break;
            while (completeFunc() == false)
            {
                yield return null;
            }

        }


        public static IEnumerator RoutineWaitUntil(this AsyncOperation operation) {

            if(operation == null)yield break;
            
            while (operation.isDone == false) {
                yield return null;
            }

        }
        
    }
}
