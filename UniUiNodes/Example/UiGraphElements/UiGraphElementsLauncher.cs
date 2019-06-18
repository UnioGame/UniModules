namespace UniGreenModules.UniUiNodes.Example.UiGraphElements
{
    using UniActors.Runtime.Actors;
    using UniActors.Runtime.Interfaces;
    using UniCore.Runtime.Interfaces;
    using UniNodeSystem.Runtime;
    using UnityEngine;

    public class UiGraphElementsLauncher : MonoBehaviour, IContextDataSource
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
