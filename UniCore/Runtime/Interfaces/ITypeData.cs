namespace UniGreenModules.UniCore.Runtime.Interfaces
{
    public interface ITypeData : 
        IContextWriter,
        IDataValueParameters
    {
        
        TData Get<TData>();

        bool Contains<TData>();
        
    }
}