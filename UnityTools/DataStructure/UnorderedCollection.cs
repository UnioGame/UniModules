using System.Collections.Generic;

namespace UniModule.UnityTools.DataStructure
{
	public class UnorderedUniqueCollection<T> 
		where T:class
	{
		private List<T> _items = new List<T>();
		private Queue<int> _unusedSlots = new Queue<int>();
		private Dictionary<T, int> _indexes = new Dictionary<T, int>();

		public int Count => _indexes.Count;

		public IReadOnlyCollection<T> Items => _indexes.Keys;

		public bool Add(T item)
		{
			if (_indexes.ContainsKey(item))
				return false;
			
			var index = _items.Count;
			
			if (_unusedSlots.Count > 0)
			{
				index = _unusedSlots.Dequeue();
				_items[index] = item;
			}

			_indexes[item] = index;
			
			return true;
		}

		public bool Remove(T item)
		{
			
			if (_indexes.TryGetValue(item, out var index))
			{
				_indexes.Remove(item);
				_items[index] = null;
				_unusedSlots.Enqueue(index);
			}

			return false;

		}

		public void Clear()
		{
			
			_items.Clear();
			_unusedSlots.Clear();
			_indexes.Clear();
			
		}

		public void AddRange(IEnumerable<T> items)
		{
			foreach (var item in items)
			{
				Add(item);
			}
		}

	}
}
