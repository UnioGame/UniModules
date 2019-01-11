namespace UnityTools.Common
{
    public interface IDataTransition
    {

        void CopyTo(IDataTransition transition);

        void Add<TData>(TData data);

    }
}
