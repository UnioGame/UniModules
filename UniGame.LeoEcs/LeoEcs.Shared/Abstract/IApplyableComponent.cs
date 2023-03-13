namespace UniGame.LeoEcs.Shared.Abstract
{
    using System.Threading;

    public interface IApplyableComponent <TComponent>
        where TComponent : struct
    {
        /// <summary>
        /// Apply component data to parameter target
        /// </summary>
        /// <param name="component"></param>
        void Apply(ref TComponent component);
    }
}
