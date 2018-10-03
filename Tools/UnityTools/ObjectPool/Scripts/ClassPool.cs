using System;
using System.Collections.Generic;
using Assets.Tools.UnityTools.ProfilerTools;

namespace Assets.Tools.UnityTools.ObjectPool.Scripts
{
    public static class ClassPool {

        public static TResult Spawn<TResult>(Action<TResult> onSpawn = null)
            where TResult : class, new()
        {
            var item = ClassPool<TResult>.Spawn(onSpawn);
            if (item == null) return new TResult();
            return item;
        }

        public static TResult GetItem<TResult>(Func<TResult> factory,Action<TResult> onSpawn = null)
            where TResult : class
        {
            var item = ClassPool<TResult>.Spawn(onSpawn);
            if (item == null && factory!=null)
                item = factory();
            return item;
        }

    }
    // This class allows you to pool normal C# classes, for example:
    // var foo = Lean.LeanClassPool<Foo>.Spawn() ?? new Foo();
    // Lean.LeanClassPool<Foo>.Despawn(foo);
    public static class ClassPool<T>
		where T : class
	{
		private static Stack<T> _cache = new Stack<T>();
		
		public static T Spawn()
		{
            GameProfiler.BeginSample("LeanClassPool.Spawn");
			var instance = Spawn(null, null);
		    GameProfiler.EndSample();
            return instance;
		}

        public static T Spawn(System.Action<T> onSpawn)
		{
			return Spawn(null, onSpawn);
		}


        public static T Spawn(System.Predicate<T> match)
		{
			return Spawn(match, null);
		}
		
		// This will either return a pooled class instance, or null
		// You can also specify a match for the exact class instance you're looking for
		// You can also specify an action to run on the class instance (e.g. if you need to reset it)
		// NOTE: Because it can return null, you should use it like this: Lean.LeanClassPool<Whatever>.Spawn(...) ?? new Whatever(...)
		public static T Spawn(System.Predicate<T> match, System.Action<T> onSpawn) {
		    if (_cache.Count == 0)
		        return null;
			// Get the matched index, or the last index
		    var item = _cache.Pop();
            // Was one found?
            if (item == null) return null;
		    // Run action?
		    if (onSpawn != null)
		    {
		        onSpawn(item);
		    }
		    return item;

		}
		
		public static void Despawn(T instance)
		{
		    GameProfiler.BeginSample("LeanClassPool.Despawn");
            Despawn(instance, null);
		    GameProfiler.EndSample();
		}


		
		// This allows you to desapwn a class instance
		// You can also specify an action to run on the class instance (e.g. if you need to reset it)
		public static void Despawn(T instance, System.Action<T> onDespawn)
		{
			// Does it exist?
			if (instance != null)
			{
				// Run action on it?
                if(onDespawn!=null)
    			    onDespawn.Invoke(instance);

			    var poolable = instance as IPoolable;

                if(poolable != null) poolable.Release();

			    // Add to _cache
				_cache.Push(instance);
			}
		}
	}
}