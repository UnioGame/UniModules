using UniModule.UnityTools.ObjectPool.Scripts;
using UniRx;
using UnityEngine;

namespace UniUiSystem.Models
{
    public class UniUiModuleData : IPoolable
    {
        public ReactiveProperty<RectTransform> Transform = new ReactiveProperty<RectTransform>();
        
        
        public void Release()
        {
            Transform.Value = null;
        }
    }
}
