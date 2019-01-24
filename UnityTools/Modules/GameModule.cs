using System.Collections;
using Assets.Tools.UnityTools.StateMachine.ContextStateMachine;
using UniModule.UnityTools.Interfaces;

namespace UniModule.UnityTools.Modules {
	
	
	public class GameModule : ContextStateBehaviour, IGameModule 
	{
		
		protected override IEnumerator ExecuteState(IContext context)
		{
			
			yield break;
			
		}
		
	}


}
