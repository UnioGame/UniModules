using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Tools.UnityTools.Common;
using Assets.Tools.UnityTools.Interfaces;
using Assets.Tools.UnityTools.StateMachine.Interfaces;
using Assets.Tools.UnityTools.StateMachine.UniStateMachine;
using Assets.Tools.UnityTools.UniRoutine;
using UnityEngine;

public class ActorComponent : MonoBehaviour, IDisposable {
		
	private IContext _context = new ContextData();
	private IDisposableItem _disposableItem;
	
	[SerializeField]
	private UniStateComponent _state;
	[SerializeField]
	private bool _launchOnStart = true;

	public IContextStateBehaviour<IEnumerator> State => _state;

	public IContext Context => _context;
	
	// Use this for initialization
	private void Start () {

		if (_launchOnStart == false)
			return;

		Launch();
	}

	public void Dispose() {
		Context?.Release();
		_disposableItem?.Dispose();
	}
	
	public void Launch() {
		
		AddContextData();
		if (State == null)
			return;
		
		var awaiter = _state.Execute(Context);
		_disposableItem = awaiter.RunWithSubRoutines();
		
	}

	protected virtual void AddContextData() {
		
	}

	private void OnDestroy() {
		
		Dispose();
		
	}

}
