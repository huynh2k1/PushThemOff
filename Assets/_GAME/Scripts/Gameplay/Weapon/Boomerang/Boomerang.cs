using System;
using UnityEngine;

public class Boomerang : BaseWeapon
{
    [SerializeField]  BoomerangData _data;
    [Header("Speed Curve")]
    public AnimationCurve flyOutCurve;   // nhanh -> chậm
    public AnimationCurve returnCurve;   // chậm -> nhanh

    private Vector3 startPos;
    private bool isReturning = false;

    [SerializeField] Transform _rotater;

    public static Action OnBoomerangHitAction;

    public override void Init(WeaponData newData, Transform ownerTf, Vector3 dir)
    {
        base.Init(newData, ownerTf, dir);

        direction = dir == Vector3.zero ? ownerTf.forward : dir.normalized;

        startPos = transform.position;

        if (flyOutCurve == null || flyOutCurve.length == 0)
            flyOutCurve = AnimationCurve.EaseInOut(0, 1, 1, 0.2f);

        if (returnCurve == null || returnCurve.length == 0)
            returnCurve = AnimationCurve.EaseInOut(0, 0.2f, 1, 1);
    }

    //public void Init(Transform ownerTf, Vector3 dir)
    //{
    //    base.Init(_data, ownerTf);

    //    direction = dir == Vector3.zero ? ownerTf.forward : dir.normalized;

    //    startPos = transform.position;

    //    if (flyOutCurve == null || flyOutCurve.length == 0)
    //        flyOutCurve = AnimationCurve.EaseInOut(0, 1, 1, 0.2f);

    //    if (returnCurve == null || returnCurve.length == 0)
    //        returnCurve = AnimationCurve.EaseInOut(0, 0.2f, 1, 1);
    //}

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
        float percent = Mathf.Clamp01(distance / data.MaxDistance);

        float speed = data.Speed * flyOutCurve.Evaluate(percent);
        transform.position += direction * speed * Time.deltaTime;

        if (distance >= data.MaxDistance)
            isReturning = true;
    }

    void Return()
    {
        float distance = Vector3.Distance(transform.position, owner.position);
        float percent = Mathf.Clamp01(1f - (distance / data.MaxDistance));

        float speed = data.Speed * returnCurve.Evaluate(percent);
        Vector3 dir = (owner.position - transform.position).normalized;

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
                float currentSpeed = 6 * (isReturning ?
                    returnCurve.Evaluate(0.1f) :
                    flyOutCurve.Evaluate(0.1f));

                OnBoomerangHitAction?.Invoke();
                enemy.TakeDamage(6);
            }
        }
    }
}
