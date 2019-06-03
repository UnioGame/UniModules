namespace UniGreenModules.UniCore.Runtime.DataFlow
{
    using Interfaces;
    using ObjectPool;
    using ObjectPool.Interfaces;

    public class LifeTimeModel : ILifeTimeContext, IDespawnable
    {
        
        private LifeTimeDefinition lifeTimeDefinition = new LifeTimeDefinition();

        public ILifeTime LifeTime => lifeTimeDefinition.LifeTime;

        /// <summary>
        /// despawn model with cleanup
        /// </summary>
        public void MakeDespawn()
        {
            Release();
            this.Despawn();
        }

        /// <summary>
        /// cleanup item without despawn
        /// </summary>
        protected void Release()
        {
            lifeTimeDefinition.Release();
            OnCleanUp();
        }
        
        /// <summary>
        /// custom cleanup action
        /// </summary>
        protected virtual void OnCleanUp(){}
    }
}
