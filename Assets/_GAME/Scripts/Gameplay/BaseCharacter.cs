using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCharacter : MonoBehaviour
{
    [SerializeField] protected Rigidbody rb;



    protected virtual void Awake()
    {
        if(rb == null)
            rb = GetComponent<Rigidbody>();
    }

    protected abstract void Attack();
    protected abstract void Dead();

    public abstract void TakeDamage(float damage);

}
