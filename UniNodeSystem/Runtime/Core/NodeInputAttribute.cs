namespace UniGreenModules.UniNodeSystem.Runtime.Core
{
    using System;

    /// <summary> Mark a serializable field as an input port. You can access this through <see cref="GetInputPort(string)"/> </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class NodeInputAttribute : Attribute
    {
        public ShowBackingValue backingValue;
        public ConnectionType connectionType;
        public bool instancePortList;

        /// <summary> Mark a serializable field as an input port. You can access this through <see cref="GetInputPort(string)"/> </summary>
        /// <param name="backingValue">Should we display the backing value for this port as an editor field? </param>
        /// <param name="connectionType">Should we allow multiple connections? </param>
        public NodeInputAttribute(ShowBackingValue backingValue = ShowBackingValue.Unconnected,
            ConnectionType connectionType = ConnectionType.Multiple, bool instancePortList = false)
        {
            this.backingValue = backingValue;
            this.connectionType = connectionType;
            this.instancePortList = instancePortList;
        }
    }
    
}