using System.Collections.Generic;
using UnityEngine;

public static class AssetsInstanceMap {

    private static Dictionary<long, long> _idMap = new Dictionary<long, long>();

    public static void Register(Object instance,Object origin) {
        if (!instance || !origin) return;
        _idMap[instance.GetInstanceID()] = origin.GetInstanceID();
    }

    public static bool IsInstance(this Object target, Object source) {

        if (!target || !source) return false;
        if (target == source) return true;
        
		if (AssetBundleManager.Instance.IsSumulateMode == true) {
			
			long targetId = target.GetInstanceID();
			var sourceId = source.GetInstanceID();
			var isInstance = IsInstance(targetId, sourceId);
			return isInstance;

		}

		return false;

    }

    public static bool IsInstanceOfAny<T>(this T instance, T[] sources) where T : Object
    {
        for (int i = 0; i < sources.Length; i++)
        {
            var item = sources[i];
            if (instance.IsInstance(item))
                return true;
        }

        return false;
    }

    public static bool IsInstanceOfAny<T>(this T instance, IList<T> sources) where T : Object
    {
        for (int i = 0; i < sources.Count; i++)
        {
            var item = sources[i];
            if (instance.IsInstance(item))
                return true;
        }

        return false;
    }

    private static bool IsInstance(long targetId, long sourceId) {
        long id;
        if (_idMap.TryGetValue(targetId, out id)) {
            if (id == sourceId)
                return true;
        }

        long sourceCloneId;
        if (_idMap.TryGetValue(sourceId, out sourceCloneId)) {
            if (targetId == sourceCloneId)
                return true;
        }

        return id == sourceCloneId || targetId == sourceId;
    }
}
