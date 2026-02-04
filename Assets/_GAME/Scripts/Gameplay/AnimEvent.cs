using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEvent : MonoBehaviour
{
    public event Action OnAttackAction;
    public event Action OnEndAnimAction;

    public void HandleAttack()
    {
        OnAttackAction?.Invoke();
    }

    public void HandleEndAttack()
    {
        OnEndAnimAction?.Invoke();
    }
}
