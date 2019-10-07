namespace UniGreenModules.UniCore.Runtime.ObjectPool
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