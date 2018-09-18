namespace Assets.Scripts.Interfaces
{
    public interface IFactory<out TProduct>
    {
        TProduct Create();
    }
}
