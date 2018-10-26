using UnityEngine;

namespace Modules.UnityToolsModule.Tools.UnityTools.DebugUtils.FPSCounter
{
	public class FPSProfiler : MonoBehaviour
	{
		[Range(0f,1f)]
		[SerializeField]
		private float _transparency;
		[Range(0f,1f)]
		[SerializeField]
		private float _height;

		private RuntimeProfiler _runtimeProfiler;
	
		// Use this for initialization
		private void Start () {
		
			_runtimeProfiler = new RuntimeProfiler(_transparency,_height);
			StartCoroutine(_runtimeProfiler.StartFrameRateMonitor());

		}

	}
}
