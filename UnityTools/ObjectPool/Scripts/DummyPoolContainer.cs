using System;
using Assets.Tools.UnityTools.ObjectPool.Scripts;

namespace UnityTools.ObjectPool.Scripts
{
    public class DummyPoolContainer : IPoolContainer
    {

        public bool Contains(Type type)
        {
            return false;
        }

        public object Pop(Type type)
        {
            return null;
        }

        public void Push(Type type, object item)
        {
            
        }
    }
}
