namespace Assets.Tools.UnityTools.Common
{
    public interface IContextData
    {
        TData Get<TData>();
        bool Remove<TData>();
        void Add<TData>(TData data);
    }
}