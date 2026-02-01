using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy : MonoBehaviour
{
    [SerializeField] float speed = 10f;

    Vector3 dir;
    float damage;
    float maxDistance;

    Vector3 startPos;

    public void Init(Vector3 dir, float damage, float maxDistance)
    {
        this.dir = dir;
        this.damage = damage;
        this.maxDistance = maxDistance;
        startPos = transform.position;
    }

    void Update()
    {
        transform.position += dir * speed * Time.deltaTime;

        if (Vector3.Distance(startPos, transform.position) >= maxDistance)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var c = other.GetComponent<BaseCharacter>();
            if (c)
            {
                c.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}
