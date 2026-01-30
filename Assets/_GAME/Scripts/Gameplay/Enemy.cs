using UnityEngine;
using UnityEngine.AI;

public class Enemy : BaseCharacter
{
    enum State { Idle, Chase, Return }

    [SerializeField] HeathBar _heathBar;
    [SerializeField] Transform _rotater;

    [Header("Detection")]
    [SerializeField] float detectRange = 8f;
    [SerializeField] LayerMask playerLayer;

    NavMeshAgent agent;
    State currentState;

    Vector3 spawnPos;
    Quaternion spawnRot;
    Transform targetPlayer;


    //STATS
    float curHP;

    protected override void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;

        spawnPos = _rotater.position;
        spawnRot = _rotater.rotation;
    }

    void Start()
    {
        OnInit();
    }

    void OnInit()
    {
        ChangeState(State.Idle);
        curHP = maxHealth;
        _heathBar.Init(curHP);
    }

    void Update()
    {
        DetectPlayer();

        switch (currentState)
        {
            case State.Idle: UpdateIdle(); break;
            case State.Chase: UpdateChase(); break;
            case State.Return: UpdateReturn(); break;
        }
    }

    void UpdateIdle()
    {
        if (agent.hasPath)
            agent.ResetPath();

        if (targetPlayer != null)
            ChangeState(State.Chase);
    }

    void UpdateChase()
    {
        if (targetPlayer == null)
        {
            ChangeState(State.Return);
            return;
        }

        agent.SetDestination(targetPlayer.position);
        FaceMovementDirection();
    }

    void UpdateReturn()
    {
        agent.SetDestination(spawnPos);
        FaceMovementDirection();

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.ResetPath();

            _rotater.rotation = Quaternion.Slerp(_rotater.rotation, spawnRot, Time.deltaTime * 5f);

            if (Quaternion.Angle(_rotater.rotation, spawnRot) < 1f)
            {
                _rotater.rotation = spawnRot;
                ChangeState(State.Idle);
            }
        }

        if (targetPlayer != null)
            ChangeState(State.Chase);
    }

    void FaceMovementDirection()
    {
        if (agent.velocity.sqrMagnitude > 0.1f)
        {
            Quaternion lookRot = Quaternion.LookRotation(agent.velocity.normalized);
            _rotater.rotation = Quaternion.Slerp(_rotater.rotation, lookRot, Time.deltaTime * 10f);
        }
    }

    void DetectPlayer()
    {
        Collider[] hits = Physics.OverlapSphere(_rotater.position, detectRange, playerLayer);
        targetPlayer = hits.Length > 0 ? hits[0].transform : null;
    }

    void ChangeState(State newState)
    {
        currentState = newState;
    }

    protected override void Attack() { }

    protected override void Dead()
    {
        agent.enabled = false;
        EffectPool.I.Spawn(EffectType.EXPLOSION, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public override void TakeDamage(float damage)
    {
        if(damage >= curHP)
        {
            curHP = 0;
            _heathBar.UpdateHealthBar(curHP);
            Dead();
            return;
        }

        curHP -= damage;
        _heathBar.UpdateHealthBar(curHP);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_rotater.position, detectRange);
    }
}
