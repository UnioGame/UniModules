namespace UniGreenModules.UniCore.Runtime.DataFlow
{
    using Interfaces;
    using Runtime.Interfaces;
    using UniGame.Core.Runtime.DataFlow;

    public class LifeTimeDefinition : IUnique
    {
        private LifeTime lifeTime;
        private int id;
        
        public LifeTimeDefinition()
        {
            lifeTime = new LifeTime();
            id = Unique.GetId();
        }

        public bool IsTerminated => lifeTime.IsTerminated;

        public ILifeTime LifeTime => lifeTime;

        public int Id => id;
        
        public void Terminate()
        {
            lifeTime.Release();
        }

        public void Release()
        {
            lifeTime.Restart();
        }

    }
}
