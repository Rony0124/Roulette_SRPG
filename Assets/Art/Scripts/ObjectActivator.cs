using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectActivator : MonoBehaviour
{
    // ��ư�� ������ ����
    public Button yourButton;
    // Ȱ��ȭ�� ������Ʈ���� �迭�� ����
    public GameObject[] objectsToActivate;
    // ���� Ȱ��ȭ�� ������Ʈ�� �ε����� �����ϴ� ����
    private int currentIndex = 0;

    void Start()
    {
        // ��ư Ŭ�� �� ActivateNextObject �Լ� ȣ��
        yourButton.onClick.AddListener(() => StartCoroutine(ActivateNextObject()));
    }

    // ��ư Ŭ�� �� �ϳ��� ������Ʈ�� Ȱ��ȭ�ϴ� �ڷ�ƾ
    IEnumerator ActivateNextObject()
    {
        // ���� Ȱ��ȭ�� ������Ʈ�� �ִٸ�
        if (currentIndex < objectsToActivate.Length)
        {
            objectsToActivate[currentIndex].SetActive(true);  // ���� �ε��� ������Ʈ Ȱ��ȭ
            currentIndex++;  // ���� ������Ʈ�� �̵�
            yield return new WaitForSeconds(0.5f);  // 0.5�� ���
        }
    }
}
