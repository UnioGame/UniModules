namespace UniGreenModules.UniCore.Runtime.ObjectPool
{
    using System;
    using System.Collections.Generic;
    using Interfaces;
    using UnityEngine;

    public class ClassPoolContainer : MonoBehaviour, IPoolContainer
    {
        public bool Contains<T>() 
            where T : class
        {
            return ClassPoolItem<T>.Instance.Count > 0;
        }

        public T Pop<T>()
            where T : class
        {
			return ClassPoolItem<T>.Instance.Pop();
        }

        public void Push<T>(T item)
            where T : class
        {
            ClassPoolItem<T>.Instance.Push(item);
        }
		
    }
}