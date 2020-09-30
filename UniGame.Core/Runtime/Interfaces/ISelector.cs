namespace UniGreenModules.UniCore.Runtime.Interfaces 
{
	using System.Collections.Generic;

	public interface ISelector<TState> {

		TState Select();

	}
	
	public interface ICollectionSelector<T>
	{
		T Select(IEnumerable<T> collection);
	}

}
