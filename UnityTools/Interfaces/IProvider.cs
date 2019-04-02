namespace UnityTools.Interfaces
{
    public interface ISelector<TSource,TResult>
    {

        TResult Select(TSource source);

    }
}
