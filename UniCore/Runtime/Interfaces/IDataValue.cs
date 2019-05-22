namespace UniGreenModules.UniCore.Runtime.Interfaces
{
	public interface IDataValue<TData> : IObservableDataValue<TData>
	{
		
		void SetValue(TData value);
		
	}
}

