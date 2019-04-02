namespace UniModule.UnityTools.Interfaces 
{
	public interface ISelector<TState> {

		TState Select();

	}
	
	public interface ISelector<TSource,TResult> {

		TResult Select(TSource value);

	}
}
