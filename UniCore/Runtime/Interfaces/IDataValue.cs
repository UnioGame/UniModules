namespace UniGreenModules.UniCore.Runtime.Interfaces
{
	using Rx;

	public interface IDataValue<TData> : IObservableDataValue<TData>,IValueWriter<TData>
	{

	}
}

