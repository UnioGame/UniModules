namespace ConsoleGPlayAPITool
{
    public interface IPlayStorePublisher
    {
        /// <summary>
        /// publish your android artifact by configuration to your target branch
        /// </summary>
        void Publish(IAndroidDistributionSettings configs);
    }
}