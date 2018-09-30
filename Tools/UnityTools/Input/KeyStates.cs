using System;
using UnityEngine;

namespace UnityToolsModule.Tools.UnityTools.Input {
	
	[Flags]
	public enum KeyStates {
    
		Down = 1<<1,
		Up= 1<<2,
		Pressed= 1<<3,
		StayUp= 1<<4,
    
	}
	
}
