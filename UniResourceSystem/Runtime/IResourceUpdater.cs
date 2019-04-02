using UnityEngine;

namespace UniModule.UnityTools.ResourceSystem
{
    public interface IResourceUpdater
    {
        /// <summary>
        /// update resource item by asset
        /// </summary>
        /// <param name="target"></param>
        void Update(Object target);

        /// <summary>
        /// update resource item
        /// </summary>
        void Update();
    }
}