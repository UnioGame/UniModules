namespace UniGreenModules.UniCore.Runtime.Camera
{
    using UnityEngine;

    public class CameraTargetDirectMotion : MonoBehaviour {


        public Transform _target;
	
        // Update is called once per frame
        private void Update () {
            if (!_target) return;
            transform.position = new Vector3(_target.position.x,_target.position.y,transform.position.z);
        }
    }
}
