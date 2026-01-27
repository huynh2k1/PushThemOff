using UnityEngine;

public class Hammer : BaseWeapon
{
    private Vector3 startPos;
    private HammerData hammerData;
    [SerializeField] Transform _rotater;

    public override void Init(WeaponData weaponData, Transform ownerTf, Vector3 dir)
    {
        base.Init(weaponData, ownerTf, dir);
        hammerData = (HammerData)weaponData; // ép kiểu an toàn vì prefab đúng loại
        startPos = transform.position;
    }

    void Update()
    {
        FlyForward();
        RotateHammer();
        CheckMaxDistance();
    }

    void FlyForward()
    {
        transform.position += direction * hammerData.Speed * Time.deltaTime;
    }

    void RotateHammer()
    {
        //_rotater.Rotate(Vector3.up, hammerData.rotateSpeed * Time.deltaTime, Space.Self);
    }

    void CheckMaxDistance()
    {
        if (Vector3.Distance(startPos, transform.position) >= hammerData.MaxDistance)
        {
            Destroy(gameObject); // sau này đổi thành SetActive(false)
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;

        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy == null) return;

        enemy.TakeDamage(direction, hammerData.Damage);
        Destroy(gameObject);
    }
}
