namespace UniGame.LeoEcs.Debug.Editor
{
    using System;
    using System.Collections.Generic;
    using Converter.Runtime;
    using Leopotam.EcsLite;
    using Runtime.ObjectPool;
    using Runtime.ObjectPool.Extensions;
    using Shared.Components;
    using Sirenix.OdinInspector;
    using UniModules.UniCore.Runtime.Utils;
    using UnityEngine;
    using UnityEngine.UIElements;

    [Serializable]
    public class EntitiesEditorView
    {
        
        #region static data

        private static Color _oddColor = new Color(0.3f, 0.5f, 0.4f);
        private static Color _rowColor = new Color(0.3f, 0.6f, 0.6f);
        
        public static bool IsInitialized;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public void ResetStaticData()
        {
            IsInitialized = false;
        }
        
        #endregion
        
        #region inspector

        [HorizontalGroup()]
        [LabelWidth(60)]
        [LabelText("status :")]
        [GUIColor(nameof(GetStatusColor))]
        public string status;

        [HorizontalGroup()]
        [LabelWidth(60)]
        [LabelText("info :")]
        public string message;

        [Space(8)]
        [Searchable(FilterOptions = SearchFilterOptions.ISearchFilterableInterface)] 
        [InlineProperty]
        [ListDrawerSettings(DraggableItems = false,HideAddButton = true,
            HideRemoveButton = true,
            ElementColor = nameof(GetElementColor))]
        public List<EntityEditorView> entities = new List<EntityEditorView>();

        #endregion

        private HashSet<int> _uniqueEntities = new HashSet<int>();
        private EcsWorld _world;
        private EcsFilterData _statusData = EcsFilterData.NoneFilterData;

        private List<IEcsWorldSearchFilter> _ecsResultFilter = new List<IEcsWorldSearchFilter>()
        {
            new CheckEditorStatusFilter(),
            new CheckEcsWorldStatusFilter(),
            new FilterEntitiesComponents(),
        };

        private List<IEntityEditorViewBuilder> _builders = new List<IEntityEditorViewBuilder>()
        {
            new ComponentsEntityBuilder(),
            new GameObjectEntityBuilder(),
        };

        public void Initialize(EcsWorld world)
        {
            _world = world;

            if (_world == null || !VerifyView()) return;

            IsInitialized = true;

            foreach (var composer in _builders)
                composer.Initialize(_world);
        }

        public void UpdateFilter(string filterValue)
        {
            VerifyView();
            
            if (!IsInitialized) return;
            
            var data = ApplyFilters(filterValue);
            if (data.type != ResultType.Success)
                return;
            
            UpdateEntitiesView(data);
        }

        public bool VerifyView()
        {
            if (Application.isPlaying && 
                _world != null)
                return true;
            
            ReleaseEntityViews();
            ResetStatus();
            IsInitialized = false;
            return false;
        }
        
        public EcsFilterData ApplyFilters(string filterValue)
        {
            ResetStatus();
            
            _statusData.filter = filterValue;
            
            var filter = _statusData;

            foreach (var searchFilter in _ecsResultFilter)
            {
                filter = searchFilter.Execute(_statusData);

                if (filter.type == ResultType.Error)
                    break;
            }

            _statusData = filter;
            status = filter.type != ResultType.Success
                ? filter.errorMessage
                : filter.type.ToStringFromCache();

            message = filter.message;

            return filter;
        }

        public void ResetStatus()
        {
            _statusData.world = _world;
            _statusData.filter = string.Empty;
            _statusData.message = string.Empty;
            _statusData.type = ResultType.Success;
            _statusData.entities.Clear();
        }
        
        public void UpdateEntitiesView(EcsFilterData data)
        {
            ReleaseEntityViews();

            _uniqueEntities.Clear();
            _uniqueEntities.UnionWith(data.entities);
            
            foreach (var dataEntity in _uniqueEntities)
            {
                var view = ClassPool.Spawn<EntityEditorView>();
                view.id = dataEntity;
                view.packedEntity = _world.PackEntity(dataEntity);
                view.name = dataEntity.ToStringFromCache();
                entities.Add(view);
            }

            foreach (var builder in _builders)
                builder.Execute(entities);
        }

        public void ReleaseEntityViews()
        {
            foreach (var view in entities)
            {
                view.Release();
                view.Despawn();
            }

            entities.Clear();
        }

        public Color GetStatusColor()
        {
            var resultType = _statusData.type;

            var resultColor = resultType switch
            {
                ResultType.Error => Color.red,
                ResultType.None => Color.yellow,
                ResultType.Success => Color.green
            };

            return resultColor;
        }

        public Color GetElementColor(int index, Color defaultColor)
        {
            return index % 2 == 0 ? _oddColor : defaultColor;
        }
    }
}