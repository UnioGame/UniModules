namespace UniGreenModules.UniCore.Runtime.DataFlow
{
    public class LifeTimeDefinition
    {
        private LifeTime _lifeTime;

        public LifeTimeDefinition()
        {
            _lifeTime = new LifeTime();
        }

        public void Terminate()
        {
            _lifeTime.Release();
        }

        public void Release()
        {
            _lifeTime.Restart();
        }

        public bool IsTerminated => _lifeTime.IsTerminated;

        public ILifeTime LifeTime => _lifeTime;

    }
}
