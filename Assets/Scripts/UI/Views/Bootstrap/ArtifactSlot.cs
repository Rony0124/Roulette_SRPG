using MoreMountains.Feedbacks;
using TSoft.Data;
using TSoft.Data.Card;
using TSoft.Managers.Event;
using UnityEngine;
using UnityEngine.UI;

namespace TSoft.UI.Views.Bootstrap
{
    public class ArtifactSlot : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private MMFeedbacks feedbacks;
        
        private RegistryId itemId;
        
        public void SetSlot(ArtifactInfo item)
        {
            icon.sprite = item.image;
            icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, 1f);

            itemId = item.effect.Id;
            EventManager.Instance.GameEvent.Subscribe(itemId, PlayFeedbackOnUse);
        }

        private void PlayFeedbackOnUse()
        {
            feedbacks.PlayFeedbacks();
        }

        private void OnDestroy()
        {
            EventManager.Instance.GameEvent.Unsubscribe(itemId, PlayFeedbackOnUse);
        }
    }
}
