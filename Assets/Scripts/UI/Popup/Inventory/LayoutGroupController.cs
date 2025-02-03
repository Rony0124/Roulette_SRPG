using UnityEngine;
using UnityEngine.UI;

namespace TSoft.UI.Popup.Inventory
{
    public class LayoutGroupController : MonoBehaviour
    {
        private LayoutGroup layoutGroup;
        void Awake()
        {
            if (!layoutGroup)
                layoutGroup = GetComponent<LayoutGroup>();
        }

        private void OnEnable()
        {
            if (layoutGroup)
                layoutGroup.enabled = true;
        }

        private void OnDisable()
        {
            if (layoutGroup)
                layoutGroup.enabled = false;
        }
    }
}
