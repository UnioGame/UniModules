namespace UnityTools.Common
{
    public interface IDataTransition<TFrom,TTo>
    {
        void Move(TFrom fromData, TTo data);

    }
}
