namespace UniGreenModules.UniNodeActors.Runtime.Interfaces
{
    using UniCore.Runtime.DataFlow;
    using UniCore.Runtime.Interfaces;
    using UniModule.UnityTools.Interfaces;

    public interface IActor : IActivatableObject
    {
        /// <summary>
        /// Actor context data
        /// </summary>
        IContext Context { get; }

        /// <summary>
        /// Actor life time object
        /// </summary>
        ILifeTime LifeTime { get; }
    }
}