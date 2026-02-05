using UnityEngine;

public class TowerEnemy : BaseEnemy
{
    [Header("Shoot")]
    [SerializeField] Transform firePoint;
    [SerializeField] BulletEnemy bulletPrefab;

    Transform _targetPlayer;

    public override void OnInit()
    {
        base.OnInit();
        attackTimer = 0f;
        _targetPlayer = null;
    }

    void Update()
    {
        DetectPlayer();

        if (_targetPlayer == null)
        {
            attackTimer = 0f;
            return;
        }

        FaceTarget(_targetPlayer.position);

        float dist = Vector3.Distance(_rotater.position, _targetPlayer.position);

        // ra khỏi vùng bắn
        if (dist > detectRange)
        {
            _targetPlayer = null;
            attackTimer = 0f;
            return;
        }

        HandleAttack();
    }

    void DetectPlayer()
    {
        if (_targetPlayer != null)
            return;
        Collider[] hits = Physics.OverlapSphere(_rotater.position, detectRange, layerTarget);
        _targetPlayer = hits.Length > 0 ? hits[0].transform : null;
    }

    void FaceTarget(Vector3 pos)
    {
        Vector3 dir = pos - _rotater.position;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.0001f) return;

        Quaternion lookRot = Quaternion.LookRotation(dir.normalized);
        _rotater.rotation = Quaternion.Slerp(
            _rotater.rotation,
            lookRot,
            Time.deltaTime * 10f);
    }

    public override void HandleEventAttack()
    {
        //Vector3 dir = _targetPlayer.position - firePoint.position;
        Vector3 dir = _rotater.forward;
        dir.y = 0f;
        dir.Normalize();

        BulletEnemy b = Instantiate(
            bulletPrefab,
            firePoint.position,
            Quaternion.LookRotation(dir));

        // tầm bay của đạn = detectRange
        float offSetDistance = Vector3.Distance(transform.position, firePoint.position);
        b.Init(dir, damage, detectRange - offSetDistance);
    }
}
