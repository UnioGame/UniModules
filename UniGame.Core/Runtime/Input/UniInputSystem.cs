namespace UniGreenModules.UniCore.Runtime.Input {
	using System;
	using System.Linq;

	public class UniInputSystem  {

		public static long[] KeyStatesValues = 
			Enum.GetValues(typeof(KeyStates)).
				OfType<KeyStates>().Select(x=> (long)x).ToArray();

		public static KeyStates[] KeyStatesItems = 
			Enum.GetValues(typeof(KeyStates)).OfType<KeyStates>().ToArray();
	
	}
}
