using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleRotation : MonoBehaviour
{
    //��ƼŬ ȸ���ϴ� ��ũ��Ʈ
    public float rotationSpeed = 50f;

    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
