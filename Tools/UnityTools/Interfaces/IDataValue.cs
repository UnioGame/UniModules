using System;
using Assets.Tools.Utils;

namespace Modules.UnityToolsModule.Tools.UnityTools.Interfaces
{
	public interface IDataValue<TData> : IDisposable
	{
		TData Value { get; }
		void SetValue(TData value);
	}
}

