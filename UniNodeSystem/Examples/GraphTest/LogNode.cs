using System.Collections;
using UniModule.UnityTools.Interfaces;
using UniStateMachine;
using UnityEngine;

namespace Tests.GraphTest
{
	using UniGreenModules.UniCore.Runtime.Interfaces;
	using UniGreenModules.UniNodeSystem.Runtime;
	using UniGreenModules.UniNodeSystem.Runtime.Extensions;
	using UniGreenModules.UniNodeSystem.Runtime.Runtime;
	using UniTools.UniRoutine.Runtime.Extension;

	public class LogNode : UniNode
	{
		private const string LogMessageName = "log message";
		
		private UniPortValue _messageValue;
		
		#region inspector
		
		[SerializeField]
		private string message;

		[SerializeField]
		private float delay = 0f;
		
		#endregion
		
		protected override IEnumerator OnExecuteState(IContext context)
		{
			if(delay > 0)
				yield return this.WaitForSecond(delay);
			
			Debug.LogFormat("LOG: {0} at {1}",message,Time.realtimeSinceStartup);
			
			_messageValue.Add(message);
			
			yield return base.OnExecuteState(context);
			
		}
		
		protected override void OnUpdatePortsCache()
		{
			
			base.OnUpdatePortsCache();
			var portValue = this.UpdatePortValue(LogMessageName, PortIO.Output);
			_messageValue = portValue.value;
			
		}
	}
}
