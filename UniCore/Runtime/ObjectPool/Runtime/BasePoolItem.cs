namespace UniGreenModules.UniCore.Runtime.ObjectPool.Runtime
{
    using System;
    using Interfaces;
    using UnityEngine;

    [Serializable]
    public class BasePoolItem : IPoolable
    {
        [SerializeField]
        public string typeName;
        [SerializeField]
        public int count;

        public virtual void Release()
        {
            
        }
    }
}