using System.Collections;
using UniModule.UnityTools.Interfaces;
using UniModule.UnityTools.UniStateMachine.ContextStateMachine;

namespace UniModule.UnityTools.Modules {
	
	
	public class GameModule : ContextStateBehaviour, IGameModule 
	{
		
		protected override IEnumerator ExecuteState(IContext context)
		{
			
			yield break;
			
		}
		
	}


}
