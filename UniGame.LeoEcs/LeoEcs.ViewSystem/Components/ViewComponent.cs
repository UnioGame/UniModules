using System;
using UniGame.ViewSystem.Runtime;

namespace UniGame.LeoEcs.ViewSystem.Components
{
    using Converters;

    [Serializable]
    public struct ViewComponent
    {
        public IView View;
        public Type Type;
    }

    [Serializable]
    public struct ViewComponent<TView>
    {
        public TView View;
    }
}