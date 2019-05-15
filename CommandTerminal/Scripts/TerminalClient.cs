using CommandTerminal;
using UnityEngine;

namespace _GetOverIt.Content._Programmers.Scripts.TerminalCommands
{
	public class TerminalClient : MonoBehaviour
	{
		private float _lastActivationTime;
		private float _touchPressedTime;
		
		private float _touchDuration = 4f;
		
		[SerializeField]
		private Terminal _terminal;
		[SerializeField]
		private int _touchCount = 5;

		private void Start()
		{

			if (Debug.isDebugBuild && _terminal) {
				
				_terminal.SetGuiButtonsState(true);
				
			}
			
		}
		
		// Update is called once per frame
		private void Update () 
		{
			if (Input.touchCount == _touchCount)
			{
				_touchPressedTime += Time.deltaTime;

				if (_touchPressedTime < _touchDuration)
					return;

				_touchPressedTime = 0;
				
				_terminal.ToggleState(TerminalState.OpenSmall);
			}
			else
			{
				_touchPressedTime = 0f;
			}
		}
	}
}
