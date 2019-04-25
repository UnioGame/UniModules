using UnityEngine;

namespace UniModule.UnityTools.Physics
{
    public class Rigidbody2DVelocityLimiter : MonoBehaviour
    {

        private float _sqrMaxValocity;
        public float _sqrMinVelocity;

        public float MinVelocity;

        public float MaxVelocity;

        public bool UseMaxValocity;

        public bool UseMinValocity;

        public Rigidbody2D Rigidbody2D;

        public void FixedUpdate()
        {

            if (UseMaxValocity && Rigidbody2D.velocity.sqrMagnitude > _sqrMaxValocity)
            {
                Rigidbody2D.velocity = Rigidbody2D.velocity.normalized * MaxVelocity;
            }

            if (UseMinValocity && Rigidbody2D.velocity.sqrMagnitude < _sqrMinVelocity)
            {
                Rigidbody2D.velocity = Rigidbody2D.velocity.normalized * MinVelocity;
            }

        }

        private void Awake()
        {

            Rigidbody2D = Rigidbody2D == null ? GetComponent<Rigidbody2D>() : Rigidbody2D;
            _sqrMinVelocity = MinVelocity * MinVelocity;
            _sqrMaxValocity = MaxVelocity * MaxVelocity;
        }

    }
}
