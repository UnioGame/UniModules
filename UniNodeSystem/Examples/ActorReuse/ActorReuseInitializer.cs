using UnityEngine;

namespace UniGreenModules.UniNodeSystem.Examples.ActorReuse
{
    using System.Collections;
    using Nodes;
    using Runtime;
    using UniActors.Runtime;
    using UniActors.Runtime.Actors;
    using UniActors.Runtime.Interfaces;
    using UniContextData.Runtime.Entities;
    using UniCore.Runtime.Interfaces;

    public class ActorReuseInitializer : MonoBehaviour, IContextDataSource
    {
        private int counter = 4;
        private string tempMessage;
        
        public Actor actor = new Actor();

        public UniGraph Graph;
        
        private IEnumerator Start()
        {
            var i = 0;
            while (true)
            {            
                tempMessage = $"ActorReuseInitializer: ITEM {i++}";
                actor.Initialize(this,Graph);
                actor.Execute();
            
                yield return new WaitForSeconds(0.5f);
            
                actor.Stop();
            }

        }

        public void Register(IContext context)
        {
            context.Add(tempMessage);
        }
    }
}
