using NaughtyAttributes;
using System;
using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{

    public Vector3 originalPosition;
    public static Action OnCompleteShake;

    Coroutine _shakeCoroutine;

    [Button("Test Shake")]
    public void TestShake()
    {
    }

    void Start()
    {
        originalPosition = transform.localPosition;
    }

    private void OnEnable()
    {
        BaseWeapon.OnWeaponHitAction += StartShake;
        //RockEnemy.OnAttackAction += StartShake; 
    }

    private void OnDestroy()
    {
        BaseWeapon.OnWeaponHitAction -= StartShake;
        //RockEnemy.OnAttackAction -= StartShake;
    }

    public void StartShake(float shakeTime, float shakeMagnitude)
    {
        StopShakeCoroutine();
        _shakeCoroutine = StartCoroutine(Shake(shakeTime, shakeMagnitude));
    }

    void StopShakeCoroutine()
    {
        if(_shakeCoroutine != null)
        {
            StopCoroutine(_shakeCoroutine); 
            _shakeCoroutine = null;
        }
    }

    private System.Collections.IEnumerator Shake(float shakeDuration, float shakeMagnitude)
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
