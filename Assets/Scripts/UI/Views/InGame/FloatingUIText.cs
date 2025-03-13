using System;
using MoreMountains.Tools;
using TMPro;
using UnityEngine;

namespace TSoft.UI.Views.InGame
{
    public class FloatingUIText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI tmp;
        [SerializeField] private AnimationCurve animateYCurve;
        [SerializeField] private float lifeTime;
        [SerializeField] private float remapYZero;
        [SerializeField] private float remapYOne;

        public TextObjectPool pool { get; set; }

        private float startedAt;
        private float elapsedTime;
        private float remappedTime;
        private Vector3 newPosition;
        private bool isPooled;

        public virtual float GetTime() => Time.unscaledTime;

        private void Update()
        {
            if (!isActiveAndEnabled)
                return;
            
            elapsedTime = GetTime() - startedAt;
            remappedTime = MMMaths.Remap(elapsedTime, 0f, lifeTime, 0f, 1f);

            HandleMovement();
        }
        
        protected virtual void HandleMovement()
        {
            transform.up = Vector3.up;

            var newPosition = transform.localPosition;
            newPosition.y += MMMaths.Remap(animateYCurve.Evaluate(remappedTime), 0f, 1, remapYZero, remapYOne);
            
            transform.localPosition = newPosition;
        }


        private void OnEnable()
        {
            startedAt = GetTime();
        }

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
