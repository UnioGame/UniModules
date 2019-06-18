namespace UniGreenModules.UniUiSystem.Example.UiGraph
{
    using UniContextData.Runtime.Entities;
    using UniCore.Runtime.Interfaces;
    using UniNodeActors.Runtime.Actors;
    using UniNodeActors.Runtime.Interfaces;
    using UniNodeSystem.Nodes;
    using UniNodeSystem.Runtime;
    using UnityEngine;

    public class DemoLauncher : MonoBehaviour, IContextDataSource
    {

        public UniGraph Graph;

        private void Start()
        {
            var context = new EntityContext();
            var actor   = new Actor();
        
            actor.Initialize(this,Graph);
        }

        public void Register(IContext context)
        {
        
        }
    }
}
