using UnityEngine;

public class TowerEnemy : BaseCharacter
{
    [SerializeField] Transform _rotater;
    [SerializeField] Animator _anim;

    [Header("Detect & Attack")]
    [SerializeField] float detectRange = 8f;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] float attackDelay = 1f;

    [Header("Shoot")]
    [SerializeField] Transform firePoint;
    [SerializeField] BulletEnemy bulletPrefab;

    float attackTimer;
    float lockTimer;

    Transform _targetPlayer;

    [SerializeField] AnimEvent _animEvent;

    private void OnEnable()
    {
        if (_animEvent != null)
            _animEvent.OnEventAnimAction += Anim_Shoot;
    }

    private void OnDisable()
    {
        if (_animEvent != null)
            _animEvent.OnEventAnimAction -= Anim_Shoot;
    }

    protected override void OnInit()
    {
        base.OnInit();
        attackTimer = 0f;
        lockTimer = 0f;
        _targetPlayer = null;
    }

    void Update()
    {
        DetectPlayer();

        if (_targetPlayer == null)
        {
            lockTimer = 0f;
            attackTimer = 0f;
            return;
        }

        FaceTarget(_targetPlayer.position);

        float dist = Vector3.Distance(_rotater.position, _targetPlayer.position);

        // ra khỏi vùng bắn
        if (dist > detectRange)
        {
            _targetPlayer = null;
            lockTimer = 0f;
            attackTimer = 0f;
            return;
        }

        // player phải đứng trong vùng liên tục 1 khoảng thời gian
        lockTimer += Time.deltaTime;

        if (lockTimer < attackDelay)
            return;

        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0f)
        {
            if(_targetPlayer != null)
                Attack();
            attackTimer = attackDelay;
        }
    }

    void DetectPlayer()
    {
        // Nếu đang có target thì chỉ cần check còn trong vùng hay không
        if (_targetPlayer != null)
            return;

        Collider[] hits = Physics.OverlapSphere(
            _rotater.position,
            detectRange,
            playerLayer);

        if (hits.Length > 0)
            _targetPlayer = hits[0].transform;
    }

    protected override void Attack()
    {
        _anim.SetTrigger("Attack");
    }

    // gọi bằng Animation Event
    void Anim_Shoot()
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

    protected override void Dead()
    {
        EffectPool.I.Spawn(
            EffectType.EXPLOSION,
            transform.position,
            Quaternion.identity);

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        if (_rotater == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_rotater.position, detectRange);
    }
}
