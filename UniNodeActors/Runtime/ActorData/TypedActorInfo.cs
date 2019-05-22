namespace UniGreenModules.UniNodeActors.Runtime.ActorData
{
	using System;
	using Interfaces;
	using Sirenix.OdinInspector;
	using UniRx;
	using UnityEngine;

	[Serializable]
	public abstract class TypedActorInfo<TModel> : 
		SerializedScriptableObject, IActorInfo<TModel> 
		where TModel : IActorModel
	{

		[SerializeField]
		private TModel _sourceModel;
		
		private Subject<TModel> _valueStream = 
			new Subject<TModel>();
		
		public TModel Create()
        {
	        var model = CreateDataSource(_sourceModel);
	        _valueStream.OnNext(model);
	        return model;
        }

        public IDisposable Subscribe(IObserver<TModel> observer)
        {
	        return _valueStream.Subscribe(observer);
        }
        
        protected abstract TModel CreateDataSource(TModel source);

	}
}