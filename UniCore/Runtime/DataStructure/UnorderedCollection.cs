using System.Collections.Generic;
using UniModule.UnityTools.UniPool.Scripts;

namespace UniModule.UnityTools.DataStructure
{
	public class UnorderedCollection<T> : IPoolable
		where T:class
	{
		private int _count;
		private List<T> _items = new List<T>();
		private Queue<int> _unusedSlots = new Queue<int>();

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
			
			_count++;
			
			if (_unusedSlots.Count > 0)
			{
				index = _unusedSlots.Dequeue();
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
			
			_unusedSlots.Enqueue(itemId);
			_items[itemId] = null;
			_count--;
			
			return item;
		}

		public void Clear()
		{
			_items.Clear();
			_unusedSlots.Clear();
			_count = 0;
		}

		public void Release()
		{
			Clear();
		}
	}
}
