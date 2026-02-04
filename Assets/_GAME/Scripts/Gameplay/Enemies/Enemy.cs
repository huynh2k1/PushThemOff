using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

public class Enemy : BaseEnemy
{
    [SerializeField] float attackRange = 1.5f;
    enum State { Idle, Chase, Return }

    NavMeshAgent _agent;
    State _curState;

    Vector3 _initPos;
    Quaternion _initRot;
    Transform _targetPlayer;

    protected override void Awake()
    {
        base.Awake();
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;

        _initPos = _rotater.position;
        _initRot = _rotater.rotation;
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
            LookAtTarget(_targetPlayer.position);

            HandleAttack();
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
        Collider[] hits = Physics.OverlapSphere(_rotater.position, detectRange, layerTarget);
        _targetPlayer = hits.Length > 0 ? hits[0].transform : null;
    }

    void ChangeState(State newState)
    {
        if (_curState == newState) return;

        _curState = newState;
    }

    protected override void Dead()
    {
        base.Dead();
        _agent.enabled = false;
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();    
        // Attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public override void HandleEventAttack()
    {
        if (_targetPlayer == null) return;

        float dist = Vector3.Distance(_rotater.position, _targetPlayer.position);

        if (dist > attackRange + 0.2f) return;

        var player = _targetPlayer.GetComponent<PlayerCtrl>();
        if (player)
            player.TakeDamage(damage);
    }
}
