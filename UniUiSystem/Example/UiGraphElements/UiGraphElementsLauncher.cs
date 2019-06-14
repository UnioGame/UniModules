using UnityEngine;

namespace UniGreenModules.UniUiSystem.Example.UiGraphElements
{
    using UniCore.Runtime.Interfaces;
    using UniNodeActors.Runtime.Actors;
    using UniNodeActors.Runtime.Interfaces;
    using UniNodeSystem.Runtime;

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
