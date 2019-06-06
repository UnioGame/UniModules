namespace UniGreenModules.UniNodeActors.Runtime.ActorData
{
	using System;
	using Interfaces;
	using Sirenix.OdinInspector;
	using UniModule.UnityTools.Interfaces;
	using UniRx;
	using UnityEngine;

	[Serializable]
	public abstract class TypedAsyncActorInfo<TModel> : AsyncActorModelInfo<TModel>
		where TModel : IActorModel
	{

		[SerializeField]
		protected TModel sourceModel;
		
        
	}
}