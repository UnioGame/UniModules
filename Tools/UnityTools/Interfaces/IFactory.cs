namespace Assets.Scripts.Interfaces
{
    public interface IFactory<out TProduct>
    {
        TProduct Create();
    }
    
    public interface IFactory<in TContext,out TProduct>
    {
        TProduct Create(TContext data);
    }
}
