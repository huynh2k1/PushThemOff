using UnityEngine;

public class Knife : BaseWeapon
{
    [SerializeField] private KnifeData _data;

    private Vector3 startPos;


    public override void Init(WeaponData newData, Transform ownerTf, Vector3 dir)
    {
        base.Init(newData, ownerTf, dir);
        direction = dir == Vector3.zero ? ownerTf.forward : dir.normalized;
        startPos = transform.position;
    }

    void Update()
    {
        FlyForward();
        CheckMaxDistance();
    }

    void FlyForward()
    {
        transform.position += direction * _data.Speed * Time.deltaTime;
    }

    void CheckMaxDistance()
    {
        float distance = Vector3.Distance(startPos, transform.position);
        if (distance >= _data.MaxDistance)
        {
            Destroy(gameObject); // sau này đổi thành SetActive(false) cho pool
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;

        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy == null) return;

        Vector3 hitDir = direction;
        enemy.TakeDamage(hitDir, _data.Damage);

        Destroy(gameObject);
    }
}
