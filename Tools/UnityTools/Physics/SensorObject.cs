using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Assets.Tools.UnityTools.Physics
{
    public class SensorObject : MonoBehaviour, ISensorObject
    {

        protected Dictionary<Transform, Collision> collisions =
            new Dictionary<Transform, Collision>();
        protected Dictionary<Transform, Collider> triggerColliders =
            new Dictionary<Transform, Collider>();

        #region inspector data

        [SerializeField]
        protected LayerMask collisionMask;

        [SerializeField]
        protected LayerMask raycastsMask;

        [SerializeField]
        protected bool disableOnStayCollisions;

        [SerializeField]
        protected Collider _collider;
    
        #endregion

        #region public properties

        public IReadOnlyDictionary<Transform, Collision> CollisionData => collisions;

        public IReadOnlyDictionary<Transform, Collider> TriggersData => triggerColliders;

        public Collider LastTriggerObject { get; protected set; }

        public Collision LastCollisionObject { get; protected set; }

        public Collider Collider => _collider;

        public LayerMask CollisionMask => collisionMask;

        public LayerMask RaycastMask => raycastsMask;

        public Vector3 Position => transform.position;

        public Transform Transform => transform;
        
        #endregion

        #region public methods

        public void SetCollisionMask(int mask)
        {
            collisionMask.value = mask;
        }

        public void SetCollisionMask(string[] mask)
        {
            collisionMask = LayerMask.GetMask(mask);
        }

        public void Reset()
        {
            collisions.Clear();
            triggerColliders.Clear();
            LastCollisionObject = null;
            LastTriggerObject = null;
        }

        #endregion

        #region collision methods

        private void OnCollisionEnter(Collision collision)
        {
            UpdateCollision(collision);
        }

        private void OnCollisionStay(Collision collision)
        {
            if (disableOnStayCollisions)
                return;
            UpdateCollision(collision);
        }

        private void OnCollisionExit(Collision collision)
        {
            RemoveCollision(collision);
        }

        void OnTriggerExit(Collider other)
        {
            RemoveTriggerCollision(other);
        }

        void OnTriggerStay(Collider other)
        {
            if (disableOnStayCollisions)
                return;
            UpdateTriggerCollision(other);
        }

        private void OnTriggerEnter(Collider other)
        {
            UpdateTriggerCollision(other);
        }

        #endregion

        private void UpdateCollision(Collision collision)
        {
            if (!ValidateCollision(collision.gameObject.layer))
                return;
            LastCollisionObject = collision;
            collisions[collision.transform] = collision;
        }

        private void RemoveCollision(Collision collision)
        {
            if (LastCollisionObject != null &&
                LastCollisionObject.transform == collision.transform)
            {
                LastCollisionObject = null;
            }

            collisions.Remove(collision.transform);
        }

        private void RemoveTriggerCollision(Collider collider)
        {
            if (LastTriggerObject != null
                && LastTriggerObject.transform == collider.transform)
            {
                LastTriggerObject = null;
            }
            triggerColliders.Remove(collider.transform);
        }

        private void UpdateTriggerCollision(Collider collision)
        {
            if (!ValidateCollision(collision.gameObject.layer))
                return;
            LastTriggerObject = collision;
            triggerColliders[collision.transform] = collision;
        }

        private bool ValidateCollision(int layer)
        {
            var mask = collisionMask.value;
            var isValidLayer = (mask | (1 << layer)) == mask;
            return isValidLayer;
        }
    }
}
