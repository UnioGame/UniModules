namespace UniGame.LeoEcs.Debug.Editor
{
    using System.Collections.Generic;
    using Leopotam.EcsLite;

    public interface IEntityEditorViewBuilder
    {
        void Initialize(EcsWorld world);
        void Execute(List<EntityEditorView> views);
    }
}