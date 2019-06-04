using UniGreenModules.UniContextData.Runtime.Entities;
using UniGreenModules.UniCore.Runtime.Interfaces;
using UniGreenModules.UniNodeActors.Runtime.Actors;
using UniGreenModules.UniNodeActors.Runtime.Interfaces;
using UniStateMachine.Nodes;
using UnityEngine;

namespace UniGreenModules.UniUiSystem.Example
{
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
