using System;
using TMPro;
using UnityEngine;

namespace TSoft.UI.Views.InGame
{
    public class FloatingUIText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI tmp;

        public TextObjectPool pool { get; set; }

        private bool isPooled;

        public void SetText(string content, Color color)
        {
            tmp.text = content;
            tmp.color = color;

            isPooled = true;
        }

        public void Release()
        {
            pool.ReturnToPool(this);
        }

        public void OnDisable()
        {
            if (!isPooled)
                return;

            isPooled = false;
            Release();
        }
    }
}
