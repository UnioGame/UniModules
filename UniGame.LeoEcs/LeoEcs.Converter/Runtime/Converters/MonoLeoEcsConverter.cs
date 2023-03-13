namespace UniGame.LeoEcs.Converter.Runtime.Converters
{
    using System.Threading;
    using Abstract;
    using Leopotam.EcsLite;
    using Sirenix.OdinInspector;
    using UnityEngine;

    public class MonoLeoEcsConverter<TConverter> : MonoBehaviour,ILeoEcsMonoComponentConverter
        where TConverter : ILeoEcsMonoComponentConverter
    {

        [HideLabel]
        [SerializeField]
        private TConverter _converter;

        public TConverter Converter => _converter;
        
        public bool IsEnabled => _converter.IsEnabled;
        
        public void Apply(GameObject target, EcsWorld world, int entity, CancellationToken cancellationToken = default)
        {
            if (_converter == null) 
                return;
            
            _converter.Apply(target, world, entity, cancellationToken);
        }
        
        private void OnDrawGizmos()
        {
            if (_converter is ILeoEcsGizmosDrawer gizmosDrawer)
                gizmosDrawer.DrawGizmos(gameObject);
        }
    }
}