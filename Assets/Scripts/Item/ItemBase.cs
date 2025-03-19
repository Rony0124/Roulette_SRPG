using System;
using TSoft.Data;
using TSoft.Utils;
using UnityEngine;

namespace TSoft.Item
{
    public abstract class ItemBase<T>
    {
        public ObservableVar<T> ItemValue = new();
        
        public ItemInfo info;

        protected ItemBase(ItemInfo info)
        {
            this.info = info;
        }

        protected ItemBase(ItemInfo info, T itemValue) : this(info)
        {
            ItemValue.Value = itemValue;
        }

        public abstract void SetValue(T value);
    }
}
