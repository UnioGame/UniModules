using System.Collections;
using Assets.Tools.UnityTools.Extension;
using Assets.Tools.UnityTools.Interfaces;
using UniStateMachine;
using UnityEngine;

namespace Tests.GraphTest
{
	public class TestLogNode : UniNode
	{
		[SerializeField]
		private string _message;

		[SerializeField]
		private float _delay = 1f;
		
		protected override IEnumerator ExecuteState(IContext context)
		{

			yield return this.WaitForSecond(_delay);
			
			Debug.LogFormat("TestLogNode {0} at {1}",_message,Time.realtimeSinceStartup);
			
			yield return base.ExecuteState(context);
			
		}
		
	}
}
