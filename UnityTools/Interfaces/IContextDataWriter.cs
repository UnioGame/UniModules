namespace Assets.Tools.UnityTools.Interfaces
{
    public interface IContextDataWriter<TContext>
    {
        void UpdateValue<TData>(TContext context, TData value);
    }
}