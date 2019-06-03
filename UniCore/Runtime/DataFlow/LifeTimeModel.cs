namespace UniGreenModules.UniCore.Runtime.DataFlow
{
    using Interfaces;
    using ObjectPool;
    using ObjectPool.Interfaces;

    public class LifeTimeModel : ILifeTimeContext, IDespawnable
    {
        
        private LifeTimeDefinition lifeTimeDefinition = new LifeTimeDefinition();

        public ILifeTime LifeTime => lifeTimeDefinition.LifeTime;

        public void MakeDespawn()
        {
            lifeTimeDefinition.Release();
            OnDespawn();
            this.Despawn();
        }

        protected virtual void OnDespawn(){}
    }
}
