namespace UniGreenModules.UniCore.Runtime.Interfaces
{
    public interface IValueWriter<in TValue>
    {
        void SetValue(TValue value);
    }
}
