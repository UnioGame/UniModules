using System;
using UniGame.LeoEcs.Bootstrap.Runtime.Abstract;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UniGame.LeoEcs.Bootstrap.Runtime.Config
{
    [Serializable]
    public class GizmosUpdateProvider : ILeoEcsSystemsPluginProvider
    {
        public ISystemsPlugin Create()
        {
            var gameObject = new GameObject();
            var gizmos = gameObject.AddComponent<LeoEcsGizmosExecutor>();
            Object.DontDestroyOnLoad(gameObject);
            return gizmos;
        }
    }
}