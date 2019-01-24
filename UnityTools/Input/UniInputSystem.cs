using System;
using System.Linq;

namespace UniModule.UnityTools.Input {
	
	public class UniInputSystem  {

		public static long[] KeyStatesValues = 
			Enum.GetValues(typeof(KeyStates)).
				OfType<KeyStates>().Select(x=> (long)x).ToArray();

		public static KeyStates[] KeyStatesItems = 
			Enum.GetValues(typeof(KeyStates)).OfType<KeyStates>().ToArray();
	
	}
}
