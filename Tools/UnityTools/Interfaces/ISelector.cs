using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Tools.StateMachine 
{
	public interface ISelector<TState> {

		TState Select();

	}
}
