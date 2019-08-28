using UnityEngine;

namespace UniGreenModules.UniNodeSystem.Components
{
    using System;
    using System.Collections;
    using Nodes;
    using UniTools.UniRoutine.Runtime;
    using UniTools.UniRoutine.Runtime.Extension;

    public class GraphLauncher : MonoBehaviour
    {

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
            
            yield return this.WaitForSecond(1);

            var inputs = graph.InputsPorts;

            foreach (var portNode in inputs) {
                portNode.PortValue.Publish($"Message {DateTime.Now.ToString()}");
            }
            
        }
    }
}
