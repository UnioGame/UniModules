namespace UniGreenModules.UniNodeActors.Runtime.Interfaces
{
    using UniModule.UnityTools.Interfaces;

    public interface IActor : IBehaviourObject
    {
        IContext Context { get; }
    }
}