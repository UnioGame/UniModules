namespace UniGreenModules.UniUiSystem.Runtime.Models
{
    using UniCore.Runtime.ObjectPool.Interfaces;
    using UniRx;
    using UnityEngine;

    public class UniUiModuleData : IPoolable
    {
        public ReactiveProperty<RectTransform> Transform = new ReactiveProperty<RectTransform>();
        
        
        public void Release()
        {
            Transform.Value = null;
        }
    }
}
