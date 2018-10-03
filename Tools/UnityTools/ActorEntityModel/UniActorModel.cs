using Assets.Tools.UnityTools.Interfaces;
using UnityEngine;

namespace Assets.Tools.UnityTools.ActorEntityModel
{
	public abstract class UniActorModel<TModel> : ScriptableObject, IFactory<TModel>
	{
	
		public abstract TModel Create();

	}
}