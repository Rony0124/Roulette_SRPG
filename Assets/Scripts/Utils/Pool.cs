using System.Collections.Generic;

namespace HF.Utils
{
    public class Pool<T> where T : new()
    {
        private HashSet<T> inUse = new HashSet<T>();
        private Stack<T> available = new Stack<T>();

        public T Create()
        {
            if (available.Count > 0)
            {
                T elem = available.Pop();
                inUse.Add(elem);
                return elem;
            }
            T newObj = new T();
            inUse.Add(newObj);
            return newObj;
        }

        public void Dispose(T elem)
        {
            inUse.Remove(elem);
            available.Push(elem);
        }

        public void DisposeAll()
        {
            foreach (T obj in inUse)
                available.Push(obj);
            inUse.Clear();
        }

        public void Clear()
        {
            inUse.Clear();
            available.Clear();
        }

        public HashSet<T> GetAllActive()
        {
            return inUse;
        }

        public int Count
        {
            get { return inUse.Count; }
        }

        public int CountAvailable
        {
            get { return available.Count; }
        }

        public int CountCapacity
        {
            get { return inUse.Count + available.Count; }
        }
    }
}
