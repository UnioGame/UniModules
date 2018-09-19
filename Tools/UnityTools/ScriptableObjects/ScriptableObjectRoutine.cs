using System;
using UnityEngine;

public abstract class ScriptableObjectRoutine<TData,TResult> : 
	ScriptableObject ,IRoutine<TData, TResult>{
	
	[NonSerialized]
	private bool _initialized = false;
	
	public TResult Execute(TData data) {
		
		if (_initialized == false)
		{
			_initialized = true;
			OnInitialize();
		}

		return OnExecute(data);
		
	}
	
	
	#region private methods

	protected virtual void OnInitialize() {}

	protected abstract TResult OnExecute(TData data);

	#endregion


}
