using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ObjectActivator : MonoBehaviour
{
    // 버튼을 연결할 변수
    public Button yourButton;
    // 활성화할 오브젝트들을 배열로 설정
    public GameObject[] objectsToActivate;
    // 현재 활성화할 오브젝트의 인덱스를 추적하는 변수
    private int currentIndex = 0;
    
    [SerializeField] private UnityEvent returnFeedback;
    [SerializeField] private float returnCardDurationOffset;

    void Start()
    {
        // 버튼 클릭 시 ActivateNextObject 함수 호출
        yourButton.onClick.AddListener(() => StartCoroutine(ActivateNextObject()));
    }

    // 버튼 클릭 시 하나씩 오브젝트를 활성화하는 코루틴
    IEnumerator ActivateNextObject()
    {
        // 아직 활성화할 오브젝트가 있다면
        if (currentIndex < objectsToActivate.Length)
        {
            objectsToActivate[currentIndex].SetActive(true);  // 현재 인덱스 오브젝트 활성화
            currentIndex++;  // 다음 오브젝트로 이동
            yield return new WaitForSeconds(0.5f);  //  0.5초 대기
        }
    }

    public async UniTask ReturnActivatedCards()
    {
        currentIndex = 0;

        float duration = objectsToActivate[0].GetComponent<ActivatedCard>().FeelDuration;
        bool shouldWait = false;
        foreach (var obj in objectsToActivate)
        {
            if (obj.activeSelf)
            {
                shouldWait = true;
                var activatedCard = obj.GetComponent<ActivatedCard>();
                activatedCard.MoveFeedback();
            }
        }

        returnFeedback?.Invoke();
        
        if(shouldWait)
            await UniTask.WaitForSeconds(duration + returnCardDurationOffset);
    }
}
