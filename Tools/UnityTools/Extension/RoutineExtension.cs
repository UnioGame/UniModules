using System;
using System.Collections;
using Assets.Scripts.Interfaces;
using Tools.AsyncOperations;
using UnityEngine;

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
        
        
        
        public static IEnumerator RoutineWaitUntil(this AsyncOperation operation) {

            if(operation == null)yield break;
            
            while (operation.isDone == false) {
                yield return null;
            }

        }
        
    }
}
