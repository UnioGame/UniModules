namespace UniGame.LeoEcs.ViewSystem.Behavriour
{
    using System.Threading;
    using Components;
    using Converter.Runtime;
    using Converter.Runtime.Abstract;
    using Core.Runtime;
    using Leopotam.EcsLite;
    using Rx.Runtime.Extensions;
    using Shared.Extensions;
    using Sirenix.OdinInspector;
    using UniModules.UniGame.Core.Runtime.DataFlow.Extensions;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(LeoEcsMonoConverter))]
    public class CloseAllViewsButton : MonoBehaviour, ILeoEcsComponentConverter, ILifeTimeContext
    {
        #region inspector

        public Button trigger;

        #endregion

        private ILifeTime _lifeTime;

        public ILifeTime LifeTime => _lifeTime;

        public void Apply(GameObject target, EcsWorld world, 
            int entity, CancellationToken cancellationToken = default)
        {
            this.Bind(trigger, x => world.GetOrAddComponent<CloseAllViewsRequest>(entity));
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