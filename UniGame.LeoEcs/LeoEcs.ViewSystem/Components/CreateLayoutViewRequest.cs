namespace UniGame.LeoEcs.ViewSystem.Components
{
    using System;
    using Leopotam.EcsLite;
    using UniModules.UniGame.UiSystem.Runtime;

    [Serializable]
    public struct CreateLayoutViewRequest : IEcsAutoReset<CreateLayoutViewRequest>
    {
        public Type Type;
        public ViewType LayoutType;

        public void AutoReset(ref CreateLayoutViewRequest c)
        {
            c.Type = null;
            c.LayoutType = ViewType.Window;
        }
    }
}