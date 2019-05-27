namespace UniGreenModules.UniCore.Runtime.DataStructure
{
	using System.Collections.Generic;
	using System.Threading;
	using ObjectPool.Interfaces;

	public class UnorderedCollection<T> : IUnorderedCollection<T> where T:class
	{
		private int _count;
		private List<T> _items = new List<T>();
		private Stack<int> _unusedSlots = new Stack<int>();

		public int Count => _count;

		public IEnumerable<T> GetItems()
		{
			for (int i = 0; i < _items.Count; i++)
			{
				var item = _items[i];
				if(item == null)
					continue;
				yield return item;
			}
		}
		
		/// <summary>
		/// add item to collection and return unique id
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public int Add(T item)
		{
			var index = _items.Count;
			
			Interlocked.Increment(ref _count);
			
			if (_unusedSlots.Count > 0)
			{
				index = _unusedSlots.Pop();
				_items[index] = item;
				return index;
			}
			
			_items.Add(item);

			return index;
		}

		public T Remove(int itemId)
		{
			var item = _items[itemId];
			if (item == null)
				return null;
			
			_unusedSlots.Push(itemId);
			_items[itemId] = null;

			Interlocked.Decrement(ref _count);
			
			return item;
		}

		public void Clear()
		{
			_items.Clear();
			_unusedSlots.Clear();
			
			Interlocked.Exchange(ref _count, 0);
		}

		public void Release()
		{
			Clear();
		}
	}
}
