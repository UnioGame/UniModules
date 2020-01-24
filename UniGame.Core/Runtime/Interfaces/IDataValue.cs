namespace UniGreenModules.UniCore.Runtime.Interfaces
{
	using Rx;

	public interface IDataValue<TData> : 
		IObservableValue<TData>,
		IValueWriter<TData>
	{

	}
}

