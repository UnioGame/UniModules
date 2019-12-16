namespace UniGreenModules.UniGameSystem.Runtime
{
    using Interfaces;
    using UniCore.Runtime.DataFlow.Interfaces;
    using UnityEngine;

    public class GameComponentService : MonoBehaviour, IGameService
    {
        protected GameService Service = new GameService();
        
        public void Dispose()
        {
            Service.Dispose();
        }

        public ILifeTime LifeTime => Service.LifeTime;

        private void OnDestroy()
        {
            Dispose();
        }
    }
}
