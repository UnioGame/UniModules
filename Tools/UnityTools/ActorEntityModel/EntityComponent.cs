using System.Collections;
using System.Collections.Generic;
using Tools.ActorModel;
using UnityEngine;

namespace Tools.ActorModel {

	public class EntityComponent : MonoBehaviour {

		private EntityObject _entity = new EntityObject();

		public IEntity Entity => _entity;

	}
}
