namespace UniModules.UniGame.Core.Runtime.Extension
{
    using UnityEngine;

    public static class MonoBehaviourExtensions
    {
        public static RectTransform RectTransform(this MonoBehaviour behaviour)
        {
            return behaviour.transform as RectTransform;
        }
    }
}