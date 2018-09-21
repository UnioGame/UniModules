using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInitializable<TContext>
{

	void Initialize(TContext context);
	
}
