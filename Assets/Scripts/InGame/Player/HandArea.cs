using UnityEngine;

namespace HF.InGame.Player
{
    public abstract class HandArea : MonoBehaviour
    {
        public GameObject card_prefab;
        public Transform card_area;
        public float card_spacing = 100f;
        public float card_angle = 10f;
        public float card_offset_y = 10f;
        public float hand_offset_y = 1f;
    }
}
