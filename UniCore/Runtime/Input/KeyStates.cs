namespace UniGreenModules.UniCore.Runtime.Input {
	using System;

	[Flags]
	public enum KeyStates {
    
		Down = 1<<0,
		Up= 1<<1,
		Pressed = 1<<2,
		StayUp = 1<<3,
    
	}
	
}
