using GBG.GameSystem.Runtime.Interfaces;
using UniGreenModules.UniCore.Runtime.DataFlow;

namespace GBG.GameSystem.Runtime
{
    using UnityEngine;

    public class GameComponentController : MonoBehaviour, IGameController
    {
        protected GameController controller = new GameController();
        
        public void Dispose()
        {
            controller.Dispose();
        }

        public ILifeTime LifeTime => controller.LifeTime;

        private void OnDestroy()
        {
            Dispose();
        }
    }
}
