namespace UniGreenModules.UniCore.Runtime.Camera
{
    using UnityEngine;

    public class CameraFollow : MonoBehaviour {

        public Vector3 Offset;
        public Transform Target;

        private void  Update ()
        {
            transform.position = Target.position + Offset;
        }

    }
}