using System;
using UnityTools.ActorEntityModel;
using Assets.Tools.UnityTools.Interfaces;
using UnityEngine;

namespace UnityTools.ActorEntityModel {

	public class EntityComponent : MonoBehaviour {

        [NonSerialized]
		private EntityObject _context = new EntityObject();

		public IContext Context => _context;

	}
}
