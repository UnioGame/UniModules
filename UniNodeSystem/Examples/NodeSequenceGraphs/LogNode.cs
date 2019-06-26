namespace UniGreenModules.UniNodeSystem.Examples.SubGraphs
{
	using System.Collections;
	using Runtime;
	using Runtime.Extensions;
	using Runtime.Runtime;
	using UniCore.Runtime.Interfaces;
	using UnityEngine;
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
			
			yield return base.OnExecute(context);
			
		}
		
		protected override void OnRegisterPorts()
		{
			
			base.OnRegisterPorts();
			var portValue = this.UpdatePortValue(LogMessageName, PortIO.Output);
			_messageValue = portValue.value;
			
		}
	}
}
