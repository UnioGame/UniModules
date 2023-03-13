using System;
using System.Threading;
using Leopotam.EcsLite;
using UniGame.LeoEcs.Converter.Runtime.Abstract;

namespace UniGame.LeoEcs.Converter.Runtime
{
    [Serializable]
    public abstract class EcsComponentConverter : IComponentConverter
    {
        public bool isEnabled = true;
        
        public virtual bool IsEnabled => isEnabled;

        public abstract void Apply(EcsWorld world, int entity, CancellationToken cancellationToken = default);
        
        protected bool IsSubstring(string value, string search)
        {
            if (string.IsNullOrEmpty(value)) return false;
            return value.Contains(search, StringComparison.OrdinalIgnoreCase);
        }

        public virtual bool IsMatch(string searchString)
        {
            if (string.IsNullOrEmpty(searchString)) return true;
            if (IsSubstring(GetType().Name,searchString))
                return true;
            return false;
        }
    }
}