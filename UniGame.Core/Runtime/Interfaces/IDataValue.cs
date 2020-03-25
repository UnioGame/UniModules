namespace UniGreenModules.UniCore.Runtime.Interfaces
{
	using Rx;

	public interface IDataValue<TData,TApi> : 
		IObservableValue<TApi>,
		IValueWriter<TData>
	{

	}
	
	public interface IDataValue<TData> : IDataValue<TData,TData>
	{

	}
}

