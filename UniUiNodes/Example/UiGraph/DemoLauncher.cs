namespace UniGreenModules.UniUiNodes.Example.UiGraph
{
    using UniActors.Runtime.Actors;
    using UniActors.Runtime.Interfaces;
    using UniContextData.Runtime.Entities;
    using UniCore.Runtime.Interfaces;
    using UniNodeSystem.Nodes;
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
