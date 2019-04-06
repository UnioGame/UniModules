using System;
using UniModule.UnityTools.Interfaces;
using UnityEngine;

namespace UniModule.UnityTools.ActorEntityModel {

	public class EntityComponent : MonoBehaviour {

        [NonSerialized]
		private EntityObject _context = new EntityObject();

		public IContext Context => _context;

	}
}
