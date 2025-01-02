using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TSoft.Utils
{
    public sealed class ObservableVar<T> : IDisposable
    {
        public delegate void OnValueChangedDelegate(T oldVal, T newVal);

        public OnValueChangedDelegate OnValueChanged;
        
        [SerializeField]
        private T internalValue;

        private T previousValue;


        private bool hasPreviousValue;
        private bool isDisposed;
        private bool isDirty;

        public bool IsDirty => isDirty;
        
        public T Value
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
        
        private void Set(T value)
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
