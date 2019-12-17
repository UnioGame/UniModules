namespace UniGreenModules.UniNodeSystem.Runtime.Core
{
    using System;

    /// <summary> Mark a serializable field as an output port. You can access this through <see cref="GetOutputPort(string)"/> </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class NodeOutputAttribute : Attribute
    {
        public ShowBackingValue backingValue;
        public ConnectionType connectionType;
        public bool instancePortList;

        /// <summary> Mark a serializable field as an output port. You can access this through <see cref="GetOutputPort(string)"/> </summary>
        /// <param name="backingValue">Should we display the backing value for this port as an editor field? </param>
        /// <param name="connectionType">Should we allow multiple connections? </param>
        public NodeOutputAttribute(ShowBackingValue backingValue = ShowBackingValue.Never,
            ConnectionType connectionType = ConnectionType.Multiple, bool instancePortList = false)
        {
            this.backingValue = backingValue;
            this.connectionType = connectionType;
            this.instancePortList = instancePortList;
        }
    }
    
}