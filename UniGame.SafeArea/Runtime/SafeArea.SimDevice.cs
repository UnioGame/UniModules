namespace GBG.SafeArea.Runtime
{
     /// <summary>
    /// Simulation device that uses safe area due to a physical notch or software home bar. For use in Editor only.
    /// </summary>
    public enum SimDevice
    {
        /// <summary>
        /// Don't use a simulated safe area - GUI will be full screen as normal.
        /// </summary>
        None,
        /// <summary>
        /// Simulate the iPhone X and Xs (identical safe areas).
        /// </summary>
        iPhoneX,
        /// <summary>
        /// Simulate the iPhone Xs Max and XR (identical safe areas).
        /// </summary>
        iPhoneXsMax
        
    }
}