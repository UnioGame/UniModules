namespace UniGreenModules.UniCore.Runtime.ObjectPool
{
	using System;
	using Interfaces;
	using ProfilerTools;
	using UnityEngine;
	using Object = UnityEngine.Object;

	public static class ClassPool {

		private static WeakReference<IPoolContainer> _persistentContainer = new WeakReference<IPoolContainer>(null);
		private static WeakReference<IPoolContainer> _container = new WeakReference<IPoolContainer>(null);

		private static IPoolContainer Container
		{
			get
			{
				if (!_container.TryGetTarget(out var container) )
				{
					container = CreateContainer(false);
					_container.SetTarget(container);
				}

				return container;
			}
		}

		public static TResult Spawn<TResult>(Action<TResult> onSpawn = null)
            where TResult : class, new()
        {
            var item = SpawnExists(onSpawn);
	        
	        if (item == null)
	        {
		        return new TResult();
	        }
            
	        return item;
        }

        public static TResult SpawnOrCreate<TResult>(Func<TResult> factory,Action<TResult> onSpawn = null)
            where TResult : class
        {
            var item = SpawnExists(onSpawn);
            	        
            if (item == null && factory!=null)
                item = factory();
	        
            return item;
        }

		public static T SpawnExists<T>()
			where T : class 
		{
            GameProfiler.BeginSample("LeanClassPool.Spawn");
			var instance = SpawnExists<T>(null);
		    GameProfiler.EndSample();
            return instance;
        }

		public static T SpawnExists<T>(Action<T> onSpawn)
			where T : class 
		{
			var type = typeof(T);
			
		    if (!Container.Contains(type))
		        return null;
			// Get the matched index, or the last index
		    var item = Container.Pop(type) as T;
		    // Run action?
			onSpawn?.Invoke(item);
			
			return item;

		}

		
		public static void Despawn<T>(T instance, Action<T> onDespawn)
			where T : class 
		{
			
			// Run action on it?
			onDespawn?.Invoke(instance);
			
			Despawn(instance);

		}

		public static void Despawn(object instance)
		{
			
			if (instance == null)
				return;
			
			GameProfiler.BeginSample("ObjectPool.Despawn");
			
			if(instance is IPoolable poolable) 
				poolable.Release();

			var type = instance.GetType();
			// Add to _cache
			Container.Push(type,instance);
			
			GameProfiler.EndSample();
			
		}


		private static IPoolContainer CreateContainer(bool persistent)
		{

			if (!Application.isPlaying)
				return new DummyPoolContainer();
				
			var container = new GameObject(persistent ? "ClassPool(Immortal)" : "ClassPool")
				.AddComponent<ClassPoolContainer>();

			if (persistent)
			{
				Object.DontDestroyOnLoad(container.gameObject);
			}
			
			return container;
			
		}
	}
}