using System;
using Assets.Tools.UnityTools.ActorEntityModel.Interfaces;
using UnityEngine;

namespace Assets.Tools.UnityTools.ActorEntityModel {

	public class EntityComponent : MonoBehaviour {

        [NonSerialized]
		private EntityObject _entity = new EntityObject();

		public IEntity Entity => _entity;

	}
}
