namespace UniGame.LeoEcs.Debug.Editor
{
    using System;
    using System.Collections.Generic;
    using Core.Runtime.ObjectPool;
    using Runtime.ObjectPool.Extensions;
    using Sirenix.OdinInspector;
    using UnityEngine;

    [Serializable]
    public class EntityGridEditorView
    {
        [HideLabel]
        public List<IEntityEditorView> items = new List<IEntityEditorView>();
    }

    [Serializable]
    [InlineProperty]
    [HideLabel]
    public class EntityIdEditorView : IPoolable, IEntityEditorView
    {
        public string name;
        public int id;

        public int Id => id;

        public string Name => name;

        public void Release()
        {
            name = string.Empty;
            id = -1;
        }

        public void Show()
        {
            
        }
    }
}