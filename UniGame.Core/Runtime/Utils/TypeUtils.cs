namespace UniGreenModules.UniCore.Runtime.Utils {
	using System.Collections.Generic;

	public static class TypeUtils {

		public static uint GetJavaHash(string s) {

			var hash = 0u;
			for (var i = 0; i < s.Length; i++) {
				var c = s[i];
				hash = 31u * hash + c;
			}

			return hash;

		}

		public static List<long> GetTypeIds(this System.Type type) {

			var typeList = new List<long>();
			var objectType = typeof(object);
			while (type != objectType) {
				typeList.Add(type.GetTypeId());
				type = type.BaseType;
			}

			return typeList;

		}
		
		public static List<string> GetTypeNames(this System.Type type) {

			var typeList   = new List<string>();
			var objectType = typeof(object);
			while (type != objectType) {
				typeList.Add(type.AssemblyQualifiedName);
				type = type.BaseType;
			}

			return typeList;

		}
		

		public static long GetTypeId(this System.Type type)
		{
			return GetJavaHash(type.FullName);
		}

	}
}
