using System.Collections.Generic;
using System;
/// <summary>
/// by nafio 优先级队列
/// </summary>
namespace n.tools {

    public class PriorityQueue<T>
    {
        int capacity;
        public int Count { get; private set; }
        IComparer<T> comparer;
        public T[] heap;

        public PriorityQueue(int capacity, IComparer<T> comparer)
        {
            this.capacity = capacity;
            this.comparer = comparer;
            this.heap = new T[capacity];
        }

        public void Push(T v)
        {
            if (Count >= heap.Length)
            {
                int _size = Count << 1;
                capacity = _size;
                Array.Resize(ref heap, _size);
            }

            heap[Count] = v;
            Up(Count++);//注意这里是传入的count，然后才使count+1

        }

        public T Pop()
        {
            var v = Top();
            heap[0] = heap[--Count];
            if (Count > 0) Down(0);
            return v;
        }

        public T Top()
        {
            if (Count > 0) return heap[0];
            throw new InvalidOperationException("PriorityQueue null");
        }

        public void Up(int n)
        {
            var v = heap[n];
            for (var n2 = (n >> 1); n > 0 && comparer.Compare(v, heap[n2]) < 0; n = n2, n2 = (n2 >> 1))
            {
                heap[n] = heap[n2];
            }
            heap[n] = v;
        }

        public void Down(int n)
        {
            var v = heap[n];
            for (var n2 = (n << 1); n2 < Count; n = n2, n2 = (n2 << 1))
            {
                if (n2 + 1 < Count && comparer.Compare(heap[n2 + 1], heap[n2]) < 0)
                {
                    n2++;
                }
                if (comparer.Compare(v, heap[n2]) <= 0) break;
                heap[n] = heap[n2];
            }
            heap[n] = v;
        }

        public void Clear()
        {
            Array.Clear(heap, 0, this.Count);
            this.Count = 0;
        }
    }
}

