namespace UniGame.LeoEcs.Debug.Editor
{
    using System;
    using System.Collections.Generic;
    using Converter.Runtime;
    using Core.Runtime.ObjectPool;
    using Leopotam.EcsLite;
    using Runtime.ObjectPool.Extensions;
    using Sirenix.OdinInspector;
    using UniModules.UniCore.Runtime.Utils;
    using UnityEngine;

    [Serializable]
    public class EntityEditorView : IPoolable, ISearchFilterable,IEntityEditorView
    {
        #region static data

        private static Color _oddColor = new Color(0.4f, 0.5f, 0.4f);
        private static Color _rowColor = new Color(0.3f, 0.6f, 0.6f);
        private static Color _buttonColor = new Color(0.2f, 1, 0.6f);
        private static Color _buttonRedColor = new Color(0.8f, 0.4f, 0.6f);
        
        #endregion

        [LabelWidth(60)]
        [HorizontalGroup()]
        [HideIf(nameof(IsNameSameAsId))]
        public string name;

        [LabelWidth(20)]
        [HorizontalGroup()]
        public int id;

        [HideInInspector]
        public EcsPackedEntity packedEntity;
        
        [HideInInspector]
        public bool isDead;
        
        [HorizontalGroup()]
        [HideLabel]
        [ShowIf(nameof(HasGameObject))]
        public GameObject gameObject;

        [PropertyOrder(10)]
        [Space(12)]
        [Searchable(FilterOptions = SearchFilterOptions.ISearchFilterableInterface)]
        [ListDrawerSettings(DraggableItems = false,HideAddButton = true,
            HideRemoveButton = true,
            ElementColor = nameof(GetElementColor))]
        [InlineProperty]
        public List<ComponentEditorView> components = new List<ComponentEditorView>();

        public EcsWorld World => LeoEcsConvertersData.World;

        public bool IsAlive => World != null && World.IsAlive();
        
        public bool IsNameSameAsId => id.ToStringFromCache() == name;
        
        public bool HasGameObject => gameObject != null;

        public int Id => id;

        public string Name => name;

        [PropertyOrder(0)]
        [ResponsiveButtonGroup()]
        [GUIColor(nameof(_buttonColor))]
        [Button(ButtonSizes.Medium,Icon = SdfIconType.Broadcast)]
        public void ApplyAllChanges()
        {
            foreach (var component in components)
                component.ApplyChanges();
        }
        
        [ResponsiveButtonGroup()]
        [GUIColor(nameof(_buttonRedColor))]
        [Button(ButtonSizes.Medium,Icon = SdfIconType.Eraser)]
        public void DeleteEntity()
        {
            if (!IsAlive) return;
            World.DelEntity(id);
            isDead = true;
        }
        
        public void Release()
        {
            isDead = false;
            name = string.Empty;
            id = -1;
            gameObject = null;
            
            foreach (var editor in components)
            {
                editor.Release();
                editor.Despawn();
            }
            
            components.Clear();
        }

        public bool IsMatch(string searchString)
        {
            if (string.IsNullOrEmpty(searchString)) return true;
            var idValue = id.ToStringFromCache();
            
            if (idValue.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            foreach (var component in components)
            {
                if(component == null) continue;
                if (component.GetType().Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            
            return false;
        }
        
        public void Show()
        {
            EntityViewWindow.OpenPopupWindow(this);
        }
        
        public Color GetElementColor(int index, Color defaultColor)
        {
            return index % 2 == 0 ? _oddColor : defaultColor;
        }
    }
}