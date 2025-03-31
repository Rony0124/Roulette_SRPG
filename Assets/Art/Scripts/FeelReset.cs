using MoreMountains.Feedbacks;
using UnityEngine;


public class FeelReset : MonoBehaviour
{
    public MMF_Player mmfPlayer;  // MMF Player (피드백 실행)

   

    void PlayFeedbacksIfActive()
    {
        // 현재 GameObject가 활성화되어 있으면 피드백을 실행
        if (gameObject.activeSelf)
        {
            mmfPlayer.PlayFeedbacks();
            Debug.Log("게임오브젝트 활성화");
        }
    }
}
