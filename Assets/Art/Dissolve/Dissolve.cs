using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Dissolve : MonoBehaviour
{
    [SerializeField]
    private float _dissolveTime = 0.75f;

    private SpriteRenderer[] _spriteRenders;
    private Material[] _materials;

    private int _dissolveAmount = Shader.PropertyToID("_DissolveAmount");
    private int _verticalDissolveAmount = Shader.PropertyToID("_VerticalDissolve");

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenders = GetComponentsInChildren<SpriteRenderer>();
        _materials = new Material[_spriteRenders.Length];


        for (int i = 0; i < _spriteRenders.Length; i++)
        {
            _materials[i] = _spriteRenders[i].material;
        }
    }

    private IEnumerator Vanish(bool useDissolve, bool useVertical)
    {
        float elapsedTime = 0f;
        while (elapsedTime < _dissolveTime)
        {
            elapsedTime += Time.deltaTime;

            float lerpedDissolve = Mathf.Lerp(0, 1.1f, (elapsedTime / _dissolveTime));
            float lerpedVerticalDissolve = Mathf.Lerp(0f, 1.1f, (elapsedTime / _dissolveTime));

            for (int i = 0; i < _materials.Length; i++)
            {
                if (useDissolve)
                _materials[i].SetFloat(_dissolveAmount, lerpedDissolve);
                
                if(useVertical)
                _materials[i].SetFloat(_verticalDissolveAmount, lerpedVerticalDissolve);
            }

            yield return null;
        }
    }

    private IEnumerator Appear(bool useDissolve, bool useVertical)
    {
        float elapsedTime = 0f;
        while (elapsedTime < _dissolveTime)
        {
            elapsedTime += Time.deltaTime;

            float lerpedDissolve = Mathf.Lerp(1.1f, 0f, (elapsedTime / _dissolveTime));
            float lerpedVerticalDissolve = Mathf.Lerp(1.1f, 0f, (elapsedTime / _dissolveTime));

            for (int i = 0; i < _materials.Length; i++)
            {
                if (useDissolve)
                    _materials[i].SetFloat(_dissolveAmount, lerpedDissolve);

                if (useVertical)
                    _materials[i].SetFloat(_verticalDissolveAmount, lerpedVerticalDissolve);
            }

            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(Vanish(true, false));
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(Appear(true, false));
        }

    }
}
