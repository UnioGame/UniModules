using System;
using UniModule.UnityTools.Interfaces;
using UniRx;

namespace UniModule.UnityTools.Modules
{
	/// <summary>
	/// test module system version
	/// </summary>
	public class GameModulesSystem
	{
	
		private IMessageBroker _messageBroker;

		public GameModulesSystem(IMessageBroker modulesMessageBroker)
		{
			_messageBroker = modulesMessageBroker;
		}
		
		public IDisposable Register(IContext moduleContext)
	    {
	        return null;
	    }
		
	
	}
}
