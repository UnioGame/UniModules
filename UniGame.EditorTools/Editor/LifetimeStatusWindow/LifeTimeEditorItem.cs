namespace UniModules.UniGame.EditorTools.Editor.LifetimeStatusWindow
{
    using System;
    using Core.Runtime.ScriptableObjects;
    using Sirenix.OdinInspector;
    using Object = UnityEngine.Object;

    [Serializable]
    [FoldoutGroup("Lifetime")]
    public class LifeTimeEditorItem
    {
        
        [InlineEditor(InlineEditorObjectFieldModes.Foldout)]
        public LifetimeScriptableObject LifeTime;

        [InlineProperty]
        public bool IsTerminated;

        [InlineProperty]
        public string Name { get; private set; }

        public LifeTimeEditorItem(LifetimeScriptableObject lifeTime)
        {
            LifeTime = lifeTime;
            Name     = LifeTime.name;
            LifeTime.AddCleanUpAction(() => IsTerminated = true);
            IsTerminated = LifeTime.LifeTime.IsTerminated;
        }
        
        [Button]
        public void Destroy()
        {
            if (LifeTime) {
                Object.DestroyImmediate(LifeTime);
            }
        }
        
    }
}