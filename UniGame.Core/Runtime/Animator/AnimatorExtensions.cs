namespace UniModules.UniGame.Core.Runtime.Animator
{
    using System.Collections;
    using UnityEngine;

    public static class AnimatorExtensions
    {
        public static IEnumerator PlayAndWaitForEnd(this Animator animator, int stateHash, int layer = 0)
        {
            while (animator.GetCurrentAnimatorStateInfo(layer).shortNameHash != stateHash) {
                yield return null;
            }

            var currentTime = 0.0f;
            var waitTime = animator.GetCurrentAnimatorStateInfo(layer).length;

            while (currentTime < waitTime) {
                currentTime += Time.deltaTime;
                yield return null;
            }
        }
    }
}