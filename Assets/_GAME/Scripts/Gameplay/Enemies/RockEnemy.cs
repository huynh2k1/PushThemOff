using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RockEnemy : BaseCharacter
{
    [Header("Rotate / Anim")]
    [SerializeField] Transform _rotater;
    [SerializeField] Animator _anim;

    [Header("Detect & Attack")]
    [SerializeField] float detectRange = 8f;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] float attackDelay = 1f;

    [Header("Shoot")]
    [SerializeField] Transform firePoint;
    [SerializeField] BulletEnemy bulletPrefab;

    [Header("Wander (NavMesh)")]
    [SerializeField] float wanderRadius = 6f;
    [SerializeField] float wanderInterval = 3f;

    [SerializeField] AnimEvent _animEvent;

    NavMeshAgent _agent;

    Vector3 _initPos;
    float attackTimer;
    float lockTimer;
    float wanderTimer;

    Transform _targetPlayer;

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

        _initPos = transform.position;

        attackTimer = 0f;
        lockTimer = 0f;
        wanderTimer = wanderInterval;
        _targetPlayer = null;

        if (_agent == null)
            _agent = GetComponent<NavMeshAgent>();

        if (_agent != null)
        {
            _agent.updateRotation = false;
        }
    }

    void Update()
    {
        DetectPlayer();

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

    void DetectPlayer()
    {
        if (_targetPlayer != null)
            return;

        Collider[] hits = Physics.OverlapSphere(
            _rotater.position,
            detectRange,
            playerLayer);

        if (hits.Length > 0)
        {
            _targetPlayer = hits[0].transform;

            attackTimer = 0f;
            lockTimer = 0f;
        }
    }

    void HandleAttack()
    {
        if (_targetPlayer == null)
            return;

        FaceTarget(_targetPlayer.position);

        float dist = Vector3.Distance(
            _rotater.position,
            _targetPlayer.position);

        if (dist > detectRange)
        {
            _targetPlayer = null;
            lockTimer = 0f;
            attackTimer = 0f;

            ResumeAgent();
            return;
        }

        lockTimer += Time.deltaTime;

        if (lockTimer < attackDelay)
            return;

        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0f)
        {
            Attack();
            attackTimer = attackDelay;
        }
    }

    protected override void Attack()
    {
        if (_anim != null)
            _anim.SetTrigger("Attack");
    }

    void Anim_Shoot()
    {
        Vector3 dir = _rotater.forward;
        dir.y = 0f;
        dir.Normalize();

        BulletEnemy b = Instantiate(
            bulletPrefab,
            firePoint.position,
            Quaternion.LookRotation(dir));

        float offSetDistance = Vector3.Distance(transform.position, firePoint.position);
        b.Init(dir, damage, detectRange - offSetDistance);
    }

    void Wander()
    {
        if (_agent == null || !_agent.enabled)
            return;

        wanderTimer -= Time.deltaTime;

        if (wanderTimer > 0f)
            return;

        Vector3 randomPoint = GetRandomPoint(
            _initPos,
            wanderRadius);

        if (randomPoint != Vector3.zero)
            _agent.SetDestination(randomPoint);

        wanderTimer = wanderInterval;
    }

    Vector3 GetRandomPoint(Vector3 center, float radius)
    {
        for (int i = 0; i < 10; i++)
        {
            Vector3 rand = center + Random.insideUnitSphere * radius;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(rand, out hit, 2f, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }

        return Vector3.zero;
    }

    void FaceTarget(Vector3 pos)
    {
        Vector3 dir = pos - _rotater.position;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.0001f)
            return;

        Quaternion lookRot = Quaternion.LookRotation(dir.normalized);

        _rotater.rotation = Quaternion.Slerp(
            _rotater.rotation,
            lookRot,
            Time.deltaTime * 10f);
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
        if (_rotater == null)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_rotater.position, detectRange);
    }
}
