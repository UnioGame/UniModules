namespace UniGreenModules.UniNodes.CommonNodes.Camera
{
    using System.Collections;
    using UniCore.Runtime.Interfaces;
    using UniNodeSystem.Runtime;
    using UniNodeSystem.Runtime.Extensions;
    using UniNodeSystem.Runtime.Runtime;
    using UniRx;
    using UnityEngine;

    public class CameraNode : UniNode
    {
        #region inspector

        [SerializeField] private bool useMainCamera = false;

        [SerializeField] private Camera sourceCamera;

        #endregion

        private UniPortValue cameraPortValue;
        
        protected override IEnumerator OnExecuteState(IContext context)
        {
            var targetCamera = useMainCamera ? Camera.main : sourceCamera;
            if (!targetCamera)
                targetCamera = context.Get<Camera>();

            var lifeTime = LifeTime;
            var disposable = context.Receive<Camera>().Subscribe(OnCameraChanged);
            lifeTime.AddDispose(disposable);
            
            yield return base.OnExecute(context);

            if (targetCamera)
            {
                cameraPortValue.Add(targetCamera);
            }
            
        }

        protected override void OnRegisterPorts()
        {
            base.OnRegisterPorts();
            var pair = this.UpdatePortValue(nameof(cameraPortValue), PortIO.Output);
            cameraPortValue = pair.value;           
        }

        private void OnCameraChanged(Camera cameraItem)
        {
            cameraPortValue.Add(cameraItem);
        }
        
    }
}