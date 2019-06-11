namespace UniStateMachine.CommonNodes.Camera
{
    using System.Collections;
    using UniGreenModules.UniCore.Runtime.Interfaces;
    using UniGreenModules.UniNodeSystem.Runtime;
    using UniGreenModules.UniNodeSystem.Runtime.Extensions;
    using UniGreenModules.UniNodeSystem.Runtime.Runtime;
    using UniModule.UnityTools.Interfaces;
    using UniStateMachine;
    using UnityEngine;
    using UniRx;
    
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
            
            yield return base.OnExecuteState(context);

            if (targetCamera)
            {
                cameraPortValue.Add(targetCamera);
            }
            
        }

        protected override void OnUpdatePortsCache()
        {
            base.OnUpdatePortsCache();
            var pair = this.UpdatePortValue(nameof(cameraPortValue), PortIO.Output);
            cameraPortValue = pair.value;           
        }

        private void OnCameraChanged(Camera cameraItem)
        {
            cameraPortValue.Add(cameraItem);
        }
        
    }
}