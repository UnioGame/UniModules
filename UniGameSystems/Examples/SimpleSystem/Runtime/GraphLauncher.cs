namespace UniGreenModules.UniGameSystems.Examples.SimpleSystem
{
    using UniNodeSystem.Nodes;
    using UnityEngine;

    public class GraphLauncher : MonoBehaviour
    {
        public UniGraph graph;

        private void OnEnable()
        {
            graph?.Execute();
        }

        private void OnDisable()
        {
            graph?.Exit();
        }

    }
}
