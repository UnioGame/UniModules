using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Tools.UnityTools.ActorEntityModel;
using Assets.Tools.UnityTools.Common;
using Assets.Tools.UnityTools.Interfaces;
using Assets.Tools.UnityTools.StateMachine.Interfaces;
using Assets.Tools.UnityTools.StateMachine.UniStateMachine;
using Assets.Tools.UnityTools.UniRoutine;
using UnityEngine;

public class ActorComponent : EntityComponent, IDisposable {
		
	private IDisposableItem _disposableItem;
	private bool _isActive;
	
	[SerializeField]
	private UniStateComponent _state;
	[SerializeField]
	private UniStateBehaviour _stateBehaviour;
	[SerializeField]
	private bool _launchOnStart = true;

	public IContextStateBehaviour<IEnumerator> State { get; protected set; }
    
	public Actor Actor { get; protected set; }

	
	// Use this for initialization
	private void Start () {

		if (_launchOnStart == false)
			return;

		SetState(true);
	}

	public void Dispose() {
		SetState(false);
		Entity?.Release();
	}

	public void SetState(bool state) {

		if (state == _isActive) {
			return;
		}

		_isActive = state;
		
		Actor.SetState(_isActive);
		
		if (_isActive) {
			Activate();
		}
		else {
			Deactivate();
		}

	}

	protected virtual void AddContextData() {
		
	}

	protected virtual void Activate() {
		
	}
	
	protected virtual void Deactivate() 
	{
		_disposableItem?.Dispose();
	}

	private void OnDisable() {
		SetState(false);
	}

	private void OnEnable() {
		SetState(true);
	}

	private void OnDestroy() {
		Dispose();
	}

	private void Awake() {
		
		Actor = new Actor();
		AddContextData();
		Actor.SetEntity(Entity);

		State = _state != null ? (IContextStateBehaviour<IEnumerator>)_state : _stateBehaviour;
		
		if (State == null)
			return;
		Actor.SetBehaviour(State);
	}
}
