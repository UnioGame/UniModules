using System;
using Assets.Tools.UnityTools.Interfaces;
using UniRx;

namespace Modules.UnityToolsModule.Tools.UnityTools.Modules
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
