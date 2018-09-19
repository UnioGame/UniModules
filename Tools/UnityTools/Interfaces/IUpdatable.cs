namespace Assets.Scripts.Common
{
    public interface IUpdatable
    {
        void Update(float delta);
    }
    
    public interface IUpdatable<TData>
    {
        void Update(TData data,float delta);
    }
}
