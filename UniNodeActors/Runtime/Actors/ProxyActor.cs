namespace UniGreenModules.UniNodeActors.Runtime.Actors
{
	using System;

	public class ProxyActor : Actor
	{
		private Action _onActivate;
		private Action _onDeactivate;
		private Action _onDispose;

		public ProxyActor(Action onActivate, Action onDeactivate, Action onDispose)
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

		protected override void OnReleased()
		{
			_onDispose?.Invoke();
			
			_onDispose = null;
			_onActivate = null;
			_onDeactivate = null;
			
			base.OnReleased();
		}
	}
}
