using UnityEngine;
using UnityEngine.AI;

public class E1_Boss : BaseEnemy
{
    enum Phase
    {
        Phase1_Melee,
        Phase2_Range
    }

    [Header("Common")]
    [SerializeField] float hpPercentToChange = 0.5f;

    [Header("Phase 1 - Melee")]
    [SerializeField] float attackRange = 1.5f;

    enum State
    {
        Idle,
        Chase,
        Return
    }

    State _state;

    [Header("Phase 2 - Range / Wander")]
    [SerializeField] Transform firePoint;
    [SerializeField] BulletEnemy bulletPrefab;
    [SerializeField] float moveRadius = 6f;
    [SerializeField] float stopTime = 3f;

    NavMeshAgent _agent;

    Transform _targetPlayer;

    Vector3 _initPos;
    Quaternion _initRot;

    float wanderTimer;

    Phase _phase;

    protected override void OnEnable()
    {
        base.OnEnable();
        _animEvent.OnAttack2Action += HandleEventAttack2;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _animEvent.OnAttack2Action -= HandleEventAttack2;
    }

    public override void OnInit()
    {
        base.OnInit();

        if (_agent == null)
            _agent = GetComponent<NavMeshAgent>();


        _agent.Warp(transform.position);   // <<< dòng này

        _agent.updateRotation = false;

        _initPos = transform.position;
        _initRot = transform.rotation;

        _state = State.Idle;
        wanderTimer = stopTime;
        attackTimer = 0f;
    }

    void Update()
    {
        if (GameController.I.CurState != H_Utils.GameState.PLAYING)
            return;

        UpdatePhase();
        DetectPlayer();

        if (_phase == Phase.Phase1_Melee)
            UpdatePhase1();
        else
            UpdatePhase2();

        UpdateAnim();
    }

    // =======================
    // Phase
    // =======================

    void UpdatePhase()
    {
        float percent = (float)_curHP / maxHealth;

        Phase newPhase = percent >= hpPercentToChange
            ? Phase.Phase1_Melee
            : Phase.Phase2_Range;

        if (newPhase == _phase)
            return;

        _phase = newPhase;

        // reset khi chuyển phase
        _state = State.Idle;
        _agent.ResetPath();
        attackTimer = 0f;
        wanderTimer = stopTime;
    }

    // =======================
    // Detect
    // =======================

    void DetectPlayer()
    {
        Collider[] hits = Physics.OverlapSphere(
            _rotater.position,
            detectRange,
            layerTarget);

        _targetPlayer = hits.Length > 0 ? hits[0].transform : null;
    }

    // =======================
    // Phase 1 : Melee (Enemy.cs)
    // =======================

    void UpdatePhase1()
    {
        if (_targetPlayer == null)
        {
            if (_state != State.Return)
                ChangeState(State.Return);
        }

        switch (_state)
        {
            case State.Idle:
                UpdateIdle();
                break;
            case State.Chase:
                UpdateChase();
                break;
            case State.Return:
                UpdateReturn();
                break;
        }
    }

    void UpdateIdle()
    {
        if (_agent.hasPath)
            _agent.ResetPath();

        if (_targetPlayer != null)
            ChangeState(State.Chase);
    }

    void UpdateChase()
    {
        if (_targetPlayer == null)
        {
            ChangeState(State.Return);
            return;
        }

        float dist = Vector3.Distance(
            transform.position,
            _targetPlayer.position);

        if (dist <= attackRange)
        {
            _agent.ResetPath();

            LookAtTarget(_targetPlayer.position);

            HandleAttack();
        }
        else
        {
            _agent.SetDestination(_targetPlayer.position);
            FaceMovementDirection();
        }
    }

    void UpdateReturn()
    {
        _agent.SetDestination(_initPos);
        FaceMovementDirection();

        if (!_agent.pathPending &&
            _agent.remainingDistance <= _agent.stoppingDistance)
        {
            _agent.ResetPath();

            _rotater.rotation = Quaternion.Slerp(
                _rotater.rotation,
                _initRot,
                Time.deltaTime * 5f);

            if (Quaternion.Angle(_rotater.rotation, _initRot) < 1f)
            {
                _rotater.rotation = _initRot;
                ChangeState(State.Idle);
            }
        }

        if (_targetPlayer != null)
            ChangeState(State.Chase);
    }

