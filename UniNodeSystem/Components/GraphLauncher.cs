using UnityEngine;

namespace UniGreenModules.UniNodeSystem.Components
{
    using System;
    using System.Collections;
    using Nodes;
    using UniRoutine.Runtime;
    using UniRoutine.Runtime.Extension;

    public class GraphLauncher : MonoBehaviour
    {

        public float startDelay = 1;
        
        public UniGraph graph;

        private void OnEnable()
        {
            graph?.Execute();
            
            this.ExecuteWhile(Progress,() => isActiveAndEnabled).ExecuteRoutine();
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
                portNode.PortValue.Publish($"Message {DateTime.Now.ToString()}");
            }
            
        }
    }
}
