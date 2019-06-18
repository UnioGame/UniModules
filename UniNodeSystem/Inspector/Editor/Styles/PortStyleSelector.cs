namespace UniGreenModules.UniNodeSystem.Inspector.Editor.Styles
{
    using BaseEditor;
    using Runtime;
    using Runtime.Runtime;
    using UnityEngine;

    public class PortStyleSelector : IStyleProvider
    {
    
        public virtual NodeGuiLayoutStyle Select(NodePort port)
        {
            var portStyle = NodeEditorGUILayout.GetDefaultPortStyle(port);
            var uniNode = port.node as UniNode;
            
            if (!uniNode) return portStyle;

            var portValue = uniNode.GetPortValue(port.fieldName);
            var hasData = portValue != null && portValue.HasValue();

            if (port.fieldName == UniNode.OutputPortName || port.fieldName == UniNode.InputPortName)
            {
                portStyle.Background = Color.blue;
                portStyle.Color = hasData ? Color.red : Color.white;
                return portStyle;
            }

            if (port.IsDynamic)
            {
                portStyle.Name = port.fieldName;
                portStyle.Background = Color.red;
                portStyle.Color = port.direction == PortIO.Input ? hasData ? new Color(128, 128, 0) : Color.green :
                    hasData ? new Color(128, 128, 0) : Color.blue;
            }

            return portStyle;
        }
 
    }
}
