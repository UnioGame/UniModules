namespace Examples.SimpleSystem.Runtime
{
    using System.Collections;
    using UniGreenModules.UniContextData.Runtime.Entities;
    using UniGreenModules.UniCore.Runtime.Interfaces;
    using UniGreenModules.UniNodeSystem.Nodes;
    using UniGreenModules.UniRoutine.Runtime;
    using UniGreenModules.UniRoutine.Runtime.Extension;
    using UnityEngine;

    public class SimpleSystemLauncher : MonoBehaviour
    {
        private EntityContext context = new EntityContext();
        
        public float startDelay = 1;
        
        public UniGraph graph;

        private void OnEnable()
        {
            graph?.Execute();
            Progress().Execute();
        }

        private void OnDisable()
        {
            graph?.Exit();
        }
        
        private IEnumerator Progress()
        {
            
            yield return this.WaitForSecond(startDelay);

            var inputs = graph.InputsPorts;

            foreach (var portNode in inputs) {
                portNode.PortValue.Publish<IContext>(context);
            }
            
        }
    }
}
