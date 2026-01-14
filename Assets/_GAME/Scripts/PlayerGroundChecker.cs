using System;
using UnityEngine;

public class PlayerGroundChecker : MonoBehaviour
{
    Rigidbody _rb;
    Collider _collider;

    public event Action OnFallOutGround;

    private void Awake()
    {
        if(_rb == null)
        {
            _rb = GetComponent<Rigidbody>();    
            _rb.isKinematic = false; 
        }

        if(_collider == null)
        {
            _collider = GetComponent<Collider>();
            _collider.isTrigger = false;
        }
    }

    void OnPlayerDead()
    {
        _rb.velocity = Vector3.zero;    
        _rb.isKinematic = true;
        _collider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            OnPlayerDead();
            OnFallOutGround?.Invoke();
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Ground"))
    //    {
    //        IsGrounded = true;
    //    }
    //}
}
