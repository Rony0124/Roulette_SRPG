using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace TSoft.UI.Views.InGame
{
    public class TextObjectPool : MonoBehaviour
    {
        [SerializeField] private int poolCount;
        [SerializeField] private GameObject poolObj;

        private Stack<FloatingUIText> poolStack;
        
        void Start()
        {
            poolStack = new Stack<FloatingUIText>();

            for (var i = 0; i < poolCount; i++)
            {
                var obj = Instantiate(poolObj, transform);
                obj.SetActive(false);
                
                var text = obj.GetOrAddComponent<FloatingUIText>();
                text.pool = this;
                
                poolStack.Push(text);
            }
        }

        public FloatingUIText GetPoolObj()
        {
            if (poolStack.Count < 1)
            {
                var fText = Instantiate(poolObj, transform).GetOrAddComponent<FloatingUIText>();
                fText.pool = this;
                return fText;
            }

            var text = poolStack.Pop();
            return text;
        }

        public void ReturnToPool(FloatingUIText textObj)
        {
            poolStack.Push(textObj);
            textObj.gameObject.SetActive(false);
        }
    }
}
