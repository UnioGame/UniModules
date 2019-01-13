using System;

namespace Assets.Tools.UnityTools.Interfaces
{
	public interface IDataValue<TData> : IDisposable
	{
		TData Value { get; }
		void SetValue(TData value);
		
	}
}

