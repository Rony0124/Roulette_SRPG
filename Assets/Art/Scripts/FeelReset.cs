using MoreMountains.Feedbacks;
using UnityEngine;


public class FeelReset : MonoBehaviour
{
    public MMF_Player mmfPlayer;  // MMF Player (�ǵ�� ����)

   

    void PlayFeedbacksIfActive()
    {
        // ���� GameObject�� Ȱ��ȭ�Ǿ� ������ �ǵ���� ����
        if (gameObject.activeSelf)
        {
            mmfPlayer.PlayFeedbacks();
            Debug.Log("���ӿ�����Ʈ Ȱ��ȭ");
        }
    }
}