    // =======================
    // Phase 2 : Range + Wander (IceEnemy.cs)
    // =======================

    void UpdatePhase2()
    {
        if (_targetPlayer != null)
        {
            StopAgent();
            HandleAttackPhase2();
        }
        else
        {
            ResumeAgent();
            Wander();
        }
    }

    void HandleAttackPhase2()
    {
        LookAtTarget(_targetPlayer.position);

        float dist = Vector3.Distance(
            _rotater.position,
            _targetPlayer.position);

        if (dist > detectRange)
        {
            _targetPlayer = null;
            attackTimer = 0f;
            return;
        }

        if (!_isAttacking)
        {
            attackTimer -= Time.deltaTime;

            if (attackTimer <= 0f)
            {
                if (_isAttacking)
                    return;
                _isAttacking = true;
                _anim.SetTrigger("Attack2");
                attackTimer = attackDelay;
            }
        }
    }

    void Wander()
    {
        wanderTimer -= Time.deltaTime;

        if (wanderTimer > 0f)
            return;

        Vector3 randomPoint = GetRandomPoint(
            transform.position,
            moveRadius);

        if (randomPoint != Vector3.zero)
            _agent.SetDestination(randomPoint);

        wanderTimer = stopTime;
    }

    Vector3 GetRandomPoint(Vector3 center, float radius)
    {
        for (int i = 0; i < 10; i++)
        {
            Vector3 rand = center + Random.insideUnitSphere * radius;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(rand, out hit, 2f, NavMesh.AllAreas))
                return hit.position;
        }

        return Vector3.zero;
    }

    // =======================
    // Common
    // =======================

    void StopAgent()
    {
        if (_agent == null) return;
        if (!_agent.isStopped)
            _agent.isStopped = true;
    }

    void ResumeAgent()
    {
        if (_agent == null) return;
        if (_agent.isStopped)
            _agent.isStopped = false;
    }

    void UpdateAnim()
    {
        if (_anim == null || _agent.enabled == false|| _agent == null)
            return;

        bool moving = !_agent.isStopped &&
                      _agent.velocity.sqrMagnitude > 0.05f;

        _anim.SetBool("Move", moving);
    }

    void FaceMovementDirection()
    {
        if (_agent.velocity.sqrMagnitude > 0.1f)
        {
            Quaternion lookRot =
                Quaternion.LookRotation(_agent.velocity.normalized);

            _rotater.rotation = Quaternion.Slerp(
                _rotater.rotation,
                lookRot,
                Time.deltaTime * 10f);
        }
    }

    void ChangeState(State s)
    {
        if (_state == s)
            return;

        _state = s;
    }

    protected override void Dead()
    {
        base.Dead();

        if (_agent != null)
            _agent.enabled = false;
    }

    // =======================
    // Animation Events
    // =======================

    // Phase 1 – melee
    public override void HandleEventAttack()
    {
        if (_phase != Phase.Phase1_Melee)
            return;

        if (_targetPlayer == null)
            return;

        float dist = Vector3.Distance(
            _rotater.position,
            _targetPlayer.position);

        if (dist > attackRange + 0.2f)
            return;

        var player = _targetPlayer.GetComponent<PlayerCtrl>();
        if (player)
            player.TakeDamage(damage);
    }

    // Phase 2 – range
    public void HandleEventAttack2()
    {
        if (_phase != Phase.Phase2_Range)
            return;

        Vector3 dir = _rotater.forward;
        dir.y = 0f;
        dir.Normalize();

        BulletEnemy b = Instantiate(
            bulletPrefab,
            firePoint.position,
            Quaternion.LookRotation(dir));

        float off = Vector3.Distance(
            transform.position,
            firePoint.position);

        b.Init(dir, damage, detectRange - off);
    }
}
