namespace UniModules.UniGame.RemoteData.MutableObject
{
    public interface IRemoteChangesStorage
    {
        void AddChange(RemoteDataChange change);
        bool IsRootLoaded();
    }
}
