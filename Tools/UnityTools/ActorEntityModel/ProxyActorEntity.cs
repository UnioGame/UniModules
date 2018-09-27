using System;

namespace Tools.ActorModel
{
	public class ProxyActorEntity : ActorEntity
	{
		private Action _onActivate;
		private Action _onDeactivate;
		private Action _onDispose;

		public ProxyActorEntity(Action onActivate, Action onDeactivate, Action onDispose)
		{
			_onActivate = onActivate;
			_onDeactivate = onDeactivate;
			_onDispose = onDispose;
		}
		
		
		
		protected override void Activate()
		{
			_onActivate?.Invoke();
			base.Activate();
		}

		protected override void Deactivate()
		{
			_onDeactivate?.Invoke();
			base.Deactivate();
		}

		protected override void OnDispose()
		{
			_onDispose?.Invoke();
			
			_onDispose = null;
			_onActivate = null;
			_onDeactivate = null;
			
			base.OnDispose();
		}
	}
}
