namespace UniModules.UniGame.RemoteData.MutableObject
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public abstract class BatchUpdater
    {
        public abstract Task PerformBatchUpdate(IEnumerable<RemoteDataChange> changes);
    }
}
