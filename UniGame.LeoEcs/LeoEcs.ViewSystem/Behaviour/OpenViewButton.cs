namespace UniGame.LeoEcs.ViewSystem.Behavriour
{
    using System;
    using System.Threading;
    using Converter.Runtime;
    using Converter.Runtime.Abstract;
    using Core.Runtime;
    using Cysharp.Threading.Tasks;
    using Leopotam.EcsLite;
    using Sirenix.OdinInspector;
    using UniGame.Core.Runtime.SerializableType;
    using UniGame.Core.Runtime.SerializableType.Attributes;
    using Converters;
    using Extensions;
    using UniGame.Rx.Runtime.Extensions;
    using UniGame.ViewSystem.Runtime;
    using UniModules.UniGame.Core.Runtime.DataFlow.Extensions;
    using UniModules.UniGame.UiSystem.Runtime;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(LeoEcsMonoConverter))]
    public class OpenViewButton : MonoBehaviour, ILeoEcsComponentConverter, ILifeTimeContext
    {
        #region inspector

        public Button trigger;

#if ODIN_INSPECTOR
        [DrawWithUnity]
#endif
        [STypeFilter(typeof(IEcsView))]
        public SType targetViewType;

        #endregion

        private ILifeTime _lifeTime;

        public ILifeTime LifeTime => _lifeTime;
        
        public ViewType layoutType = ViewType.Window;
        
        public void Apply(GameObject target, EcsWorld world, 
            int entity, CancellationToken cancellationToken = default)
        {
            this.Bind(trigger, x => world
                .MakeViewRequest(targetViewType, layoutType));
        }
        
        [OnInspectorInit]
        private void OnInspectorInitialize()
        {
            if (trigger == null)
                trigger = GetComponent<Button>();
        }

        private void Awake()
        {
            _lifeTime = this.GetAssetLifeTime();
        }
    }
}
