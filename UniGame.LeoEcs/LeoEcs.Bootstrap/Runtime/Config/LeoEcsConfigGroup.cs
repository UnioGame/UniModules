
namespace UniGame.LeoEcs.Bootstrap.Runtime.Config
{
    using System;
    using System.Collections.Generic;
    using Abstract;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.Pool;
    using System.Collections;
    
    [Serializable]
    public class LeoEcsConfigGroup : ISearchFilterable
    {
        [GUIColor(0.2f,0.9f,0f)]
        [ValueDropdown(nameof(GetUpdateIds))]
        public string updateType;
        
        [Space(8)]
        [TitleGroup("ECS Features")]
        [SerializeField]
        [InlineProperty]
        [Searchable(FilterOptions = SearchFilterOptions.ISearchFilterableInterface)]
        //[ListDrawerSettings(ListElementLabelName = "FeatureName")]
        public List<LeoEcsFeatureData> features = new List<LeoEcsFeatureData>();

        public override bool Equals(object obj)
        {
            if (obj is LeoEcsConfigGroup configGroup) return configGroup.updateType == updateType;
            return false;
        }

        public override int GetHashCode() => string.IsNullOrEmpty(updateType) 
            ? 0 
            : updateType.GetHashCode();

        public bool IsMatch(string searchString)
        {
            if (string.IsNullOrEmpty(searchString)) return true;
            foreach (var featureData in features)
            {
                if (featureData.IsMatch(searchString))
                    return true;
            }
            return false;
        }

        public IEnumerable GetUpdateIds()
        {
            return LeoEcsUpdateQueueIds.GetUpdateIds();
        }
        
    }
}