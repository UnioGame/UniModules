namespace UniGreenModules.UniCore.Runtime.Interfaces
{
    public interface ISelector<TSource,TResult>
    {

        TResult Select(TSource source);

    }
}
