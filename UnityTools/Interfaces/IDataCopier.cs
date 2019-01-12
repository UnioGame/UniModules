namespace UnityTools.Interfaces
{
    public interface IDataCopier<in TTarget>
    {
        void CopyTo(TTarget target);
    }
}
