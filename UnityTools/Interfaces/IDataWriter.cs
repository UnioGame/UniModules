using Assets.Tools.UnityTools.Interfaces;

namespace UnityTools.Common
{
    public interface IDataWriter
    {
        
        void Add<TValue>(TValue value);

    }

}
