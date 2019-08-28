namespace GBG.GameSystem.Runtime
{
    using System.Collections.Generic;
    using Interfaces;
    using UniGreenModules.UniGameSystem.Runtime.Interfaces;

    public class GameStateComponent : GameComponentController
    {
        private List<IBinder> binders = new List<IBinder>();

        protected void AddBinder(IBinder binder)
        {
            binder.Bind(LifeTime);
            binders.Add(binder);
        }

        private void Awake()
        {
            LifeTime.AddCleanUpAction(() => binders.Clear());
        }
    }
}
