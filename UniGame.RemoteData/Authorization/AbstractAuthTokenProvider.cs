namespace UniModules.UniGame.RemoteData.Authorization
{
    using System.Threading.Tasks;

    public abstract class AbstractAuthTokenProvider<T> where T : class, IAuthToken
    {
        public abstract Task<T> FetchToken();
    }
}
