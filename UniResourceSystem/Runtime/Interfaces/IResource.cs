namespace UniGreenModules.UniResourceSystem.Runtime.Interfaces
{
    using UniCore.Runtime.Interfaces;
    using UnityEngine;

    /// <summary>
    /// resource item wrapper
    /// </summary>
    public interface IResource : INamedItem, IResourceUpdater
    {
        /// <summary>
        /// is value assigned
        /// </summary>
        /// <returns></returns>
        bool HasValue();

        /// <summary>
        /// load resource with type T
        /// </summary>
        T Load<T>() where T : Object;
    }
}