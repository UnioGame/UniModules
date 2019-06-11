namespace UniGreenModules.UniNodeActors.Runtime.ActorData
{
    using System.Threading.Tasks;
    using Interfaces;

    public abstract class AsyncActorModelInfo<TModel>  : 
        AsyncActorInfo<TModel>
        where TModel : IActorModel 
    {

        public override async Task<TModel> Create()
        {
            var task = CreateDataSource();
            valueStream.OnNext(task);
            var model = await task;
            return model;
        }

    }
}
