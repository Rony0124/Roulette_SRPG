using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TSoft.Utils
{
    public class ObservableVar<T> : IDisposable
    {
        public delegate void OnValueChangedDelegate(T oldVal, T newVal);

        public OnValueChangedDelegate OnValueChanged;
        
        [SerializeField]
        private protected T internalValue;

        private protected T previousValue;


        private bool hasPreviousValue;
        private bool isDisposed;
        private bool isDirty;

        public bool IsDirty => isDirty;
        
        public virtual T Value
        {
            get => internalValue;
            set
            {
                if (ReferenceEquals(internalValue, value))
                {
                    return;
                }

                Set(value);
                isDisposed = false;
            }
        }
        
        internal ref T RefValue()
        {
            return ref internalValue;
        }
        
        private protected void Set(T value)
        {
            isDirty = true;
            T preValue = internalValue;
            internalValue = value;
            OnValueChanged?.Invoke(preValue, internalValue);
        }

        public void Dispose()
        {
            if (isDisposed)
            {
                return;
            }

            isDisposed = true;
            if (internalValue is IDisposable internalValueDisposable)
            {
                internalValueDisposable.Dispose();
            }

            internalValue = default;
            if (hasPreviousValue && previousValue is IDisposable previousValueDisposable)
            {
                hasPreviousValue = false;
                previousValueDisposable.Dispose();
            }

            previousValue = default;
        }
        
        ~ObservableVar()
        {
            Dispose();
        }
    }
}
