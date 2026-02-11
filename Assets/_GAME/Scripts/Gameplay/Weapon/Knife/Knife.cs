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
        if (!_isStart || !GameController.I.IsPlaying)
            return;
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
            OnDestroyed(); // sau này đổi thành SetActive(false) cho pool
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            BaseCharacter enemy = other.GetComponent<BaseCharacter>();
            if (enemy == null) return;

            Vector3 pos = enemy.transform.position + Vector3.up;
            Vector3 hitDir = direction;
            OnWeaponHitAction?.Invoke(data.shakeDuration, data.shakeMagnitude);
            enemy.TakeDamage(_data.Damage);

            EffectPool.I.Spawn(
            EffectType.KNIFEHIT,
            pos,
            Quaternion.identity);

            SoundCtrl.I.PlaySFXByType(TypeSFX.SWORDHIT);

            OnDestroyed();
        }

        if (other.CompareTag("Ground"))
        {
            EffectPool.I.Spawn(
            EffectType.HITGROUND,
            transform.position,
            Quaternion.identity);

            SoundCtrl.I.PlaySFXByType(TypeSFX.HITGROUND);

            OnDestroyed();
        }

    }
}
