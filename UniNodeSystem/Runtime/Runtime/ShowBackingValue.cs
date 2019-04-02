namespace UniNodeSystem
{
    /// <summary> Used by <see cref="InputAttribute"/> and <see cref="OutputAttribute"/> to determine when to display the field value associated with a <see cref="NodePort"/> </summary>
    public enum ShowBackingValue
    {
        /// <summary> Never show the backing value </summary>
        Never,

        /// <summary> Show the backing value only when the port does not have any active connections </summary>
        Unconnected,

        /// <summary> Always show the backing value </summary>
        Always
    }
    
}