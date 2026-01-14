using System;
using UnityEngine;

public class PlayerGroundChecker : MonoBehaviour
{
    public bool IsGrounded { get; private set; } = true;

    public event Action OnFallOutGround;

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            IsGrounded = false;
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
