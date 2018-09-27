using System.Collections;
using System.Collections.Generic;
using Tools.ActorModel;
using UnityEngine;

public class UniActorEntity : MonoBehaviour, IEntity
{
	private ProxyActorEntity _actorEntity;

	public int Id { get; }

	public TData GetContext<TData>() where TData : class
	{
		return _actorEntity.GetContext<TData>();
	}

	public void SetState(bool state)
	{
		_actorEntity.SetState(state);
	}

	public void AddContext<TData>(TData data)
	{
		_actorEntity.AddContext<TData>(data);
	}


	#region private methods


	protected virtual void Awake()
	{
		
		_actorEntity = new ProxyActorEntity(Activate,Deactivate,OnDispose);
		
	}

	protected virtual void Start()
	{
		
	}

	protected virtual void Activate()
	{
	}

	protected virtual void Deactivate()
	{
            
	}

	protected virtual void OnDispose()
	{
	}
	
	public virtual void Dispose()
	{
		_actorEntity.Dispose();	
	}

	#endregion


}
