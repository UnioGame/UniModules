namespace UniGreenModules.AssetBundleManager.Editor.DependencyViewer
{
    using System.Collections.Generic;
    using System.IO;
    using UnityEditor;
    using UnityEngine;

    public enum GraphEdgeType {

        Direction,
        Line,

    }

    public class BundlesGraphExporter {
        
        private string _graphTemplate = "digraph {{ \n{0}\n }}";
        private string _directionNode = "{0} -> {1}";
        private string _lineNode = "{0} -- {1}";

        public void Export(List<DependencyViewerModel> models) {

            var path = EditorUtility.SaveFilePanel("Save Graph", Application.persistentDataPath, "graph", "gv");
            if (string.IsNullOrEmpty(path)) return;

            var edges = CreateEdges(models);
            var body = string.Join(";\n", edges.ToArray());
            var graphValue = CreateGraph(body);

            Debug.Log(graphValue);
            File.WriteAllText(path, graphValue);
        }

        private List<string> CreateEdges(List<DependencyViewerModel> models) {

            var edges = new List<string>();
            foreach (var model in models) {
                foreach (var dependency in model.Dependencies) {
                    var edge = CreateEdge(model.BundleName, dependency, GraphEdgeType.Direction);
                    edges.Add(edge);
                }
            }

            return edges;
        }

        private string CreateEdge(string from, string to, GraphEdgeType edgeType) {
            switch (edgeType) {

                case GraphEdgeType.Direction:
                    return CreateNode(_directionNode,from,to);
                case GraphEdgeType.Line:
                    return CreateNode(_lineNode, from, to);
                default:
                    return CreateNode(_lineNode, from, to);
            }
        }

        private string CreateGraph(string body) {
            return string.Format(_graphTemplate, body);
        }

        private string CreateNode(string template, string left, string right) {
            return string.Format(template, left, right);
        }
    }
}
