using System;
using UnityEngine;
using UnityEngine.AI;

public class E3_Boss : BaseEnemy
{
    [Header("Detect (Trigger)")]
    [SerializeField] EnemyDetect _detect;

    [Header("Shoot")]
    [SerializeField] Transform firePoint;
    [SerializeField] BulletEnemy bulletPrefab;

    [Header("Wander (NavMesh)")]
    [SerializeField] float wanderRadius = 6f;
    [SerializeField] float wanderInterval = 3f;

    [SerializeField] ParticleSystem _chargeEffect;

    NavMeshAgent _agent;

    Vector3 _initPos;
    float wanderTimer;

    Transform _targetPlayer;

    public static Action<float, float> OnAttackAction;

    public override void OnInit()
    {
        base.OnInit();

        _initPos = transform.position;

        attackTimer = 0f;
        wanderTimer = wanderInterval;
        _targetPlayer = null;

        _agent = GetComponent<NavMeshAgent>();

        _agent.Warp(transform.position);   // <<< dòng này

        _agent.updateRotation = false;

        // >>> detect bằng trigger giống E1
        if (_detect != null)
        {
            _detect.OnPlayerEnter += HandlePlayerEnter;
            _detect.OnPlayerExit += HandlePlayerExit;
        }
    }

    void Update()
    {
        if (GameController.I.CurState != H_Utils.GameState.PLAYING)
            return;

        if (_targetPlayer != null)
        {
            StopAgent();
            HandleAttack();
        }
        else
        {
            Wander();
        }

        UpdateAnim();
    }

    // ================== Detect bằng trigger ==================

    void HandlePlayerEnter(Transform player)
    {
        if (isDead) return;

        _targetPlayer = player;
        attackTimer = 0f;
    }

    void HandlePlayerExit()
    {
        if (isDead) return;

        _targetPlayer = null;
        attackTimer = 0f;

        ResumeAgent();
    }

    // =========================================================

    protected override void HandleAttack()
    {
        if (_targetPlayer == null)
            return;

        LookAtTarget(_targetPlayer.position);

        if(_isAttacking == false)
        {
            attackTimer -= Time.deltaTime;

            if (attackTimer <= 0f)
            {
                Attack();
                _chargeEffect.Play();
                attackTimer = attackDelay;
            }
        }
    }

    void Wander()
    {
        if (_agent == null || !_agent.enabled)
            return;

        wanderTimer -= Time.deltaTime;

        if (wanderTimer > 0f)
            return;

        Vector3 randomPoint = GetRandomPoint(_initPos, wanderRadius);

        if (randomPoint != Vector3.zero)
            _agent.SetDestination(randomPoint);

        wanderTimer = wanderInterval;
    }

    Vector3 GetRandomPoint(Vector3 center, float radius)
    {
        for (int i = 0; i < 10; i++)
        {
            Vector3 rand = center + UnityEngine.Random.insideUnitSphere * radius;

            if (NavMesh.SamplePosition(rand, out NavMeshHit hit, 2f, NavMesh.AllAreas))
                return hit.position;
        }

        return Vector3.zero;
    }

    void StopAgent()
    {
        if (_agent == null)
            return;

        if (!_agent.isStopped)
            _agent.isStopped = true;
    }

    void ResumeAgent()
    {
        if (_agent == null)
            return;

        if (_agent.isStopped)
            _agent.isStopped = false;
    }

    void UpdateAnim()
    {
        if (_anim == null || _agent == null)
            return;

        bool isMoving = !_agent.isStopped && _agent.velocity.sqrMagnitude > 0.05f;
        _anim.SetBool("Move", isMoving);
    }

    public override void HandleEventAttack()
    {
        if (_targetPlayer == null)
            return;

        OnAttackAction?.Invoke(0.3f, 0.5f);

        Vector3 baseDir = _rotater.forward;
        baseDir.y = 0f;
        baseDir.Normalize();

        float offSetDistance = Vector3.Distance(transform.position, firePoint.position);
        float travelDistance = detectRange - offSetDistance;

        // Góc lệch
        float spreadAngle = 20f;

        // 3 hướng: trái - giữa - phải
        Vector3 leftDir = Quaternion.AngleAxis(-spreadAngle, Vector3.up) * baseDir;
        Vector3 rightDir = Quaternion.AngleAxis(spreadAngle, Vector3.up) * baseDir;

        SpawnBullet(leftDir, travelDistance);
        SpawnBullet(baseDir, travelDistance);
        SpawnBullet(rightDir, travelDistance);
    }

    void SpawnBullet(Vector3 dir, float distance)
    {
        BulletEnemy b = PoolManager.I.Spawn(
            bulletPrefab,
            firePoint.position,
            Quaternion.LookRotation(dir));

        b.Init(dir, damage, distance);
    }

    protected override void Dead()
    {
        base.Dead();

        if (_detect != null)
        {
            _detect.OnPlayerEnter -= HandlePlayerEnter;
            _detect.OnPlayerExit -= HandlePlayerExit;
        }
    }
}
