namespace UniGreenModules.UniCore.Runtime.Physics
{
    using System.Collections.Generic;
    using UniRx;
    using UnityEngine;

    [RequireComponent(typeof(UnityEngine.Collider))]
    public class SensorObject : MonoBehaviour, ISensorObject
    {
        protected BoolReactiveProperty _triggerConnectionChanged = new BoolReactiveProperty(false);
        protected BoolReactiveProperty _collisionConnectionChanged = new BoolReactiveProperty(false);

        
        protected Dictionary<Transform, Collision> collisions =
            new Dictionary<Transform, Collision>();
        protected Dictionary<Transform, UnityEngine.Collider> triggerColliders =
            new Dictionary<Transform, UnityEngine.Collider>();

        #region inspector data

        [SerializeField]
        protected LayerMask collisionMask;

        [SerializeField]
        protected bool disableOnStayCollisions;

        [SerializeField]
        protected UnityEngine.Collider _collider;
    
        #endregion

        #region public properties

        public IReadOnlyCollection<Collision> CollisionData => collisions.Values;

        public IReadOnlyCollection<UnityEngine.Collider> TriggersData => triggerColliders.Values;

        public UnityEngine.Collider LastTriggerObject { get; protected set; }

        public Collision LastCollisionObject { get; protected set; }

        public UnityEngine.Collider Collider => _collider;

        public LayerMask CollisionMask => collisionMask;

        public Vector3 Position => transform.position;

        public Transform Transform => transform;

        public IReadOnlyReactiveProperty<bool> TriggerConnectionChanged => _triggerConnectionChanged;

        public IReadOnlyReactiveProperty<bool> CollisionConnectionChanged => _collisionConnectionChanged;
        
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

        void OnTriggerExit(UnityEngine.Collider other)
        {
            RemoveTriggerCollision(other);
        }

        void OnTriggerStay(UnityEngine.Collider other)
        {
            if (disableOnStayCollisions)
                return;
            UpdateTriggerCollision(other);
        }

        private void OnTriggerEnter(UnityEngine.Collider other)
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

            _collisionConnectionChanged.Value = collisions.Count > 0;
        }

        private void RemoveTriggerCollision(UnityEngine.Collider collider)
        {
            if (LastTriggerObject != null
                && LastTriggerObject.transform == collider.transform)
            {
                LastTriggerObject = null;
            }
            triggerColliders.Remove(collider.transform);
            
            _triggerConnectionChanged.Value = triggerColliders.Count > 0;
        }

        private void UpdateTriggerCollision(UnityEngine.Collider collision)
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
