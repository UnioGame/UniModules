namespace UniGreenModules.UniNodeActors.Runtime.ActorData
{
	using System;
	using Interfaces;
	using Sirenix.OdinInspector;
	using UniRx;

	[Serializable]
	public abstract class ActorInfo : 
		SerializedScriptableObject, 
		IActorInfo<IActorModel> 
	{

		private Subject<IActorModel> _valueStream = 
			new Subject<IActorModel>();
		
		public IActorModel Create()
        {
	        var model = CreateDataSource();
	        _valueStream.OnNext(model);
	        return model;
        }

        public IDisposable Subscribe(IObserver<IActorModel> observer)
        {
	        return _valueStream.Subscribe(observer);
        }
        
        protected abstract IActorModel CreateDataSource();

	}
}