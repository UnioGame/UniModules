using System;
using Modules.UnityToolsModule.Tools.UnityTools.Interfaces;
using UnityEngine;

public abstract class ScriptableObjectRoutine<TResult> : 
	ScriptableObject ,IRoutine<TResult>{
	
	[NonSerialized]
	private bool _initialized = false;
	
	public TResult Execute() {
		
		if (_initialized == false)
		{
			_initialized = true;
			OnInitialize();
		}

		return OnExecute();
		
	}
	
	
	#region private methods

	protected virtual void OnInitialize() {}

	protected abstract TResult OnExecute();

	#endregion


}
