namespace UniGreenModules.UniCore.Runtime.Utils {
	using System.Collections.Generic;

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
			var objectType = typeof(object);
			while (type != objectType) {

				typeList.Add(type.GetTypeId());
				type = type.BaseType;

			}

			return typeList.ToArray();

		}

		public static long GetTypeId(this System.Type type)
		{
			return (long) GetJavaHash(type.FullName);
		}

	}
}
