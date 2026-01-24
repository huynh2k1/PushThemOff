using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCharacter : MonoBehaviour
{
    protected bool isKnockbacking;
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected PlayerPhysics _characterPhysic;
    [SerializeField] protected LayerMask groundLayer;


    [Header("Knockback Settings")]
    [SerializeField] protected float knockbackForceMultiplier = 1f;
    [SerializeField] protected float knockbackDrag = 8f;
    [SerializeField] protected float knockbackDuration = 0.4f;

    //Ground Check
    [SerializeField] float groundCheckDistance = 0.2f;
    protected bool isGrounded;
    protected bool isFalling;

    protected virtual void Awake()
    {
        if(rb == null)
            rb = GetComponent<Rigidbody>();
        isFalling = false;
    }

    protected virtual void OnEnable()
    {
        _characterPhysic.OnTriggerEnterAction += OnPlayerTriggerEnter;
    }

    protected virtual void OnDestroy()
    {
        _characterPhysic.OnTriggerEnterAction -= OnPlayerTriggerEnter;
    }

    protected virtual void FixedUpdate()
    {
        CheckGround();

        if (isFalling)
        {
            Fall();// nếu bạn có animation rơi
        }
    }

    protected virtual void Fall()
    {
        //isDead = true;
    }


    void CheckGround()
    {
        isGrounded = Physics.Raycast(
            transform.position + Vector3.up * 0.1f,
            Vector3.down,
            groundCheckDistance,
            groundLayer
        );

        isFalling = !isGrounded && rb.velocity.y < -0.1f;
    }

    protected abstract void Attack();
    protected abstract void Dead();

    public virtual void TakeDamage(Vector3 forceDir, float force)
    {
        if (!isKnockbacking)
            StartCoroutine(KnockbackPhysics(forceDir, force));
    }


    IEnumerator KnockbackPhysics(Vector3 dir, float force)
    {
        isKnockbacking = true;

        // reset lực cũ
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        float originalDrag = rb.drag;
        rb.drag = knockbackDrag; // tạo ma sát

        // đẩy văng
        rb.AddForce(dir.normalized * force * knockbackForceMultiplier, ForceMode.Impulse);

        yield return new WaitForSeconds(knockbackDuration);

        rb.drag = originalDrag;
        isKnockbacking = false;
    }

    void OnPlayerTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            Dead();
        }
    }

    //IEnumerator KnockbackCoroutine(Vector3 dir, float force)
    //{
    //    isKnockbacking = true;

    //    float duration = 0.6f; // thời gian bị đẩy
    //    float timer = 0f;

    //    Vector3 startPos = transform.position;
    //    Vector3 targetPos = startPos + dir.normalized * force;

    //    while (timer < duration)
    //    {
    //        timer += Time.deltaTime;
    //        float t = timer / duration;

    //        // easing cho mượt
    //        t = 1f - Mathf.Pow(1f - t, 3);

    //        transform.position = Vector3.Lerp(startPos, targetPos, t);
    //        yield return null;
    //    }

    //    transform.position = targetPos;
    //    isKnockbacking = false;
    //}

}
