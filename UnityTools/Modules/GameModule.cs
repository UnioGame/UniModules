using System.Collections;
using Assets.Tools.UnityTools.Interfaces;
using Assets.Tools.UnityTools.StateMachine.ContextStateMachine;

namespace Assets.Tools.UnityTools.Moduls {
	
	
	public class GameModule : ContextStateBehaviour, IGameModule 
	{
		
		protected override IEnumerator ExecuteState(IContext context)
		{
			
			yield break;
			
		}
		
	}


}
