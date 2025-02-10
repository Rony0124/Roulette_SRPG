using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleRotation : MonoBehaviour
{
    //파티클 회전하는 스크립트
    public float rotationSpeed = 50f;

    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
