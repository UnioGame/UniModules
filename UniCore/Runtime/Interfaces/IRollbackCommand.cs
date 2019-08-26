namespace UniGreenModules.UniCore.Runtime.Interfaces
{
    public interface IRollbackCommand : ICommand
    {
        bool Rollback();
    }
}