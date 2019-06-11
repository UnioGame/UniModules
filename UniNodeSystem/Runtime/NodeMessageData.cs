namespace UniGreenModules.UniNodeSystem.Runtime
{
    using Interfaces;

    public struct NodeMessageData
    {
        public string Name;
        public IPortValue Input;
        public IPortValue Output;
    }
}