namespace UniGreenModules.UniCore.Runtime.ObjectPool.Runtime
{
	using System;
	using Interfaces;
	using ProfilerTools;
	using UnityEngine;
	using Object = UnityEngine.Object;

	public static class ClassPool {

		private static IPoolContainer _container;

		private static IPoolContainer Container
		{
			get
			{
				if (_container == null )
				{
					_container = CreateContainer(false);
				}
				else if(Application.isPlaying && _container is DummyPoolContainer) {
					_container = CreateContainer(false);
				}

				return _container;
			}
		}

		public static TResult Spawn<TResult>(this object _, Action<TResult> onSpawn = null)
			where TResult : class, new()
		{
			return Spawn<TResult>(onSpawn);
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
			if (!Container.Contains<T>())
		        return null;
			// Get the matched index, or the last index
		    var item = Container.Pop<T>();

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

		public static void Despawn<T>(T instance)
			where T:class
		{
			if (instance == null)
				return;
			
			GameProfiler.BeginSample("ObjectPool.Despawn");

			if(instance is IPoolable poolable) 
				poolable.Release();

			// Add to _cache
			Container.Push(instance);
			
			GameProfiler.EndSample();
		}

		public static void Despawn<T>(ref T instance, T @default = null) where T : class
		{
			Despawn(instance);
			instance = @default;
		}

		private static readonly DummyPoolContainer dummyPoolContainer = new DummyPoolContainer();

		private static IPoolContainer CreateContainer(bool persistent)
		{
			if (!Application.isPlaying)
				return dummyPoolContainer;
				
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