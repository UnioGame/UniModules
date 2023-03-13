namespace UniGame.LeoEcs.Converter.Runtime
{
    using System;
    using System.Threading;
    using Abstract;
    using Core.Runtime.Extension;
    using Leopotam.EcsLite;
    using Sirenix.OdinInspector;
    using UniModules.UniCore.Runtime.ReflectionUtils;
    using UnityEngine;

    [Serializable]
    [InlineProperty]
    public class ComponentConverterValue : IComponentConverter
    {
        [SerializeReference]
        [InlineProperty]
        [ShowIf(nameof(IsSerializableConverter))]
        [HideLabel]
        [FoldoutGroup("$GroupTitle",false)]
        public IComponentConverter converter;
        
        [InlineEditor()] 
        [FoldoutGroup("$GroupTitle",false)]
        [ShowIf(nameof(IsAssetConverter))]
        [HideLabel]
        public ComponentConverterAsset convertersAsset;

        [InlineEditor()] 
        [FoldoutGroup("$GroupTitle",false)]
        [ShowIf(nameof(IsNestedConverter))]
        [HideLabel]
        public LeoEcsConverterAsset nesterConverter;

        public string GroupTitle => GetValueTitle();
        
        public bool IsEmpty => converter == null && 
                               convertersAsset == null &&
                               nesterConverter == null;
        
        public bool IsAssetConverter =>  IsEmpty || convertersAsset != null;
        
        public bool IsNestedConverter =>  IsEmpty || nesterConverter != null;

        public bool IsSerializableConverter => IsEmpty || converter!= null;

        public IComponentConverter Value => GetValue();

        public IComponentConverter GetValue()
        {
            if (converter != null) return converter;
            if (convertersAsset != null) return convertersAsset;
            if (nesterConverter != null) return nesterConverter;

            return null;
        }
        
        public string GetValueTitle()
        {
            if (converter != null) return converter.GetType().GetFormattedName();
            if (convertersAsset != null) return convertersAsset.name;
            if (nesterConverter != null) return nesterConverter.name;

            return "EMPTY";
        }
        
        public bool IsEnabled => Value?.IsEnabled ?? false;
        
        public void Apply(EcsWorld world, int entity, CancellationToken cancellationToken = default)
        {
            Value.Apply(world,entity,cancellationToken);
        }

        public bool IsMatch(string searchString)
        {
            if (string.IsNullOrEmpty(searchString)) return true;
            if (!string.IsNullOrEmpty(GroupTitle) && 
                GroupTitle.Contains(searchString,StringComparison.CurrentCultureIgnoreCase)) return true;
            
            var result = Value?.IsMatch(searchString) ?? false;
            return result;
        }
    }
}