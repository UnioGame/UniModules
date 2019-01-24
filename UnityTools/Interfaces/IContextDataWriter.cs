namespace UniModule.UnityTools.Interfaces
{
    public interface IContextDataWriter<TContext>
    {
        void UpdateValue<TData>(TContext context, TData value);
    }
}