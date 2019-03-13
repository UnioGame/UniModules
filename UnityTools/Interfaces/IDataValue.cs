using System;

namespace UniModule.UnityTools.Interfaces
{
	public interface IDataValue<TData> : IDisposable, IReadonlyDataValue<TData>
	{
		
		void SetValue(TData value);
		
	}
}

