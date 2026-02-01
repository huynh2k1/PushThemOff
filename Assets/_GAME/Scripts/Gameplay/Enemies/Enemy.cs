using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

public class Enemy : BaseCharacter
{
    enum State { Idle, Chase, Return }

    [SerializeField] Transform _rotater;
    [SerializeField] Animator _anim;

    [Header("Detection")]
    [SerializeField] float detectRange = 8f;
    [SerializeField] LayerMask playerLayer;

    NavMeshAgent _agent;
    State _curState;

    Vector3 _initPos;
    Quaternion _initRot;
    Transform _targetPlayer;


    [Header("Attack")]
    [SerializeField] float attackRange = 1.5f;
    [SerializeField] float attackDelay = 1f;

    float attackTimer;

    //Anim Event
    [SerializeField] AnimEvent _animEvent;

    protected override void Awake()
    {
        base.Awake();
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;

        _initPos = _rotater.position;
        _initRot = _rotater.rotation;
    }

    private void OnEnable()
    {
        _animEvent.OnEventAnimAction += Anim_DoDamage;
    }

    private void OnDestroy()
    {
        _animEvent.OnEventAnimAction -= Anim_DoDamage;
    }

    protected override void OnInit()
    {
        base.OnInit();
        ChangeState(State.Idle);
    }
    void Update()
    {
        DetectPlayer();

        switch (_curState)
        {
            case State.Idle: UpdateIdle(); break;
            case State.Chase: UpdateChase(); break;
            case State.Return: UpdateReturn(); break;
        }
    }

    void UpdateIdle()
    {
        _anim.SetBool("Move", false);

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

        float dist = Vector3.Distance(transform.position, _targetPlayer.position);

        if (dist <= attackRange)
        {
            _anim.SetBool("Move", false);

            _agent.ResetPath();
            FaceTarget(_targetPlayer.position);

            attackTimer -= Time.deltaTime;

            if (attackTimer <= 0f)
            {
                Attack();
                attackTimer = attackDelay;
            }
        }
        else
        {
            _anim.SetBool("Move", true);
            _agent.SetDestination(_targetPlayer.position);
            FaceMovementDirection();
        }
    }

    void UpdateReturn()
    {
        _agent.SetDestination(_initPos);
        FaceMovementDirection();

        if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
        {
            _agent.ResetPath();

            _rotater.rotation = Quaternion.Slerp(_rotater.rotation, _initRot, Time.deltaTime * 5f);

            if (Quaternion.Angle(_rotater.rotation, _initRot) < 1f)
            {
                _rotater.rotation = _initRot;
                ChangeState(State.Idle);
            }
        }

        if (_targetPlayer != null)
            ChangeState(State.Chase);
    }

    void FaceMovementDirection()
    {
        if (_agent.velocity.sqrMagnitude > 0.1f)
        {
            Quaternion lookRot = Quaternion.LookRotation(_agent.velocity.normalized);
            _rotater.rotation = Quaternion.Slerp(_rotater.rotation, lookRot, Time.deltaTime * 10f);
        }
    }

    void DetectPlayer()
    {
        Collider[] hits = Physics.OverlapSphere(_rotater.position, detectRange, playerLayer);
        _targetPlayer = hits.Length > 0 ? hits[0].transform : null;
    }

    void ChangeState(State newState)
    {
        if (_curState == newState) return;

        _curState = newState;

        if (newState != State.Chase)
        {
            attackTimer = 0f;
        }
    }

    protected override void Attack()
    {
        _anim.SetTrigger("Attack");
    }

    public void Anim_DoDamage()
    {
        if (_targetPlayer == null) return;

        float dist = Vector3.Distance(_rotater.position, _targetPlayer.position);

        if (dist > attackRange + 0.2f) return;

        var player = _targetPlayer.GetComponent<BaseCharacter>();
        if (player)
            player.TakeDamage(damage);
    }

    protected override void Dead()
    {
        _agent.enabled = false;
        EffectPool.I.Spawn(EffectType.EXPLOSION, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    void FaceTarget(Vector3 pos)
    {
        Vector3 dir = pos - _rotater.position;
        dir.y = 0;

        if (dir.sqrMagnitude < 0.0001f) return;

        Quaternion lookRot = Quaternion.LookRotation(dir.normalized);
        _rotater.rotation = Quaternion.Slerp(_rotater.rotation, lookRot, Time.deltaTime * 10f);
    }

    private void OnDrawGizmosSelected()
    {
        if (_rotater == null) return;

        // Detect range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);

        // Attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
