using System;

namespace UniModule.UnityTools.Interfaces
{
	public interface IDataValue<TData> : IObservableDataValue<TData>
	{
		
		void SetValue(TData value);
		
	}
}

