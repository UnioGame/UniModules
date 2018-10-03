using UnityEngine;

namespace Assets.Tools.UnityTools.Camera
{
    public class CameraFollow : MonoBehaviour {

        public Vector3 Offset;
        public Transform Target;

        private void  Update ()
        {
            transform.position = Target.position + Offset;
        }

    }
}