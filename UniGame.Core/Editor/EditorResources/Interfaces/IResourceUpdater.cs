namespace UniGreenModules.UniResourceSystem.Runtime.Interfaces
{
    using UnityEngine;

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