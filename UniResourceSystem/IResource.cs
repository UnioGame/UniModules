using UnityEngine;
using UniUiSystem;

namespace UniModule.UnityTools.ResourceSystem
{
    public interface IResource : INamedItem
    {
        bool HasValue();

        T Load<T>()
            where T : Object;

        void Update(Object target);
        void Update();
    }
}