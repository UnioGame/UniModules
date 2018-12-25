using System.Collections.Generic;

namespace Assets.Tools.UnityTools {
	
	public static class TypeUtils {

		public static uint GetJavaHash(string s) {

			uint hash = 0u;
			foreach (char c in s) {

				hash = 31u * hash + c;

			}
			return hash;

		}

		public static long[] GetTypeIds(this System.Type type) {

			var typeList = new List<long>();
			while (type != typeof(object)) {

				typeList.Add((long)GetJavaHash(type.FullName));
				type = type.BaseType;

			}

			return typeList.ToArray();

		}

	}
}
