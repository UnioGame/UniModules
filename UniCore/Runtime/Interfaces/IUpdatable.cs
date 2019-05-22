namespace UniGreenModules.UniCore.Runtime.Interfaces
{
    public interface IUpdatable
    {
        void Update(float delta);
    }
    
    public interface IUpdatable<TData>
    {
        void Update(TData data);
    }
    
    
}
