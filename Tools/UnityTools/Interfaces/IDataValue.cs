using Assets.Tools.Utils;

namespace Modules.UnityToolsModule.Tools.UnityTools.Interfaces
{
	public interface IDataValue<TData> : IPoolable
	{
		TData Value { get; }
		void SetValue(TData value);
	}
}

