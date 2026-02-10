using UnityEngine;

public class Hammer : BaseWeapon
{
    private Vector3 startPos;
    private HammerData hammerData;
    [SerializeField] Transform _rotater;
    [SerializeField] LayerMask enemyLayer;

    [SerializeField] float radius;

    public override void Init(WeaponData weaponData, Transform ownerTf, Vector3 dir)
    {
        base.Init(weaponData, ownerTf, dir);
        hammerData = (HammerData)weaponData; // ép kiểu an toàn vì prefab đúng loại
        startPos = transform.position;
        _isStart = true;
    }

    void Update()
    {
        if (!_isStart || !GameController.I.IsPlaying)
            return;
        FlyForward();
        CheckMaxDistance();
    }

    void FlyForward()
    {
        transform.position += direction * hammerData.Speed * Time.deltaTime;
    }


    void CheckMaxDistance()
    {
        if (Vector3.Distance(startPos, transform.position) >= hammerData.MaxDistance)
        {
            OnDestroyed();
        }
    }

    void Explode(Vector3 center)
    {
        Collider[] hits = Physics.OverlapSphere(
            center,
            radius,
            enemyLayer
        );

        foreach (Collider hit in hits)
        {
            BaseCharacter enemy = hit.GetComponent<BaseCharacter>();
            if (enemy == null) continue;

            enemy.TakeDamage(hammerData.Damage);
        }

        EffectPool.I.Spawn(
            EffectType.HAMMERHIT,
            center,
            Quaternion.identity
        );

        OnWeaponHitAction?.Invoke(data.shakeDuration, data.shakeMagnitude);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;


        //BaseCharacter enemy = other.GetComponent<BaseCharacter>();
        //if (enemy == null) return;

        //EffectPool.I.Spawn(EffectType.HAMMERHIT, enemy.transform.position, Quaternion.identity);
        //enemy.TakeDamage(hammerData.Damage);
        //OnWeaponHitAction?.Invoke(data.shakeDuration, data.shakeMagnitude);
        Explode(transform.position);
        OnDestroyed();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
