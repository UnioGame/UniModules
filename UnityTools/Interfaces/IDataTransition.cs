using Assets.Tools.UnityTools.Interfaces;

namespace UnityTools.Common
{
    public interface IDataTransition
    {
        
        void Move<TValue>(TValue value);

    }

}
