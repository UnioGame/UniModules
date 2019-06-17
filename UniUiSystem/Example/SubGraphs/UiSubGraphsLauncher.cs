namespace UniGreenModules.UniUiSystem.Example.SubGraphs
{
    using UniCore.Runtime.Interfaces;
    using UniNodeActors.Runtime.Actors;
    using UniNodeActors.Runtime.Interfaces;
    using UniNodeSystem.Runtime;
    using UnityEngine;

    public class UiSubGraphsLauncher : MonoBehaviour, IContextDataSource
    {
        [SerializeField]
        private UniNode behaviour;
        
        private Actor actor = new Actor();
    
        // Start is called before the first frame update
        private void Start()
        {
            actor.Initialize(this,behaviour);
            actor.Execute();
        }

        public void Register(IContext context)
        {
            
        }
    }
}
