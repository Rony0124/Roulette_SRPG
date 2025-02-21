using TSoft.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    public class ArtifactSlot : MonoBehaviour
    {
        [SerializeField] private Image icon;

        public void SetSlot(ItemSO item)
        {
            icon.sprite = item.image;
            icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, 1f);
        }
    }
}
