using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : BaseCharacter
{
    [Header("Movement")]
    [SerializeField] float moveRadius = 10f;
    [SerializeField] float waitTime = 1f;

    NavMeshAgent agent;
    float waitTimer;

    protected override void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        MoveToRandomPoint();
    }

    void Update()
    {
        if (isFalling || isKnockbacking)
            return;

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            waitTimer += Time.deltaTime;

            if (waitTimer >= waitTime)
            {
                MoveToRandomPoint();
                waitTimer = 0f;
            }
        }
    }

    // =========================
    // KNOCKBACK (CÁCH 1)
    // =========================
    public override void TakeDamage(Vector3 forceDir, float force)
    {
        if (isKnockbacking)
            return;

        StartCoroutine(KnockbackWithStopNavMesh(forceDir, force));
    }

    IEnumerator KnockbackWithStopNavMesh(Vector3 dir, float force)
    {
        isKnockbacking = true;

        // ⛔ DỪNG NAVMESH
        agent.enabled = false;
        //agent.isStopped = true;
        //agent.velocity = Vector3.zero;

        // reset rigidbody
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        float originalDrag = rb.drag;
        rb.drag = knockbackDrag; // tạo ma sát

        // đẩy văng
        rb.AddForce(dir.normalized * force, ForceMode.Impulse);

        yield return new WaitForSeconds(0.4f);

        rb.drag = originalDrag;
        isKnockbacking = false;
        if (isGrounded)
        {
            agent.enabled = true;
            MoveToRandomPoint();
        }
        // ▶️ CHẠY LẠI NAVMESH
        //agent.isStopped = false;

    }

    protected override void Attack()
    {
    }

    protected override void Dead()
    {
        Destroy(gameObject);
    }

    void MoveToRandomPoint()
    {

        Vector3 randomPos = GetRandomPointOnNavMesh(transform.position, moveRadius);
        agent.SetDestination(randomPos);
    }

    Vector3 GetRandomPointOnNavMesh(Vector3 center, float radius)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * radius;
            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }
        return transform.position;
    }
}
