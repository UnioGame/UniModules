using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStateSelector<TState>
{

	TState Select();

}
