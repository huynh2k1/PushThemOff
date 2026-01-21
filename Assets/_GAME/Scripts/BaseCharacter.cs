using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCharacter : MonoBehaviour
{
    protected bool isKnockbacking;
    [SerializeField] protected Rigidbody rb;

    [Header("Knockback Settings")]
    [SerializeField] float knockbackForceMultiplier = 1f;
    [SerializeField] float knockbackDrag = 8f;
    [SerializeField] float knockbackDuration = 0.4f;

    protected virtual void Awake()
    {
        if(rb == null)
            rb = GetComponent<Rigidbody>(); 
    }

    protected abstract void Attack();
    protected abstract void Dead();

    public virtual void TakeDamage(Vector3 forceDir, float force)
    {
        if (!isKnockbacking)
            StartCoroutine(KnockbackPhysics(forceDir, force));
    }

    IEnumerator KnockbackCoroutine(Vector3 dir, float force)
    {
        isKnockbacking = true;

        float duration = 0.6f; // thời gian bị đẩy
        float timer = 0f;

        Vector3 startPos = transform.position;
        Vector3 targetPos = startPos + dir.normalized * force;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;

            // easing cho mượt
            t = 1f - Mathf.Pow(1f - t, 3);

            transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        transform.position = targetPos;
        isKnockbacking = false;
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


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Water"))
        {
            Dead();
        }
    }

}
