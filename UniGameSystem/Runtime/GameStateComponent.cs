namespace UniGreenModules.UniGameSystem.Runtime
{
    using System.Collections.Generic;
    using Interfaces;

    public class GameStateComponent : GameComponentService
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
