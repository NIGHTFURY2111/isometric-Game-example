using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue<T> where T : IHeapItem<T>
{
    private List<T> items = new List<T>();

    public int Count => items.Count;

    public void Enqueue(T item)
    {
        item.HeapIndex = items.Count;
        items.Add(item);
        HeapifyUp(item.HeapIndex);
    }

    public T Dequeue()
    {
        if (Count == 0) throw new InvalidOperationException("The priority queue is empty.");

        T firstItem = items[0];
        T lastItem = items[Count - 1];
        items[0] = lastItem;
        lastItem.HeapIndex = 0;
        items.RemoveAt(Count - 1);

        if (Count > 0)
        {
            HeapifyDown(0);
        }

        return firstItem;
    }

    public void UpdateItem(T item)
    {
        HeapifyUp(item.HeapIndex);
    }

    public bool Contains(T item)
    {
        return item.HeapIndex < items.Count;
    }

    private void HeapifyUp(int index)
    {
        T item = items[index];
        while (index > 0)
        {
            int parentIndex = (index - 1) / 2;
            T parent = items[parentIndex];
            if (item.CompareTo(parent) >= 0) break;

            items[index] = parent;
            parent.HeapIndex = index;
            index = parentIndex;
        }

        items[index] = item;
        item.HeapIndex = index;
    }

    private void HeapifyDown(int index)
    {
        T item = items[index];
        int length = items.Count;
        int halfLength = length / 2;

        while (index < halfLength)
        {
            int childIndex = 2 * index + 1;
            T child = items[childIndex];
            int rightIndex = childIndex + 1;

            if (rightIndex < length && items[rightIndex].CompareTo(child) < 0)
            {
                childIndex = rightIndex;
                child = items[childIndex];
            }

            if (item.CompareTo(child) <= 0) break;

            items[index] = child;
            child.HeapIndex = index;
            index = childIndex;
        }

        items[index] = item;
        item.HeapIndex = index;
    }
}

public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex { get; set; }
}