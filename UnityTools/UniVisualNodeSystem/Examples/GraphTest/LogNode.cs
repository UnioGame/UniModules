using System.Collections;
using Assets.Tools.UnityTools.Extension;
using Assets.Tools.UnityTools.Interfaces;
using UniStateMachine;
using UnityEngine;

namespace Tests.GraphTest
{
	public class LogNode : UniNode
	{
		[SerializeField]
		private string _message;

		[SerializeField]
		private float _delay = 0f;
		
		protected override IEnumerator ExecuteState(IContext context)
		{
			if(_delay > 0)
				yield return this.WaitForSecond(_delay);
			
			Debug.LogFormat("LOG: {0} at {1}",_message,Time.realtimeSinceStartup);
			
			yield return base.ExecuteState(context);
			
		}
		
	}
}
