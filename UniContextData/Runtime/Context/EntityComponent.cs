namespace UniGreenModules.UniContextData.Runtime.Entities
{
    using System;
    using UniCore.Runtime.Interfaces;
    using UnityEngine;

    public class EntityComponent : MonoBehaviour
    {
        [NonSerialized] private EntityContext _context = new EntityContext();

        public IContext Context => _context;
    }
}