using NaughtyAttributes;
using System;
using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0.1f; // Thời gian rung
    public float shakeMagnitude = 0.1f; // Độ mạnh của rung

    public Vector3 originalPosition;
    public static Action OnCompleteShake;

    Coroutine _shakeCoroutine;

    [Button("Test Shake")]
    public void TestShake()
    {
        StartShake();
    }

    void Start()
    {
        originalPosition = transform.localPosition;
    }

    private void OnEnable()
    {
        BaseWeapon.OnWeaponHitAction += StartShake;
    }

    private void OnDestroy()
    {
        BaseWeapon.OnWeaponHitAction -= StartShake;
    }

    public void StartShake()
    {
        StopShakeCoroutine();
        _shakeCoroutine = StartCoroutine(Shake());
    }

    void StopShakeCoroutine()
    {
        if(_shakeCoroutine != null)
        {
            StopCoroutine(_shakeCoroutine); 
            _shakeCoroutine = null;
        }
    }

    private System.Collections.IEnumerator Shake()
    {
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            float x = UnityEngine.Random.Range(-1f, 1f) * shakeMagnitude;
            float y = UnityEngine.Random.Range(0f, 1f) * shakeMagnitude;

            transform.localPosition = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPosition; // Trả về vị trí ban đầu sau khi rung
        OnCompleteShake?.Invoke();
    }
}
