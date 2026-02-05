using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RockEnemy : BaseEnemy
{
    [Header("Shoot")]
    [SerializeField] Transform firePoint;
    [SerializeField] BulletEnemy bulletPrefab;

    [Header("Wander (NavMesh)")]
    [SerializeField] float wanderRadius = 6f;
    [SerializeField] float wanderInterval = 3f;

    NavMeshAgent _agent;

    Vector3 _initPos;
    float wanderTimer;

    Transform _targetPlayer;

    public static Action OnAttackAction;

    public override void OnInit()
    {
        base.OnInit();

        _initPos = transform.position;

        attackTimer = 0f;
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

        Collider[] hits = Physics.OverlapSphere(_rotater.position, detectRange, layerTarget);
        _targetPlayer = hits.Length > 0 ? hits[0].transform : null;
    }

    protected override void HandleAttack()
    {
        if (_targetPlayer == null)
            return;

        LookAtTarget(_targetPlayer.position);

        float dist = Vector3.Distance(
            _rotater.position,
            _targetPlayer.position);

        if (dist > detectRange)
        {
            _targetPlayer = null;
            attackTimer = 0f;

            ResumeAgent();
            return;
        }

        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0f)
        {
            Attack();
            attackTimer = attackDelay;
        }
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
            Vector3 rand = center + UnityEngine.Random.insideUnitSphere * radius;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(rand, out hit, 2f, NavMesh.AllAreas))
            {
                return hit.position;
            }
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
        OnAttackAction?.Invoke();

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
}
