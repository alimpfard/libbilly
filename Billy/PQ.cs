﻿using System;
using System.Collections.Generic;

namespace Billy
{
	internal sealed class PQ<TK, TV> where TK : IComparable<TK>
	{
		private long _count = long.MinValue;
		private IndexedItem[] _items;
		private int _size;

		public PQ()
			: this(16)
		{
		}

		public PQ(int capacity)
		{
			_items = new IndexedItem[capacity];
			_size = 0;
		}

		private bool IsHigherPriority(int left, int right)
		{
			return _items[left].CompareTo(_items[right]) < 0;
		}

		private int Percolate(int index)
		{
			if (index >= _size || index < 0)
			{
				return index;
			}

			var parent = (index - 1) / 2;
			while (parent >= 0 && parent != index && IsHigherPriority(index, parent))
			{
				// swap index and parent
				var temp = _items[index];
				_items[index] = _items[parent];
				_items[parent] = temp;

				index = parent;
				parent = (index - 1) / 2;
			}

			return index;
		}

		private void Heapify(int index)
		{
			if (index >= _size || index < 0)
			{
				return;
			}

			while (true)
			{
				var left = 2 * index + 1;
				var right = 2 * index + 2;
				var first = index;

				if (left < _size && IsHigherPriority(left, first))
				{
					first = left;
				}

				if (right < _size && IsHigherPriority(right, first))
				{
					first = right;
				}

				if (first == index)
				{
					break;
				}

				// swap index and first
				var temp = _items[index];
				_items[index] = _items[first];
				_items[first] = temp;

				index = first;
			}
		}

		public int Count => _size;

		public TV Peek()
		{
			if (_size == 0)
			{
				throw new InvalidOperationException("Heap Empty");
			}

			return _items[0].Value;
		}

		private void RemoveAt(int index)
		{
			_items[index] = _items[--_size];
			// _items[_size];

			if (Percolate(index) == index)
			{
				Heapify(index);
			}

			if (_size < _items.Length / 4)
			{
				var temp = _items;
				_items = new IndexedItem[_items.Length / 2];
				Array.Copy(temp, 0, _items, 0, _size);
			}
		}

		public TV Dequeue()
		{
			var result = Peek();
			RemoveAt(0);
			return result;
		}

		public void Enqueue(TK priority, TV item)
		{
			if (_size >= _items.Length)
			{
				var temp = _items;
				_items = new IndexedItem[_items.Length * 2];
				Array.Copy(temp, _items, temp.Length);
			}

			var index = _size++;
			_items[index] = new IndexedItem { Value = item, Pro = priority, Id = ++_count };
			Percolate(index);
		}

		public bool Remove(TV item)
		{
			for (var i = 0; i < _size; ++i)
			{
				if (EqualityComparer<TV>.Default.Equals(_items[i].Value, item))
				{
					RemoveAt(i);
					return true;
				}
			}

			return false;
		}

		private struct IndexedItem : IComparable<IndexedItem>
		{
			public TV Value;
			public TK Pro;
			public long Id;

			public int CompareTo(IndexedItem other)
			{
				var c = Pro.CompareTo(other.Pro);
				if (c == 0)
				{
					c = Id.CompareTo(other.Id);
				}

				return c;
			}
		}
	}
}