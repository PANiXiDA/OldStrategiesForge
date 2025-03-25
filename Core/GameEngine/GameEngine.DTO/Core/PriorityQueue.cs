using System;
using System.Collections.Generic;

namespace GameEngine.DTO.Core
{
    public class PriorityQueue<TElement, TPriority>
    {
        private List<(TElement Element, TPriority Priority)> _heap;
        private readonly IComparer<TPriority> _comparer;

        public PriorityQueue() : this(0, null)
        {
        }

        public PriorityQueue(int initialCapacity, IComparer<TPriority> comparer = null)
        {
            _heap = (initialCapacity > 0)
                ? new List<(TElement, TPriority)>(initialCapacity)
                : new List<(TElement, TPriority)>();
            _comparer = comparer ?? Comparer<TPriority>.Default;
        }

        public int Count => _heap.Count;

        public void Enqueue(TElement element, TPriority priority)
        {
            _heap.Add((element, priority));
            HeapifyUp(_heap.Count - 1);
        }

        public TElement Dequeue()
        {
            if (_heap.Count == 0)
            {
                throw new InvalidOperationException("Очередь пуста.");
            }

            TElement result = _heap[0].Element;
            int lastIndex = _heap.Count - 1;
            _heap[0] = _heap[lastIndex];
            _heap.RemoveAt(lastIndex);

            if (_heap.Count > 0)
            {
                HeapifyDown(0);
            }

            return result;
        }

        public TElement Peek()
        {
            if (_heap.Count == 0)
            {
                throw new InvalidOperationException("Очередь пуста.");
            }

            return _heap[0].Element;
        }

        public void Clear()
        {
            _heap.Clear();
        }

        public void EnqueueRange(IEnumerable<(TElement Element, TPriority Priority)> items)
        {
            foreach (var item in items)
            {
                Enqueue(item.Element, item.Priority);
            }
        }

        public TElement EnqueueDequeue(TElement element, TPriority priority)
        {
            if (_heap.Count > 0 && _comparer.Compare(priority, _heap[0].Priority) > 0)
            {
                TElement result = _heap[0].Element;
                _heap[0] = (element, priority);
                HeapifyDown(0);
                return result;
            }
            else
            {
                return element;
            }
        }

        private void HeapifyUp(int index)
        {
            int childIndex = index;
            while (childIndex > 0)
            {
                int parentIndex = (childIndex - 1) / 2;
                if (_comparer.Compare(_heap[childIndex].Priority, _heap[parentIndex].Priority) >= 0)
                {
                    break;
                }
                Swap(childIndex, parentIndex);
                childIndex = parentIndex;
            }
        }

        private void HeapifyDown(int index)
        {
            int parentIndex = index;
            int lastIndex = _heap.Count - 1;
            while (true)
            {
                int leftChildIndex = 2 * parentIndex + 1;
                if (leftChildIndex > lastIndex)
                {
                    break;
                }

                int rightChildIndex = leftChildIndex + 1;
                int smallestChildIndex = leftChildIndex;

                if (rightChildIndex <= lastIndex &&
                    _comparer.Compare(_heap[rightChildIndex].Priority, _heap[leftChildIndex].Priority) < 0)
                {
                    smallestChildIndex = rightChildIndex;
                }

                if (_comparer.Compare(_heap[parentIndex].Priority, _heap[smallestChildIndex].Priority) <= 0)
                {
                    break;
                }

                Swap(parentIndex, smallestChildIndex);
                parentIndex = smallestChildIndex;
            }
        }

        private void Swap(int i, int j)
        {
            var temp = _heap[i];
            _heap[i] = _heap[j];
            _heap[j] = temp;
        }
    }
}
