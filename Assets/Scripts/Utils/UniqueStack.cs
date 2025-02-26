using System;
using System.Collections.Generic;

namespace Utils
{
    public class StackNode<T>
    {
        public T Value;
        public StackNode<T> Next;

        public StackNode(T value)
        {
            Value = value;
            Next = null;
        }
    }
    
    public class UniqueStack<T>
    {
        private StackNode<T> _top;
        private int _count;
        private HashSet<T> _set;
        
        public int Count => _count;
        public bool IsEmpty => _top == null;
        
        public UniqueStack()
        {
            _set = new HashSet<T>();
        }
        
        public void Push(T item)
        {
            if (_set.Contains(item))
            {
                Remove(item);
            }
            
            StackNode<T> newNode = new StackNode<T>(item);
            newNode.Next = _top;
            _top = newNode;

            _set.Add(item);
            _count++;
        }

        public T Pop()
        {
            if (IsEmpty)
                throw new InvalidOperationException("Stack is empty.");

            T value = _top.Value;
            _top = _top.Next;

            _set.Remove(value);
            _count--;

            return value;
        }

        public T Peek()
        {
            if (IsEmpty)
                throw new InvalidOperationException("Stack is empty.");

            return _top.Value;
        }

        private void Remove(T item)
        {
            StackNode<T> prev = null;
            StackNode<T> current = _top;

            while (current != null)
            {
                if (EqualityComparer<T>.Default.Equals(current.Value, item))
                {
                    if (prev == null)
                    {
                        _top = current.Next;
                    }
                    else
                    {
                        prev.Next = current.Next;
                    }

                    _set.Remove(item);
                    _count--;
                    return;
                }

                prev = current;
                current = current.Next;
            }
        }
    }
}
