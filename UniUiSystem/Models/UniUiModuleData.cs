using UniRx;
using UnityEngine;

namespace UniUiSystem.Models
{
    using UniGreenModules.UniCore.Runtime.ObjectPool.Interfaces;

    public class UniUiModuleData : IPoolable
    {
        public ReactiveProperty<RectTransform> Transform = new ReactiveProperty<RectTransform>();
        
        
        public void Release()
        {
            Transform.Value = null;
        }
    }
}
