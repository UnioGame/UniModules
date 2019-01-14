using System;

namespace Assets.Tools.UnityTools.Interfaces
{
	public interface IDataValue<TData> : IDisposable, IReadonlyDataValue<TData>
	{
		void SetValue(TData value);
		
	}
}

