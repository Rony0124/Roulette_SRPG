using Cysharp.Threading.Tasks;
using MoreMountains.Feedbacks;
using TSoft.Item;
using TSoft.Managers.Event;
using UnityEngine;
using UnityEngine.UI;

namespace TSoft.UI.Views.InGame
{
    public class JokerEffectIcon : MonoBehaviour
    {
        [SerializeField] public MMFeedbacks usedFeedback;
        [SerializeField] public float feedBackDuration;
        [SerializeField] private Image icon;

        private Joker joker;

        public void SetIcon(Joker joker)
        {
            this.joker = joker;
            icon.sprite = joker.info.image;
            
            EventManager.Instance.GameEvent.Subscribe(joker.info.effect.Id,() =>  PlayFeedbackOnUse().Forget());
        }
        
        private async UniTaskVoid PlayFeedbackOnUse()
        {
            usedFeedback.PlayFeedbacks();
            await UniTask.WaitForSeconds(feedBackDuration);
            
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            EventManager.Instance.GameEvent.Unsubscribe(joker.info.Id, () =>  PlayFeedbackOnUse().Forget());
        }
    }
}
