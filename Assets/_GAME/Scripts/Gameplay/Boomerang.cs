using System;
using UnityEngine;

public class Boomerang : MonoBehaviour
{
    [Header("Movement")]
    public float maxSpeed = 20f;
    public float forcePush = 10f;

    [Header("Speed Curve")]
    public AnimationCurve flyOutCurve;   // nhanh -> chậm
    public AnimationCurve returnCurve;   // chậm -> nhanh

    private Transform player;
    private Vector3 direction;
    [SerializeField] float maxDistance = 8f;
    private Vector3 startPos;
    private bool isReturning = false;

    [SerializeField] Transform _rotater;

    public static Action OnBoomerangHitAction;

    public void Init(Transform playerTf, Vector3 dir, float force)
    {
        player = playerTf;

        if (dir == Vector3.zero)
            dir = playerTf.forward; // fallback

        direction = dir.normalized;
        maxDistance = Mathf.Max(force, 0.5f);
        startPos = transform.position;

        if (flyOutCurve == null || flyOutCurve.length == 0)
            flyOutCurve = AnimationCurve.EaseInOut(0, 1, 1, 0.3f);

        if (returnCurve == null || returnCurve.length == 0)
            returnCurve = AnimationCurve.EaseInOut(0, 0.3f, 1, 1);
    }

    void Update()
    {
        if (!isReturning)
            FlyOut();
        else
            Return();

        _rotater.Rotate(0, 1440 * Time.deltaTime, 0);
    }

    void FlyOut()
    {
        float distance = Vector3.Distance(startPos, transform.position);
        float percent = Mathf.Clamp01(distance / maxDistance);

        float speed = maxSpeed * flyOutCurve.Evaluate(percent);
        transform.position += direction * speed * Time.deltaTime;

        if (distance >= maxDistance)
            isReturning = true;
    }

    void Return()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        float percent = Mathf.Clamp01(1f - (distance / maxDistance));

        float speed = maxSpeed * returnCurve.Evaluate(percent);
        Vector3 dir = (player.position - transform.position).normalized;

        transform.position += dir * speed * Time.deltaTime;

        if (distance < 0.5f)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();

            if (enemy != null)
            {
                Vector3 temp = transform.position;
                temp.y = other.transform.position.y;
                // hướng boomerang bay tới enemy
                Vector3 hitDir = (other.transform.position - temp).normalized;

                // lực đẩy dựa theo tốc độ hiện tại
                float currentSpeed = forcePush * (isReturning ?
                    returnCurve.Evaluate(0.1f) :
                    flyOutCurve.Evaluate(0.1f));

                OnBoomerangHitAction?.Invoke();
                enemy.TakeDamage(hitDir, forcePush);
            }
        }
    }
}
