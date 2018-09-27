using System.Collections;
using System.Collections.Generic;
using Modules.UnityToolsModule.Tools.UnityTools.Interfaces;
using UnityEngine;

namespace Tools.ActorModel
{
	public abstract class UniActorModel<TModel> : ScriptableObject, IFactory<TModel>
	{
	
		public abstract TModel Create();

	}
}