using System;
using UnityEngine;
using UnityEngine.UI;

namespace TCGStarter
{
    public class TargetingArrow : MonoBehaviour
    {
        [SerializeField] private GameObject arrowBodyPrefab;
        [SerializeField] private GameObject arrowTipPrefab;
        [SerializeField] private int bodyPartsCount = 10;
        [SerializeField] private float tipRotateRange = 150f;
        [SerializeField] private bool isAnimated = true;
        [SerializeField] private float animationSpeed = 1f;
        [Range(0, 0.5f)]
        [SerializeField] private float scaling = 0.03f;

        private Transform tip;
        private Transform[] bodyParts;
        private Image[] bodyImages;

        private Transform partsContainer;
        private Color colorOn;
        private Color colorOff;

        private void Start()
        {
            GameObject g = new GameObject("parts");
            partsContainer = g.transform;
            partsContainer.SetParent(transform);
            partsContainer.localPosition = Vector3.zero;
            bodyParts = new Transform[bodyPartsCount];
            bodyImages = new Image[bodyPartsCount];

            for (int i = 0; i < bodyParts.Length; i++)
            {
                bodyParts[i] = Instantiate(arrowBodyPrefab, partsContainer).transform;
                bodyImages[i] = bodyParts[i].GetComponentInChildren<Image>();

                bodyImages[i].transform.localScale = Vector3.one * (1 + (bodyParts.Length - i) * scaling);
            }

            colorOn = bodyImages[0].color;
            colorOff = bodyImages[0].color;
            colorOff.a = 0f;

            if (arrowTipPrefab != null)
                tip = Instantiate(arrowTipPrefab, transform).transform;

        }

        private void Update()
        {
            AdjustBody();
            AdjustTip();
        }

        private void AdjustBody()
        {
            Vector3 mousePosition = Input.mousePosition;
            Vector2 dir = mousePosition - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            float distance = Vector2.Distance(mousePosition, transform.position);
            Vector3 normalized = dir.normalized;
            for (int i = 0; i < bodyParts.Length; i++)
            {
                bodyParts[i].rotation = Quaternion.Euler(0, 0, angle);
                bodyParts[i].localPosition = i * normalized * distance / bodyParts.Length;
            }

            if (isAnimated)
                AnimateBodyParts(distance);

        }

        private void AnimateBodyParts(float distance)
        {
            Vector3 target = Vector3.right * distance / bodyParts.Length;

            for (int i = 0; i < bodyImages.Length; i++)
            {
                bodyImages[i].transform.localPosition = Vector3.MoveTowards(bodyImages[i].transform.localPosition,
                    target, Time.deltaTime * distance / bodyParts.Length * animationSpeed);


                if (Vector3.Distance(bodyImages[i].transform.localPosition, target) < 0.5f)
                {
                    bodyImages[i].transform.localPosition = Vector3.zero;
                }

                bodyImages[i].transform.localScale =
                 Vector3.Lerp(Vector3.one * (1 + (bodyParts.Length - (i)) * scaling),
                 Vector3.one * (1 + (bodyParts.Length - (i + 1)) * scaling),
                 bodyImages[i].transform.localPosition.sqrMagnitude / target.sqrMagnitude);


                // FadeIn , FadeOut
                if (i == 0)
                {
                    bodyImages[i].color = Color.Lerp(colorOff, colorOn,
                        (bodyImages[i].transform.localPosition.sqrMagnitude / target.sqrMagnitude));
                }
                else if (i == bodyParts.Length - 1)
                {
                    bodyImages[i].color = Color.Lerp(colorOn, colorOff,
                         (bodyImages[i].transform.localPosition.sqrMagnitude / target.sqrMagnitude));
                }
            }
        }

        private void AdjustTip()
        {
            if (tip == null)
                return;

            Vector3 mousePosition = Input.mousePosition;
            tip.position = mousePosition;

            if (mousePosition.x > transform.position.x + tipRotateRange)
            {
                tip.rotation = Quaternion.Euler(0, 0, 0);

            }
            else if (mousePosition.x < transform.position.x - tipRotateRange)
            {
                tip.rotation = Quaternion.Euler(0, 0, -180);
            }
            else
            {
                Vector2 dir = mousePosition - transform.position;
                float percent = (dir.x - tipRotateRange) / (tipRotateRange * 2);
                float angle = percent * 180;
                tip.rotation = Quaternion.Euler(0, 0, angle);
            }
        }
    }
}
