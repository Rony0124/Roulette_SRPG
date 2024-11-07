using System.Collections.Generic;
using System.ComponentModel;

namespace TSoft.Utils
{
    public sealed class ObservableList<T> : List<T>
    {
        public event ListChangedEventHandler ListChanged;

        private void OnListChanged(ListChangedEventArgs e)
        {
            ListChanged?.Invoke(this, e);
        }
        
        public new void Add(T item)
        {
            base.Add(item);
            OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, this.Count - 1));
        }
        
        public new bool Remove(T item)
        {
            var index = IndexOf(item);
            var removed = base.Remove(item);

            if (removed && index >= 0)
            {
                OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, index));
            }
            return removed;
        }
    }
}
