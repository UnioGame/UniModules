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
        
        long targetId = target.GetInstanceID();
        var sourceId = source.GetInstanceID();
        if (IsInstance(targetId, sourceId) || IsInstance(sourceId, targetId))
            return true;

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
        if (_idMap.TryGetValue(targetId, out id))
        {
            return id == sourceId;
        }

        return false;
    }
}
