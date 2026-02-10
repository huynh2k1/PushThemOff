using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletEnemy : PooledObject
{
    [SerializeField] float speed = 10f;

    Vector3 dir;
    float damage;
    float maxDistance;

    Vector3 startPos;

    bool _isStart = false;

    public void Init(Vector3 dir, float damage, float maxDistance)
    {
        this.dir = dir;
        this.damage = damage;
        this.maxDistance = maxDistance;
        startPos = transform.position;
        _isStart = true;
    }

    void Update()
    {
        if (!_isStart || !GameController.I.IsPlaying)
            return;
        transform.position += dir * speed * Time.deltaTime;

        if (Vector3.Distance(startPos, transform.position) >= maxDistance)
            DestroyObj();
    }

    void DestroyObj()
    {
        _isStart = false;
        gameObject.SetActive(false);
        RequestDespawn();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var c = other.GetComponent<BaseCharacter>();
            if (c)
            {
                c.TakeDamage(damage);
                DestroyObj();   
            }
        }

        if (other.CompareTag("Ground"))
        {
            DestroyObj();
        }
    }
}
