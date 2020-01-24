using System.Threading.Tasks;

namespace UniGreenModules.UniCore.Runtime.Interfaces
{
    public interface IAsyncFactory<TResult>
    {

        Task<TResult> Create();

    }
}
