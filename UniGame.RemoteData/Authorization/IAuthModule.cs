namespace UniModules.UniGame.RemoteData.Authorization
{
    using System.Threading.Tasks;

    public interface IAuthModule
    {
        // TO DO other Auth Type
        // Init with providers for different networks

        string CurrentUserId { get; }
        bool IsLogged { get; }

        Task Init();
        Task Login(IAuthToken authType);
        Task Logout();
    }
}